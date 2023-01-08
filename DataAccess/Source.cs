using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class Source
    {
        public Source()
        {
            SymbolSource = new HashSet<SymbolSource>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ApiKey { get; set; }
        public bool IsSandbox { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<SymbolSource> SymbolSource { get; set; }
    }
}
