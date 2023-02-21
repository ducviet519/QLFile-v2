using Google.Apis.Docs.v1.Data;
using Google.Apis.Drive.v3.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebTools.Models.Entities;

namespace WebTools.Services.Interface
{
    public interface IGoogleDriveAPI
    {
        string UploadFile(string path);
        void DownloadFile(string blobId, string savePath);
        Task<List<GoogleDriveFile>> GetDriveFiles();
        Task<List<GoogleDriveFile>> SearchDriveFiles(string searchString);
        Task<Document> CreateDocumentFile(string doctitle, string email, string folderId = null);
        Task<IList<string>> DriveMoveFileToFolder(string fileId, string folderId);
        IList<string> DriveShareFile(string realFileId, string email);
    }
}
