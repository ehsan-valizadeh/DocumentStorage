using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DocumentStorage.Core;

namespace DocumentStorage.EfProvider.Maps
{
    internal class DocumentArchiveMap : EntityTypeConfiguration<DocumentArchive>
    {
        public DocumentArchiveMap()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}