using System;
namespace UserManagementService.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }  // Plain text password (only for input)
        public string PasswordHash { get; set; } // Hashed password
    }
}
