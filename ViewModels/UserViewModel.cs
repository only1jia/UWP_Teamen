using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using teamen.Models;
using Windows.UI.Xaml.Media.Imaging;

namespace teamen.ViewModels
{
    public class UserViewModel
    {
        public User user { get; set; }

        private Models.User selectedItem = default(Models.User);
        public Models.User SelectedItem
        {
            get { return selectedItem; }
            set { this.selectedItem = value; }
        }


        public UserViewModel()
        {
            user = new User();
        }

        public User TheUser() { return user; }

        public void UpdateUser(string telePhone, string userName, string sid, string schoolName)
        {
            this.user.telePhone = telePhone;
            this.user.userName = userName;
            this.user.sid = sid;
            this.user.schoolName = schoolName;
        }

        public void addFriends(string telephone, string name, BitmapImage image)
        {
            this.user.friends.Add(new User(telephone, name, "", "", "", image));
        }
        public void removeFriends(string name)
        {
            for (int i = 0; i < this.user.friends.Count - 1; i++)
            {
                if (this.user.friends[i].userName == name)
                {
                    this.user.friends.Remove(this.user.friends[i]);
                    break;
                }
            }
        }
        public bool isExisted(string telephone)
        {
            for (int i = 0; i < this.user.friends.Count; i++)
            {
                if (this.user.friends[i].telePhone.ToString().Equals(telephone.ToString()))
                    return true;
            }
            return false;
        }
    }
}
