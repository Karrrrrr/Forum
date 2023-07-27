using System;
using System.Collections.Generic;
using System.Text;

namespace Library1
{
    [Serializable]
    public partial class Section
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AccessLevel { get; set; }
        public virtual ICollection<Branch> Branch { get; set; }
    }
}
