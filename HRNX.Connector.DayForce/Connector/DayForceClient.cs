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
using HRNX.Connector.DayForce.Utils;

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
        public List<Item> employeeList { get; set; }
        public List<Item1> employeeList1 { get; set; }
        public List<Item2> employeeList2 { get; set; }
        public List<Item3> employeeList3 { get; set; }
        public List<Item4> employeeList4 { get; set; }
        public string filterUrl;
        #endregion
        /// <summary>
        /// Provide all list of Employee Xrefcodes 
        /// </summary>
        /// <param name="_connectionInfo"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<Employees> GetEmployees(IDictionary<string, string> _connectionInfo, DayForceFilter filter)
        {
            string Name, Value;
            ServiceUtils service = new ServiceUtils();
            List<Datum> employeeResponselist = new List<Datum>();
            Datum employeeXrefCode = new Datum();
            EmployeeResponse employeeResponsedata;



            List<string> fieldNames = new List<string>();

            List<Employees> employees = new List<Employees>();

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



                    employees = HRNX.Connector.DayForce.FlatAndHierarchicalConverter.EmployeeHierarchicalToFlatConverter.employeeHierarchicalToFlatConverter(employeeResponsedata);



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



                    employees = HRNX.Connector.DayForce.FlatAndHierarchicalConverter.EmployeeHierarchicalToFlatConverter.employeeHierarchicalToFlatConverter(employeeResponsedata);



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
            ServiceUtils service = new ServiceUtils();
            List<EmployeeDetails> employeeResponselist = new List<EmployeeDetails>();
            EmployeeDetails employeeDetails = new EmployeeDetails();

            EmployeeDetailsBasicResponse employeeResponsedata = new EmployeeDetailsBasicResponse();
            List<string> fieldNames = new List<string>();


            if (filter != null)
            {
                if (String.IsNullOrEmpty(filter.value[0]))
                {
                    throw new Exception("Please specify value of Xrefcode to get Employee Details.");
                }

                string completeUrl = string.Concat(BaseUrl, GetEmployeeDetailsUri, filter.value[0]);

                HttpWebResponse response = service.HttpRequest(_connectionInfo, completeUrl, HttpMethods.GET.ToString(), null);

                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var encoding = ASCIIEncoding.ASCII;
                    using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                    {
                        string responseText = reader.ReadToEnd();
                        employeeResponsedata = JsonConvert.DeserializeObject<EmployeeDetailsBasicResponse>(responseText);

                    }

                    employeeDetails = HRNX.Connector.DayForce.FlatAndHierarchicalConverter.EmployeeHierarchicalToFlatConverter.EmployeeDetailsHierarchicalToFlatConverter(employeeResponsedata);

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
            string completeUrl = string.Concat(BaseUrl, CreateEmployeeUri);
            ServiceUtils service = new ServiceUtils();

            EmployeeCreate employeeRequest = new EmployeeCreate();
            employeeRequest.FirstName = employee.FirstName;
            employeeRequest.LastName = employee.LastName;
            employeeRequest.XRefCode = employee.XRefCode;
            employeeRequest.BioExempt = employee.BioExempt;
            employeeRequest.BirthDate = employee.BirthDate;
            employeeRequest.Gender = employee.Gender;
            employeeRequest.HireDate = employee.HireDate;
            employeeRequest.PhotoExempt = employee.PhotoExempt;
            employeeRequest.RequiresExitInterview = employee.RequiresExitInterview;
            employeeRequest.SocialSecurityNumber = employee.SocialSecurityNumber;
            employeeRequest.SendFirstTimeAccessEmail = employee.SendFirstTimeAccessEmail;
            employeeRequest.FirstTimeAccessEmailSentCount = employee.FirstTimeAccessEmailSentCount;
            employeeRequest.FirstTimeAccessVerificationAttempts = employee.FirstTimeAccessVerificationAttempts;
            employeeRequest.Culture = new CultureFirst();
            employeeRequest.Culture.XRefCode = employee.CultureFirstXRefCode;
            employeeRequest.Addresses = new Addresses();
            employeeRequest.Contacts = new Contacts();
            employeeRequest.EmploymentStatuses = new Employmentstatuses();
            employeeRequest.Roles = new Roles();
            employeeRequest.WorkAssignments = new Workassignments();



            if (!String.IsNullOrEmpty(employee.Address1) || !String.IsNullOrEmpty(employee.City) || !String.IsNullOrEmpty(employee.PostalCode) || employee.EffectiveStart == default(DateTime) || !String.IsNullOrEmpty(employee.CountryXRefCode) || !String.IsNullOrEmpty(employee.StateXRefCode) || !String.IsNullOrEmpty(employee.ContactinformationtypeXRefCode))
            {

                employeeRequest.Addresses.Items = new List<Item>();
                Item item = new Item();
                item.Address1 = employee.Address1;
                item.City = employee.City;
                item.PostalCode = employee.PostalCode;
                item.EffectiveStart = employee.EffectiveStart;
                item.Country = new Country();
                item.Country.XRefCode = employee.CountryXRefCode;
                item.State = new State();
                item.State.XRefCode = employee.StateXRefCode;
                item.ContactInformationType = new Contactinformationtype();
                item.ContactInformationType.XRefCode = employee.ContactinformationtypeXRefCode;

                employeeRequest.Addresses.Items.Add(item);

            }
            if (!String.IsNullOrEmpty(employee.Contactinformationtype1XRefCode) || !String.IsNullOrEmpty(employee.ContactNumber) || employee.Item1EffectiveStart == default(DateTime) || !String.IsNullOrEmpty(employee.Country1XRefCode) || !String.IsNullOrEmpty(employee.Contactinformationtype1XRefCode) || employee.ShowRejectedWarning == true || employee.IsForSystemCommunications == true || employee.IsPreferredContactMethod == true || employee.NumberOfVerificationRequests > 0)

            {
                employeeRequest.Contacts.Items = new List<Item1>();
                Item1 item1 = new Item1();
                item1.ContactInformationType = new Contactinformationtype1();
                item1.ContactInformationType.XRefCode = employee.Contactinformationtype1XRefCode;
                item1.ContactNumber = employee.ContactNumber;
                item1.Country = new Country1();
                item1.Country.XRefCode = employee.Country1XRefCode;
                item1.EffectiveStart = employee.Item1EffectiveStart;
                item1.IsForSystemCommunications = employee.IsForSystemCommunications;
                item1.IsPreferredContactMethod = employee.IsPreferredContactMethod;
                item1.NumberOfVerificationRequests = employee.NumberOfVerificationRequests;
                item1.ShowRejectedWarning = employee.ShowRejectedWarning;
                employeeRequest.Contacts.Items.Add(item1);

            }
            if (!String.IsNullOrEmpty(employee.EmploymentstatusXRefCode) || !String.IsNullOrEmpty(employee.PaytypeXRefCode) || employee.Item2EffectiveStart == default(DateTime) || !String.IsNullOrEmpty(employee.PaygroupXRefCode) || !String.IsNullOrEmpty(employee.PayclassXRefCode) || employee.CreateShiftRotationShift == true || !String.IsNullOrEmpty(employee.EmploymentstatusreasonXrefCode) || employee.BaseRate > 0)

            {
                employeeRequest.EmploymentStatuses.Items = new List<Item2>();
                Item2 item2 = new Item2();
                item2.EffectiveStart = employee.Item2EffectiveStart;
                item2.EmploymentStatus = new Employmentstatus();
                item2.EmploymentStatus.XRefCode = employee.EmploymentstatusXRefCode;
                item2.PayClass = new Payclass();
                item2.PayClass.XRefCode = employee.PayclassXRefCode;
                item2.PayGroup = new Paygroup();
                item2.PayGroup.XRefCode = employee.PaygroupXRefCode;
                item2.PayType = new Paytype();
                item2.PayType.XRefCode = employee.PaytypeXRefCode;
                item2.CreateShiftRotationShift = employee.CreateShiftRotationShift;
                item2.BaseRate = employee.BaseRate;
                item2.EmploymentStatusReason = new Employmentstatusreason();
                item2.EmploymentStatusReason.XrefCode = employee.EmploymentstatusreasonXrefCode;
                employeeRequest.EmploymentStatuses.Items.Add(item2);

            }
            if (!String.IsNullOrEmpty(employee.RoleXRefCode) || employee.RolesEffectiveStart == default(DateTime) || employee.isDefault == true)

            {
                employeeRequest.Roles.Items = new List<Item3>();
                Item3 item3 = new Item3();
                item3.Role = new Role();
                item3.Role.XRefCode = employee.RoleXRefCode;
                item3.EffectiveStart = employee.RolesEffectiveStart;
                item3.isDefault = employee.isDefault;
                employeeRequest.Roles.Items.Add(item3);

            }
            if (!String.IsNullOrEmpty(employee.DepartmentXRefCode) || !String.IsNullOrEmpty(employee.JobXRefCode) || !String.IsNullOrEmpty(employee.LocationXRefCode) || employee.WorkassignmentsEffectiveStart == default(DateTime) || employee.IsPAPrimaryWorkSite == true || employee.IsPrimary == true || employee.IsVirtual == true || !String.IsNullOrEmpty(employee.Employmentstatusreason1XrefCode))

            {

                employeeRequest.WorkAssignments.Items = new List<Item4>();
                Item4 item4 = new Item4();
                item4.Position = new Position();
                item4.Position.Department = new Department();
                item4.Position.Job = new Job();
                item4.Position.Department.XRefCode = employee.DepartmentXRefCode;
                item4.Position.Job.XRefCode = employee.JobXRefCode;
                item4.Location = new Location();
                item4.Location.XRefCode = employee.LocationXRefCode;
                item4.EffectiveStart = employee.WorkassignmentsEffectiveStart;
                item4.EmploymentStatusReason = new Employmentstatusreason1();
                item4.EmploymentStatusReason.XrefCode = employee.Employmentstatusreason1XrefCode;
                item4.IsPAPrimaryWorkSite = employee.IsPAPrimaryWorkSite;
                item4.IsPrimary = employee.IsPrimary;
                item4.IsVirtual = employee.IsVirtual;

                employeeRequest.WorkAssignments.Items.Add(item4);

            }

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



                    Stream receiveStream = response.GetResponseStream();
                    var reader = new StreamReader(receiveStream);
                    string text = reader.ReadToEnd();
                    var newData = (JObject)JsonConvert.DeserializeObject(text);
                    string textMessage = newData["message"].Value<string>();
                    throw new WebException("Request is malformed. Correct and resubmit.");

                }
                else if ((response.StatusCode == HttpStatusCode.InternalServerError))
                {



                    Stream receiveStream = response.GetResponseStream();
                    var reader = new StreamReader(receiveStream);
                    string text = reader.ReadToEnd();

                    var newData = (JObject)JsonConvert.DeserializeObject(text);
                    string textMessage = newData["message"].Value<string>();
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
