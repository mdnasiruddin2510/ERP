using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interface.Service.Common
{
    public interface IUtilityService
    {
        string DateTimeFormat(string datetime, bool tDate, int formatnumber = 0);
    }
}
