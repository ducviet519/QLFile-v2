using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using WebTools.Services.Interface;

namespace WebTools.Services.Repositories
{
    public class GoogleDriverV2 : IGoogleDriverV2
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GoogleDriverV2(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;

        }

        private string[] Scopes = { DriveService.Scope.Drive };
        private string ApplicationName = "VanBanNoiBo-BVTA";
        public string PathToServiceAccountKeyFile { get; private set; }
        public string p12Path { get; private set; }
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

        public async Task<IList<Comment>> GetDriverComments(string fileId)
        {
            DriveService service = GetDriveService();
            try
            {
                CommentList comments = await service.Comments.List(fileId).ExecuteAsync();
                return comments.Items;
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
    }
}
