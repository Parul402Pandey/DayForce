using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRNX.Connector.DayForce.Entities
{
    
    public class EmployeeDetailsBasicResponse
        {
        public Data Data { get; set; }
         }

    public class Data
    {
        public bool BioExempt { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime ChecksumTimestamp { get; set; }
        public bool ClockSupervisor { get; set; }
        public Culture Culture { get; set; }
        public string EligibleForRehire { get; set; }
        public string Gender { get; set; }
        public DateTime HireDate { get; set; }
        public string HomePhone { get; set; }
        public DateTime NewHireApprovalDate { get; set; }
        public bool NewHireApproved { get; set; }
        public string NewHireApprovedBy { get; set; }
        public DateTime OriginalHireDate { get; set; }
        public bool PhotoExempt { get; set; }
        public string RegisteredDisabled { get; set; }
        public bool RequiresExitInterview { get; set; }
        public DateTime SeniorityDate { get; set; }
        public DateTime StartDate { get; set; }
        public bool TaxExempt { get; set; }
        public int FirstTimeAccessEmailSentCount { get; set; }
        public int FirstTimeAccessVerificationAttempts { get; set; }
        public bool SendFirstTimeAccessEmail { get; set; }
        public Employeebadge EmployeeBadge { get; set; }
        public Homeorganization HomeOrganization { get; set; }
        public string EmployeeNumber { get; set; }
        public string XRefCode { get; set; }
        public string CommonName { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class Culture
    {
        public string XRefCode { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
    }

    public class Employeebadge
    {
        public string BadgeNumber { get; set; }
        public DateTime EffectiveStart { get; set; }
    }

    public class Homeorganization
    {
        public string XRefCode { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
    }

}
