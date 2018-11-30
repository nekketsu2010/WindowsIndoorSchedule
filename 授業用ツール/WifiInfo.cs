using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 授業用ツール
{
    class WifiInfo
    {
        private List<string> SSIDs = new List<string>();
        private List<string> BSSIDs = new List<string>();
        private List<int> RSSIs = new List<int>();


        public int Length() { return SSIDs.Count; }
        public List<string> getSSIDs() { return SSIDs; }
        public List<string> getBSSIDs() { return BSSIDs; }
        public List<int> getRSSIs() { return RSSIs; }
        public string getSSID(int i) { return SSIDs[i]; }
        public string getBSSID(int i) { return BSSIDs[i]; }
        public int getRSSI(int i) { return RSSIs[i]; }


        public void setSSID(string ssid) { SSIDs.Add(ssid); }
        public void setBSSID(string bssid) { BSSIDs.Add(bssid); }
        public void setRSSI(int rssi) { RSSIs.Add(rssi); }

    }
}
