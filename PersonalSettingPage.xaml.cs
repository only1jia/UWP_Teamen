using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace teamen
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PersonalSettingPage : Page
    {
        public PersonalSettingPage()
        {
            this.InitializeComponent();
            this.TheUserViewModel = LoginPage.TheUserViewModel;
            TheUserViewModel.SelectedItem = null;
            //NavigationCacheMode = NavigationCacheMode.Enabled;    //缓存
        }

        ViewModels.UserViewModel TheUserViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

            if (e.Parameter.GetType() == typeof(ViewModels.UserViewModel))
            {
                this.TheUserViewModel = (ViewModels.UserViewModel)(e.Parameter);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SQLiteConnection db = App.conn;
            using (var statement = db.Prepare("UPDATE User SET userName = ?, sid = ?, schoolName = ? WHERE telePhone = ?"))
            {
                
                statement.Bind(1, userNameBox.Text);
                statement.Bind(2, sidBox.Text);
                statement.Bind(3, schoolBox.Text);
                statement.Bind(4, telePhoneBox.Text);
                statement.Step();
            }
            TheUserViewModel.UpdateUser(telePhoneBox.Text, userNameBox.Text, sidBox.Text, schoolBox.Text);
            var success = new MessageDialog("保存成功！").ShowAsync();
        }

        private void Chatting_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ChatPage), "");
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

        }

        private void LiveTile_Click(object sender, RoutedEventArgs e)
        {

        }

     
    }
}
