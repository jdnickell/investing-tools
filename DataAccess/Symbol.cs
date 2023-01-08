using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Symbol
    {
        public Symbol()
        {
            ExtendedHoursBiggestMovers = new HashSet<ExtendedHoursBiggestMovers>();
            SymbolSource = new HashSet<SymbolSource>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol1 { get; set; }
        public int SymbolTypeId { get; set; }

        public virtual ICollection<ExtendedHoursBiggestMovers> ExtendedHoursBiggestMovers { get; set; }
        public virtual ICollection<SymbolSource> SymbolSource { get; set; }
    }
}
