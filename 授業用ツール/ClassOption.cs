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

            //資料一覧の読み込み
            for (int i = 0; i < UserData.scheduleClasses[ShareData.num].DocumentSize(); i++)
            {
                checkedListBox1.Items.Add(schedule.getDocument(i).getDocumentName(),
                    schedule.getDocument(i).getOpen());
            }

        }

        public void RenewCheckList()
        {
            checkedListBox1.Items.Clear();
            //資料一覧の読み込み
            for (int i = 0; i < schedule.DocumentSize(); i++)
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

            //資料状態の保存
            for (int i = 0; i < schedule.DocumentSize(); i++)
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

        //新規追加ボタンを押したとき
        private void button4_Click(object sender, EventArgs e)
        {
            TimeOption timeOption = new TimeOption();
            timeOption.FormClosed += new FormClosedEventHandler(SubFormClosed);
            timeOption.Show();
            this.Enabled = false;
        }

        private void SubFormClosed(object sender, EventArgs e)
        {
            checkedListBox2.Items.Clear();
            //shedule更新
            for (int i = 0; i < UserData.scheduleClasses[ShareData.num].TimeSize(); i++)
            {
                TimeClass time = UserData.scheduleClasses[ShareData.num].getTime(i);
                string str = "";
                for (int j = 0; j < ShareData.dayofWeek.Length; j++)
                {
                    if (time.getDay()[j])
                    {
                        str += ShareData.dayofWeek[j];
                    }
                }
                str += " " + time.getRoomName() + " " + time.getBeginTime() + "～" + time.getEndTime();
                checkedListBox2.Items.Add(str,
                    schedule.getDocument(i).getOpen());
            }

            this.Enabled = true;
        }
    }
}
