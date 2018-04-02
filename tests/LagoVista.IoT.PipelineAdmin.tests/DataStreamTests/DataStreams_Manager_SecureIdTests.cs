using LagoVista.IoT.Pipeline.Admin;
using LagoVista.IoT.Pipeline.Models;
using System;
using LagoVista.IoT.Pipeline.Admin.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LagoVista.IoT.Pipeline.Admin.Repos;
using Moq;
using System.Threading.Tasks;
using LagoVista.Core;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Validation;
using LagoVista.IoT.Pipeline.Admin.Models;

namespace LagoVista.IoT.PipelineAdmin.tests.DataStreamTests
{
    [TestClass]
    public class DataStreams_Manager_SecureIdTests
    {
        Mock<IDataStreamRepo> _repo = new Moq.Mock<IDataStreamRepo>();
        Mock<ISecureStorage> _secureStorage = new Moq.Mock<ISecureStorage>();

        IDataStreamManager _dataStreamManager;

        Core.Models.EntityHeader _org;
        Core.Models.EntityHeader _user;

        const string GENERATD_SECURE_ID_VALUE = "FB4AE20FE1B841D78804E2C056661079";
        const string OLD_SECURE_ID_VALUE = "8DBA4F3CD1414F08BB6E1DA843FCA8F3";


        [TestInitialize]
        public void TestInit()
        {
            _dataStreamManager = new DataStreamManager(_repo.Object, new Logging.Loggers.AdminLogger(new Utils.LogWriter()), _secureStorage.Object, new Mock<IAppConfig>().Object, new Mock<IDependencyManager>().Object, new Mock<ISecurity>().Object);

            _secureStorage.Setup<Task<Core.Validation.InvokeResult<string>>>(obj => obj.AddSecretAsync(It.IsAny<string>())).ReturnsAsync(InvokeResult<string>.Create(GENERATD_SECURE_ID_VALUE));
            _secureStorage.Setup<Task<Core.Validation.InvokeResult>>(obj => obj.RemoveSecretAsync(It.IsAny<string>())).ReturnsAsync(InvokeResult.Success);

            _org = Core.Models.EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "myorg");
            _user = Core.Models.EntityHeader.Create("14AEF7E53D2043538222FB18C2CA6733", "myuser");
        }


        private DataStream GetDataStream(DataStreamTypes type)
        {
            var stream = new DataStream();
            stream.Id = "A8A087E53D2043538F32FB18C2CA67F7";
            stream.Name = "mystream";
            stream.Key = "streamkey";
            stream.CreationDate = DateTime.Now.ToJSONString();
            stream.LastUpdatedDate = DateTime.Now.ToJSONString();
            stream.CreatedBy = _user;
            stream.LastUpdatedBy = _user;
            stream.OwnerOrganization = _org;
            stream.StreamType = Core.Models.EntityHeader<DataStreamTypes>.Create(type);


            switch (type)
            {
                case DataStreamTypes.AWSElasticSearch:
                    stream.AWSAccessKey = "accesskey";
                    stream.AWSSecretKey = "accesskey";
                    stream.AWSRegion = "us-west-1";
                    stream.ESDomainName = "domain";
                    stream.ESIndexName = "index";
                    stream.ESTypeName = "type";
                    break;
                case DataStreamTypes.AWSS3:
                    stream.AWSAccessKey = "accesskey";
                    stream.AWSSecretKey = "accesskey";
                    stream.AWSRegion = "USWest1";
                    stream.S3BucketName = "mybucket";
                    break;
                case DataStreamTypes.AzureBlob:
                    stream.AzureAccessKey = "accesskey";
                    stream.AzureAccountId = "accountid";
                    stream.AzureBlobStorageContainerName = "blobcontainer";
                    break;
                case DataStreamTypes.AzureEventHub:
                    stream.AzureAccessKey = "accesskey";
                    stream.AzureAccountId = "accountid";
                    stream.AzureEventHubName = "ehname";
                    stream.AzureEventHubEntityPath = "path";
                    break;
                case DataStreamTypes.AzureTableStorage:
                    stream.AzureAccessKey = "accesskey";
                    stream.AzureAccountId = "accountid";
                    stream.AzureTableStorageName = "tablestorage";
                    break;
                case DataStreamTypes.AzureTableStorage_Managed:
                    stream.AzureAccessKey = "accesskey";
                    stream.AzureAccountId = "accountid";
                    stream.AzureTableStorageName = "tablestorage";
                    break;
                case DataStreamTypes.SQLServer:
                    stream.DbName = "mydb";
                    stream.DbPassword = "mypwd";
                    stream.DbTableName = "mytablename";
                    stream.DbURL = "www.foo.com";
                    stream.DbUserName = "myusername";
                    break;
            }
            return stream;
        }

        private async Task AWSInsertAsync(DataStreamTypes streamType)
        {
            var stream = GetDataStream(streamType);
            var originalSecretKey = stream.AWSSecretKey;
            var result = await _dataStreamManager.AddDataStreamAsync(stream, _org, _user);
            Assert.IsTrue(result.Successful);

            _secureStorage.Verify<Task<Core.Validation.InvokeResult<string>>>(obj => obj.AddSecretAsync(originalSecretKey), Times.Once);

            Assert.IsNull(stream.AWSSecretKey);
            Assert.AreEqual(GENERATD_SECURE_ID_VALUE, stream.AWSSecretKeySecureId);
        }

