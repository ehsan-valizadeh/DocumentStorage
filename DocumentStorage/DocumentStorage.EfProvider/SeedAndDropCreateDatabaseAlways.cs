using System.Data.Entity;
using DocumentStorage.Core;

namespace DocumentStorage.EfProvider
{
    public class SeedAndDropCreateDatabaseAlways<TContext> : DropCreateDatabaseAlways<TContext> where TContext : DbContext
    {
        public override void InitializeDatabase(TContext context)
        {
            if (context.Database.Exists())
            {
                context.Database.ExecuteSqlCommand(
                    TransactionalBehavior.DoNotEnsureTransaction, $"ALTER DATABASE [{context.Database.Connection.Database}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
            }

            base.InitializeDatabase(context);
        }

        protected override void Seed(TContext context)
        {
            var document = new Document
            {
                Name = "Test1",
                ContentType = "image/png",
                Size = 0,
                BlobName = ""
            };

            context.Set<Document>().Add(document);

            base.Seed(context);
        }
    }
}