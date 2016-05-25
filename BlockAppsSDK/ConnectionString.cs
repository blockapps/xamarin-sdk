using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace BlockAppsSDK
{
    public static class ConnectionString
    {
        public static string BlocUrl { get; set; } = "http://localhost:8000/";
        public static string StratoUrl { get; set; } = "http://strato-dev3.blockapps.net/eth/v1.1/";
    }
}
