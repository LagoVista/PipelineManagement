using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Pipeline.Admin.Interfaces
{
    public interface IPostgresqlServices
    {
        Task<InvokeResult> CreatePostgresStorage(DataStream stream, String dbPassword, bool forPointStorage);
    }
}
