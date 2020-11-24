using System;
using System.Collections.Generic;

namespace StackX.ServiceModel.Types
{
  
     
    public class AppUserDto 
    {
        
        public string ProfileUrl { get; set; }
        public string LastLoginIp { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public virtual int? Id { get; set; }
        public virtual string UserName { get; set; }
        
        public virtual string Email { get; set; }

        public virtual string PrimaryEmail { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string Company { get; set; }
        public virtual DateTime? BirthDate { get; set; }
        public virtual string BirthDateRaw { get; set; }
        public virtual string Address { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string Country { get; set; }
        public virtual string Culture { get; set; }
        public virtual string FullName { get; set; }
        public virtual string Gender { get; set; }
        public virtual string Language { get; set; }
        public virtual string MailAddress { get; set; }
        public virtual string Nickname { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string TimeZone { get; set; } 
        public virtual List<string> Roles { get; set; } = new List<string>();
        public virtual List<string> Permissions { get; set; } = new List<string>();
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual int InvalidLoginAttempts { get; set; }
        public virtual DateTime? LastLoginAttempt { get; set; }
        public virtual DateTime? LockedDate { get; set; }
        public virtual string RecoveryToken { get; set; }
 
        public virtual Dictionary<string, string> Meta { get; set; }
    }
    
    
}