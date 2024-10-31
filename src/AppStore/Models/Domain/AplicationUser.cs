using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace AppStore.Models.Domain
{
    public class AplicationUser : IdentityUser
    {
        public string? Nombre { get; set; }
    }
}