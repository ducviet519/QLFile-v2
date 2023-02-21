using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Docs.v1.Data;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Requests;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using WebTools.Models.Entities;
using WebTools.Services.Interface;

namespace WebTools.Services
{
    public class GoogleDriveAPI : IGoogleDriveAPI
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GoogleDriveAPI(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;

        }

        private string[] Scopes = { DriveService.Scope.Drive };
        private string ApplicationName = "VanBanNoiBo-BVTA";
        public string PathToServiceAccountKeyFile { get; private set; }
        public string p12Path { get; private set; }

        #region Oauth 2.0
        private DriveService GetDriveServiceOauth2()
        {
            string JsonPathFile = Path.Combine(_webHostEnvironment.ContentRootPath, "credentials.json");
            string[] Scopes = { DriveService.Scope.Drive };
            UserCredential credential;
            using (var stream = new FileStream(JsonPathFile, FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = Path.Combine(_webHostEnvironment.WebRootPath, "\\keys\\token.json"); // Sau khi cấp quyền xong thì sẽ lưu token ở đây
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                             GoogleClientSecrets.Load(stream).Secrets,
                             Scopes,
                             "user",
                             CancellationToken.None,
                             new FileDataStore(credPath, true)).Result;
            }
            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Google Drive Api v3",
            });

            return service;
        }

        public void DownloadFile(string blobId, string savePath)
        {
            var service = GetDriveServiceOauth2();
            var request = service.Files.Get(blobId);
            var stream = new MemoryStream();
            // Add a handler which will be notified on progress changes.
            // It will notify on each chunk download and when the
            // download is completed or failed.
            request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case Google.Apis.Download.DownloadStatus.Downloading:
                        {
                            Console.WriteLine(progress.BytesDownloaded);
                            break;
                        }
                    case Google.Apis.Download.DownloadStatus.Completed:
                        {
                            Console.WriteLine("Download complete.");
                            SaveStream(stream, savePath);
                            break;
                        }
                    case Google.Apis.Download.DownloadStatus.Failed:
                        {
                            Console.WriteLine("Download failed.");
                            break;
                        }
                }
            };
            request.Download(stream);
        }

        public string UploadFile(string path)
        {
            var service = GetDriveServiceOauth2();
            var fileMetadata = new Google.Apis.Drive.v3.Data.File();
            fileMetadata.Name = Path.GetFileName(path);
            fileMetadata.MimeType = MimeTypes.GetMimeType(path);
            FilesResource.CreateMediaUpload request;
            using (var stream = new FileStream(path, FileMode.Open))
            {
                request = service.Files.Create(fileMetadata, stream, MimeTypes.GetMimeType(path));
                request.Fields = "id";
                request.Upload();
            }

            var file = request.ResponseBody;

            return file.Id;
        }

        private static void SaveStream(MemoryStream stream, string saveTo)
        {
            using (FileStream file = new FileStream(saveTo, FileMode.Create, FileAccess.Write))
            {
                stream.WriteTo(file);
            }
        }
        #endregion

        #region Google Drive API

        private DriveService GetDriveService()
        {
            string[] scopes = new string[] { DriveService.Scope.Drive }; // Full access
            p12Path = Path.Combine(_webHostEnvironment.WebRootPath, "keys\\google-service-accounts-key.p12");
            var serviceAccountEmail = "vb-bvta-865@vanbannoibo-bvta.iam.gserviceaccount.com";
            var certificate = new X509Certificate2(p12Path, "notasecret", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);
            try
            {
                ServiceAccountCredential credential = new ServiceAccountCredential(
               new ServiceAccountCredential.Initializer(serviceAccountEmail)
               {
                   Scopes = scopes
               }.FromCertificate(certificate));

                DriveService service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });
                return service;
            }
            catch (Exception e)
            {
                // TODO(developer) - handle error appropriately
                if (e is AggregateException)
                {
                    Console.WriteLine("Credential Not found");
                }
                else if (e is GoogleApiException)
                {
                    Console.WriteLine("File or Folder not found");
                }
                else
                {
                    throw;
                }
            }
            return null;
        }

        //get all files from Google Drive.
        public async Task<List<GoogleDriveFile>> GetDriveFiles()
        {
            DriveService service = GetDriveService();
            // Define parameters of request.
            FilesResource.ListRequest FileListRequest = service.Files.List();

            // for getting folders only.
            //FileListRequest.Q = "mimeType='application/vnd.google-apps.folder'";
            FileListRequest.Fields = "nextPageToken, files(*)";
            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = (await FileListRequest.ExecuteAsync()).Files;
            List<GoogleDriveFile> FileList = new List<GoogleDriveFile>();
            // For getting only folders
            // files = files.Where(x => x.MimeType == "application/vnd.google-apps.folder").ToList();
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    GoogleDriveFile File = new GoogleDriveFile
                    {
                        Id = file.Id,
                        Name = file.Name,
                        Size = file.Size,
                        Version = file.Version,
                        CreatedTime = file.CreatedTime,
                        Parents = file.Parents,
                        MimeType = file.MimeType,
                        FileName = file.Name
                    };
                    FileList.Add(File);
                }
            }
            return FileList;
        }

        //get all files from Google Drive.
        public async Task<List<GoogleDriveFile>> SearchDriveFiles(string searchString)
        {
            DriveService service = GetDriveService();

            // Define parameters of request.
            FilesResource.ListRequest FileListRequest = service.Files.List();

            //FileListRequest.Q = $"(fullText contains '{searchString}') or (mimeType = 'application/vnd.google-apps.document')";
            FileListRequest.Q = $"(fullText contains '{searchString}')";
            // List files.
            var files2 = await FileListRequest.ExecuteAsync();
            IList<Google.Apis.Drive.v3.Data.File> files = (await FileListRequest.ExecuteAsync()).Files;
            List<GoogleDriveFile> FileList = new List<GoogleDriveFile>();

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    GoogleDriveFile File = new GoogleDriveFile
                    {
                        Id = file.Id,
                        Name = file.Name,
                        Size = file.Size,
                        Version = file.Version,
                        CreatedTime = file.CreatedTime,
                        Parents = file.Parents,
                        MimeType = file.MimeType,
                        FileName = file.Name
                    };
                    FileList.Add(File);
                }
            }
            return FileList;
        }

        //move file to folder
        public async Task<IList<string>> DriveMoveFileToFolder(string fileId, string folderId)
        {
            DriveService service = GetDriveService();

            // Retrieve the existing parents to remove
            var getRequest = service.Files.Get(fileId);
            getRequest.Fields = "parents";
            var file = await getRequest.ExecuteAsync();
            var previousParents = String.Join(",", file.Parents);
            // Move the file to the new folder
            var updateRequest =
                service.Files.Update(new Google.Apis.Drive.v3.Data.File(),
                    fileId);
            updateRequest.Fields = "id, parents";
            updateRequest.AddParents = folderId;
            updateRequest.RemoveParents = previousParents;
            file = await updateRequest.ExecuteAsync();

            return file.Parents;
        }

        public IList<string> DriveShareFile(string realFileId, string email)
        {
            DriveService service = GetDriveService();

            var ids = new List<String>();
            var batch = new BatchRequest(service);
            BatchRequest.OnResponse<Permission> callback = delegate (
                Permission permission,
                RequestError error,
                int index,
                HttpResponseMessage message)
            {
                if (error != null)
                {
                        // Handle error
                        Console.WriteLine(error.Message);
                }
                else
                {
                    Console.WriteLine("Permission ID: " + permission.Id);
                }
            };
            Permission userPermission = new Permission()
            {
                Type = "user", //user, group, domain, or anyone
                Role = "writer", //writer, reader, owner
                EmailAddress = email
            };

            var request = service.Permissions.Create(userPermission, realFileId);
            request.Fields = "id";
            batch.Queue(request, callback);

            var task = batch.ExecuteAsync();
            task.Wait();
            return ids;
        }

        #endregion

        #region Google Docs API
        private DocsService GetDocsService()
        {
            //string GoogleCredentialsFileName = "google-credentials.json";
            //var stream = new FileStream(GoogleCredentialsFileName, FileMode.Open, FileAccess.Read);

            string[] scopes = new string[] { DocsService.Scope.Drive }; // Full access
            p12Path = Path.Combine(_webHostEnvironment.WebRootPath, "keys\\google-service-accounts-key.p12");
            var serviceAccountEmail = "vb-bvta-865@vanbannoibo-bvta.iam.gserviceaccount.com";
            var certificate = new X509Certificate2(p12Path, "notasecret", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);

            try
            {
                ServiceAccountCredential credential = new ServiceAccountCredential(
                       new ServiceAccountCredential.Initializer(serviceAccountEmail)
                       {
                           Scopes = scopes
                       }.FromCertificate(certificate));

                DocsService service = new DocsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    //HttpClientInitializer = GoogleCredential.FromStream(stream).CreateScoped(Scopes),
                    ApplicationName = ApplicationName
                });
                return service;
            }
            catch (Exception e)
            {
                // TODO(developer) - handle error appropriately
                if (e is AggregateException)
                {
                    Console.WriteLine("Credential Not found");
                }
                else if (e is GoogleApiException)
                {
                    Console.WriteLine("File or Folder not found");
                }
                else
                {
                    throw;
                }
            }
            return null;
        }

        public async Task<Document> CreateDocumentFile(string doctitle, string email, string folderId = null)
        {
            DocsService service = GetDocsService();
            Document doc = new Document()
            {
                Title = doctitle,
            };
            doc = await service.Documents.Create(doc).ExecuteAsync();

            //default folder move: https://drive.google.com/open?id=1weiHxt8GBuFeaA1zWWYsiVQFcb3FogVn
            if (String.IsNullOrEmpty(folderId)) { folderId = "1weiHxt8GBuFeaA1zWWYsiVQFcb3FogVn"; }
            //move file to folder
            var file = await DriveMoveFileToFolder(doc.DocumentId, folderId);
            var share = DriveShareFile(doc.DocumentId, email);

            return doc;
        }
        #endregion
    }
}
