using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 授業用ツール
{
    class Room
    {
        private string num;
        private string roomName;

        public Room()
        {

        }

        public void setNum(string num)
        {
            this.num = num;
        }
        public string getNum()
        {
            return num;
        }

        public void setRoomName(string roomName)
        {
            this.roomName = roomName;
        }
        public string getRoomName()
        {
            return roomName;
        }
    }
}
