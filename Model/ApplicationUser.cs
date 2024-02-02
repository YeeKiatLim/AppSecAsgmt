using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Assignment_1.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string CreditCard { get; set; }
        public string Gender { get; set; }
        public string DeliveryAddress {  get; set; }
    }
}
