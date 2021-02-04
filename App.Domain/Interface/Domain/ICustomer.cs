using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interface.Domain
{
    public interface ICustomer : ICommonProperty
    {
        string CustName { get; set; }
        string MobileNo { get; set; }
       
    }
}
