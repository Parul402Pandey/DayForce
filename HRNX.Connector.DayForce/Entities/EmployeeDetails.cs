using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HRNX.Connector.DayForce.Connector.MetaDataProvider;

namespace HRNX.Connector.DayForce.Entities
{
   public  class EmployeeDetails
    {
        //The XRefcode identifying the retrieve employeee data
       // [QuerySelectAttribute]
      //  [ReadOnly(false), Required]
      //  public string XRefCode { get; set; }

        //Use to retrieve the Employee details according to context date as that were on that day or not
        [QuerySelectAttribute]
        public DateTime? ContextDate { get; set; }
        //ByDefault only basic information of employees has been access to get the more information retrieve 
        //by using expand parameters.
        [QuerySelectAttribute]
        public List<string> Expand { get; set; }
        [QueryConstraintAttribute]
          public bool BioExempt { get; set; }
        [QueryConstraintAttribute]
        public DateTime BirthDate { get; set; }
        [QueryConstraintAttribute]
        public DateTime ChecksumTimestamp { get; set; }
        [QueryConstraintAttribute]
        public bool ClockSupervisor { get; set; }
       
        [QueryConstraintAttribute]
        public string CultureXRefCode { get; set; }
        [QueryConstraintAttribute]
        public string CultureShortName { get; set; }
        [QueryConstraintAttribute]
        public string CultureLongName { get; set; }
        [QueryConstraintAttribute]
        public string EligibleForRehire { get; set; }
        [QueryConstraintAttribute]
        public string Gender { get; set; }
        [QueryConstraintAttribute]
        public DateTime HireDate { get; set; }
        [QueryConstraintAttribute]
        public string HomePhone { get; set; }
        [QueryConstraintAttribute]
        public DateTime NewHireApprovalDate { get; set; }
        [QueryConstraintAttribute]
        public bool NewHireApproved { get; set; }
        [QueryConstraintAttribute]
        public string NewHireApprovedBy { get; set; }
        [QueryConstraintAttribute]
        public DateTime OriginalHireDate { get; set; }
        [QueryConstraintAttribute]
        public bool PhotoExempt { get; set; }
        [QueryConstraintAttribute]
        public string RegisteredDisabled { get; set; }
        [QueryConstraintAttribute]
        public bool RequiresExitInterview { get; set; }
        [QueryConstraintAttribute]
        public DateTime SeniorityDate { get; set; }
        [QueryConstraintAttribute]
        public DateTime StartDate { get; set; }
        [QueryConstraintAttribute]
        public bool TaxExempt { get; set; }
        [QueryConstraintAttribute]
        public int FirstTimeAccessEmailSentCount { get; set; }
        [QueryConstraintAttribute]
        public int FirstTimeAccessVerificationAttempts { get; set; }
        [QueryConstraintAttribute]
        public bool SendFirstTimeAccessEmail { get; set; }
        [QueryConstraintAttribute]
        public string EmployeebadgeBadgeNumber { get; set; }
        [QueryConstraintAttribute]
        public DateTime EmployeebadgeEffectiveStart { get; set; }
        [QueryConstraintAttribute]
        public string HomeorganizationXRefCode { get; set; }
        [QueryConstraintAttribute]
        public string HomeorganizationShortName { get; set; }
        [QueryConstraintAttribute]
        public string HomeorganizationLongName { get; set; }
        [QueryConstraintAttribute]
        public string EmployeeNumber { get; set; }
        //  [QueryConstraintAttribute]
        [ReadOnly(false), Required]
        public string XRefCode { get; set; }
        [QueryConstraintAttribute]
        public string CommonName { get; set; }
        [QueryConstraintAttribute]
        public string DisplayName { get; set; }
        [QueryConstraintAttribute]
        public string FirstName { get; set; }
        [QueryConstraintAttribute]
        public string LastName { get; set; }
       
    }
}
