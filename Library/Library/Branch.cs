using System;
using System.Collections.Generic;
using System.Text;

namespace Library1
{
    [Serializable]
    public partial class Branch
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int NumberOfMessages { get; set; }
        public string AccessLevel { get; set; }
        public string Description { get; set; }
        public int OwnerID { get; set; }
        public int SectionID { get; set; }
        public virtual Account Account { get; set; }
        public virtual Section Section { get; set; }
        public virtual ICollection<Message> Message { get; set; }
    }
}
