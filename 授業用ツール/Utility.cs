using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 授業用ツール
{
    class Utility
    {
        public static string[] Upload()
        {
            string pass = "";
            string fileName = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult dr = openFileDialog.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                pass = openFileDialog.FileName;
                fileName = openFileDialog.SafeFileName;
            }
            return new string[] { pass, fileName };
        }

        public static DateTime GetNearestDayOfWeek(DateTime day, DayOfWeek wantDayOfWeek)
        {
            DayOfWeek dayOfWeek = day.DayOfWeek;

            if (dayOfWeek == wantDayOfWeek)
            {
                return day;
            }
            else
            {
                return day.AddDays(
                    ((int)(DayOfWeek.Saturday - day.DayOfWeek + wantDayOfWeek) % 7) + 1);
            }
        }

        public static DateTime[] createCalendar(int beginHour, int beginMinute, int endHour, int endMinute)
        {
            DateTime beginCal = DateTime.Now;
            DateTime endCal = DateTime.Now;
            beginCal.set(DateTime.HOUR_OF_DAY, beginHour);
            beginCal.set(DateTime.MINUTE, beginMinute);
            endCal.set(DateTime.HOUR_OF_DAY, endHour);
            endCal.set(DateTime.MINUTE, endMinute);
            return new DateTime[] { beginCal, endCal };
        }
    }
}