        private async Task AWSUpdateAsync(DataStreamTypes streamType)
        {
            var stream = GetDataStream(streamType);
            stream.AWSSecretKeySecureId = OLD_SECURE_ID_VALUE;
            stream.AWSSecretKey = null;
            var result = await _dataStreamManager.UpdateDataStreamAsync(stream, _org, _user);
            Assert.IsTrue(result.Successful);

            _secureStorage.Verify<Task<Core.Validation.InvokeResult<string>>>(obj => obj.AddSecretAsync(It.IsAny<string>()), Times.Never);
            _secureStorage.Verify<Task<Core.Validation.InvokeResult>>(obj => obj.RemoveSecretAsync(It.IsAny<string>()), Times.Never);

            Assert.IsNull(stream.AWSSecretKey);
            Assert.AreEqual(OLD_SECURE_ID_VALUE, stream.AWSSecretKeySecureId);
        }

        private async Task AWSReplaceKeyAsync(DataStreamTypes streamType)
        {
            var updatedSecretKey = "newlycreatedaccesskey";
            var stream = GetDataStream(streamType);
            stream.AWSSecretKeySecureId = OLD_SECURE_ID_VALUE;
            stream.AWSSecretKey = updatedSecretKey;
            var result = await _dataStreamManager.UpdateDataStreamAsync(stream, _org, _user);
            Assert.IsTrue(result.Successful);

            _secureStorage.Verify<Task<Core.Validation.InvokeResult<string>>>(obj => obj.AddSecretAsync(updatedSecretKey), Times.Once);
            _secureStorage.Verify<Task<Core.Validation.InvokeResult>>(obj => obj.RemoveSecretAsync(OLD_SECURE_ID_VALUE), Times.Once);

            Assert.IsNull(stream.AWSSecretKey);
            Assert.AreEqual(GENERATD_SECURE_ID_VALUE, stream.AWSSecretKeySecureId);
        }

        [TestMethod]
        public async Task DataStream_Manager_AWS_ElasticSearch_Insert_Valid()
        {
            await AWSInsertAsync(DataStreamTypes.AWSElasticSearch);
        }

        [TestMethod]
        public async Task DataStream_Manager_AWS_ElasticSearch_Update_Valid()
        {
            await AWSUpdateAsync(DataStreamTypes.AWSElasticSearch);
        }

        [TestMethod]
        public async Task DataStream_Manager_AWS_ElasticSearch_ReplaceKey_Valid()
        {
            await AWSReplaceKeyAsync(DataStreamTypes.AWSElasticSearch);
        }

        [TestMethod]
        public async Task DataStream_Manager_AWS_S3_Insert_Valid()
        {
            await AWSInsertAsync(DataStreamTypes.AWSS3);
        }

        [TestMethod]
        public async Task DataStream_Manager_AWS_S3_Update_Valid()
        {
            await  AWSUpdateAsync(DataStreamTypes.AWSS3);
        }

        [TestMethod]
        public async Task DataStream_Manager_AWS_S3_ReplaceKey_Valid()
        {
            await AWSUpdateAsync(DataStreamTypes.AWSS3);
        }

        private async Task AzureInsertAsync(DataStreamTypes streamType)
        {
            var stream = GetDataStream(streamType);
            var originalAccessKey = stream.AzureAccessKey;
            var result = await _dataStreamManager.AddDataStreamAsync(stream, _org, _user);
            Assert.IsTrue(result.Successful);
            _secureStorage.Verify<Task<Core.Validation.InvokeResult<string>>>(obj => obj.AddSecretAsync(originalAccessKey), Times.Once);
            Assert.IsNull(stream.AzureAccessKey);
            Assert.AreEqual(GENERATD_SECURE_ID_VALUE, stream.AzureAccessKeySecureId);
        }

        private async Task AzureUpdateAsync(DataStreamTypes streamType)
        {
            var stream = GetDataStream(streamType);
            stream.AzureAccessKeySecureId = OLD_SECURE_ID_VALUE;
            stream.AzureAccessKey = null;
            var result = await _dataStreamManager.UpdateDataStreamAsync(stream, _org, _user);
            Assert.IsTrue(result.Successful);

            _secureStorage.Verify<Task<Core.Validation.InvokeResult<string>>>(obj => obj.AddSecretAsync(It.IsAny<string>()), Times.Never);
            _secureStorage.Verify<Task<Core.Validation.InvokeResult>>(obj => obj.RemoveSecretAsync(It.IsAny<string>()), Times.Never);

            Assert.IsNull(stream.AzureAccessKey);
            Assert.AreEqual(OLD_SECURE_ID_VALUE, stream.AzureAccessKeySecureId);
        }

