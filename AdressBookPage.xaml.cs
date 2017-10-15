using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace teamen
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AdressBookPage : Page
    {
        public AdressBookPage()
        {
            this.InitializeComponent();
            userViewModel = LoginPage.TheUserViewModel;
            userViewModel.SelectedItem = null;
        }
        ViewModels.UserViewModel userViewModel;

        private void list1_Click(object sender, RoutedEventArgs e)
        {
            if (friendView.Visibility == Visibility.Collapsed)
                friendView.Visibility = Visibility.Visible;
            else
                friendView.Visibility = Visibility.Collapsed;
        }

        private void Chatting_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ChatPage), "");
        }


        private void Around_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PersonalSettingPage), "");
        }

        private void ContactList_Click(object sender, RoutedEventArgs e)
        {

        }

        private void friendView_ItemClicked(object sender, ItemClickEventArgs e)
        {
            userViewModel.SelectedItem = (Models.User)(e.ClickedItem);
            Frame.Navigate(typeof(ChatPage), userViewModel);
        }

        private void LiveTile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
