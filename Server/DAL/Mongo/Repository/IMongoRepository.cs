using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.DAL.Mongo.Repository
{
    public interface IMongoRepository
    {
        string GetNewFileName(string fileExtension);
        Task<string> UploadFileAsync(FileStream fileStream, string fileName);
        Task DownloadFileByNameAsync(string id, string fullFileName);
        Task DeleteFileAsync(string id);
        Task ChangeFileAsync(string id, FileStream newFileStream, string newFileName);
    }
}
