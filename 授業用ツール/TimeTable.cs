using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 授業用ツール
{
    class TimeTable
    {
        private string name;
        private string unipaName;
        private DateTime beginTime = new DateTime();
        private DateTime endTime = new DateTime();

        public TimeTable()
        {

        }

        public void setName(string name)
        {
            this.name = name;
        }
        public string getName()
        {
            return name;
        }

        public void setUnipaName(string name)
        {
            this.unipaName = name;
        }
        public string getUnipaName()
        {
            return unipaName;
        }

        public void setTime(DateTime beginTime, DateTime endTime)
        {
            this.beginTime = beginTime;
            this.endTime = endTime;
        }
        public DateTime getBeginTime()
        {
            return beginTime;
        }
        public DateTime getEndTime()
        {
            return endTime;
        }
    }
}
