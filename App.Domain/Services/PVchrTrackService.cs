using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using App.Domain.Interface.Repository;
using App.Domain.Interface.Service;
using App.Domain.Services.Common;

namespace App.Domain.Services
{
    public class PVchrTrackService : Service<PVchrTrack>, IPVchrTrackService
    {
        public PVchrTrackService(IPVchrTrackRepository repository)
            : base(repository)
        {

        }
    }
}