using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.IoT.Pipeline.Admin.Managers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.CloudRepos
{
    public class DefaultInternalDataStreamConnectionSettings : IDefaultInternalDataStreamConnectionSettings
    {
        public IConnectionSettings PointArrayConnectionSettings { get; }

        public IConnectionSettings GeoSpatialConnectionSettings { get; }

        public IConnectionSettings DefaultInternalDataStreamConnectionSettingsTableStorage { get; }

        public DefaultInternalDataStreamConnectionSettings(IConfiguration configuration)
        {
            var paSection = configuration.GetRequiredSection("PointArrayStorage");
            PointArrayConnectionSettings = new ConnectionSettings()
            {
                Uri = paSection.Require("Uri"),
                UserName = paSection.Require("UserId"),
                Password = paSection.Require("Password")
            };

            var geoSection = configuration.GetRequiredSection("GeoStorage");
            GeoSpatialConnectionSettings = new ConnectionSettings()
            {
                Uri = geoSection.Require("Uri"),
                UserName = geoSection.Require("UserId"),
                Password = geoSection.Require("Password")
            };

            DefaultInternalDataStreamConnectionSettingsTableStorage = configuration.CreateDefaultTableStorageSettings();
        }
    }
}
