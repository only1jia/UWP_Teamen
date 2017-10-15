using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace teamen
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            TheUserViewModel = new ViewModels.UserViewModel();
            teleViewModel = new ViewModels.telePhoneViewModel();
        }

        public static ViewModels.UserViewModel TheUserViewModel { get; set; }
        ViewModels.telePhoneViewModel teleViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {  
            initial();      
        }

        private void  initial()
        {
            SQLiteConnection db = App.conn;
            using (var statement = db.Prepare("SELECT telePhone FROM User"))
            {
                while (SQLiteResult.ROW == statement.Step())
                {
                    teleViewModel.AddToUser(statement[0] as string);
                }
            }
        }

        private string GetpassWord(string telePhone)
        {
            string password = "";
            SQLiteConnection db = App.conn;
            using (var statement = db.Prepare("SELECT passWord FROM User WHERE telePhone = ?"))
            {
                statement.Bind(1, telePhone);
                while (SQLiteResult.ROW == statement.Step())
                {
                    password = statement[0] as string;
                    if (!(password.Equals(""))) return password;
                }
            }
            return password;
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage), "");
        }

        private void passWordButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ChangepassWordPage), "");
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string password = GetpassWord(telePhoneBox.Text);
            if (password.Equals(""))
            {
                var err = new MessageDialog("手机号未注册").ShowAsync();
            }
            else if (!(password.Equals(passWordBox.Password)))
            {
                var err = new MessageDialog("密码错误").ShowAsync();
            }
            else
            {
                SQLiteConnection db = App.conn;
                string[] ans = new string[3];
                using (var statement = db.Prepare("SELECT userName, sid, schoolName FROM User WHERE telePhone = ?"))
                {
                    statement.Bind(1, telePhoneBox.Text);
                    while (SQLiteResult.ROW == statement.Step())
                    {
                        password = statement[0] as string;
                        if (!(password.Equals("")))
                        {
                            ans[0] = statement[0] as string;
                            ans[1] = statement[1] as string;
                            ans[2] = statement[2] as string;
                            break;
                        }
                    }
                }
                TheUserViewModel.UpdateUser(telePhoneBox.Text, ans[0], ans[1], ans[2]);
                var user = new Models.User(telePhoneBox.Text, ans[0], ans[1], password, ans[2]);
                //var err = new MessageDialog(ViewModel.user.telePhone).ShowAsync();
                TheUserViewModel.user = user;
                BitmapImage image = new BitmapImage();
                image.UriSource = new Uri("ms-appx:///Assets/26.jpg");
                TheUserViewModel.addFriends("800-820-3800","小冰", image);
                Frame.Navigate(typeof(ChatPage), user);
            }
           
        }

        private void telePhoneBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

        }

        private void telePhoneBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }
    }
}
