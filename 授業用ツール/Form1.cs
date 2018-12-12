using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManagedNativeWifi;

namespace 授業用ツール
{
    public partial class Form1 : Form
    {
        private string url = "http://d0259c06.ngrok.io/sample-game-server/libsvm/predict";
        private string isRoom = ""; //現在いる部屋
        public Form1()
        {
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {
            //BSSIDリストの読み込み
            using (var reader = new StreamReader(Environment.CurrentDirectory + "\\bssid_db.txt"))
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    string[] csv = line.Split(',');     
                    ShareData.bssids.Add(csv[1]);
                    //Console.WriteLine(csv[1]);
                }
            }

            //部屋番号の読み込み
            using (var reader = new StreamReader(Environment.CurrentDirectory + "\\room_db.txt"))
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    string[] csv = line.Split(',');
                    Room room = new Room();
                    room.setNum(csv[0]);
                    room.setRoomName(csv[1]);
                    ShareData.rooms.Add(room);
                    //Console.WriteLine(csv[1]);
                }
            }

            //時間割の読み込み
            using (var reader = new StreamReader(Environment.CurrentDirectory + "\\時間割.txt"))
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    string[] csv = line.Split(',');
                    TimeTable timeTable = new TimeTable();
                    timeTable.setName(csv[0]);
                    timeTable.setTime(DateTime.Parse(csv[1]), DateTime.Parse(csv[2]));
                    timeTable.setUnipaName(csv[3]);
                    ShareData.timeTables.Add(timeTable);
                    //Console.WriteLine(csv[0]);
                }
            }


        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // フォームを表示する
            this.Visible = true;
            // 現在の状態が最小化の状態であれば通常の状態に戻す
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            // フォームをアクティブにする
            this.Activate();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // トレイリストのアイコンを非表示にする
            notifyIcon1.Visible = false;
        }

        private void Form1_ClientSizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.Forms.FormWindowState.Minimized)
            {
                // フォームが最小化の状態であればフォームを非表示にする
                this.Hide();
                // トレイリストのアイコンを表示する
                notifyIcon1.Visible = true;
            }
        }

        private void 講義登録ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 通知設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NotificationSetting notificationSetting = new NotificationSetting();
            notificationSetting.FormClosed += new FormClosedEventHandler(SubFormClosed);
            notificationSetting.Show();
            this.Enabled = false;
        }

        private void SubFormClosed(object sender, EventArgs e)
        {
            var strip = menuStrip1.Items[0] as ToolStripMenuItem;
            strip.DropDownItems.Clear();
            //shedule更新
            for (int i = 0; i < UserData.scheduleClasses.Count; i++)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = UserData.scheduleClasses[i].getName();
                item.Click += scheduleClicked;
                strip.DropDownItems.Add(item);
            }

            this.Enabled = true;
        }

        private void scheduleClicked(object sender, EventArgs e)
        {
            Console.WriteLine(sender.ToString());
            for (int i = 0; i < UserData.scheduleClasses.Count; i++)
            {
                if (UserData.scheduleClasses[i].getName() == sender.ToString())
                {
                    ShareData.num = i;
                    ClassOption classOption = new ClassOption();
                    classOption.FormClosed += new FormClosedEventHandler(SubFormClosed);
                    classOption.Show();
                    this.Enabled = false;
                }
            }
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            //講義時間が近づいたら通知するシステム
            for (int i = 0; i < UserData.scheduleClasses.Count; i++)
            {
                ScheduleClass schedule = UserData.scheduleClasses[i];
                for (int j = 0; j < schedule.TimeSize(); j++)
                {
                    TimeClass time = schedule.getTime(j);
                    DateTime beginTime = time.getBeginTime();
                    DateTime endTime = time.getEndTime();
                    TimeSpan beginSpan = beginTime.TimeOfDay;
                    TimeSpan endSpan = endTime.TimeOfDay;
                    TimeSpan fromSpan = DateTime.Now.TimeOfDay - beginSpan;
                    TimeSpan toSpan = DateTime.Now.TimeOfDay - endSpan;
                    double beginSecond = fromSpan.TotalSeconds;
                    double endSecond = toSpan.TotalSeconds;
                    bool[] day = time.getDay();
                    //Console.WriteLine((DateTime.Now.TimeOfDay - beginSpan).TotalSeconds + "秒前");
                    //10分前になったら　もちろん曜日も考慮
                    if (beginSecond > -600 && beginSecond < -599 && day[(int)DateTime.Now.DayOfWeek])
                    {
                        notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                        notifyIcon1.BalloonTipTitle = "まもなく「" + schedule.getName() + "」";
                        notifyIcon1.BalloonTipText = time.getRoomName() + "\n" + time.getBeginTime().ToShortTimeString() + "～";
                        notifyIcon1.ShowBalloonTip(5000);
                    }

                    //授業前後は出席していないことにする（実施されていないので当然である）
                    if (beginSecond < -10 && endSecond > 0)
                    {
                        time.setClass(false);
                        UserData.scheduleClasses[i].renewTime(time, j);
                    }

                    //スケジュール実行中の時間帯はこのタイマーを切る
                    if (beginSecond > -10 && endSecond < 0 && day[(int)DateTime.Now.DayOfWeek])
                    {
                        timer1.Enabled = false;
                        label3.Visible = true;
                        timer2.Enabled = true;
                    }
                }
            }
        }

        //アップロードボタン
        private void button1_Click(object sender, EventArgs e)
        {
            //ファイルパスを追加（現在の講義の）
        }

        //ファイル選択ボタン
        private void button3_Click(object sender, EventArgs e)
        {
            string[] names = Utility.Upload();
            textBox1.Text = names[0]; //ファイルパス

            button1.Enabled = true;
        }

        //4.1秒ごとにWifi測る（現在位置部屋特定）
        //このタイマーは授業中のみ実施
        private async void timer2_Tick(object sender, EventArgs e)
        {
            WifiInfo wifiInfo = new WifiInfo();
            foreach (BssNetworkPack network in NativeWifi.EnumerateBssNetworks())
            {
                wifiInfo.setSSID(network.Ssid.ToString());
                wifiInfo.setBSSID(network.Bssid.ToString().ToLower());
                wifiInfo.setRSSI(network.SignalStrength);
            }

            string rssi = WifiRSSI.SVMList(wifiInfo);
            Console.WriteLine(rssi);

            //ここからサーバー通信
            WebClient wc = new WebClient();
            NameValueCollection ps = new NameValueCollection();
            ps.Add("rssi", rssi);
            byte[] resData = wc.UploadValues(url, ps);
            wc.Dispose();

            string resText = System.Text.Encoding.UTF8.GetString(resData);
            Console.WriteLine(resText);
            resText = resText.Trim();
            resText = WifiRSSI.RoomConv(resText);
            //label2.Text = resText;
            isRoom = resText;

            int num = 0;
            for (int i = 0; i < UserData.scheduleClasses.Count; i++)
            {
                ScheduleClass schedule = UserData.scheduleClasses[i];
                for (int j = 0; j < schedule.TimeSize(); j++)
                {
                    TimeClass time = schedule.getTime(j);
                    DateTime beginTime = time.getBeginTime();
                    DateTime endTime = time.getEndTime();
                    TimeSpan beginSpan = beginTime.TimeOfDay;
                    TimeSpan endSpan = endTime.TimeOfDay;
                    TimeSpan fromSpan = DateTime.Now.TimeOfDay - beginSpan;
                    TimeSpan toSpan = DateTime.Now.TimeOfDay - endSpan;
                    double beginSecond = fromSpan.TotalSeconds;
                    double endSecond = toSpan.TotalSeconds;
                    bool[] day = time.getDay();
                    //実行中である
                    if (beginSecond > 0 && endSecond < 0 && day[(int)DateTime.Now.DayOfWeek])
                    {
                        num++;

                        label1.Text = "現在";
                        label3.Text = UserData.scheduleClasses[i].getName();
                        //部屋にいるか？
                        if (isRoom == time.getRoomName())
                        {
                            label4.Visible = true;
                            //講義資料自動オープンをするよ
                            for (int k = 0; k < UserData.scheduleClasses[i].DocumentSize(); k++)
                            {
                                DocumentClass document = UserData.scheduleClasses[i].getDocument(k);
                                if (document.getOpen() && !time.getClass())
                                {
                                    //"C:\test\1.txt"を関連付けられたアプリケーションで開く
                                    System.Diagnostics.Process p =
                                        System.Diagnostics.Process.Start(Environment.CurrentDirectory + "\\" + UserData.scheduleClasses[i].getName() + "\\" + document.getDocumentName());

                                    if (k == UserData.scheduleClasses[i].DocumentSize() - 1)
                                    {
                                        time.setClass(true);
                                        UserData.scheduleClasses[i].renewTime(time, j);
                                    }
                                    //この辺に出席を送信する処理入れたい

                                    Console.WriteLine(document.getDocumentName() + " 開いた");
                                }
                                await Task.Delay(1000);
                            }
                        }
                    }

                }
            }
            //実行中のものがないときタイマー２を切ってタイマー１をオンにする
            if (num == 0)
            {
                timer1.Enabled = true;
                label3.Visible = false;
                label4.Visible = false;
                timer2.Enabled = false;
            }

            //更新
            RefreshAsync();

        }

        public static Task RefreshAsync()
        {
            return NativeWifi.ScanNetworksAsync(timeout: TimeSpan.FromSeconds(10));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void 手動登録ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScheduleOption scheduleOption = new ScheduleOption();
            scheduleOption.FormClosed += new FormClosedEventHandler(SubFormClosed);
            scheduleOption.Show();
            this.Enabled = false;
        }

        private void uNIPAからToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Unipa unipa = new Unipa();
            unipa.FormClosed += new FormClosedEventHandler(SubFormClosed);
            unipa.Show();
            this.Enabled = false;

        }
    }
}