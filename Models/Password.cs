using System;
using System.Collections.Generic;

#nullable disable

namespace MSCuenta.Models
{
    public partial class Password
    {
        public int PasswordId { get; set; }
        public string PasswordString { get; set; }
        public int OwnerId { get; set; }

        public virtual Account Owner { get; set; }
    }
}
