using System.Collections.Generic;
using System.Configuration;
using System.IO;
using EmpDetails;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace EmployeeInformation.Models
{
    public interface IEmpTableOperations
    {
        void CreateEntity(EmpDetailsClass entity);
        List<EmpDetailsClass> GetEntities();
    }

    public class EmpTableOperations : IEmpTableOperations
    {
        //Represent the Cloud Storage Account, this will be instantiated 
        //based on the appsettings
        CloudStorageAccount storageAccount;
        //The Table Service Client object used to 
        //perform operations on the Table
        CloudTableClient tableClient;

        // Constructor to Create Storage Account and the Table
        public EmpTableOperations()
        {
            //Get the Storage Account from the connection string
            storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["empstorage"]);
            //Create a Table Client Object
            tableClient = storageAccount.CreateCloudTableClient();
            //Create Table if it does not exist
            CloudTable table = tableClient.GetTableReference("EmployeeTable");
            table.CreateIfNotExists();
        }

        //Method to Create Entity
        public void CreateEntity(EmpDetailsClass entity)
        {
            CloudTable table = tableClient.GetTableReference("EmployeeTable");

            //Create a TableOperation object used to insert Entity into Table
            TableOperation insertOperation = TableOperation.Insert(entity);
            //Execute an Insert Operation
            table.Execute(insertOperation);
        }

        /// Method to retrieve all the entities
        public List<EmpDetailsClass> GetEntities()
        {
            List<EmpDetailsClass> Details = new List<EmpDetailsClass>();
            CloudTable table = tableClient.GetTableReference("EmployeeTable");

            TableQuery<EmpDetailsClass> query = new TableQuery<EmpDetailsClass>();

            string cdnurl = ConfigurationManager.AppSettings["cdnurl"];

            foreach (var item in table.ExecuteQuery(query))
            {
                Details.Add(new EmpDetailsClass()
                {
                    RowKey = item.RowKey,
                    PartitionKey = item.PartitionKey,
                    EmpName = item.EmpName,
                    ContactNo = item.ContactNo,
                    Email = item.Email,
                   // ProfileImage = item.ProfileImage

                    ProfileImage = cdnurl + item.RowKey.ToString() + Path.GetExtension(item.ProfileImage)
                });
            }

            return Details;
        }

        public List<EmpDetailsClass> DeleteEntity(EmpDetailsClass obj)
        {
           if(obj == null)
            {
                throw new System.ArgumentNullException();
            }
            List<EmpDetailsClass> Details = new List<EmpDetailsClass>();
            CloudTable table = tableClient.GetTableReference("EmployeeTable");
            obj.ETag = "*";
            TableOperation tbop = TableOperation.Delete(obj);
            // TableQuery<EmpDetailsClass> query = new TableQuery<EmpDetailsClass>();

            // string cdnurl = ConfigurationManager.AppSettings["cdnurl"];
            table.Execute(tbop);
            var details = GetEntities();
            return details;
        }


        public List<EmpDetailsClass> UpdateEntity(EmpDetailsClass obj)
        {
            CloudTable table = tableClient.GetTableReference("EmployeeTable");
            obj.ETag = "*";

            List<EmpDetailsClass> Details = new List<EmpDetailsClass>();
            //Create a TableOperation object used to update Entity into Table
            TableOperation UpdateOperation = TableOperation.Replace(obj);
            //Execute an update Operation
            table.Execute(UpdateOperation);
            return Details;
            
        }

    }
}
