using System;
namespace UserManagementService.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }  // Note: In a real application, passwords should be hashed
        public string Email { get; set; }
    }
}
