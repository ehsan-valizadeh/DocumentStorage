using System;

namespace DocumentStorage.Core
{
    public class DocumentArchive
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ContentType { get; set; }

        public long Size { get; set; }

        public string OrignalBlobName { get; set; }

        public string ArchivedBlobName { get; set; }

        public bool IsArchived { get; set; }

        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
    }
}