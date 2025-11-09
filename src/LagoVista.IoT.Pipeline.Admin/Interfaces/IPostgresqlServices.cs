// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: e61e0dedeab2d0b943e31c6326131f58b5313fb94f29f7e79c65c8eb4b753c26
// IndexVersion: 2
// --- END CODE INDEX META ---
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
