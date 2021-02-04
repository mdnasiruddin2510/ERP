using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Interface.Service.Common;

namespace App.Domain.Services.Common
{
   public class UtilityService : IUtilityService
    {
       public string DateTimeFormat(string datetime, bool tDate, int formatnumber = 0)
       {
           string time = "";
           if (tDate)
           {
               time = " 23:59:00";
           }
           if (formatnumber == 1)
           {

               string[] st = new string[3];
               st = datetime.Split('/');
               return st[2] + "-" + st[1] + "-" + st[0] + time;
           }
           else if (formatnumber == 2)
           {
               string[] st = new string[3];
               st = datetime.Split('/');
               return st[1] + "-" + st[0] + "-" + st[2] + time;
           }
           else if (formatnumber == 3)
           {
               string[] st = new string[3];
               st = datetime.Split('/');
               return st[0] + "-" + st[1] + "-" + st[2] + time;
           }
           else
           {
               return datetime + time;
           }
       }
    }
}
