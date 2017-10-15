using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace teamen.Models
{
    public class User
    {
        public string telePhone { get; set; }
        public string userName { get; set; }
        public string sid { get; set; }
        public string passWord { get; set; }
        public string schoolName { get; set; }
        public BitmapImage userImage { get; set; }
        public ObservableCollection<User> friends { get; set; }

        public User(string telePhone, string username, string sid, string password,string schoolName, BitmapImage bitmap = null)
        {
            this.telePhone = telePhone;
            this.userName = username;
            this.sid = sid;
            this.passWord = password;
            this.schoolName = schoolName;
            this.userImage = bitmap;
            friends = new ObservableCollection<User>();
        }

        public User()
        {
        }
    }
}
