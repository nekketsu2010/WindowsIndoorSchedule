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
                                Console.WriteLine(element["TimeName"]);
                                Console.WriteLine(element["Name"]);
                                

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
    }
}
