using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 授業用ツール
{
    class WifiRSSI
    {
        //LibSVMに使うリストを作成
        public static string SVMList(WifiInfo wifiInfo)
        {
            //部屋番号をクラス名にする
            string roomName = "51018";


            int[] rssis = new int[ShareData.bssids.Count];
            Console.WriteLine("検出されたAPの数：　" + wifiInfo.Length());
            for (int i = 0; i < wifiInfo.Length(); i++)
            {
                for (int j = 0; j < ShareData.bssids.Count; j++)
                {
                    if (wifiInfo.getBSSID(i).Equals(ShareData.bssids[j]))
                    {
                        rssis[j] = wifiInfo.getRSSI(i);
                        break;
                    }
                }
            }

            for (int i = 0; i < rssis.Length; i++)
            {
                if (rssis[i] == 0)
                {
                    //ないのでRSSIを-999として登録(どの端末でもあり得ないので)
                    rssis[i] = -999;
                }
            }

            string result = roomName + " ";
            for (int i = 0; i < rssis.Length; i++)
            {
                if (rssis[i] == -999)
                {
                    continue;
                }
                result += (i + 1) + ":" + rssis[i] + " ";
            }
            return result;
        }

        public static string RoomConv(string roomNum)
        {
            string result = "";
            roomNum = ((int)double.Parse(roomNum)).ToString();
            for (int i = 0; i < ShareData.rooms.Count; i++)
            {
                if (roomNum == ShareData.rooms[i].getNum())
                {
                    result = ShareData.rooms[i].getRoomName();
                    break;
                }
            }

            return result;
        }

    }
}
