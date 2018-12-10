using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 授業用ツール
{
    class TimeClass
    {
        private string roomName; //情報通信サービス研究室　などが入る

        private int type = 0; //0:講義　1:スケジュール（ゼミなど）
        private bool isOnce = false; //１回のみかどうか

        //isOnceがFalseのとき使う
        private bool[] day = new bool[7]; //日～土　何曜日に発動するか

        //type=0のとき使用
        private string timeTable; //千住１限　といった時間割の文字列

        private DateTime beginTime = new DateTime();
        private DateTime endTime = new DateTime();

        //通知をしたかどうか
        private bool isClass = false;

        public TimeClass()
        {

        }

        public void setRoomName(string roomName)
        {
            this.roomName = roomName;
        }
        public string getRoomName()
        {
            return roomName;
        }

        public void setType(int type)
        {
            this.type = type;
        }
        public int getType()
        {
            return type;
        }

        public void setOnce(bool isOnce)
        {
            this.isOnce = isOnce;
        }
        public bool getOnce()
        {
            return isOnce;
        }

        public void setDay(bool[] day)
        {
            this.day = day;
        }
        public void setOneDay(bool day, int num)
        {
            this.day[num] = day;
        }
        public bool[] getDay()
        {
            return day;
        }

        public void setTimeTable(string timeTable)
        {
            this.timeTable = timeTable;
        }
        public string getTimeTable()
        {
            return timeTable;
        }

        public void setBeginTime(DateTime beginTime)
        {
            this.beginTime = beginTime;
        }
        public DateTime getBeginTime()
        {
            return beginTime;
        }

        public void setEndTime(DateTime endTime)
        {
            this.endTime = endTime;
        }
        public DateTime getEndTime()
        {
            return endTime;
        }

        public void setClass(bool isClass)
        {
            this.isClass = isClass;
        }
        public bool getClass()
        {
            return isClass;
        }
    }
}
