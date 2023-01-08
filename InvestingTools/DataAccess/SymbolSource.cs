using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class SymbolSource
    {
        public int Id { get; set; }
        public int SymbolId { get; set; }
        public int SourceId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Source Source { get; set; }
        public virtual Symbol Symbol { get; set; }
    }
}
