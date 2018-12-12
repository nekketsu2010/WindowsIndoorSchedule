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
            for (int i = 0; i < schedule.DocumentSize(); i++)
            {
                checkedListBox1.Items.Add(schedule.getDocument(i).getDocumentName(),
                    schedule.getDocument(i).getOpen());
            }

            //時間一覧の読み込み
            for (int i = 0; i < schedule.TimeSize(); i++)
            {
                TimeClass time = schedule.getTime(i);
                string text = "";
                for (int j = 0; j < ShareData.dayofWeek.Length; j++)
                {
                    if (time.getDay()[j])
                    {
                        text += ShareData.dayofWeek[j];
                    }
                }
                text += " " + time.getBeginTime().ToShortTimeString() + "～" + time.getEndTime().ToShortTimeString() + " " + time.getRoomName();
                checkedListBox2.Items.Add(text);               
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
            //時間一覧の読み込み
            for (int i = 0; i < schedule.TimeSize(); i++)
            {
                TimeClass time = schedule.getTime(i);
                string text = "";
                for (int j = 0; j < ShareData.dayofWeek.Length; j++)
                {
                    if (time.getDay()[j])
                    {
                        text += ShareData.dayofWeek[j];
                    }
                }
                text += " " + time.getBeginTime().ToShortTimeString() + "～" + time.getEndTime().ToShortTimeString() + " " + time.getRoomName();
                checkedListBox2.Items.Add(text);
            }

            this.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //インデックスを保存して編集画面へ
            ShareData.timeNum = checkedListBox2.SelectedIndex;
            TimeOption timeOption = new TimeOption();
            timeOption.FormClosed += new FormClosedEventHandler(SubFormClosed);
            timeOption.Show();
            this.Enabled = false;
        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button5.Enabled = checkedListBox2.SelectedItems.Count == 1;
        }
    }
}
