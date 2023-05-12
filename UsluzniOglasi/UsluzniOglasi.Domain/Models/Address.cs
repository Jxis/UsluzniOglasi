using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace UsluzniOglasi.Domain.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int PostalCode { get; set; }
    }
}
