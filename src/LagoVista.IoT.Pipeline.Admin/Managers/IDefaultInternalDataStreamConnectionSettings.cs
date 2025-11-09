// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: d6b426abb5ace13dd090c1af5e939c8fabd8133994285635dc47ee254c0dd702
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Interfaces;

namespace LagoVista.IoT.Pipeline.Admin.Managers
{
    public interface IDefaultInternalDataStreamConnectionSettings
    {
        IConnectionSettings PointArrayConnectionSettings { get; }
        IConnectionSettings GeoSpatialConnectionSettings { get; }

        IConnectionSettings DefaultInternalDataStreamConnectionSettingsTableStorage { get;  }
    }
}
