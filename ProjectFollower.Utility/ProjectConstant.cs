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
            public const string EmptyAvatar = "/images/profile-avatar.jpg";

            public const string RootAsset = "/assets/";
            public const string Customers = "customers/";
            public const string Users = "users/";
            public const string Projects = "projects/";
            public const string Documents = "documents/";
            public const string Img = "img/";

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
        public static class LocFileForWeb
        {
            public const string RootAsset = @"\assets\";
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
        public static class SchedulerHtml
        {
            public const string Before = @"

<!DOCTYPE html>
<html lang='en'>

<head>
    <meta charset='UTF-8'>
    <title>Many Points</title>
    <meta name='viewport' content='width=device-width, initial-scale=1, maximum-scale=1'>

    <!--<meta http-equiv='Cache-Control' content='no-cache, no-store, must-revalidate' />-->
    <meta http-equiv='Pragma' content='no-cache' />
    <meta http-equiv='Expires' content='-1' />

    <link rel='stylesheet' href='~/font/iconsmind-s/css/iconsminds.css' />
    <link rel='stylesheet' type='text/css' href='https://cdn3.devexpress.com/jslib/21.2.4/css/dx.common.css' />
    <link rel='stylesheet' type='text/css' href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css' />
    <link rel='stylesheet' type='text/css' href='~/css/vendor/dx-light.css' />
    <link rel='stylesheet' href='~/css/vendor/dropzone.min.css' />
    <link rel='stylesheet' href='~/css/vendor/quill.snow.css' />
    <link rel='stylesheet' href='~/css/vendor/quill.bubble.css' />
    <link rel='stylesheet' href='~/css/vendor/dataTables.bootstrap4.min.css' />
    <link rel='stylesheet' href='~/css/vendor/datatables.responsive.bootstrap4.min.css' />
    <link rel='stylesheet' href='~/font/simple-line-icons/css/simple-line-icons.css' />
    <link rel='stylesheet' href='~/css/vendor/bootstrap.min.css' />
    <link rel='stylesheet' href='~/css/vendor/bootstrap.rtl.only.min.css' />
    <link rel='stylesheet' href='~/css/vendor/component-custom-switch.min.css' />
    <link rel='stylesheet' href='~/css/vendor/perfect-scrollbar.css' />
    <link rel='stylesheet' href='~/css/vendor/bootstrap-float-label.min.css' />
    <link rel='stylesheet' href='~/css/vendor/select2.min.css' />
    <link rel='stylesheet' href='~/css/vendor/select2-bootstrap.min.css' />
    <link rel='stylesheet' href='~/css/vendor/bootstrap-datepicker3.min.css' />
    <link rel='stylesheet' href='~/css/vendor/bootstrap-tagsinput.css' />
    <link rel='stylesheet' href='~/css/vendor/dropzone.min.css' />
    <link rel='stylesheet' href='~/css/vendor/sweetalert2.min.css' />
    <link rel='stylesheet' href='~/css/vendor/nouislider.min.css' />
    <link rel='stylesheet' href='~/css/vendor/bootstrap-stars.css' />
    <link rel='stylesheet' href='~/css/vendor/cropper.min.css' />
    <link rel='stylesheet' href='~/css/vendor/dataTables.bootstrap4.min.css' />
    <link rel='stylesheet' href='~/css/vendor/datatables.responsive.bootstrap4.min.css' />
    <link rel='stylesheet' href='~/css/vendor/component-custom-switch.min.css' />
    <link rel='stylesheet' href='~/css/vendor/jquery.contextMenu.min.css' />
    <link rel='stylesheet' href='~/css/main.css' />
    <link rel='shortcut icon' href='~/logos/black.svg' />
<script src='~/js/vendor/jquery-3.3.1.min.js'></script>
<script src='~/js/vendor/bootstrap.bundle.min.js'></script>
<script src='~/js/vendor/moment.min.js'></script>
<script src='~/js/vendor/perfect-scrollbar.min.js'></script>
<script src='~/js/vendor/bootstrap-notify.min.js'></script>
<script src='~/js/vendor/select2.full.js'></script>
<script src='~/js/vendor/bootstrap-datepicker.js'></script>
<script src='https://cdn.jsdelivr.net/bootstrap.datepicker-fork/1.3.0/js/locales/bootstrap-datepicker.tr.js'></script>
<script src='~/js/vendor/dropzone.min.js'></script>
<script src='~/js/vendor/bootstrap-tagsinput.min.js'></script>
<script src='~/js/vendor/nouislider.min.js'></script>
<script src='~/js/vendor/jquery.barrating.min.js'></script>
<script src='~/js/vendor/cropper.min.js'></script>
<script src='~/js/vendor/typeahead.bundle.js'></script>
<script src='~/js/vendor/mousetrap.min.js'></script>
<script src='~/js/vendor/mousetrap.min.js'></script>
<script src='~/js/vendor/jquery.contextMenu.min.js'></script>
<script src='~/js/vendor/sweetalert2.min.js'></script>
<script src='~/js/vendor/follower-ezsper.js'></script>
<script src='~/js/dore-plugins/select.from.library.js'></script>
<script src='~/js/dore.script.js'></script>
<script src='~/js/scripts.js'></script>
<script src='https://cdn3.devexpress.com/jslib/21.2.4/js/dx.all.js'></script>
<script src='https://cdn3.devexpress.com/jslib/21.2.4/js/localization/dx.messages.tr.js'></script>
<script src='~/js/site.js'></script>
</head>
<body id='app-container' class='menu-hidden show-spinner'>
";

            public const string After = @"
</body>
</html>
";
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
