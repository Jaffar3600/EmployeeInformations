using System;
using System.Web;
using System.Web.Mvc;
using EmpDetails;
using EmployeeInformation.Models;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace EmployeeInformation.Controllers
{
    public class EmployeeController : Controller
    {
        EmpBlobOperations blobOperations;
        EmpTableOperations tableOperations;

        public EmployeeController()
        {
            blobOperations = new EmpBlobOperations();
            tableOperations = new EmpTableOperations();
        }
        // GET: Employee
        public ActionResult Index()
        {
            var details = tableOperations.GetEntities();
            return View(details);
        }
        public ActionResult Create()
        {
            var Details = new EmpDetailsClass();
            Details.RowKey = new Random().Next().ToString(); //Generate the Employee Id Randomly
            return View(Details);
        }

        public ActionResult Delete(EmpDetailsClass obj)
        {
            var delete = tableOperations.DeleteEntity(obj);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(EmpDetailsClass obj)
        {
           
            return View();
        }

        public ActionResult Update(EmpDetailsClass obj)
        {
            var update = tableOperations.UpdateEntity(obj);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EmpDetailsClass obj, HttpPostedFileBase profileFile)
        {
            CloudBlockBlob profileBlob = null;
            #region Upload File In Blob Storage
            //Step 1: Uploaded File in BLob Storage
            if (profileFile != null && profileFile.ContentLength != 0)
            {
                profileBlob = await blobOperations.UploadBlob(profileFile, obj.RowKey);
                obj.ProfileImage = profileBlob.Uri.ToString();
            }
            //Ends Here 
            #endregion

            #region Save Information in Table Storage
            //Step 2: Save the Information in the Table Storage
            tableOperations.CreateEntity(obj);
            //Ends Here 
            #endregion

            return RedirectToAction("Index");
        }
    }
}
