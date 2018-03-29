using System.Collections.Generic;

namespace LagoVista.IoT.Pipeline.Models
{
    public static class AWSUtils
    {
        public const string USEast1 = "USEast1";
        public const string CACentral1 = "CACentral1";
        public const string CNNorthWest1 = "CNNorthWest1";
        public const string CNNorth1 = "CNNorth1";
        public const string USGovCloudWest1 = "USGovCloudWest1";
        public const string SAEast1 = "SAEast1";
        public const string APSoutheast1 = "APSoutheast1";
        public const string APSouth1 = "APSouth1";
        public const string APNortheast2 = "APNortheast2";
        public const string APSoutheast2 = "APSoutheast2";
        public const string EUCentral1 = "EUCentral1";
        public const string EUWest3 = "EUWest3";
        public const string EUWest2 = "EUWest2";
        public const string EUWest1 = "EUWest1";
        public const string USWest2 = "USWest2";
        public const string USWest1 = "USWest1";
        public const string USEast2 = "USEast2";
        public const string APNortheast1 = "APNortheast1";

        public static List<string> AWSRegions
        {
            get
            {
                return new List<string>() {
                                USEast1,
                                CACentral1,
                                CNNorthWest1,
                                CNNorth1,
                                USGovCloudWest1,
                                SAEast1,
                                APSoutheast1,
                                APSouth1,
                                APNortheast2,
                                APSoutheast2,
                                EUCentral1,
                                EUWest3,
                                EUWest2,
                                EUWest1,
                                USWest2,
                                USWest1,
                                USEast2,
                                APNortheast1
                };
            }
        }
    }
}
