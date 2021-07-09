using System; 
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text; 
using Course.Web.Share.Domain;


namespace Course.Web.Share.Domain {
    
    
    [DataContract(Name="AppUserData" , Namespace="")]
    public class AppUserData {

        [DataMember(Order = 1)]
        public virtual String Id
        {
            get;
            set;
        }

        [DataMember(Order = 2)]
        public virtual String UserName
        {
            get;
            set;
        }

        [DataMember(Order = 3)]
        public virtual String NormalizedUserName
        {
            get;
            set;
        }

        [DataMember(Order = 4)]
        public virtual String Email
        {
            get;
            set;
        }

        [DataMember(Order = 5)]
        public virtual String NormalizedEmail
        {
            get;
            set;
        }

        [DataMember(Order = 6)]
        public virtual Boolean EmailConfirmed
        {
            get;
            set;
        }

        [DataMember(Order = 7)]
        public virtual String PasswordHash
        {
            get;
            set;
        }

        [DataMember(Order = 8)]
        public virtual String SecurityStamp
        {
            get;
            set;
        }

        [DataMember(Order = 9)]
        public virtual String ConcurrencyStamp
        {
            get;
            set;
        }

        [DataMember(Order = 10)]
        public virtual String PhoneNumber
        {
            get;
            set;
        }

        [DataMember(Order = 11)]
        public virtual Boolean PhoneNumberConfirmed
        {
            get;
            set;
        }

        [DataMember(Order = 12)]
        public virtual Boolean TwoFactorEnabled
        {
            get;
            set;
        }

        [DataMember(Order = 13)]
        public virtual String LockoutEnd
        {
            get;
            set;
        }

        [DataMember(Order = 14)]
        public virtual Boolean LockoutEnabled
        {
            get;
            set;
        }

        [DataMember(Order = 15)]
        public virtual Int32 AccessFailedCount
        {
            get;
            set;
        }

        [DataMember(Order=16)]
        public virtual DateTime CreateTime {
            get;
            set;
        }
        
        [DataMember(Order=17)]
        public virtual DateTime? LastLogin {
            get;
            set;
        }
        
        [DataMember(Order=18)]
        public virtual Int32 LoginCount {
            get;
            set;
        }
        
        [DataMember(Order=19)]
        public virtual DateTime? DateOfBirth {
            get;
            set;
        }
        
        [DataMember(Order=20)]
        public virtual String FullName {
            get;
            set;
        }
        
        [DataMember(Order=21)]
        public virtual String JobTitle {
            get;
            set;
        }
        
        [DataMember(Order=22)]
        public virtual Boolean IsActive {
            get;
            set;
        }        
        [DataMember(Order=23)]
        public virtual String KhoaHocIds
        {
            get;
            set;
        }        
        [DataMember(Order=24)]
        public virtual DateTime? ExpiredDate
        {
            get;
            set;
        }
    }
}
