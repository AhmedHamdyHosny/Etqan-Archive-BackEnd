using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Security.TableEntity
{
    [Table("UserType", Schema = "security")]
    public partial class UserType
    {
        [Column(Order = 0)]
        public Guid UserTypeId { get; set; }

        [Column(Order = 1)]
        [Required]
        [StringLength(50)]
        public string UserTypeName { get; set; }

        [Column(Order = 2)]
        [StringLength(50)]
        public string UserTypeAltName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
