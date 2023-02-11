using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Common
{
    public class CommonEntity
    {
        [Column(Order = 500)]
        [System.ComponentModel.DefaultValue(false)]
        public bool IsBlock { get; set; }

        [Column(Order = 501)]
        [System.ComponentModel.DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column(Order = 502)]
        public Guid CreateUserId { get; set; }
        
        [Column(Order = 503, TypeName = "datetimeoffset(7)")]
        public DateTimeOffset CreateDate { get; set; }

        [Column(Order = 504)]
        public Guid? ModifyUserId { get; set; }

        [Column(Order = 505, TypeName = "datetimeoffset(7)")]
        public DateTimeOffset? ModifyDate { get; set; } 
    }

    public class CommonEntityWithNote : CommonEntity
    {
        [Column(Order = 499)]
        [StringLength(200)]
        public virtual string Note { get; set; }
    }

    public class CommonCreatorEntity
    {
        [Column(Order = 500)]
        [System.ComponentModel.DefaultValue(false)]
        public bool IsBlock { get; set; }

        [Column(Order = 501)]
        [System.ComponentModel.DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Column(Order = 502)]
        public Guid? CreateUserId { get; set; }

        [Column(Order = 503, TypeName = "datetimeoffset(7)")]
        public DateTimeOffset CreateDate { get; set; }
    }

    public class CommonBlock_Delete
    {
        [Column(Order = 500)]
        [System.ComponentModel.DefaultValue(false)]
        public bool IsBlock { get; set; }

        [Column(Order = 501)]
        [System.ComponentModel.DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }

    public class CommonBlock
    {
        [Column(Order = 500)]
        [System.ComponentModel.DefaultValue(false)]
        public bool IsBlock { get; set; }
    }
}
