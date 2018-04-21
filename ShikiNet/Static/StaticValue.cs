using System;
using System.Collections.Generic;
using System.Text;

namespace ShikiNet.Static
{
    public static class StaticValue
    {
        public const int MAX_PAGE_COUNT = 10000; //for search
        public const int MAX_LIMIT = 10000;

        public const string SITE_DOMAIN = "https://shikimori.org";
        public const string API_DOMAIN = SITE_DOMAIN + "/api";

        public const string DEFAULT_APP_NAME = "ShikiNet";
        public const string DEFAULT_DEV_NAME = "MrModest";
    }
}
