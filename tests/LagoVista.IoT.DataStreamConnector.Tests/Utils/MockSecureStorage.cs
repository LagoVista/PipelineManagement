using LagoVista.Core;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.DataStreamConnector.Tests.Utils
{
    public class MockSecureStorage : ISecureStorage
    {
        Dictionary<string, string> _storage = new Dictionary<string, string>();

        public Task<InvokeResult<string>> AddSecretAsync(EntityHeader org, string id, string value)
        {
            _storage.Add(id, value);

            return Task.FromResult(InvokeResult<string>.Create(id));
        }

        public Task<InvokeResult<string>> AddSecretAsync(EntityHeader org, string value)
        {
            return AddSecretAsync(org, Guid.NewGuid().ToId(), value);
        }

        public Task<InvokeResult<string>> GetSecretAsync(EntityHeader org, string id, EntityHeader user)
        {
            if (_storage.ContainsKey(id))
                return Task.FromResult(InvokeResult<string>.Create(_storage[id]));
            else
                return Task.FromResult(InvokeResult<string>.FromError("Could not find key."));
        }

        public Task<InvokeResult> RemoveSecretAsync(EntityHeader org, string id)
        {
            _storage.Remove(id);

            return Task.FromResult( InvokeResult.Success);
        }
    }
}
