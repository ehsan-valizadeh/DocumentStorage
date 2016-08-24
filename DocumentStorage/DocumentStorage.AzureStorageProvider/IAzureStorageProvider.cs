using System.IO;
using System.Threading.Tasks;

namespace DocumentStorage.AzureStorageProvider
{
    public interface IAzureStorageProvider
    {
        /// <summary>
        /// Uploads a stream to Azure Blob Storage.
        /// </summary>
        /// <param name="blobReference">Blob name.</param>
        /// <param name="contentType">MIME type.</param>
        /// <param name="stream">File content.</param>
        Task<bool> UploadAsync(string blobReference, string contentType, Stream stream);

        /// <summary>
        /// Returns public URL to specified blob.
        /// </summary>
        /// <param name="blobReference">Blob name.</param>
        /// <returns>Returns file URL.</returns>
        string GetUrl(string blobReference);

        /// <summary>
        /// Moves file to 'delete' folder.
        /// <remarks>This is an Azure folder, so it is a folder representation not a real folder.</remarks>
        /// <exception cref="System.ArgumentException">If blob doesn't exist this exception will be thrown.</exception>
        /// </summary>
        /// <param name="blobReference">Blob name.</param>
        Task<bool> RenameAsync(string blobReference);

        /// <summary>
        /// Removes a blob from Azure Blob Storage.
        /// <exception cref="System.ArgumentException">If blob doesn't exist this exception will be thrown.</exception>
        /// </summary>
        /// <param name="blobReference">Blob name.</param>
        Task<bool> DeleteAsync(string blobReference);
    }
}