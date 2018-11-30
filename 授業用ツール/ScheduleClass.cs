using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 授業用ツール
{
    class ScheduleClass
    {
        private string name; //スケジュールの名前(講義名、ゼミなど)
        private string roomName; //教室名
        private int type = 0; //0=講義、1=スケジュール（ゼミなど）

        private bool[] day = new bool[7]; //日～土　何曜日に発動するのか

        //type=0のとき使用
        private string timeTable; //情環1時限　といった時間割の文字列

        //type=1のとき使用
        private DateTime beginTime = new DateTime(); //開始時間
        private DateTime endTime = new DateTime(); //終了時間

        private List<DocumentClass> documents = new List<DocumentClass>(); //資料のリスト

        private bool isClass = false;

        public ScheduleClass()
        {

        }

        public void setName(string name)
        {
            this.name = name;
        }
        public string getName()
        {
            return this.name;
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

        public void setDay(bool[] day)
        {
            this.day = day;
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

        public void addDocument(DocumentClass document)
        {
            documents.Add(document);
        }
        public void removeDocument(int num)
        {
            documents.RemoveAt(num);
        }
        public DocumentClass getDocument(int num)
        {
            return documents[num];
        }
        public void renewDocument(DocumentClass document, int num)
        {
            documents[num] = document;
        }
        public int size()
        {
            return documents.Count;
        }

        public void setIsClass(bool isClass)
        {
            this.isClass = isClass;
        }
        public bool getIsClass()
        {
            return isClass;
        }
    }
}
