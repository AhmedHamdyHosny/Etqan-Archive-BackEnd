using DataLayer.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tazkara.DataLayer.ViewEntity
{
    [NotMapped]
    public class UserView : CommonViewEntity
    {
        [Key]
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string UserAltFullName { get; set; }
        public Guid UserTypeId { get; set; }
        public string UserTypeName { get; set; }
        public string UserTypeAltName { get; set; }
        public bool AllowAccess { get; set; }
        public string AllowAccess_str { get; set; }
        public string ImageURL { get; set; }
        public string ImageContentType { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Gender { get; set; }
        public Guid? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAltName { get; set; }
        public Guid? CityId { get; set; }
        public string CityName { get; set; }
        public string CityAltName { get; set; }
    }
}
