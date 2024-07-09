namespace BlazorApp3.Client.Modules.Common.Vars
{
    public static class Vars
    {

    }
}

namespace BlazorApp3.Client.Modules.Common.Vars.Http
{

    public static class APIUrl
    {
        public static class FileAPI
        {
            public const string download = "api/file/download";
            public const string upload = "api/file/upload";
            public const string delete = "api/file/delete";
            
        }

        public static class DataBaseAPI
        {
            public const string getData = "api/database/getdata";
        }
    }
}


