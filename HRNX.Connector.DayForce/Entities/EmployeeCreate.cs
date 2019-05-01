using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRNX.Connector.DayForce.Entities
{
   
        public class EmployeeCreate
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string XRefCode { get; set; }
            public bool BioExempt { get; set; }
            public DateTime BirthDate { get; set; }
           public CultureFirst Culture { get; set; }
            public string Gender { get; set; }
            public DateTime HireDate { get; set; }
            public bool PhotoExempt { get; set; }
            public bool RequiresExitInterview { get; set; }
            public string SocialSecurityNumber { get; set; }
            public bool SendFirstTimeAccessEmail { get; set; }
            public int FirstTimeAccessEmailSentCount { get; set; }
            public int FirstTimeAccessVerificationAttempts { get; set; }
            public Addresses Addresses { get; set; }
            public Contacts Contacts { get; set; }
            public Employmentstatuses EmploymentStatuses { get; set; }
            public Roles Roles { get; set; }
            public Workassignments WorkAssignments { get; set; }
        }

        public class CultureFirst
        {
            public string XRefCode { get; set; }
        }

        public class Addresses
        {
            public List<Item> Items { get; set; }
        }

        public class Item
        {
            public string Address1 { get; set; }
            public string City { get; set; }
            public string PostalCode { get; set; }
            public Country Country { get; set; }
            public State State { get; set; }
            public Contactinformationtype ContactInformationType { get; set; }
            public DateTime EffectiveStart { get; set; }
        }

        public class Country
        {
            public string XRefCode { get; set; }
        }

        public class State
        {
            public string XRefCode { get; set; }
        }

        public class Contactinformationtype
        {
            public string XRefCode { get; set; }
        }

        public class Contacts
        {
            public List<Item1> Items { get; set; }
        }

        public class Item1
        {
            public Contactinformationtype1 ContactInformationType { get; set; }
            public string ContactNumber { get; set; }
            public Country1 Country { get; set; }
            public DateTime EffectiveStart { get; set; }
            public bool ShowRejectedWarning { get; set; }
            public bool IsForSystemCommunications { get; set; }
            public bool IsPreferredContactMethod { get; set; }
            public int NumberOfVerificationRequests { get; set; }
        }

        public class Contactinformationtype1
        {
            public string XRefCode { get; set; }
        }

        public class Country1
        {
            public string XRefCode { get; set; }
        }

        public class Employmentstatuses
        {
            public List<Item2> Items { get; set; }
        }

        public class Item2
        {
            public DateTime EffectiveStart { get; set; }
            public Employmentstatus EmploymentStatus { get; set; }
            public Paytype PayType { get; set; }
            public Payclass PayClass { get; set; }
            public Paygroup PayGroup { get; set; }
            public bool CreateShiftRotationShift { get; set; }
            public float BaseRate { get; set; }
            public Employmentstatusreason EmploymentStatusReason { get; set; }
        }

        public class Employmentstatus
        {
            public string XRefCode { get; set; }
        }

        public class Paytype
        {
            public string XRefCode { get; set; }
        }

        public class Payclass
        {
            public string XRefCode { get; set; }
        }

        public class Paygroup
        {
            public string XRefCode { get; set; }
        }

        public class Employmentstatusreason
        {
            public string XrefCode { get; set; }
        }

        public class Roles
        {
            public List<Item3> Items { get; set; }
        }

        public class Item3
        {
            public Role Role { get; set; }
            public DateTime EffectiveStart { get; set; }
            public bool isDefault { get; set; }
        }

        public class Role
        {
            public string XRefCode { get; set; }
        }

        public class Workassignments
        {
            public List<Item4> Items { get; set; }
        }

        public class Item4
        {
            public Position Position { get; set; }
            public Location Location { get; set; }
            public DateTime EffectiveStart { get; set; }
            public bool IsPAPrimaryWorkSite { get; set; }
            public bool IsPrimary { get; set; }
            public bool IsVirtual { get; set; }
            public Employmentstatusreason1 EmploymentStatusReason { get; set; }
        }

        public class Position
        {
            public Department Department { get; set; }
            public Job Job { get; set; }
        }

        public class Department
        {
            public string XRefCode { get; set; }
        }

        public class Job
        {
            public string XRefCode { get; set; }
        }

        public class Location
        {
            public string XRefCode { get; set; }
        }

        public class Employmentstatusreason1
        {
            public string XrefCode { get; set; }
        }

    }

