using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace Library1
{
    public partial class EntityModelContainer : DbContext
    {
        public EntityModelContainer() : base("name=EntityModelContainer") { }
        public virtual DbSet<User> UserSet { get; set; }
        public virtual DbSet<Account> AccountSet { get; set; }
        public virtual DbSet<Branch> BranchSet { get; set; }
        public virtual DbSet<Section> SectionSet { get; set; }
        public virtual DbSet<Message> MessageSet { get; set; }
    }
}
