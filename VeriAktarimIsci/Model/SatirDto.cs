using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeriAktarimIsci.Model
{
    public class SatirDto
    {
        public int Id { get; set; }
        public int ActivityLogTypeId { get; set; }
        public int CustomerId { get; set; }
        public string Comment { get; set; }

        public DateTime LogCreatedOnUtc { get; set; }

        public string Email { get; set; }
        public bool HasShoppingCartItems { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public string LastIpAddress { get; set; }

        public DateTime AccountCreatedOnUtc { get; set; }

        public DateTime LastLoginDateUtc { get; set; }
        public DateTime LastActivityDateUtc { get; set; }

        public int BillingAddressId { get; set; }
        public int ShippingAddressId { get; set; }

        public string Reference { get; set; }
        public int AuthenticationTypeId { get; set; }
        public DateTime AuthenticatedDateOnUtc { get; set; }

        public DateTime? SmsAuthenticatedDateOnUtc { get; set; }
        public int? SmsAuthenticationTypeId { get; set; }

        public string MobilePlatform { get; set; }
    }
}

 


