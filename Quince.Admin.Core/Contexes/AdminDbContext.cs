using Quince.Admin.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Contexes
{
    public class AdminDbContext : DbContext
    {
        public AdminDbContext()
            : base("DefaultConnection")
        {
            // remove default initializer
            Database.SetInitializer<AdminDbContext>(null);
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }
        public DbSet<User> Users { set; get;}
        public DbSet<EntityType> EntityTypes { set; get; }
        public DbSet<Entity> Entities { set; get; }
        public DbSet<RelationType> RelationTypes { set; get; }
        public DbSet<Relation> Relations { set; get; }

        public static AdminDbContext Create()
        {
            return new AdminDbContext();
        }
    }
}
