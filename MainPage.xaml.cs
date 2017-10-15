using System;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Data.Xml.Dom;
using System.IO;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using System.Collections.Generic;
using Windows.ApplicationModel;
using SQLitePCL;
using teamen.Models;


//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace MiddleProject
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
            NavigationCacheMode = NavigationCacheMode.Enabled;
            ViewModel = new ViewModels.ViewRecruitTeamMembers();
        }

        ViewModels.ViewRecruitTeamMembers ViewModel { get; set; }
        SQLiteConnection db = App.conn;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter.GetType() == typeof(ViewModels.ViewRecruitTeamMembers))
            {
                ViewModel = (ViewModels.ViewRecruitTeamMembers)(e.Parameter);
            }
            else
            {
                string id, title,date, description, filepath, comment;               
                int likenumber;
                using (var newitem = db.Prepare("select Id, Title, Description, Date, Filepath, Likenumber, Comment From RecruitDetails"))
                {
                    while (SQLiteResult.DONE != newitem.Step())
                    {
                        id = newitem[0].ToString();
                        title = newitem[1].ToString();
                        description = newitem[2].ToString();
                        date = newitem[3].ToString();
                        filepath = newitem[4].ToString();
                        likenumber = int.Parse(newitem[5].ToString());
                        comment = newitem[6].ToString();
                        ViewModel.AllItems.Add(new Model.RecruitTeamMembers(id, title, description, date, filepath, likenumber, comment));
                    }                 
                }                       
                Frame rootFrame = Window.Current.Content as Frame;
            }

        }
        private void ContactList_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Chatting_Click(object sender, RoutedEventArgs e)
        {

        }
       
        private void Setting_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddMyRecruit_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddMyRecruit), ViewModel);
        }

        private void SearchRecruit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Like_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton a = (AppBarButton)e.OriginalSource;
            Grid b = (Grid)a.Parent;
            TextBlock c = (TextBlock)b.Children[2];
            ViewModel.SelectedItem  = (RecruitTeamMembers)a.DataContext;
            ViewModel.SelectedItem.likeNumber++;
            c.Text = ViewModel.SelectedItem.likeNumber.ToString();
            ViewModel.UpdateRecruitTeamMembers(ViewModel.SelectedItem.id, ViewModel.SelectedItem.title, ViewModel.SelectedItem.description, ViewModel.SelectedItem.date, ViewModel.SelectedItem.filepath, ViewModel.SelectedItem.likeNumber, ViewModel.SelectedItem.comment);
            
        }

        private void Sent_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void comment_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
