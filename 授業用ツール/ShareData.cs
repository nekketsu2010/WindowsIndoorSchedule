using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 授業用ツール
{
    class ShareData
    {
        public static List<Room> rooms = new List<Room>();
        public static List<TimeTable> timeTables = new List<TimeTable>();
        public static List<string> bssids = new List<string>();

        public static string[] dayofWeek = new string[] { "日", "月", "火", "水", "木", "金", "土" };

        public static int num = 0; //ほかフォームに配列のアドレスを送る
    }
}
