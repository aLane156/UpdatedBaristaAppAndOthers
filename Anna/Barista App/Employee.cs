using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barista_App
{
    public class Employee
    {
        public string FirstName;
        public string Surname;
        public string EmployeeID;
        public string Position;
        public string HashedPassword;
    }

    public class EmployeeList : Dictionary<string, Employee>
    {

    }

    public class CurrentUser
    {
        public string UFirstName;
        public string USurname;
        public string UID;
        public string UPosition;
        public string UHashedPassword;
    }
}
