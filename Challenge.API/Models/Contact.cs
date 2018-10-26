using System;

namespace Challenge.API.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }
        public string PersonalPhoneNumber { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }
    }
}