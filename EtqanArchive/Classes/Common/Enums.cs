

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

        public class ContentType
        {
            public static readonly Guid Other = Guid.Parse("0E2B9825-567D-4BF9-B85E-1AFB032D9C0E");
            public static readonly Guid Video = Guid.Parse("823F9E36-AF47-45FF-A386-526EE38D7C2D");
        }
        public class FileExtension
        {
            public static readonly Guid Unknown = Guid.Parse("62042995-4842-44A9-BD0A-7DD09D246229");
        }
    }

}
