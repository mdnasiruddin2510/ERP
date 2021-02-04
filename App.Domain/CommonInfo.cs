using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain
{
    public static class CommonInfo
    {
        public class Month
        {
            public static Dictionary<int, string> GetMonth()
            {
                Dictionary<int, string> months = new Dictionary<int, string>();
                months.Add(1, "January");
                months.Add(2, "February");
                months.Add(3, "March");
                months.Add(4, "April");
                months.Add(5, "May");
                months.Add(6, "June");
                months.Add(7, "July");
                months.Add(8, "August");
                months.Add(9, "September");
                months.Add(10, "October");
                months.Add(11, "November");
                months.Add(12, "December");
                return months;
            }
        }

    }
}
