using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace DocumentStorage.AzureStorageProvider
{
    /// <summary>
    /// Implementation of <see cref="IAzureStorageProvider"/>.
    /// </summary>
    public class AzureStorageProvider : IAzureStorageProvider
    {
        private readonly CloudBlobContainer _rootContainer;

        #region Constructor

        /// <summary>
        /// Initializes new instance of <see cref="AzureStorageProvider"/>
        /// </summary>
        /// <param name="accountName">Azure account name.</param>
        /// <param name="apiKey">Azure API  key.</param>
        /// <param name="rootContainerName">Root container name. 
        /// <remarks>This should be company name and it must be in lowercase.</remarks>
        /// </param>
        public AzureStorageProvider(string accountName, string apiKey, string rootContainerName)
        {
            var account = new CloudStorageAccount(new StorageCredentials(accountName, apiKey), true);
            var client = account.CreateCloudBlobClient();

            _rootContainer = client.GetContainerReference(rootContainerName);
            _rootContainer.CreateIfNotExists();
        }

        #endregion

        #region IAzureProvider

        /// <summary>
        /// <see cref="IAzureStorageProvider.UploadAsync"/>.
        /// </summary>
        public async Task<bool> UploadAsync(string blobReference, string contentType, Stream stream)
        {
            try
            {
                var blob = _rootContainer.GetBlockBlobReference(blobReference);
                blob.Properties.ContentType = contentType;

                await blob.UploadFromStreamAsync(stream).ConfigureAwait(false);
            }
            catch
            {
                // TODO: Move Logging from Service project, so it is reachable here.
                return false;
            }

            return true;
        }

        /// <summary>
        /// <see cref="IAzureStorageProvider.GetUrl"/>.
        /// </summary>
        public string GetUrl(string blobReference)
        {
            var blob = _rootContainer.GetBlockBlobReference(blobReference);

            var query = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.List,
                SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(1)
            });

            return blob.Uri + query;
        }

        /// <summary>
        /// <see cref="IAzureStorageProvider.RenameAsync"/>.
        /// </summary>
        public async Task<bool> RenameAsync(string blobReference)
        {
            var deletedReference = $"deleted/{blobReference}";

            try
            {
                var blob = _rootContainer.GetBlockBlobReference(blobReference);

                if (!blob.Exists())
                {
                    throw new ArgumentException($"Blob: {blobReference} doesn't exist.");
                }

                var deletedBlob = _rootContainer.GetBlockBlobReference(deletedReference);

                await deletedBlob.StartCopyAsync(blob);
                await DeleteAsync(blobReference);

                return true;
            }
            catch
            {
                // TODO: Move Logging from Service project, so it is reachable here.
                return false;
            }
        }

        /// <summary>
        /// <see cref="IAzureStorageProvider.DeleteAsync"/>.
        /// </summary>
        public async Task<bool> DeleteAsync(string blobReference)
        {
            try
            {
                var blob = _rootContainer.GetBlockBlobReference(blobReference);

                if (!blob.Exists())
                {
                    throw new ArgumentException($"Blob: {blobReference} doesn't exist.");
                }

                await blob.DeleteAsync();

                return true;
            }
            catch
            {
                // TODO: Move Logging from Service project, so it is reachable here.
                return false;
            }
        }

        #endregion
    }
}