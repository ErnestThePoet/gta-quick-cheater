using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAQuickCheater.Entities
{
    internal record CheatItem(string keys, string code, string description);
    internal record CheatSet(string name, List<CheatItem> cheats);
}
