using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataActions.Interfaces
{
    public interface ISerilog
    {
        void LogPush(IEnumerable<EntityEntry> entityEntries);
    }
}
