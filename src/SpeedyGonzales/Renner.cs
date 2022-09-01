using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyGonzales
{

    public record Renner(
        bool IsLeader,
        Team Team)
    {
        public override string ToString()
            => IsLeader
            ? Team.Naam.ToUpperInvariant()
            : Team.Naam.ToLowerInvariant();
    }
}
