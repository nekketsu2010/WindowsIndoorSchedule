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
    public partial class ClassOption : Form
    {
        private ScheduleClass schedule = new ScheduleClass();

        private string[] names;

        public ClassOption()
        {
            InitializeComponent();

            schedule = UserData.scheduleClasses[ShareData.num];
            textBox1.Text = schedule.getName();
            if (schedule.getType() != 0)
            {
                radioButton2.Checked = true;
            }
            bool[] day = schedule.getDay();
            checkBox1.Checked = day[0];
            checkBox2.Checked = day[1];
            checkBox3.Checked = day[2];
            checkBox4.Checked = day[3];
            checkBox5.Checked = day[4];
            checkBox6.Checked = day[5];
            checkBox7.Checked = day[6];
            for (int i = 0; i < ShareData.rooms.Count; i++)
            {
                listView2.Items.Add(ShareData.rooms[i].getRoomName());
            }
            Console.WriteLine(schedule.getRoomName());
            for (int i = 0; i < listView2.Items.Count; i++)
            {
                Console.WriteLine(ShareData.rooms[i].getRoomName());
                if (schedule.getRoomName() == ShareData.rooms[i].getRoomName())
                {
                    listView2.Items[i].Selected = true;
                }
            }
            for (int i = 0; i < ShareData.timeTables.Count; i++)
            {
                listView1.Items.Add(ShareData.timeTables[i].getName());
            }
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (schedule.getTimeTable() == ShareData.timeTables[i].getName())
                {
                    listView1.Items[i].Selected = true;
                }
            }
            dateTimePicker1.Value = schedule.getBeginTime();
            dateTimePicker2.Value = schedule.getEndTime();

            //資料一覧の読み込み
            for (int i = 0; i < UserData.scheduleClasses[ShareData.num].size(); i++)
            {
                checkedListBox1.Items.Add(schedule.getDocument(i).getDocumentName(),
                    schedule.getDocument(i).getOpen());
            }

        }

        public void RenewCheckList()
        {
            checkedListBox1.Items.Clear();
            //資料一覧の読み込み
            for (int i = 0; i < schedule.size(); i++)
            {
                checkedListBox1.Items.Add(schedule.getDocument(i).getDocumentName(),
                    schedule.getDocument(i).getOpen());
            }
        }

        //アップロードボタン
        private void button1_Click(object sender, EventArgs e)
        {
            DocumentClass document = new DocumentClass();
            document.setDocumentName(names[1]);
            document.setDocumentPass(names[0]);
            schedule.addDocument(document);
            button1.Enabled = false;
            RenewCheckList();
        }

        //ファイル選択ボタン
        private void button3_Click(object sender, EventArgs e)
        {
            names = Utility.Upload();
            textBox2.Text = names[0]; //ファイルパス
            button1.Enabled = true;
        }

        //変更を保存して終了するボタン
        private void button2_Click(object sender, EventArgs e)
        {
            schedule.setName(textBox1.Text);
            schedule.setDay(new bool[] { checkBox1.Checked, checkBox2.Checked, checkBox3.Checked, checkBox4.Checked, checkBox5.Checked, checkBox6.Checked, checkBox7.Checked });
            for (int i = 0; i < ShareData.rooms.Count; i++)
            {
                if (listView2.Items[i].Selected)
                {
                    schedule.setRoomName(listView2.Items[i].Text);
                    break;
                }
            }
            for (int i = 0; i < ShareData.timeTables.Count; i++)
            {
                if (listView1.Items[i].Selected)
                {
                    schedule.setTimeTable(listView1.Items[i].Text);
                    break;
                }
            }
            if (radioButton1.Checked)
            {
                //リストから情環とか選んだものからBeginTimeとEndTimeを取得する
                Console.WriteLine("これが現実だ！" + listView1.SelectedItems[0].Text);
                for (int i = 0; i < ShareData.timeTables.Count; i++)
                {
                    Console.WriteLine(ShareData.timeTables[i].getName());
                    if (listView1.SelectedItems[0].Text == ShareData.timeTables[i].getName())
                    {
                        schedule.setBeginTime(ShareData.timeTables[i].getBeginTime());
                        schedule.setEndTime(ShareData.timeTables[i].getEndTime());
                        break;
                    }
                }

            }
            else
            {
                schedule.setBeginTime(dateTimePicker1.Value);
                schedule.setEndTime(dateTimePicker2.Value);
            }

            //資料状態の保存
            for (int i = 0; i < schedule.size(); i++)
            {
                schedule.getDocument(i).setOpen(checkedListBox1.GetItemChecked(i));
                //ファイルをコピーする（ファイルが存在していない場合のみ）
                if (!System.IO.File.Exists(schedule.getName() + "\\" + schedule.getDocument(i).getDocumentName()))
                {
                    System.IO.File.Copy(schedule.getDocument(i).getDocumentPass(),
                        Environment.CurrentDirectory + "\\" + schedule.getName() + "\\" + schedule.getDocument(i).getDocumentName(), false);
                }
            }

            UserData.scheduleClasses[ShareData.num] = schedule;

            this.Close();
        }
    }
}
