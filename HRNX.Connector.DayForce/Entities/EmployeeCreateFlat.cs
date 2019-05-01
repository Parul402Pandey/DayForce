using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRNX.Connector.DayForce.Entities
{
    public class EmployeeCreateFlat
    {
        [ReadOnly(false), Required]
        public string FirstName { get; set; }
        [ReadOnly(false), Required]
        public string LastName { get; set; }
        [ReadOnly(false), Required]
        public string XRefCode { get; set; }
        [ReadOnly(false), Required]
        public bool BioExempt { get; set; }
        [ReadOnly(false), Required]
        public DateTime BirthDate { get; set; }
        [ReadOnly(false), Required]
        public string CultureFirstXRefCode { get; set; }
        [ReadOnly(false), Required]
        public string Gender { get; set; }
        [ReadOnly(false), Required]
        public DateTime HireDate { get; set; }
        [ReadOnly(false), Required]
        public bool PhotoExempt { get; set; }
        [ReadOnly(false), Required]
        public bool RequiresExitInterview { get; set; }
        [ReadOnly(false), Required]
        public string SocialSecurityNumber { get; set; }
        [ReadOnly(false), Required]
        public bool SendFirstTimeAccessEmail { get; set; }
        [ReadOnly(false), Required]
        public int FirstTimeAccessEmailSentCount { get; set; }
        [ReadOnly(false), Required]
        public int FirstTimeAccessVerificationAttempts { get; set; }
        [ReadOnly(false), Required]
        public string Address1 { get; set; }
        [ReadOnly(false), Required]
        public string City { get; set; }
        [ReadOnly(false), Required]
        public string PostalCode { get; set; }
        [ReadOnly(false), Required]
        public string CountryXRefCode { get; set; }
        [ReadOnly(false), Required]
        public string StateXRefCode { get; set; }
        [ReadOnly(false), Required]
        public string ContactinformationtypeXRefCode { get; set; }
        [ReadOnly(false), Required]
        public DateTime EffectiveStart { get; set; }
        [ReadOnly(false), Required]
        public string Contactinformationtype1XRefCode { get; set; }
        [ReadOnly(false), Required]
        public string ContactNumber { get; set; }
        [ReadOnly(false), Required]
        public string Country1XRefCode { get; set; }
        [ReadOnly(false), Required]
        public DateTime Item1EffectiveStart { get; set; }
        [ReadOnly(false), Required]
        public bool ShowRejectedWarning { get; set; }
        [ReadOnly(false), Required]
        public bool IsForSystemCommunications { get; set; }
        [ReadOnly(false), Required]
        public bool IsPreferredContactMethod { get; set; }
        [ReadOnly(false), Required]
        public int NumberOfVerificationRequests { get; set; }
        [ReadOnly(false), Required]
        public DateTime Item2EffectiveStart { get; set; }
        [ReadOnly(false), Required]
        public string EmploymentstatusXRefCode { get; set; }
        [ReadOnly(false), Required]
        public string PaytypeXRefCode { get; set; }
        [ReadOnly(false), Required]
        public string PayclassXRefCode { get; set; }
        [ReadOnly(false), Required]
        public string PaygroupXRefCode { get; set; }
        [ReadOnly(false), Required]
        public bool CreateShiftRotationShift { get; set; }
        [ReadOnly(false), Required]
        public float BaseRate { get; set; }
        [ReadOnly(false), Required]
        public string EmploymentstatusreasonXrefCode { get; set; }
        [ReadOnly(false), Required]
        public string RoleXRefCode { get; set; }
        [ReadOnly(false), Required]
        public DateTime RolesEffectiveStart { get; set; }
        [ReadOnly(false), Required]
        public bool isDefault { get; set; }
        [ReadOnly(false), Required]
        public string DepartmentXRefCode { get; set; }
        [ReadOnly(false), Required]
        public string JobXRefCode { get; set; }
        [ReadOnly(false), Required]
        public string LocationXRefCode { get; set; }
        [ReadOnly(false), Required]
        public DateTime WorkassignmentsEffectiveStart { get; set; }
        [ReadOnly(false), Required]
        public bool IsPAPrimaryWorkSite { get; set; }
        [ReadOnly(false), Required]
        public bool IsPrimary { get; set; }
        [ReadOnly(false), Required]
        public bool IsVirtual { get; set; }
        [ReadOnly(false), Required]
        public string Employmentstatusreason1XrefCode { get; set; }
    }
}
