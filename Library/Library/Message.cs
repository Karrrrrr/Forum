using System;
using System.Collections.Generic;
using System.Text;

namespace Library1
{
    [Serializable]
    public partial class Message
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public System.DateTime CreationDate { get; set; }
        public int OwnerID { get; set; }
        public int BranchID { get; set; }
        public virtual Account Account { get; set; }
        public virtual Branch Branch { get; set; }
    }
}
