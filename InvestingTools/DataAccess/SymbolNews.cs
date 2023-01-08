using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class SymbolNews
    {
        public SymbolNews()
        {
            InverseSymbol = new HashSet<SymbolNews>();
        }

        public int Id { get; set; }
        public int SymbolId { get; set; }
        public string PublisherName { get; set; }
        public string PublisherUrl { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Author { get; set; }
        public DateTime? PublishedDateTime { get; set; }
        public string Url { get; set; }
        public string RelevantSymbolsCsv { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public virtual SymbolNews Symbol { get; set; }
        public virtual ICollection<SymbolNews> InverseSymbol { get; set; }
    }
}
