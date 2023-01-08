using System;
using System.Collections.Generic;

namespace DataAccess
{
    public partial class RecommendationType
    {
        public RecommendationType()
        {
            Recommendation = new HashSet<Recommendation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Recommendation> Recommendation { get; set; }
    }
}
