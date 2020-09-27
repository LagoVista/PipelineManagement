using LagoVista.Core.Interfaces;

namespace LagoVista.IoT.Pipeline.Admin.Managers
{
    public interface IDefaultInternalDataStreamConnectionSettings
    {
        IConnectionSettings PointArrayConnectionSettings { get; }

        IConnectionSettings DefaultInternalDataStreamConnectionSettingsTableStorage { get;  }
    }
}
