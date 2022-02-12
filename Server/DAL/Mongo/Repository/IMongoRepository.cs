using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.DAL.Mongo.Repository
{
    public interface IMongoRepository
    {
        // Read api
        string GetNewFileName(string fileExtension);
        Task<byte[]> GetFileBytesByIdAsync(string mongoId);

        // Upload api
        Task<string> UploadFileAsync(Stream fileStream, string fileName);
        
        // Download api
        Task<Stream> DownloadFileByIdAsync(string mongoId);

        // Delete api
        Task DeleteFileAsync(string mongoId);

        // Update api
        Task<string> ChangeFileAsync(string mongoId, Stream newFileStream, string newFileName);
        
    }
}
