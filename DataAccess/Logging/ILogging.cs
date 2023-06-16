using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Logging
{
    public interface ILogging
    {
        void PushSerilog(IEnumerable<EntityEntry> entityEntries);
    }
}
