using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TingStore.Client.Areas.Admin.Models.Users
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
    }
}
