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
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace teamen
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            this.InitializeComponent();
            flag = new bool[5];
            suggestions = new ObservableCollection<string>();
            initial();
        }

        private bool[] flag;
        private ObservableCollection<string> suggestions;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private void initial()
        {
            for (int i = 0; i < 5; ++i) flag[i] = false;
            suggestions.Add("中山大学");
            suggestions.Add("武汉大学");
            suggestions.Add("清华大学");
            suggestions.Add("北京大学");
            suggestions.Add("华南理工大学");
            //schoolBox.ItemsSource = suggestions;
        }

        private bool isExisted(string telePhone)
        {
            SQLiteConnection db = App.conn;
            using (var statement = db.Prepare("SELECT userName FROM User WHERE telePhone = ?"))
            {
                statement.Bind(1, telePhone);
                while(SQLiteResult.ROW == statement.Step())
                {
                    string temp = statement[0] as string;
                    if (!(temp.Equals(""))) return true;
                }
            }
            return false;
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            if (flag[0] && flag[1] && flag[2] && flag[3])
            {
                if (isExisted(telePhoneBox.Text))
                {
                    telePhoneWarnings.Text = "用户名已注册";
                }
                else
                {
                    SQLiteConnection db = App.conn;
                    try
                    {
                        using (var NewUser = db.Prepare("INSERT INTO User (telePhone,userName,sid,passWord,schoolName) VALUES (?,?,?,?,?)"))
                        {
                            NewUser.Bind(1, telePhoneBox.Text);
                            NewUser.Bind(2, userNameBox.Text);
                            NewUser.Bind(3, sidBox.Text);
                            NewUser.Bind(4, passWordBox.Password);
                            NewUser.Bind(5, schoolBox.Text);
                            NewUser.Step();
                        }
                        Frame.Navigate(typeof(LoginPage), "");
                    }
                    catch (Exception ex)
                    {
                        var err = new MessageDialog(ex.Message).ShowAsync();
                    }
                }
            }
                      
        }

        private void telePhoneBox_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(telePhoneBox.Text, @"^[1]+[3,5]+\d{9}"))
            {
                telePhoneWarnings.Text = "手机号不正确";
                flag[0] = false;
            }
            else
            {
                telePhoneWarnings.Text = "";
                flag[0] = true;
            }
        }

        private void userNameBox_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            
            
            if (userNameBox.Text.Length < 6)
            {
                userNameWarnings.Text = "长度不能小于6";
                flag[1] = false;
                return;
            }
            if (userNameBox.Text.Length > 18)
            {
                userNameWarnings.Text = "长度不能大于18";
                flag[1] = false;
                return;
            }
            if (!Char.IsLetter(userNameBox.Text[0]))
            {
                userNameWarnings.Text = "字母开头！！";
                flag[1] = false;
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(userNameBox.Text, @"^[a-zA-Z][a-zA-Z0-9_]{6,18}$"))
            {
                userNameWarnings.Text = "只能包含字母数字或下划线";
                flag[1] = false;
                return;
            }
            userNameWarnings.Text = "";
            flag[1] = true;
        }

        private void passWordBox_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (passWordBox.Password.Length <10)
            {
                passWordWarnings.Text = "密码长度不能小于10";
                flag[2] = false;
                return;
            }
            if(!System.Text.RegularExpressions.Regex.IsMatch(passWordBox.Password, @"^[a-zA-Z0-9_]{9,100}$"))
            {
                passWordWarnings.Text = "只能包含字母数字下划线";
                flag[2] = false;
                return;
            }
            passWordWarnings.Text = "";
            flag[2] = true;
        }

        private void reapeatpassWordBox_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if(!passWordBox.Password.Equals(reapeatpassWordBox.Password))
            {
                repeatpassWordWarnings.Text = "两次密码不一致";
                flag[3] = false;
            }
            else
            {
                repeatpassWordWarnings.Text = "";
                flag[3] = true;
            }
        }
    }
}
