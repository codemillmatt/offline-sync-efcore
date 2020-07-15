using System;

namespace FitApp.Core
{
    public static class Constants
    {
        public static readonly string UserIdPreference = "userid";

#error Enter your website name
        public static readonly string WebServerBaseUrl = "ENTER YOUR WEB SERVICE URL HERE";

        public static readonly string DataSyncReasonFull = "Full";
        public static readonly string DataSyncReasonDiff = "Diff";

        public const string DataOpInsert = "I";
        public const string DataOpUpdate = "U";
        public const string DataOpDelete = "D";
    }
}
