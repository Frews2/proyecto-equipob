using System;
using System.Collections.Generic;

#nullable disable

namespace MSCuenta.Models
{
    public partial class Account
    {
        public Account()
        {
            Passwords = new HashSet<Password>();
        }

        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public sbyte IsUser { get; set; }
        public int StatusId { get; set; }

        public virtual Status Status { get; set; }
        public virtual ICollection<Password> Passwords { get; set; }
    }
}
