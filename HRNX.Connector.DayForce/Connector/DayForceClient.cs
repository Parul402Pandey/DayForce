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
        public List<Item> employeeList { get; set;}
        public List<Item1> employeeList1 { get; set; }
        public List<Item2> employeeList2 { get; set; }
        public List<Item3> employeeList3 { get; set; }
        public List<Item4> employeeList4 { get; set; }
        public string filterUrl;
        #endregion
        public HttpWebResponse HttpRequest(IDictionary<string,string> connection,string requestUrl,string methodName,string requestContent)
        {
            HttpWebResponse response = null;
            try
            {
                //create http request Url
                HttpWebRequest httprequest = WebRequest.Create(requestUrl) as HttpWebRequest;
                String str = connection[ConstantUtils.Username];
                String str1 = connection[ConstantUtils.Password];
                str = String.Concat(str, str1);
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);

                //  string data = System.Convert.ToBase64String(bytes);
                string data = "REZXU1Rlc3Q6REZXU1Rlc3Q=";

                if (!string.IsNullOrEmpty(str))
                {
                    httprequest.Headers.Add("Authorization", string.Format("Basic {0}", data));
                }
                httprequest.Method = methodName;
                httprequest.ContentType = "application/json";
                if(requestUrl== "https://usconfigr56.dayforcehcm.com/Api/ddn/V1/Employees")
                {
                    httprequest.Headers.Add("isValidateOnly", "true");
                }
              //  httprequest.Headers.Add("Connection", "keep-alive");
              //  httprequest.Headers.Add("accept-encoding", "gzip, deflate");
              //  httprequest.Headers.Add("Host", "usconfigr56.dayforcehcm.com");
              //  httprequest.Headers.Add("Postman-Token", "a682b497-c7e2-4460-8e22-f39f9222e915");
              //  httprequest.Headers.Add("Cache-Control", "no-cache");
              //  httprequest.Headers.Add("Accept", "*/*");
              //  httprequest.Headers.Add("User-Agent", "PostmanRuntime/7.11.0");
                if (!string.IsNullOrEmpty(requestContent))
                {
                    Byte[] bt = System.Text.Encoding.UTF8.GetBytes(requestContent);
                    Stream st = httprequest.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                }
                response = httprequest.GetResponse() as HttpWebResponse;
                return response;
            }
            catch(WebException e)
            {
                using (WebResponse resp = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)resp;
                    using (Stream data = resp.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        if((httpResponse.StatusCode==HttpStatusCode.NotFound)&&(methodName==HttpMethods.GET.ToString()))
                            {
                            Stream receiveStream = httpResponse.GetResponseStream();
                            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

                            var newData = (JObject)JsonConvert.DeserializeObject(text);
                            string textMessage = newData["message"].Value<string>();
                            if (textMessage == "Resource not found")
                            {
                                return null;
                            }
                        }
                        if((httpResponse.StatusCode==HttpStatusCode.BadRequest)&&(methodName==HttpMethods.POST.ToString()))
                        {
                            throw new WebException("Request is malformed. Correct and resubmit.");
                        }
                        if ((httpResponse.StatusCode == HttpStatusCode.BadRequest) && (methodName == HttpMethods.GET.ToString()))
                        {
                            throw new WebException("Request is malformed. Correct and resubmit.");
                        }
                        if ((httpResponse.StatusCode == HttpStatusCode.InternalServerError) && (methodName == HttpMethods.POST.ToString()))
                        {
                            throw new WebException("An Unexpected Server Error Occured");
                        }
                        if ((httpResponse.StatusCode == HttpStatusCode.InternalServerError) && (methodName == HttpMethods.GET.ToString()))
                        {
                            throw new WebException("An Unexpected Server Error Occured");
                        }

                        throw new WebException("Unable to post/get the request to DayForce Api:" + text, e.InnerException);
                    }
                }
            }
            catch (Exception e)
            {
                throw new WebException("Unable to post/get the request to DayForce api :" + e.Message, e.InnerException);
            }
        }

        public List<Employees> GetEmployees(IDictionary<string, string> _connectionInfo, DayForceFilter filter)
        {
           string Name,Value,Comparison,Logical;
         
            List<Datum> employeeResponselist = new List<Datum>();
            Datum employeeXrefCode = new Datum();
            EmployeeResponse employeeResponsedata;
           // string completeUrl = "https://usconfigr56.dayforcehcm.com/Api/ddn/V1/Employees";


            List<string> fieldNames = new List<string>();

            List<Employees> employees = new List<Employees>();

            if (filter == null)
            {
                string completeUrl = String.Concat(BaseUrl,GetEmployeesUri);

                HttpWebResponse response = this.HttpRequest(_connectionInfo, completeUrl, HttpMethods.GET.ToString(), null);

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
            if(filter!=null)
            {
                String completeUrl = String.Concat(BaseUrl, GetEmployeesUri);
                for (int i=0;i<filter.name.Count;i++)
                {
                     Name=filter.name[i];
                     Value = filter.value[i];
                    if(i==0)
                    {
                        filterUrl= "?";
                    }
                    if(i==(filter.name.Count-1))
                    {
                        if (filter.name[i] == "Employees.contextDate" || filter.name[i] == "Employees.filterHireStartDate" || filter.name[i] == "filterHireEndDate" || filter.name[i] == " filterTerminationStartDate" || filter.name[i] == "filterTerminationEndDate" || filter.name[i] == " filterUpdatedStartDate" || filter.name[i] == "filterUpdatedEndDate")
                        {
                            string line = filter.value[i];
                            string[] wordArray = line.Split('-');
                            var reverse=wordArray.Reverse();
                            string joinValue;
                            Value=String.Join("-",reverse);
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
                            string[] wordArray = line.Split('-');
                            var reverse = wordArray.Reverse();
                            string joinValue;
                            Value = String.Join("-", reverse);
                            filterUrl += Name + "=" + Value + "&";
                        }
                        else
                        {
                            filterUrl += Name + "=" + Value + "&";
                        }
                    }
                    
                }
                filterUrl = String.Concat(completeUrl, filterUrl);
                filterUrl=filterUrl.Replace("Employees."," ");
                filterUrl = filterUrl.Replace(" ", "");
                completeUrl = filterUrl;
                HttpWebResponse response = this.HttpRequest(_connectionInfo, completeUrl, HttpMethods.GET.ToString(), null);

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
        //GetEmployeeDetails() use to retrieve employee detail on the basis of XRefcode
        public List<EmployeeDetails> GetEmployeeDetails(IDictionary<string, string> _connectionInfo, DayForceFilter filter)
        {

            List<EmployeeDetails> employeeResponselist = new List<EmployeeDetails>();
            EmployeeDetails employeeDetails = new EmployeeDetails();
           
            EmployeeDetailsBasicResponse employeeResponsedata= new EmployeeDetailsBasicResponse();
         //   string completeUrl = "https://usconfigr56.dayforcehcm.com/Api/ddn/V1/Employees/42199";


            List<string> fieldNames = new List<string>();


            if (filter != null)
            {
                if(String.IsNullOrEmpty(filter.value[0]))
                {
                    throw new Exception("Please specify value of Xrefcode to get Employee Details.");
                }
               
                string completeUrl = string.Concat(BaseUrl,GetEmployeeDetailsUri,filter.value[0]);

                HttpWebResponse response = this.HttpRequest(_connectionInfo, completeUrl, HttpMethods.GET.ToString(), null);

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
            if(filter==null)
            {
                throw new WebException("Please sepecify the Value of XrefCode");
            }
            return employeeResponselist;
        }
        public string CreateEmployee(IDictionary<string, string> _connectionInfo, EmployeeCreateFlat employee)
        {
            string completeUrl = string.Concat(BaseUrl,CreateEmployeeUri);


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
            employeeRequest.Culture= new CultureFirst();
            employeeRequest.Culture.XRefCode = employee.CultureFirstXRefCode;
            employeeRequest.Addresses = new Addresses();
            employeeRequest.Contacts = new Contacts();
            employeeRequest.EmploymentStatuses = new Employmentstatuses();
            employeeRequest.Roles = new Roles();
            employeeRequest.WorkAssignments = new Workassignments();



            if (!String.IsNullOrEmpty(employee.Address1)|| !String.IsNullOrEmpty(employee.City)|| !String.IsNullOrEmpty(employee.PostalCode)||employee.EffectiveStart == default(DateTime)|| !String.IsNullOrEmpty(employee.CountryXRefCode)|| !String.IsNullOrEmpty(employee.StateXRefCode)|| !String.IsNullOrEmpty(employee.ContactinformationtypeXRefCode))
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
               if (!String.IsNullOrEmpty(employee.Contactinformationtype1XRefCode) || !String.IsNullOrEmpty(employee.ContactNumber) || employee.Item1EffectiveStart == default(DateTime) || !String.IsNullOrEmpty(employee.Country1XRefCode)|| !String.IsNullOrEmpty(employee.Contactinformationtype1XRefCode)||employee.ShowRejectedWarning==true||employee.IsForSystemCommunications==true||employee.IsPreferredContactMethod==true||employee.NumberOfVerificationRequests>0)

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
             if (!String.IsNullOrEmpty(employee.RoleXRefCode) ||employee.RolesEffectiveStart== default(DateTime)|| employee.isDefault == true)

          {
                employeeRequest.Roles.Items = new List<Item3>();
                Item3 item3 = new Item3();
                item3.Role = new Role();
                item3.Role.XRefCode = employee.RoleXRefCode;
                item3.EffectiveStart = employee.RolesEffectiveStart;
                 item3.isDefault = employee.isDefault;
                employeeRequest.Roles.Items.Add(item3);

            }
              if (!String.IsNullOrEmpty(employee.DepartmentXRefCode) || !String.IsNullOrEmpty(employee.JobXRefCode) || !String.IsNullOrEmpty(employee.LocationXRefCode) || employee.WorkassignmentsEffectiveStart == default(DateTime) || employee.IsPAPrimaryWorkSite == true|| employee.IsPrimary == true|| employee.IsVirtual == true|| !String.IsNullOrEmpty(employee.Employmentstatusreason1XrefCode))

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

            
            HttpWebResponse response = this.HttpRequest(_connectionInfo, completeUrl, HttpMethods.POST.ToString(), employeeJsonContent);

            try
            {
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    var encoding = ASCIIEncoding.ASCII;
                    using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                    {
                        var resp = reader.ReadToEnd();
                        //  responseValue = JsonConvert.DeserializeObject<EmployeeCreate>(resp);
                        // this.responseObject = EmployeeHierarchicalToFlatConverter(responseValue);


                        return "Ok";

                        //   return responseObject;
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
                    throw new WebException("Unable to Create this information for DayForce");                }
            }
            catch (WebException ex)
            {
                throw new WebException(ex.Message);
            }

        }

    }
}
