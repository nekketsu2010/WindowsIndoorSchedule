using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 授業用ツール
{
    public partial class TimeOption : Form
    {
        private TimeClass timeClass = new TimeClass();

        public TimeOption()
        {
            InitializeComponent();

            //ShareData.TimeNumを見て、-1かそうでないかを判断
            if (ShareData.timeNum != -1)
            {
                timeClass = UserData.scheduleClasses[ShareData.num].getTime(ShareData.timeNum);
                dateTimePicker1.Value = timeClass.getBeginTime();
                dateTimePicker2.Value = timeClass.getEndTime();
            }
            else
            {
                dateTimePicker2.Value = DateTime.Parse("23:59");
            }


            //部屋を追加処理
            for (int i = 0; i < ShareData.rooms.Count; i++)
            {
                listView2.Items.Add(ShareData.rooms[i].getRoomName());
            }
            for (int i = 0; i < listView2.Items.Count; i++)
            {
                Console.WriteLine(ShareData.rooms[i].getRoomName());
                if (timeClass.getRoomName() == ShareData.rooms[i].getRoomName())
                {
                    listView2.Items[i].Selected = true;
                }
            }

            //時間割を追加処理
            for (int i = 0; i < ShareData.timeTables.Count; i++)
            {
                listView1.Items.Add(ShareData.timeTables[i].getName());
            }
            if (timeClass.getType() == 0)
            {
                radioButton1.Checked = true;
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (timeClass.getTimeTable() == ShareData.timeTables[i].getName())
                    {
                        listView1.Items[i].Selected = true;
                    }
                }
            }
            else
            {
                radioButton2.Checked = true;
            }


            bool[] day = timeClass.getDay();
            checkBox1.Checked = day[0];
            checkBox2.Checked = day[1];
            checkBox3.Checked = day[2];
            checkBox4.Checked = day[3];
            checkBox5.Checked = day[4];
            checkBox6.Checked = day[5];
            checkBox7.Checked = day[6];
        }

        //決定ボタンを押したとき
        private void button1_Click(object sender, EventArgs e)
        {
            timeClass.setDay(new bool[] { checkBox1.Checked, checkBox2.Checked, checkBox3.Checked, checkBox4.Checked, checkBox5.Checked, checkBox6.Checked, checkBox7.Checked });
            for (int i = 0; i < ShareData.rooms.Count; i++)
            {
                if (listView2.Items[i].Selected)
                {
                    timeClass.setRoomName(listView2.Items[i].Text);
                    break;
                }
            }
            for (int i = 0; i < ShareData.timeTables.Count; i++)
            {
                if (listView1.Items[i].Selected)
                {
                    timeClass.setTimeTable(listView1.Items[i].Text);
                    break;
                }
            }
            if (radioButton1.Checked)
            {
                //リストから情環とか選んだものからBeginTimeとEndTimeを取得する
                Console.WriteLine("これが現実だ！" + listView1.SelectedItems[0].Text);
                timeClass.setType(0);
                for (int i = 0; i < ShareData.timeTables.Count; i++)
                {
                    Console.WriteLine(ShareData.timeTables[i].getName());
                    if (listView1.SelectedItems[0].Text == ShareData.timeTables[i].getName())
                    {
                        timeClass.setBeginTime(ShareData.timeTables[i].getBeginTime());
                        timeClass.setEndTime(ShareData.timeTables[i].getEndTime());
                        break;
                    }
                }

            }
            else
            {
                timeClass.setType(1);
                timeClass.setBeginTime(dateTimePicker1.Value);
                timeClass.setEndTime(dateTimePicker2.Value);
            }

            if (ShareData.timeNum != -1)
            {
                //時間を上書き
                UserData.scheduleClasses[ShareData.num].renewTime(timeClass, ShareData.timeNum);
            }
            else
            {
                //時間を追加
                UserData.scheduleClasses[ShareData.num].addTime(timeClass);
            }
            this.Close();
        }
    }
}
