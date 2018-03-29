using Amazon;
using LagoVista.IoT.Pipeline.Models;
using System;

namespace LagoVista.IoT.DataStreamConnectors
{
    public static class AWSRegionMappings
    {
        public static RegionEndpoint MapRegion(string regionName)
        {
            if(String.IsNullOrEmpty(regionName))
            {
                throw new InvalidOperationException("Must supply a valid region AWS region name.");
            }

            switch (regionName)
            {
                case AWSUtils.USEast1: return RegionEndpoint.USEast1;
                case AWSUtils.CACentral1: return RegionEndpoint.CACentral1;
                case AWSUtils.CNNorthWest1: return RegionEndpoint.CNNorthWest1;
                case AWSUtils.CNNorth1: return RegionEndpoint.CNNorth1;
                case AWSUtils.USGovCloudWest1: return RegionEndpoint.USGovCloudWest1;
                case AWSUtils.SAEast1: return RegionEndpoint.SAEast1;
                case AWSUtils.APSoutheast1: return RegionEndpoint.APSoutheast1;
                case AWSUtils.APSouth1: return RegionEndpoint.APSouth1;
                case AWSUtils.APNortheast2: return RegionEndpoint.APNortheast2;
                case AWSUtils.APSoutheast2: return RegionEndpoint.APSoutheast2;
                case AWSUtils.EUCentral1: return RegionEndpoint.EUCentral1;
                case AWSUtils.EUWest3: return RegionEndpoint.EUWest3;
                case AWSUtils.EUWest2: return RegionEndpoint.EUWest2;
                case AWSUtils.EUWest1: return RegionEndpoint.EUWest1;
                case AWSUtils.USWest2: return RegionEndpoint.USWest2;
                case AWSUtils.USWest1: return RegionEndpoint.USWest1;
                case AWSUtils.USEast2: return RegionEndpoint.USEast2;
                case AWSUtils.APNortheast1: return RegionEndpoint.APNortheast1;
            }

            throw new InvalidOperationException($"Unknown AWS Region {regionName}");
        }
    }
}
