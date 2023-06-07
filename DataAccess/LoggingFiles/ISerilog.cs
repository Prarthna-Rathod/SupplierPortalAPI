using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.LoggingFiles
{
    public interface ISerilog
    {
        void PushSerilog(IEnumerable<EntityEntry> entityEntries);
    }
}
