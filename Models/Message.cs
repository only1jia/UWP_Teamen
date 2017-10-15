using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace teamen.Models
{
    class Message
    {
        public BitmapImage icon { get; set; }
        public string name { get; set; }
        public string time { get; set; }
        string receive_name;
        public string message { get; set; }
        public int isSelf { get; set; }

        public Message(string message, string name, string time, string receive_name, int isSelf, BitmapImage icon)
        {
            this.message = message;
            this.name = name;
            this.time = time;
            this.receive_name = receive_name;
            this.isSelf = isSelf;
            this.icon = icon;
        }
        public Message() { }
    }
  
}
