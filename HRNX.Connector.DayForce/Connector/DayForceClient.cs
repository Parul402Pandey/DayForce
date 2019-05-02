using HRNX.Connector.DayForce.Entities;
using HRNX.Connector.DayForce.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HRNX.Connector.DayForce.FlatAndHierarchicalConverter;



namespace HRNX.Connector.DayForce.Connector
{
    public class DayForceClient
    {
        #region
        private const string BaseUrl = "https://usconfigr56.dayforcehcm.com/Api/ddn/V1";
        private const string GetEmployeesUri = "/Employees";
        private const string GetEmployeeDetailsUri = "/Employees/";
        private const string CreateEmployeeUri = "/Employees";
        public EmployeeCreateFlat responseObject;
        public EmployeeCreate responseValue;
        public ServiceUtils service = new ServiceUtils();
        public string filterUrl,Name,Value,completeUrl;
        public  static int count = 0;
        #endregion
        /// <summary>
        /// Provide all list of Employee Xrefcodes 
        /// </summary>
        /// <param name="_connectionInfo"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<Employees> GetEmployees(IDictionary<string, string> _connectionInfo, DayForceFilter filter)
        {
            EmployeeResponse employeeResponsedata=null;
            List<Employees> employees= new List<Employees>(); 

            if (filter == null)
            {
                string completeUrl = String.Concat(BaseUrl, GetEmployeesUri);

                HttpWebResponse response = service.HttpRequest(_connectionInfo, completeUrl, HttpMethods.GET.ToString(), null);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var encoding = ASCIIEncoding.ASCII;
                    using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                    {
                        string responseText = reader.ReadToEnd();
                        employeeResponsedata = JsonConvert.DeserializeObject<EmployeeResponse>(responseText);

                    } 
                    employees =EmployeeHierarchicalToFlatConverter.employeeHierarchicalToFlatConverter(employeeResponsedata);

                }
            }
            if (filter != null)
            {
                String completeUrl = String.Concat(BaseUrl, GetEmployeesUri);
                for (int i = 0; i < filter.name.Count; i++)
                {
                    Name = filter.name[i];
                    Value = filter.value[i];
                    if (i == 0)
                    {
                        filterUrl = "?";
                    }
                    if (i == (filter.name.Count - 1))
                    {
                        if (filter.name[i] == "Employees.contextDate" || filter.name[i] == "Employees.filterHireStartDate" || filter.name[i] == "filterHireEndDate" || filter.name[i] == " filterTerminationStartDate" || filter.name[i] == "filterTerminationEndDate" || filter.name[i] == " filterUpdatedStartDate" || filter.name[i] == "filterUpdatedEndDate")
                        {

                            string line = filter.value[i];
                            DateTime dt = new DateTime();
                            dt = Convert.ToDateTime(line);
                            var date = dt.ToString("yyyy-MM-dd" + "T" + "HH:mm:ss");
                            Value = date;
                            filterUrl += Name + " = " + Value;
                        }
                        else
                        {
                            filterUrl += Name + " = " + Value;
                        }
                    }
                    else
                    {
                        if (filter.name[i] == "Employees.contextDate" || filter.name[i] == "Employees.filterHireStartDate" || filter.name[i] == "filterHireEndDate" || filter.name[i] == " filterTerminationStartDate" || filter.name[i] == "filterTerminationEndDate" || filter.name[i] == " filterUpdatedStartDate" || filter.name[i] == "filterUpdatedEndDate")
                        {

                            string line = filter.value[i];
                            DateTime dt = new DateTime();
                            dt = Convert.ToDateTime(line);
                            var date = dt.ToString("yyyy-MM-dd" + "T" + "HH:mm:ss");
                            Value = date;
                            filterUrl += Name + "=" + Value + "&";
                        }
                        else
                        {
                            filterUrl += Name + "=" + Value + "&";
                        }
                    }

                }
                filterUrl = String.Concat(completeUrl, filterUrl);
                filterUrl = filterUrl.Replace("Employees.", " ");
                filterUrl = filterUrl.Replace(" ", "");
                completeUrl = filterUrl;
                HttpWebResponse response = service.HttpRequest(_connectionInfo, completeUrl, HttpMethods.GET.ToString(), null);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var encoding = ASCIIEncoding.ASCII;
                    using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                    {
                        string responseText = reader.ReadToEnd();
                        employeeResponsedata = JsonConvert.DeserializeObject<EmployeeResponse>(responseText);

                    }
                    employees =EmployeeHierarchicalToFlatConverter.employeeHierarchicalToFlatConverter(employeeResponsedata);

                }

            }
            return employees;
        }
        /// <summary>
        /// Use to Get Employee Details on the basis of XRefcode
        /// </summary>
        /// <param name="_connectionInfo"></param>
        /// <param name="filter"></param>
        /// <returns></returns>

        public List<EmployeeDetails> GetEmployeeDetails(IDictionary<string, string> _connectionInfo, DayForceFilter filter)
        {
           
            List<EmployeeDetails> employeeResponselist = new List<EmployeeDetails>();
            EmployeeDetails employeeDetails = new EmployeeDetails();
            EmployeeDetailsBasicResponse employeeResponsedata = new EmployeeDetailsBasicResponse();
            completeUrl = string.Concat(BaseUrl, GetEmployeeDetailsUri);
            if (filter != null)
            {
                for (int i = 0; i < filter.name.Count; i++)
                {
                    if(filter.name[i]=="EmployeeDetails.XRefCode")
                    {
                        count=++count;
                    }
                }
                if(count==0)
                {
                    throw new WebException("XRefCode required,please provide XRefCode");
                }
                for (int i = 0; i < filter.name.Count; i++)
                {
                    Name = filter.name[i];
                    Value = filter.value[i];
                    if (filter.name[i] != "EmployeeDetails.XRefCode")
                    {
                        if (i == 0)
                        {
                            filterUrl = "?";
                        }
                    }
                    if (i == (filter.name.Count - 1))
                    {
                        if (filter.name[i] == "EmployeeDetails.XRefCode")
                        {
                            filterUrl = filter.value[i];
                        }
                        if (filter.name[i] == "EmployeeDetails.contextDate")
                        {

                            string line = filter.value[i];
                            DateTime dt = new DateTime();
                            dt = Convert.ToDateTime(line);
                            var date = dt.ToString("yyyy-MM-dd" + "T" + "HH:mm:ss");
                            Value = date;
                            filterUrl += Name + " = " + Value;
                        }
                        else
                        {
                            if (filter.name[i] != "EmployeeDetails.XRefCode")
                            {
                                filterUrl += Name + " = " + Value;
                            }
                        }
                    }
                    else
                    {
                        if (filter.name[i] == "EmployeeDetails.XRefcode")
                        {
                            filterUrl = filter.value[i];
                        }
                        if (filter.name[i] == "EmployeeDetails.contextDate")
                        {

                            string line = filter.value[i];
                            DateTime dt = new DateTime();
                            dt = Convert.ToDateTime(line);
                            var date = dt.ToString("yyyy-MM-dd" + "T" + "HH:mm:ss");
                            Value = date;
                            filterUrl += Name + "=" + Value + "&";
                        }
                        else
                        {
                            if (filter.name[i] != "EmployeeDetails.XRefcode")
                            {
                                filterUrl += Name + "=" + Value + "&";
                            }
                        }
                    }

                }
                filterUrl = filterUrl.Replace("EmployeeDetails.", " ");
                filterUrl = filterUrl.Replace(" ", "");
                completeUrl = string.Concat(BaseUrl, GetEmployeeDetailsUri, filterUrl);

                HttpWebResponse response = service.HttpRequest(_connectionInfo, completeUrl, HttpMethods.GET.ToString(), null);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var encoding = ASCIIEncoding.ASCII;
                    using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                    {
                        string responseText = reader.ReadToEnd();
                        employeeResponsedata = JsonConvert.DeserializeObject<EmployeeDetailsBasicResponse>(responseText);

                    }

                    employeeDetails =EmployeeHierarchicalToFlatConverter.EmployeeDetailsHierarchicalToFlatConverter(employeeResponsedata);

                    employeeResponselist.Add(employeeDetails);
                }

            }
            if (filter == null)
            {
                throw new WebException("Please sepecify the Value of XrefCode");
            }
            return employeeResponselist;
        }
        /// <summary>
        /// Use to create an Employee on Scribe with required details
        /// </summary>
        /// <param name="_connectionInfo"></param>
        /// <param name="employee"></param>
        /// <returns></returns>

        public string CreateEmployee(IDictionary<string, string> _connectionInfo, EmployeeCreateFlat employee)
        {
            completeUrl = string.Concat(BaseUrl, CreateEmployeeUri);
            FlatToHierarchical flat = new FlatToHierarchical();
            EmployeeCreate employeeRequest=flat.ConvertEmployeeFlatToHierarchical(employee);

            string employeeJsonContent = JsonConvert.SerializeObject(employeeRequest,
                 new JsonSerializerSettings
                 {
                     NullValueHandling = NullValueHandling.Ignore,
                     //DefaultValueHandling = DefaultValueHandling.Ignore
                 }
                );


            HttpWebResponse response = service.HttpRequest(_connectionInfo, completeUrl, HttpMethods.POST.ToString(), employeeJsonContent);

            try
            {
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var encoding = ASCIIEncoding.ASCII;
                    using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                    {
                        var resp = reader.ReadToEnd();
                        return "Ok";

                    }

                }
                else if ((response.StatusCode == HttpStatusCode.BadRequest))
                {

                    throw new WebException("Request is malformed. Correct and resubmit.");

                }
                else if ((response.StatusCode == HttpStatusCode.InternalServerError))
                {
                    throw new WebException("An Unexpected Server Error Occued.");

                }
                else
                {
                    throw new WebException("Unable to Create this information for DayForce");
                }
            }
            catch (WebException ex)
            {
                throw new WebException(ex.Message);
            }

        }

    }
}
