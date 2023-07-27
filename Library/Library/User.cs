using System;

namespace Library1
{
    [Serializable]
    public partial class User
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public virtual Account Account { get; set; }
    }
}
