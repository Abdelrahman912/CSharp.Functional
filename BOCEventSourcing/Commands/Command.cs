using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOC.Core.Commands
{
    public abstract record Command
    {
        public DateTime TimeStamp { get; init; }

    }
}
