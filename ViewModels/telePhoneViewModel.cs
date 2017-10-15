using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teamen.ViewModels
{
    public class telePhoneViewModel
    {
        private ObservableCollection<string> allUser = new ObservableCollection<string>();
        public ObservableCollection<string> AllUsers { get { return allUser; } }
        public void AddToUser(string telePhone)
        {
            this.allUser.Add(telePhone);
        }
    }
}
