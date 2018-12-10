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
    public partial class ScheduleOption : Form
    {
        public ScheduleOption()
        {
            InitializeComponent();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            //リストに同じ名前のものがあるか確認する
            for (int i = 0; i < UserData.scheduleClasses.Count; i++)
            {
                if (UserData.scheduleClasses[i].getName() == textBox1.Text)
                {
                    MessageBox.Show("すでに登録されている名前を新しく登録することはできません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }


            //新規フォルダ作成
            System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + "\\" + textBox1.Text);

            //ScheduleClass作成
            ScheduleClass schedule = new ScheduleClass();
            TimeClass time = new TimeClass();
            schedule.setName(textBox1.Text);
            time.setRoomName(listView2.SelectedItems[0].Text);
            int type = 0;
            if (!radioButton1.Checked)
            {
                type = 1;
            }
            time.setType(type);

            if(type==0)
            {
                //リストから情環とか選んだものからBeginTimeとEndTimeを取得する
                Console.WriteLine("これが現実だ！" + listView1.SelectedItems[0].Text);
                time.setTimeTable(listView1.SelectedItems[0].Text);
                for (int i = 0; i < ShareData.timeTables.Count; i++)
                {
                    Console.WriteLine(ShareData.timeTables[i].getName());
                    if (listView1.SelectedItems[0].Text == ShareData.timeTables[i].getName())
                    {
                        time.setBeginTime(ShareData.timeTables[i].getBeginTime());
                        time.setEndTime(ShareData.timeTables[i].getEndTime());
                        break;
                    }
                }

            }
            else
            {
                time.setBeginTime(dateTimePicker1.Value);
                time.setEndTime(dateTimePicker2.Value);
            }

            bool[] day = new bool[7];
            day[0] = checkBox1.Checked;
            day[1] = checkBox2.Checked;
            day[2] = checkBox3.Checked;
            day[3] = checkBox4.Checked;
            day[4] = checkBox5.Checked;
            day[5] = checkBox6.Checked;
            day[6] = checkBox7.Checked;
            time.setDay(day);

            schedule.addTime(time);
            UserData.scheduleClasses.Add(schedule);

            this.Close();
        }
    }
}
