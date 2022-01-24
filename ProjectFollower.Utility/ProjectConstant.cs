using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectFollower.Utility
{
    public static class ProjectConstant
    {
        public static class UserRoles
        {
            public const string Admin = "Yönetici";

            public const string Personel = "Personel";
            public const string Manager = "Yetkili";
        }

        public static class LocFilePaths
        {
            public const string RootAsset = @"C:\project-followerAssets\";
            public const string Customers = @"customers\";
            public const string Users = @"users\";
            public const string Documents = @"documents\";
            public const string Img = @"img\";

            public const string DIR_Customer_Main = RootAsset + Customers;
            public const string DIR_Customer_Doc = RootAsset + Customers + Documents;
            public const string DIR_Customer_Img = RootAsset + Customers + Img;

            public const string DIR_Users_Main = RootAsset + Users;
            public const string DIR_Users_Doc = RootAsset + Users + Documents;
            public const string DIR_Users_Img = RootAsset + Users + Img;
        }
    }
}
