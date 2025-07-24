namespace Gudena.Api.DTOs;

using System.ComponentModel.DataAnnotations;

    public class BusinessRegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        // AccountDetails fields
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        // Business-specific field
        [Required]
        public string CompanyName { get; set; }
    }

