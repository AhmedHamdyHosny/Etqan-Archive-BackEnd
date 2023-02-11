

using System;

namespace Classes.Common
{

    public class DBEnums
    {
        public class UserType
        {
            public static readonly Guid Super_System_Administrator = Guid.Parse("9491BE11-F8DA-4B5A-BAB4-224311824C6A");
            public static readonly Guid System_Administrator = Guid.Parse("B0CB460C-4076-4EDC-B37A-F16ADABCCBB6");
        }

        public class Roles
        {
            public const string Admin = "Admin";
        }
    }

}
