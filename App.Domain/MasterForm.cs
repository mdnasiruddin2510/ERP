﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace App.Domain
{  
    public class MasterForm
    {
        [Key]
        public int FormID { get; set; }
        public string FormName { get; set; }
    }

}
