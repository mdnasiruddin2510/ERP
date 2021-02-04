using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interface.Domain
{
   public interface ICommonProperty 
    {
        string EntryUser { get; set; }
        string EntryDate { get; set; }
    }
}
