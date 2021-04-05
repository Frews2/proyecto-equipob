using System;
using System.Collections.Generic;

#nullable disable

namespace MSCuenta.Models
{
    public partial class Status
    {
        public Status()
        {
            Accounts = new HashSet<Account>();
        }

        public int StatusId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
