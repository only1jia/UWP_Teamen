using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class ChangepassWordPage : Page
    {
        public ChangepassWordPage()
        {
            this.InitializeComponent();
            flag = new bool[3];
        }

        private bool[] flag;

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

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            string password = GetpassWord(telePhoneBox.Text);
            if (password.Equals(""))
            {
                telePhoneWarnings.Text = "手机号未注册";
            }
            else if (!(password.Equals(oldpassWordBox.Password)))
            {
                oldpassWordWarnings.Text = "原密码错误";
            }
            else
            {
                SQLiteConnection db = App.conn;
                using (var statement = db.Prepare("UPDATE User SET passWord = ? WHERE telePhone = ?"))
                {
                    statement.Bind(1, newpassWordBox.Password);
                    statement.Bind(2, telePhoneBox.Text);
                    statement.Step();
                }
                Frame.Navigate(typeof(LoginPage), "");
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

        private void oldpassWordBox_PointerExited(object sender, PointerRoutedEventArgs e)
        {

        }

        private void newpassWordBox_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (newpassWordBox.Password.Length < 10)
            {
                newpassWordWarnings.Text = "密码长度不能小于10";
                flag[2] = false;
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(newpassWordBox.Password, @"^[a-zA-Z0-9_]{9,100}$"))
            {
                newpassWordWarnings.Text = "只能包含字母数字下划线";
                flag[2] = false;
                return;
            }
            newpassWordWarnings.Text = "";
            flag[2] = true;
        }

   

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage), "");
        }
    }
}
