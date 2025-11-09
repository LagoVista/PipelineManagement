// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 33ede1c818d69ae93d6a30e19e8dcaac26d665e2e9044203cf819b2f8e8fc9f3
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Models;
using LagoVista.IoT.Pipeline.Admin.Models;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Interfaces
{
    public interface IDataStreamServices
    {
        Task<IDataStreamConnector> GetDataStreamConnectorAsync(DataStream dataStream, EntityHeader instanceUser);
    }
}
