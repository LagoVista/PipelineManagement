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

        public const string ESUSEast2 = "us-east-2";
        public const string ESUSEast1 = "us-east-1";
        public const string ESUSWest1 = "us-west-1";
        public const string ESUSWest2 = "us-west-2";
        public const string ESAPNorthEast1 = "ap-northeast-1";
        public const string ESAPNorthEast2 = "ap-northeast-2";
        public const string ESAPSouth1 = "ap-south-1";
        public const string ESAPSouthEast1 = "ap-southeast-1";
        public const string ESSouthEast2 = "ap-southeast-2";
        public const string ESCACenteral1 = "ca-central-1";
        public const string ESCNNorthWest1 = "cn-northwest-1";
        public const string ESUCenteral1 = "eu-central-1";
        public const string ESEUWest1 = "eu-west-1";
        public const string ESEUWest2 = "eu-west-2";
        public const string ESEUWest3 = "eu-west-3";
        public const string ESSAEast1 = "sa-east-1";
        public const string ESUSGovWest1 = "us-gov-west-1";

        public static List<string> AWSS3Regions
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

        public static List<string> AWSESRegions
        {
            get
            {
                return new List<string>()
                {
                    ESUSEast2,
                    ESUSEast1,
                    ESUSWest1,
                    ESUSWest2,
                    ESAPNorthEast1,
                    ESAPNorthEast2,
                    ESAPSouth1,
                    ESAPSouthEast1,
                    ESSouthEast2,
                    ESCACenteral1,
                    ESCNNorthWest1,
                    ESUCenteral1,
                    ESEUWest1,
                    ESEUWest2,
                    ESEUWest3,
                    ESSAEast1,
                    ESUSGovWest1
                };
            }
        }
    }
}
