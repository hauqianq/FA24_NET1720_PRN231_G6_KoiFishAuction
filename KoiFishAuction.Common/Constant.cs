﻿namespace KoiFishAuction.Common
{
    public static class Constant
    {
        public const string Token = "Token";
        public static class EndPoint
        {
            public static string APIEndPoint = "https://localhost:7143/api/";
        }

        public static class StatusCode
        {

            public static int SuccessStatusCode = 1;
            public static int FailedStatusCode = -1;

            public static int ERROR_EXEPTION = -1;

            public static int WARNING_NO_DATA = 4;
            public static string WARNING_NO_DATA_MSG = "No data";

            public static int SUCCESS_CREATE = 1;
            public static string SUCCESS_CREATE_MSG = "Create data success";
            public static int SUCCESS_READ = 1;
            public static string SUCCESS_READ_MSG = "Read datasuccess";
            public static int SUCCESS_UDATE = 1;
            public static string SUCCESS_UDATE_MSG = "Update data success";
            public static int SUCCESS_DELETE = 1;
            public static string SUCCESS_DELETE_MSG = "Delete data success";

            public static int FAIL_CREATE = 0;
            public static string FAIL_CREATE_MSG = "Create data fail";
            public static int FAIL_READ = 0;
            public static string FAIL_READ_MSG = "Read data fail";
            public static int FAIL_UDATE = 0;
            public static string FAIL_UDATE_MSG = "Update data fail";
            public static int FAIL_DELETE = 0;
            public static string FAIL_DELETE_MSG = "Delete data fail";
        }
    }
}