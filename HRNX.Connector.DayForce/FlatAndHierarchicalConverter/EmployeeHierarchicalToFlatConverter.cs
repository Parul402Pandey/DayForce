using HRNX.Connector.DayForce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRNX.Connector.DayForce.FlatAndHierarchicalConverter
{
    public class EmployeeHierarchicalToFlatConverter
    {
        /// <summary>
        /// convert employee data from hierachical to flat model
        /// </summary>
        /// <param name="employeeResponse"></param>
        /// <returns></returns>
        public static List<Employees> employeeHierarchicalToFlatConverter(EmployeeResponse employeeResponse)
        {
            List<Employees> employeeEntity = new List<Employees>();
            Employees employees = new Employees();

            if (employeeResponse != null)
            {

                foreach (var employee in employeeResponse.Data)
                {

                    employees = new Employees();
                    employees.XRefCode = employee.XRefCode;
                    employeeEntity.Add(employees);

                }
            }

            return employeeEntity;
        }
        public static EmployeeDetails EmployeeDetailsHierarchicalToFlatConverter(EmployeeDetailsBasicResponse employeeDetailsResponse)
        {
            EmployeeDetails employeeEntity = new EmployeeDetails();
            if (employeeDetailsResponse != null)
            {
                employeeEntity.BioExempt = employeeDetailsResponse.Data.BioExempt;
                employeeEntity.BirthDate = employeeDetailsResponse.Data.BirthDate;
                employeeEntity.ChecksumTimestamp = employeeDetailsResponse.Data.ChecksumTimestamp;
                employeeEntity.ClockSupervisor = employeeDetailsResponse.Data.ClockSupervisor;
                employeeEntity.CultureXRefCode = employeeDetailsResponse.Data.Culture.XRefCode;
                employeeEntity.CultureShortName = employeeDetailsResponse.Data.Culture.ShortName;
                employeeEntity.CultureLongName = employeeDetailsResponse.Data.Culture.LongName;
                employeeEntity.EligibleForRehire = employeeDetailsResponse.Data.EligibleForRehire;
                employeeEntity.Gender = employeeDetailsResponse.Data.Gender;
                employeeEntity.HireDate = employeeDetailsResponse.Data.HireDate;
                employeeEntity.HomePhone = employeeDetailsResponse.Data.HomePhone;
                employeeEntity.NewHireApprovalDate = employeeDetailsResponse.Data.NewHireApprovalDate;
                employeeEntity.NewHireApproved = employeeDetailsResponse.Data.NewHireApproved;
                employeeEntity.NewHireApprovedBy = employeeDetailsResponse.Data.NewHireApprovedBy;
                employeeEntity.OriginalHireDate = employeeDetailsResponse.Data.OriginalHireDate;
                employeeEntity.PhotoExempt = employeeDetailsResponse.Data.PhotoExempt;
                employeeEntity.RegisteredDisabled = employeeDetailsResponse.Data.RegisteredDisabled;
                employeeEntity.RequiresExitInterview = employeeDetailsResponse.Data.RequiresExitInterview;
                employeeEntity.SeniorityDate = employeeDetailsResponse.Data.SeniorityDate;
                employeeEntity.StartDate = employeeDetailsResponse.Data.StartDate;
                employeeEntity.TaxExempt = employeeDetailsResponse.Data.TaxExempt;
                employeeEntity.FirstTimeAccessEmailSentCount = employeeDetailsResponse.Data.FirstTimeAccessEmailSentCount;
                employeeEntity.FirstTimeAccessVerificationAttempts = employeeDetailsResponse.Data.FirstTimeAccessVerificationAttempts;
                employeeEntity.SendFirstTimeAccessEmail = employeeDetailsResponse.Data.SendFirstTimeAccessEmail;
                employeeDetailsResponse.Data.EmployeeBadge = new Employeebadge();
                employeeEntity.EmployeebadgeBadgeNumber = employeeDetailsResponse.Data.EmployeeBadge.BadgeNumber;
                employeeEntity.EmployeebadgeEffectiveStart = employeeDetailsResponse.Data.EmployeeBadge.EffectiveStart;
                employeeDetailsResponse.Data.HomeOrganization = new Homeorganization();
                employeeEntity.HomeorganizationXRefCode = employeeDetailsResponse.Data.HomeOrganization.XRefCode;
                employeeEntity.HomeorganizationShortName = employeeDetailsResponse.Data.HomeOrganization.ShortName;
                employeeEntity.HomeorganizationLongName = employeeDetailsResponse.Data.HomeOrganization.LongName;
                employeeEntity.EmployeeNumber = employeeDetailsResponse.Data.EmployeeNumber;
                employeeEntity.XRefCode = employeeDetailsResponse.Data.XRefCode;
                employeeEntity.CommonName = employeeDetailsResponse.Data.CommonName;
                employeeEntity.DisplayName = employeeDetailsResponse.Data.DisplayName;
                employeeEntity.FirstName = employeeDetailsResponse.Data.FirstName;
                employeeEntity.LastName = employeeDetailsResponse.Data.LastName;
            }

            return employeeEntity;
        }


    }
}
