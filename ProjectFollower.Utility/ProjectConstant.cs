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
        public static class ProjectStatus
        {
            public const int New = 0;
            public const int CustomerApproving = 1;
            public const int Cunstroction = 2;
            public const int Done = 3;
        }
        public static class LocFilePaths
        {
            //public const string RootAsset = @"C:\project-followerAssets\";
            public const string RootAsset = @"\assets\files\";
            public const string Customers = @"customers\";
            public const string Users = @"users\";
            public const string Projects = @"projects\";
            public const string Documents = @"documents\";
            public const string Img = @"img\";

            public const string DIR_Customer_Main = RootAsset + Customers;
            public const string DIR_Customer_Doc = RootAsset + Customers + Documents;
            public const string DIR_Customer_Img = RootAsset + Customers + Img;

            public const string DIR_Users_Main = RootAsset + Users;
            public const string DIR_Users_Doc = RootAsset + Users + Documents;
            public const string DIR_Users_Img = RootAsset + Users + Img;

            public const string DIR_Projects_Main = RootAsset + Projects;
            public const string DIR_Projects_Doc = RootAsset + Projects + Documents;
            public const string DIR_Projects_Img = RootAsset + Projects + Img;
        }

        public static class WebRootPaths
        {
            public const string RootAsset = @"wwwroot\assets\";
            public const string Customers = @"customers\";
            public const string Users = @"users\";
            public const string Projects = @"projects\";
            public const string Documents = @"documents\";
            public const string Img = @"img\";

            public const string DIR_Customer_Main = RootAsset + Customers;
            public const string DIR_Customer_Doc = RootAsset + Customers + Documents;
            public const string DIR_Customer_Img = RootAsset + Customers + Img;

            public const string DIR_Users_Main = RootAsset + Users;
            public const string DIR_Users_Doc = RootAsset + Users + Documents;
            public const string DIR_Users_Img = RootAsset + Users + Img;

            public const string DIR_Projects_Main = RootAsset + Projects;
            public const string DIR_Projects_Doc = RootAsset + Projects + Documents;
            public const string DIR_Projects_Img = RootAsset + Projects + Img;
        }
        public static class ContentTypes
        {
            public const string Pdf = "application/pdf";
            public const string Rar = "application/vnd.rar";
            public const string Docx = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            public const string Gif = "image/gif";
            public const string Jpeg = "image/jpeg";
            public const string Jpg = "image/jpeg";
            public const string Zip = "application/zip";


        }
        public static void GenerateAsset()
        {
            //Generate assets

        }
    }
}
