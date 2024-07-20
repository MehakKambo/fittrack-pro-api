using System;
using System.ComponentModel.DataAnnotations;
namespace UserManagementService.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        public string Password { get; set; }  // Plain text password (only for input)
        public string PasswordHash { get; set; } // Hashed password
    }
}
