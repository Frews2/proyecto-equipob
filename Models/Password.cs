using System;
using System.Collections.Generic;

#nullable disable

namespace MSCuenta.Models
{
    public partial class Password
    {
        public string PasswordId { get; set; }
        public string PasswordString { get; set; }
        public string OwnerId { get; set; }

        public virtual Account Owner { get; set; }
    }
}
