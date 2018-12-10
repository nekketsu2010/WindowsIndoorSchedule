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

        private List<TimeClass> timeClasses = new List<TimeClass>(); //時間のリスト

        private List<DocumentClass> documents = new List<DocumentClass>(); //資料のリスト


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

        public void addTime(TimeClass timeClass)
        {
            timeClasses.Add(timeClass);
        }
        public void removeTime(int num)
        {
            timeClasses.RemoveAt(num);
        }
        public TimeClass getTime(int num)
        {
            return timeClasses[num];
        }
        public void renewTime(TimeClass timeClass, int num)
        {
            timeClasses[num] = timeClass;
        }
        public int TimeSize()
        {
            return timeClasses.Count;
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
        public int DocumentSize()
        {
            return documents.Count;
        }
    }
}
