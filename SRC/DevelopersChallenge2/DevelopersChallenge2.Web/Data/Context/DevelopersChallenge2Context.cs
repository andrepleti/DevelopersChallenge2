using DevelopersChallenge2.Web.Data.Context.Mappers;
using DevelopersChallenge2.Web.Migrations;
using DevelopersChallenge2.Web.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DevelopersChallenge2.Web.Data.Context
{
    public class DevelopersChallenge2Context : DbContext
    {
        public DevelopersChallenge2Context()
            : base(typeof(DevelopersChallenge2Context).Name)
        {
            InitializerContext();
        }

        private void InitializerContext()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<DevelopersChallenge2Context>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DevelopersChallenge2Context, Configuration>());

            Configuration.ValidateOnSaveEnabled = false;
        }

        public DbSet<TransactionBank> TransactionsBank { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new TransactionBankMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}