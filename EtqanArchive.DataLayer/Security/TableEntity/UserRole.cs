using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DataLayer.Security.TableEntity
{
    [Table("UserRole", Schema = "security")]
    public partial class UserRole : IdentityUserRole<Guid>
    {

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
