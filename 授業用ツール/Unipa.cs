using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 授業用ツール
{
    public partial class Unipa : Form
    {
        private string url = "http://d0259c06.ngrok.io/sample-game-server/unipa_load?";
        private bool idFilled = false;
        private bool passFilled = false;

        public Unipa()
        {
            InitializeComponent();
            button1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ここからサーバー通信
            WebRequest req = WebRequest.Create(url + "user_id=" + textBox1.Text + "&password=" + textBox2.Text);
            WebResponse rsp = req.GetResponse();
            Stream stm = rsp.GetResponseStream();
            if (stm != null)
            {
                StreamReader reader = new StreamReader(stm, System.Text.Encoding.GetEncoding("UTF-8"));
                String result = reader.ReadToEnd();
                if (result.Contains("エラー"))
                {
                    MessageBox.Show("入力内容が違います！\nそれかUNIPAメンテナンス",
                        "エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);                       
                }
                //エラーでないなら時間割取得成功！JSONが返ってきているはず
                else
                {
                    var json = (dynamic)JsonConvert.DeserializeObject(result);
                    int num = 0;
                    //string[] dayofWeek = new string[] { "sun", "mon", "tue", "wed", "thu", "fri", "sat" };
                    //曜日ずつforeach
                    foreach (var day in json)
                    {
                        //Console.WriteLine(day.ToString());
                        foreach (var time in day)
                        {
                            //時限ずつforeach
                            foreach (var element in time)
                            {
                                string TimeName = element["TimeName"];
                                string Name = element["Name"];

                                TimeClass timeClass = new TimeClass();
                                timeClass.setType(0);

                                for (int i = 0; i < ShareData.timeTables.Count; i++)
                                {
                                    TimeTable timeTable = ShareData.timeTables[i];
                                    if (TimeName == timeTable.getUnipaName())
                                    {
                                       
                                        break;
                                    }
                                }


                                //もしないときこうなります
                                if (UserData.scheduleClasses.Count == 0)
                                {
                                    ScheduleClass schedule = new ScheduleClass();

                                    schedule.setName(Name);
                                    //TimeTableの特定
                                    for (int i = 0; i < ShareData.timeTables.Count; i++)
                                    {
                                        TimeTable timeTable = ShareData.timeTables[i];
                                        //もし"J1限"といった文字列が同じだったら
                                        if (TimeName == timeTable.getUnipaName())
                                        {
                                            timeClass.setTimeTable(TimeName);
                                            timeClass.setBeginTime(timeTable.getBeginTime());
                                            timeClass.setEndTime(timeTable.getEndTime());
                                            break;
                                        }
                                    }
                                }
                                //すでにスケジュールが入っている場合
                                else
                                {
                                    for (int i = 0; i < UserData.scheduleClasses.Count; i++)
                                    {
                                        ScheduleClass schedule = UserData.scheduleClasses[i];
                                        if (Name == schedule.getName())
                                        {
                                            //開始時間と終了時間が同じなら曜日のみ追加
                                            for (int j = 0; j < schedule.TimeSize(); j++)
                                            {
                                                if(schedule.getTime(j).getBeginTime().Hour == )
                                            }
                                        }
                                    }
                                }                         
                            }
                        }
                        num++;
                    }
                }
                stm.Close();
            }
            rsp.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            idFilled = !string.IsNullOrEmpty(textBox1.Text);
            if (idFilled && passFilled)
            {
                button1.Enabled = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            passFilled = !string.IsNullOrEmpty(textBox2.Text);
            if (idFilled && passFilled)
            {
                button1.Enabled = true;
            }
        }

        public DateTime[] timeTableTime(String TimeName)
        {
            DateTime[] time = new DateTime[2];
            time[0] = DateTime.Now;
            time[1] = DateTime.Now;
            switch (TimeName)
            {
                case "J1限":
                    time = Utility.createCalendar(9, 10, 10, 00);
                    break;
                case "J2限":
                    time = Utility.createCalendar(10, 10, 11, 00);
                    break;
                case "J3限":
                    time = Utility.createCalendar(11, 10, 12, 00);
                    break;
                case "J4限":
                    time = Utility.createCalendar(12, 40, 13, 30);
                    break;
                case "J5限":
                    time = Utility.createCalendar(13, 40, 14, 30);
                    break;
                case "J6限":
                    time = Utility.createCalendar(14, 40, 15, 30);
                    break;
                case "J7限":
                    time = Utility.createCalendar(15, 40, 16, 30);
                    break;
                case "J8限":
                    time = Utility.createCalendar(16, 40, 17, 30);
                    break;
                case "1限":
                    time = Utility.createCalendar(9, 20, 11, 00);
                    break;
                case "2限":
                    time = Utility.createCalendar(11, 10, 12, 50);
                    break;
                case "3限":
                    time = Utility.createCalendar(13, 40, 15, 20);
                    break;
                case "4限":
                    time = Utility.createCalendar(15, 30, 17, 10);
                    break;
                case "5限":
                    time = Utility.createCalendar(17, 20, 19, 00);
                    break;
            }
            return time;
        }
    }
}
