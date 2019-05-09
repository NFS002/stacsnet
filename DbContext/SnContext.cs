using Microsoft.EntityFrameworkCore;
using stacsnet.Models;
using stacsnet.Util;
using System;

namespace stacsnet
{
    public class SnContext : DbContext
    {
       
        public DbSet<QAPost> QAPosts {get; set;}

        public DbSet<Account> Accounts { get; set; }

        public DbSet<GradeReport> GradeReports  { get; set; }
        /*public QAContext(): base("./Db/QAPosts.sqlite") {
            Database.SetInitializer<DataContext>(new CreateDatabaseIfModelChanges<DataContext>());
            Database.SetInitializer<DataContext>(new CreateDatabaseIfNotExists<DataContext>());                                                            
            Database.SetInitializer<DataContext>(new DropCreateDatabaseAlways<DataContext>());
        }*/

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite(@"Data Source="  + Static.DB_PATH );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<QAPost>()
            .Property(q => q.uname)
            .HasDefaultValue("anon");
        }
    }
}