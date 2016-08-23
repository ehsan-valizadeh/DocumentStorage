using System.Data.Entity;

namespace DocumentStorage.EfProvider
{
    public class DocumentContext : DbContext
    {
        public DocumentContext() : base("DocumentDb")
        {
            Database.SetInitializer(new SeedAndDropCreateDatabaseAlways<DocumentContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}