        private async Task AzureReplaceKeyAsync(DataStreamTypes streamType)
        {
            var updatedAccesKey = "newlycreatedaccesskey";
            var stream = GetDataStream(streamType);
            stream.AzureAccessKeySecureId = OLD_SECURE_ID_VALUE;
            stream.AzureAccessKey = updatedAccesKey;
            var result = await _dataStreamManager.UpdateDataStreamAsync(stream, _org, _user);
            Assert.IsTrue(result.Successful);
            _secureStorage.Verify<Task<Core.Validation.InvokeResult<string>>>(obj => obj.AddSecretAsync(updatedAccesKey), Times.Once);
            _secureStorage.Verify<Task<Core.Validation.InvokeResult>>(obj => obj.RemoveSecretAsync(OLD_SECURE_ID_VALUE), Times.Once);
            Assert.IsNull(stream.AzureAccessKey);
            Assert.AreEqual(GENERATD_SECURE_ID_VALUE, stream.AzureAccessKeySecureId);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureBlob_Insert_Valid()
        {
            await AzureInsertAsync(DataStreamTypes.AzureBlob);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureBlob_Update_Valid()
        {
            await AzureUpdateAsync(DataStreamTypes.AzureBlob);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureBlob_Replace_Valid()
        {
            await AzureReplaceKeyAsync(DataStreamTypes.AzureBlob);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureEventHub_Insert_Valid()
        {
            await AzureInsertAsync(DataStreamTypes.AzureEventHub);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureEventHub_Update_Valid()
        {
            await AzureUpdateAsync(DataStreamTypes.AzureEventHub);
        }


        [TestMethod]
        public async Task DataStream_Manager_AzureEventHub_Replaced_Valid()
        {
            await AzureReplaceKeyAsync(DataStreamTypes.AzureEventHub);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureTableStorage_Insert_Valid()
        {
            await AzureInsertAsync(DataStreamTypes.AzureTableStorage);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureEventTableStorage_Update_Valid()
        {
            await AzureUpdateAsync(DataStreamTypes.AzureTableStorage);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureEventTableStorage_Replace_Valid()
        {
            await AzureReplaceKeyAsync(DataStreamTypes.AzureTableStorage);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureTableStorageManaged_Insert_Valid()
        {
            await AzureInsertAsync(DataStreamTypes.AzureTableStorage_Managed);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureEventTableStorageManaged_Update_Valid()
        {
            await AzureUpdateAsync(DataStreamTypes.AzureTableStorage_Managed);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureEventTableStorageManaged_ReplaceKey_Valid()
        {
            await AzureReplaceKeyAsync(DataStreamTypes.AzureTableStorage_Managed);
        }

        [TestMethod]
        public async Task DataStream_Manager_SQLServer_Insert_Valid()
        {
            var stream = GetDataStream(DataStreamTypes.SQLServer);
            var originalPassword = stream.DbPassword;
            var result = await _dataStreamManager.AddDataStreamAsync(stream, _org, _user);
            Assert.IsTrue(result.Successful);
            _secureStorage.Verify<Task<Core.Validation.InvokeResult<string>>>(obj => obj.AddSecretAsync(originalPassword), Times.Once);
            Assert.IsNull(stream.AzureAccessKey);
            Assert.AreEqual(GENERATD_SECURE_ID_VALUE, stream.DBPasswordSecureId);
        }

        [TestMethod]
        public async Task DataStream_Manager_SQLServer_Update_Valid()
        {
            var stream = GetDataStream(DataStreamTypes.SQLServer);
            stream.DBPasswordSecureId = OLD_SECURE_ID_VALUE;
            stream.DbPassword = null;
            var result = await _dataStreamManager.UpdateDataStreamAsync(stream, _org, _user);
            Assert.IsTrue(result.Successful);

            _secureStorage.Verify<Task<Core.Validation.InvokeResult<string>>>(obj => obj.AddSecretAsync(It.IsAny<string>()), Times.Never);
            _secureStorage.Verify<Task<Core.Validation.InvokeResult>>(obj => obj.RemoveSecretAsync(It.IsAny<string>()), Times.Never);

            Assert.IsNull(stream.DbPassword);
            Assert.AreEqual(OLD_SECURE_ID_VALUE, stream.DBPasswordSecureId);
        }

        [TestMethod]
        public async Task DataStream_Manager_SQLServer_Replace_Valid()
        {
            var updatedPassword = "updatedpassword";
            var stream = GetDataStream(DataStreamTypes.SQLServer);
            stream.DBPasswordSecureId = OLD_SECURE_ID_VALUE;
            stream.DbPassword = updatedPassword;
            var result = await _dataStreamManager.UpdateDataStreamAsync(stream, _org, _user);
            Assert.IsTrue(result.Successful);
            _secureStorage.Verify<Task<Core.Validation.InvokeResult<string>>>(obj => obj.AddSecretAsync(updatedPassword), Times.Once);
            _secureStorage.Verify<Task<Core.Validation.InvokeResult>>(obj => obj.RemoveSecretAsync(OLD_SECURE_ID_VALUE), Times.Once);
            Assert.IsNull(stream.DbPassword);
            Assert.AreEqual(GENERATD_SECURE_ID_VALUE, stream.DBPasswordSecureId);
        }

    }
}
