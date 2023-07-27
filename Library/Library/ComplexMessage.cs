using System;
using System.Collections.Generic;
using System.Text;

namespace Library1
{
    [Serializable]
    public class ComplexMessage
    {
        public Mess First { get; set; }
        public Mess Second { get; set; }
        public int NumberStatus { get; set; }
    }
}
