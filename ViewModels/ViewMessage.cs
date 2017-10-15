using SQLitePCL;
using System;
using System.Collections.Generic;
using teamen.Models;

namespace teamen.ViewModels
{
     class ViewMessage
    {
        public List<Models.Message> MessageList { get; set; }

        public ViewMessage()
        {
            InitSampleData();
        }

        private void InitSampleData()
        {
            MessageList = new List<Models.Message>
            {
                //new Message
                //{
                //    message = "天王盖地虎", name="张三" ,time = DateTime.Now, isSelf = false
                //},
                //new Message
                //{
                //    message = "宝塔镇河妖", isSelf = true, name="政委", time = DateTime.Now
                //}
            };
        }

        public void addMessage(string _message, string _name, int _isSelf)
        {
            SQLiteConnection db = App.conn_message;
            using (var newitem = db.Prepare("INSERT INTO ChatMessage (Id, Message, Send_name, Date, Receive_name, IsSelf) VALUES (?,?,?,?,?,?)"))
            {
                newitem.Bind(2, _message);
                newitem.Bind(3, _name);
                newitem.Bind(4, DateTime.Now.ToString());
                newitem.Bind(6, _isSelf);
                newitem.Step();
            }
            //MessageList.Add(new Message(_message, _name, DateTime.Now.ToString(), "", _isSelf));
        }
    }
}
