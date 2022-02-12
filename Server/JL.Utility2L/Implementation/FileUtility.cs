using JL.DAL.Mongo.Repository;
using JL.DAL.Repository.Abstraction;
using JL.PersistModels;
using JL.Utility2L.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL.Utility2L.Implementation
{
    public class FileUtility : IFileUtility
    {
        private readonly IMongoRepository _mongoRepository;
        private readonly IFileDataRepository _fileDataRepository;

        public FileUtility(IMongoRepository mongoRepository,
                           IFileDataRepository fileDataRepository)
        {
            _mongoRepository = mongoRepository ?? throw new NullReferenceException(nameof(mongoRepository));
            _fileDataRepository = fileDataRepository ?? throw new NullReferenceException(nameof(fileDataRepository));
        }

        public async Task<int> CreateNewFileAsync(Stream fileStream, string originalFileName, string fileExtension)
        {
            string mongoName = _mongoRepository.GetNewFileName(fileExtension);

            var file = new FileData();
            file.MongoName = mongoName;
            file.OriginalName = originalFileName;

            var mongoId = await _mongoRepository.UploadFileAsync(fileStream, mongoName);
            file.MongoId = mongoId;

            file = _fileDataRepository.Insert(file);
            _fileDataRepository.SaveChanges();

            return file.Id;
        }

        public async Task<int> UpdateFileAsync(Stream fileStream, string mongoId, string originalFileName, string fileExtension)
        {
            string mongoName = _mongoRepository.GetNewFileName(fileExtension);
            
            var updatedFile = new FileData();
            updatedFile.MongoName = mongoName;
            updatedFile.OriginalName = originalFileName;

            var newMongoId = await _mongoRepository.ChangeFileAsync(mongoId, fileStream, mongoName);
            updatedFile.MongoId = newMongoId;

            updatedFile = _fileDataRepository.Insert(updatedFile);
            _fileDataRepository.SaveChanges();

            return updatedFile.Id;
        }

        public async Task<byte[]> GetFileAsBytesById(string mongoId)
        {
            return await _mongoRepository.GetFileBytesByIdAsync(mongoId);
        }

        public FileData GetFileData(int fileId)
        {
            return _fileDataRepository.GetById(fileId) ?? throw new NullReferenceException();
        }
    }
}
