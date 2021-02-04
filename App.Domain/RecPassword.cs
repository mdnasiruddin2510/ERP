using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Domain
{
    public class RecPassword
    {
        public int Id { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public DateTime MaxValidTime { set; get; }
        public string ValidationToken { set; get; }
    }
}
