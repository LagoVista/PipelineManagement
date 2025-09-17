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
