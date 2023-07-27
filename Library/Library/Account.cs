using System;
using System.Collections.Generic;
using System.Text;

namespace Library1
{
    [Serializable]
    public partial class Account
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Picture { get; set; }
        public int NumberOfMessages { get; set; }
        public string Gender { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Branch> Branch { get; set; }
        public virtual ICollection<Message> Message { get; set; }
    }
}
