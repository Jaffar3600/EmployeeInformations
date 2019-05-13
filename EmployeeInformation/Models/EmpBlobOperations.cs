using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using System.Threading.Tasks;
using System.IO;
using System;

namespace EmployeeInformation.Models
{
    public class EmpBlobOperations
    {
        private static CloudBlobContainer profileBlobContainer;
        // Initialize BLOB and Queue Here
        public EmpBlobOperations()
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["empstorage"].ToString());
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            // Get the blob container reference.
            profileBlobContainer = blobClient.GetContainerReference("employees");
            //Create Blob Container if not exist
            profileBlobContainer.CreateIfNotExists();
            profileBlobContainer.CreateIfNotExistsAsync();
            profileBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

        }

        // Method to Upload the BLOB
        public async Task<CloudBlockBlob> UploadBlob(HttpPostedFileBase profileImage, String EmpId)
        {
            string blobName = EmpId + Path.GetExtension(profileImage.FileName);
            // GET a blob reference. 
            CloudBlockBlob profileBlob = profileBlobContainer.GetBlockBlobReference(blobName);
            // Uploading a local file and Create the blob.
            using (var fs = profileImage.InputStream)
            {
                await profileBlob.UploadFromStreamAsync(fs);
            }
            return profileBlob;
        }
    }
}
