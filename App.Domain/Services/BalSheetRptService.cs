﻿using App.Domain.Interface.Repository;
using App.Domain.Interface.Service;
using App.Domain.Services.Common;
using App.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Services
{
    public class BalSheetRptService : Service<BalSheetRptVM>, IBalSheetRptService
    {
        public BalSheetRptService(IBalSheetRptRepository repository)
            : base(repository)
        {

        }
    }
}
