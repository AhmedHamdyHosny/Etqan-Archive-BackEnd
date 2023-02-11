
namespace DataLayer.Common
{
    public class CommonViewEntity: CommonEntity
    {
        public string IsBlock_str { get; set; }
        public string CreateUser_FullName { get; set; }
        public string CreateUser_FullAltName { get; set; }
        public string ModifyUser_FullName { get; set; }
        public string ModifyUser_FullAltName { get; set; }
    }

    public class CommonViewEntityWithNote : CommonEntityWithNote
    {
        public string IsBlock_str { get; set; }
        public string CreateUser_FullName { get; set; }
        public string CreateUser_FullAltName { get; set; }
        public string ModifyUser_FullName { get; set; }
        public string ModifyUser_FullAltName { get; set; }
    }

    public class CommonViewCreatorEntity : CommonCreatorEntity
    {
        public string IsBlock_str { get; set; }
        public string CreateUser_FullName { get; set; }
        public string CreateUser_FullAltName { get; set; }
    }
}
