using Newtonsoft.Json;
using SQLitePCL;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using teamen.ViewModels;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using SocketBusiness;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace teamen
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ChatPage : Page
    {
        public ChatPage()
        {
            this.InitializeComponent();
            theMessage = new ViewMessage();
            userViewModel = LoginPage.TheUserViewModel;
           
            Startup();
           
        }

        private async void Startup()
        {
            ClientSocket client = new ClientSocket("172.18.68.162", "22233");
            await client.Start();
            if (!client.Working) return;
            //当新消息到达时的行为  
            client.MsgReceivedAction += data => { Messenger.Default.Send(data, "MsgReceivedAction"); };
            //要发送的消息对象  
            client.OnStartSuccess += () =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() => SocketStateTxt = "已连接");
            };

            //连接失败时的行为  
            client.OnStartFailed += exc =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() => SocketStateTxt = $"断开的连接：{exc.Message}");
            };
            await new MessageDialog(SocketStateTxt.ToString()).ShowAsync();

            //开始连接远程服务端  
            await client.Start();
            var msg = new MessageModel
            {
                MessageType = MessageType.TextMessage,
                Message = "helloworld",
                SetDateTime = DateTime.Now
            };
            await client.SendMsg(msg);
        }

        ViewModels.UserViewModel userViewModel;
        ViewMessage theMessage;

        public string SocketStateTxt { get; private set; }

        private async void getAnswer(string message)
        {
            try
            {
                // 创建一个HTTP client实例对象
                HttpClient httpClient = new HttpClient();

                // Add a user-agent header to the GET request. 
                /*
                默认情况下，HttpClient对象不会将用户代理标头随 HTTP 请求一起发送到 Web 服务。
                某些 HTTP 服务器（包括某些 Microsoft Web 服务器）要求从客户端发送的 HTTP 请求附带用户代理标头。
                如果标头不存在，则 HTTP 服务器返回错误。
                在 Windows.Web.Http.Headers 命名空间中使用类时，需要添加用户代理标头。
                我们将该标头添加到 HttpClient.DefaultRequestHeaders 属性以避免这些错误。
                */
                var headers = httpClient.DefaultRequestHeaders;
                headers.Add("apikey", "59877b8e55952a5b8ae4bb3f6e97a818");
                // The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
                // especially if the header value is coming from user input.
                string header = "ie Mozilla/5.0 (Windows NT 6.2; WOW64; rv:25.0) Gecko/20100101 Firefox/25.0";
                if (!headers.UserAgent.TryParseAdd(header))
                {
                    throw new Exception("Invalid header value: " + header);
                }


                string url = "http://www.tuling123.com/openapi/api?key=879a6cb3afb84dbf4fc84a1df2ab7319&info=" + message;

                //发送GET请求
                HttpResponseMessage response = await httpClient.GetAsync(url);

                // 确保返回值为成功状态
                response.EnsureSuccessStatusCode();

                Byte[] getByte = await response.Content.ReadAsByteArrayAsync();

                // 可以用来测试返回的结果
                //string returnContent = await response.Content.ReadAsStringAsync();

                // UTF-8是Unicode的实现方式之一。这里采用UTF-8进行编码
                Encoding code = Encoding.GetEncoding("UTF-8");
                string result = code.GetString(getByte, 0, getByte.Length);

                JsonTextReader json = new JsonTextReader(new StringReader(result));
                string jsonVal = "", answer = "";
                while (json.Read())
                {
                    jsonVal += json.Value;
                    if (jsonVal.Equals("text"))  // 读到“cityCode”时，取出下一个json token（即城市对应代码）
                    {
                        json.Read();
                        answer += json.Value;  // 该对象重载了“+=”,故可与字符串进行连接
                        break;
                    }
                    jsonVal = "";
                }
                receiveMessage(answer);
                //await new MessageDialog(result).ShowAsync(); 
            }
            catch (HttpRequestException ex1)
            {
                var i = new MessageDialog(ex1.ToString()).ShowAsync();
            }
            catch (Exception ex2)
            {
                var i = new MessageDialog(ex2.ToString()).ShowAsync();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter.GetType() == typeof(ViewModels.UserViewModel))
            {
                userViewModel = (ViewModels.UserViewModel)(e.Parameter);
            }
            else if (e.Parameter.GetType() == typeof(ViewModels.ViewMessage))
            {
                theMessage = (ViewModels.ViewMessage)(e.Parameter);
            }
            string message, send_name, date;
            int isSelf;
            if (userViewModel.SelectedItem != null && userViewModel.SelectedItem.userName == "小冰")
            {
                using (var newitem = App.conn_message.Prepare("select Message, Send_name, Date, IsSelf From ChatMessage"))
                {
                    while (SQLiteResult.DONE != newitem.Step())
                    {
                        message = newitem[0].ToString();
                        send_name = newitem[1].ToString();
                        date = newitem[2].ToString();
                        isSelf = int.Parse(newitem[3].ToString());
                        BitmapImage icon;
                        if (isSelf == 0)
                            icon = userViewModel.SelectedItem.userImage;

                        else
                            icon = userViewModel.user.userImage;
                        theMessage.MessageList.Add(new Models.Message(message, send_name, date, "", isSelf, icon));
                    }
                }
            }

            //scroll to bottom
            chatListView.UpdateLayout();
            if (chatListView.Items.Count > 0 )
                chatListView.ScrollIntoView(chatListView.Items[chatListView.Items.Count-1]);
        }



        private void Chatting_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ContactList_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AdressBookPage), "");
        }

        private void Around_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PersonalSettingPage), "");
        }

     

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string name = userViewModel.user.userName;
            string message = Message.Text.ToString();
            theMessage.addMessage(message, name, 1);
            if (userViewModel.SelectedItem != null && userViewModel.SelectedItem.userName == "小冰")
                getAnswer(message);
            Message.Text = "";
            Frame.Navigate(typeof(ChatPage), "");

        }

        private void receiveMessage(string message)
        {
            theMessage.addMessage(message, "小冰", 0);
            Frame.Navigate(typeof(ChatPage), "");
        }

       

        private void LiveTile_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
