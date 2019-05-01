using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRNX.Connector.DayForce.Utils
{
    class ConstantUtils
    {
        internal static string CryptoKey
        {
            get { return "{C0AC6CD1-03D2-416B-8E3B-25AF777045E9C0AC6CD1-03D2-416B-8E3B-25AF777045E9}"; }

        }
        //Field Use during connection
        public static string Username = "Username";
        public static string Password = "Password";
        public static string clientNamespace = "clientNamespace";
        //Scribe shown Entity
        public const string Employee_Entity = "Employees";
        public const string EmployeeDetails_Entity = "EmployeeDetails";
        public const string EmployeeCreateFlat_Entity = "EmployeeCreateFlat";
    }
}
