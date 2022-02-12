using JL.PersistModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Utility2L.Abstraction
{
    public interface IFileUtility
    {
        Task<int> CreateNewFileAsync(Stream fileStream, string originalFileName, string fileExtension);
        Task<int> UpdateFileAsync(Stream fileStream, string mongoId, string originalFileName, string fileExtension);
        Task<byte[]> GetFileAsBytesById(string mongoId);
        FileData GetFileData(int fileId);
    }
}
