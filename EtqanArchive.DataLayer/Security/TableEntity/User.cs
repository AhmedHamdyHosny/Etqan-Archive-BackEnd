using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DataLayer.Security.TableEntity
{
    public interface IUser
    {
        string UserFullName { get; set; }
        string UserAltFullName { get; set; }
        string PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
    }

    [Table("User", Schema = "security")]
    public partial class User : IdentityUser<Guid>, IUser
    {
        [StringLength(256)]
        public override string PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }

        [StringLength(150), Required]
        public string UserFullName { get; set; }

        [StringLength(150)]
        public string UserAltFullName { get; set; }
        public Guid UserTypeId { get; set; }

        [StringLength(20)]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

        [Column(TypeName = "date")]
        public DateTime? BirthDate { get; set; }

        [StringLength(6)]
        public string Gender { get; set; }

        [DefaultValue(true)]
        public bool AllowAccess { get; set; }

        [StringLength(200)]
        public string ImageURL { get; set; }

        [StringLength(20)]
        public string ImageContentType { get; set; }

        [DefaultValue(false)]
        public bool IsBlock { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        public Guid CreateUserId { get; set; }

        [Column(Order = 503, TypeName = "datetimeoffset(7)")]
        public DateTimeOffset CreateDate { get; set; }
        public Guid? ModifyUserId { get; set; }

        [Column(Order = 503, TypeName = "datetimeoffset(7)")]
        public DateTimeOffset? ModifyDate { get; set; }


        public virtual UserType UserType { get; set; }
        public virtual ICollection<UserClaim> UserClaims { get; set; }
        public virtual ICollection<UserLogin> UserLogins { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserToken> UserTokens { get; set; }
    }
}
