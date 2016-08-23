using System;
using System.Linq;
using DocumentStorage.Core;
using DocumentStorage.EfProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocumentStorage.Test
{
    [TestClass]
    public class DocumentContextTest
    {
        [TestMethod]
        public void ShouldCreateDatabaseAndSeed()
        {
            using (var context = new DocumentContext())
            {
                var document = context.Set<Document>().FirstOrDefault(x => x.Name == "Test1");

                Assert.IsNotNull(document);
            }
        }
    }
}
