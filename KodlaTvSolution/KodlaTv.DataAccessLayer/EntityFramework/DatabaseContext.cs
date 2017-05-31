using KodlaTv.DataAccessLayer.Migrations;
using KodlaTv.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KodlaTv.DataAccessLayer.EntityFramework
{
    public class DatabaseContext:DbContext
    {

        public DatabaseContext() :base("DatabaseContext")
            {
     
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Configuration>("DatabaseContext"));
              
        }

    public DbSet<KodlatvUser> KodlatvUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<WebsiteInfo> WebsiteInfos { get; set; }
        public DbSet<StreamerInfo> StreamerInfos { get; set; }
        public DbSet<SendMessage> SendMessages { get; set; }
        public DbSet<Chat> Chats { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)

            {

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);

            }

}
}
