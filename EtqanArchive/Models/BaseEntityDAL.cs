using GenericBackEndCore.Classes.Common;
using Tazkara.DataLayer;
using Microsoft.EntityFrameworkCore;
using EtqanArchive.DataLayer;

namespace EtqanArchive.BackEnd.Models
{
    public class BaseEntityDAL<TDBEntity, TDBViewEntity, TViewEntityDAL> : GenericEntityDAL<TDBEntity, TDBViewEntity, TViewEntityDAL>
        where TDBEntity : class, new()
        where TDBViewEntity : class, new()
        where TViewEntityDAL : class, new()
    {
        public BaseEntityDAL() : base(new EtqanArchiveDBContext())
        {

        }

        public BaseEntityDAL(DbContext context) : base(context)
        {

        }
    }
}
