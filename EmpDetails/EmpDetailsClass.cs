using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace EmpDetails
{
    public class EmpDetailsClass:TableEntity
    {

        public EmpDetailsClass()
        {
        }
        public EmpDetailsClass(int empId, string company)
        {
            this.RowKey = empId.ToString();
            this.PartitionKey = company;
        }
        

        public EmpDetailsClass(int empId, string company, string EmpName, string ContactNo, string Email, string s)
        {
            this.RowKey = empId.ToString();
            this.PartitionKey = company;
            this.EmpName = EmpName;
            this.ContactNo = ContactNo;
            this.Email = Email;
            this.ProfileImage = ProfileImage;
        }

        public string EmpName { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string ProfileImage { get; set; }
    }
}
