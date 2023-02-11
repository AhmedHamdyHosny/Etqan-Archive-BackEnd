
namespace DataLayer.Common
{
    public interface ICommonViewEntity
    {
        string IsBlock_str { get; set; }
        string IsDeleted_str { get; set; }
        string CreateUser_FullName { get; set; }
        string CreateUser_FullAltName { get; set; }
        string ModifyUser_FullName { get; set; }
        string ModifyUser_FullAltName { get; set; }
    }
}
