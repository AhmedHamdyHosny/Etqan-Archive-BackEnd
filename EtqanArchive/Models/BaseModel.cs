using GenericBackEndCore.Classes.Common;
using Microsoft.EntityFrameworkCore;

namespace EtqanArchive.BackEnd.Models
{
    public class BaseModel<TDBEntity, TDBViewEntity, TEntityDAL> : GenericModel<TDBEntity, TDBViewEntity, TEntityDAL>
    {
        public BaseModel() : base()
        {

        }

        public BaseModel(DbContext context) : base(context)
        {

        }
    }
}
