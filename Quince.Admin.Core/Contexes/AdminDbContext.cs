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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quince.Admin.Core.Contracts.Attribute>()
                        .Map<EntityAttribute>(m => m.Requires("Type").HasValue(1))
                        .Map<RelationAttribute>(m => m.Requires("Type").HasValue(2));

            modelBuilder.Entity<Quince.Admin.Core.Contracts.Reference>()
                        .Map<EntityReference>(m => m.Requires("Type").HasValue(1))
                        .Map<RelationReference>(m => m.Requires("Type").HasValue(2));
        }
        public static AdminDbContext Create()
        {
            return new AdminDbContext();
        }
    }
}
