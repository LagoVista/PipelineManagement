// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 183b52c5282232bfc46432374028bc507bc727c5a81558ae277bfeafda98f3e1
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.IoT.Pipeline.Admin;
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
using LagoVista.Core.Models;
using LagoVista.UserAdmin;
using System.Diagnostics;
using LagoVista.IoT.Pipeline.Admin.Interfaces;

namespace LagoVista.IoT.PipelineAdmin.tests.DataStreamTests
{
    [TestClass]
    public class DataStreams_Manager_SecureIdTests
    {
        Mock<IDataStreamRepo> _repo = new Moq.Mock<IDataStreamRepo>();
        Mock<ISecureStorage> _secureStorage = new Moq.Mock<ISecureStorage>();
        Mock<IOrgUtils> _orgUtils = new Moq.Mock<IOrgUtils>();

        Mock<IDefaultInternalDataStreamConnectionSettings> _mockSettings = new Moq.Mock<IDefaultInternalDataStreamConnectionSettings>();

        IDataStreamManager _dataStreamManager;

        Core.Models.EntityHeader _org;
        Core.Models.EntityHeader _user;

        const string GENERATD_SECURE_ID_VALUE = "FB4AE20FE1B841D78804E2C056661079";
        const string OLD_SECURE_ID_VALUE = "8DBA4F3CD1414F08BB6E1DA843FCA8F3";

        const string DEFAULT_TS_ACCOUNT_ID = "TSACCOUNTID";
        const string DEFAULT_TS_ACCESS_KEY = "TSACCESSKEY";


        [TestInitialize]
        public void TestInit()
        {
            _dataStreamManager = new DataStreamManager(_repo.Object, new Mock<IPostgresqlServices>().Object, new Mock<ISharedConnectionManager>().Object, _mockSettings.Object, _orgUtils.Object, new Logging.Loggers.AdminLogger(new Utils.LogWriter()), _secureStorage.Object, new Mock<IAppConfig>().Object, new Mock<IDependencyManager>().Object, new Mock<ISecurity>().Object);

            _mockSettings.Setup(ms => ms.DefaultInternalDataStreamConnectionSettingsTableStorage).Returns(new ConnectionSettings() { AccessKey = DEFAULT_TS_ACCESS_KEY, AccountId = DEFAULT_TS_ACCOUNT_ID });

            _org = Core.Models.EntityHeader.Create("A8A087E53D2043538F32FB18C2CA67F7", "myorg");
            _user = Core.Models.EntityHeader.Create("14AEF7E53D2043538222FB18C2CA6733", "myuser");

            _secureStorage.Setup<Task<Core.Validation.InvokeResult<string>>>(obj => obj.AddSecretAsync(_org, It.IsAny<string>())).ReturnsAsync(InvokeResult<string>.Create(GENERATD_SECURE_ID_VALUE));
            _secureStorage.Setup<Task<Core.Validation.InvokeResult>>(obj => obj.RemoveSecretAsync(_org, It.IsAny<string>())).ReturnsAsync(InvokeResult.Success);
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
                    stream.AwsAccessKey = "accesskey";
                    stream.AwsSecretKey = "accesskey";
                    stream.AwsRegion = "us-west-1";
                    stream.ElasticSearchDomainName = "https://www.foo.com";
                    stream.ElasticSearchIndexName = "index";
                    stream.ElasticSearchTypeName = "type";
                    break;
                case DataStreamTypes.AWSS3:
                    stream.AwsAccessKey = "accesskey";
                    stream.AwsSecretKey = "accesskey";
                    stream.AwsRegion = "USWest1";
                    stream.S3BucketName = "mybucket";
                    break;
                case DataStreamTypes.AzureBlob:
                    stream.AzureAccessKey = "accesskey";
                    stream.AzureStorageAccountName = "accountid";
                    stream.AzureBlobStorageContainerName = "blobcontainer";
                    break;
                case DataStreamTypes.AzureEventHub:
                    stream.AzureAccessKey = "accesskey";
                    stream.AzureStorageAccountName = "accountid";
                    stream.AzureEventHubName = "ehname";
                    stream.AzureEventHubEntityPath = "path";
                    break;
                case DataStreamTypes.AzureTableStorage:
                    stream.AzureAccessKey = "accesskey";
                    stream.AzureStorageAccountName = "accountid";
                    stream.AzureTableStorageName = "tablestorage";
                    break;
                case DataStreamTypes.AzureTableStorage_Managed:
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

        private async Task AWSInsertAsync(EntityHeader org, DataStreamTypes streamType)
        {
            var stream = GetDataStream(streamType);
            var originalSecretKey = stream.AwsSecretKey;
            var result = await _dataStreamManager.AddDataStreamAsync(stream, _org, _user);
            Assert.IsTrue(result.Successful);

            _secureStorage.Verify<Task<Core.Validation.InvokeResult<string>>>(obj => obj.AddSecretAsync(org, originalSecretKey), Times.Once);

            Assert.IsNull(stream.AwsSecretKey);
            Assert.AreEqual(GENERATD_SECURE_ID_VALUE, stream.AWSSecretKeySecureId);
        }

        private async Task AWSUpdateAsync(EntityHeader org, DataStreamTypes streamType)
        {
            var stream = GetDataStream(streamType);
            stream.AWSSecretKeySecureId = OLD_SECURE_ID_VALUE;
            stream.AwsSecretKey = null;
            var result = await _dataStreamManager.UpdateDataStreamAsync(stream, _org, _user);
            Assert.IsTrue(result.Successful);

            _secureStorage.Verify<Task<Core.Validation.InvokeResult<string>>>(obj => obj.AddSecretAsync(It.IsAny<EntityHeader>(), It.IsAny<string>()), Times.Never);
            _secureStorage.Verify<Task<Core.Validation.InvokeResult>>(obj => obj.RemoveSecretAsync(It.IsAny<EntityHeader>(), It.IsAny<string>()), Times.Never);

            Assert.IsNull(stream.AwsSecretKey);
            Assert.AreEqual(OLD_SECURE_ID_VALUE, stream.AWSSecretKeySecureId);
        }

        private async Task AWSReplaceKeyAsync(EntityHeader org, DataStreamTypes streamType)
        {
            var updatedSecretKey = "newlycreatedaccesskey";
            var stream = GetDataStream(streamType);
            stream.AWSSecretKeySecureId = OLD_SECURE_ID_VALUE;
            stream.AwsSecretKey = updatedSecretKey;
            var result = await _dataStreamManager.UpdateDataStreamAsync(stream, _org, _user);
            Assert.IsTrue(result.Successful);

            _secureStorage.Verify<Task<Core.Validation.InvokeResult<string>>>(obj => obj.AddSecretAsync(org, updatedSecretKey), Times.Once);
            _secureStorage.Verify<Task<Core.Validation.InvokeResult>>(obj => obj.RemoveSecretAsync(org, OLD_SECURE_ID_VALUE), Times.Once);

            Assert.IsNull(stream.AwsSecretKey);
            Assert.AreEqual(GENERATD_SECURE_ID_VALUE, stream.AWSSecretKeySecureId);
        }

        [TestMethod]
        public async Task DataStream_Manager_AWS_ElasticSearch_Insert_Valid()
        {
            await AWSInsertAsync(_org, DataStreamTypes.AWSElasticSearch);
        }

        [TestMethod]
        public async Task DataStream_Manager_AWS_ElasticSearch_Update_Valid()
        {
            await AWSUpdateAsync(_org, DataStreamTypes.AWSElasticSearch);
        }

        [TestMethod]
        public async Task DataStream_Manager_AWS_ElasticSearch_ReplaceKey_Valid()
        {
            await AWSReplaceKeyAsync(_org, DataStreamTypes.AWSElasticSearch);
        }

        [TestMethod]
        public async Task DataStream_Manager_AWS_S3_Insert_Valid()
        {
            await AWSInsertAsync(_org, DataStreamTypes.AWSS3);
        }

        [TestMethod]
        public async Task DataStream_Manager_AWS_S3_Update_Valid()
        {
            await AWSUpdateAsync(_org, DataStreamTypes.AWSS3);
        }

        [TestMethod]
        public async Task DataStream_Manager_AWS_S3_ReplaceKey_Valid()
        {
            await AWSUpdateAsync(_org, DataStreamTypes.AWSS3);
        }

        private async Task AzureInsertAsync(EntityHeader org, DataStreamTypes streamType)
        {
            var stream = GetDataStream(streamType);
            var originalAccessKey = stream.AzureAccessKey;
            var result = await _dataStreamManager.AddDataStreamAsync(stream, _org, _user);
            Assert.IsTrue(result.Successful);

            if (streamType == DataStreamTypes.AzureTableStorage_Managed)
            {
                Assert.AreEqual(DEFAULT_TS_ACCOUNT_ID, stream.AzureStorageAccountName);
                _secureStorage.Verify<Task<Core.Validation.InvokeResult<string>>>(obj => obj.AddSecretAsync(org, DEFAULT_TS_ACCESS_KEY), Times.Once);
            }
            else
            {
                _secureStorage.Verify<Task<Core.Validation.InvokeResult<string>>>(obj => obj.AddSecretAsync(org, originalAccessKey), Times.Once);
            }

            Assert.IsNull(stream.AzureAccessKey);

            Assert.AreEqual(GENERATD_SECURE_ID_VALUE, stream.AzureAccessKeySecureId);
        }

        private async Task AzureUpdateAsync(EntityHeader org, DataStreamTypes streamType)
        {
            var stream = GetDataStream(streamType);
            stream.AzureAccessKeySecureId = OLD_SECURE_ID_VALUE;
            stream.AzureAccessKey = null;
            var result = await _dataStreamManager.UpdateDataStreamAsync(stream, _org, _user);
            Assert.IsTrue(result.Successful);

            _secureStorage.Verify<Task<Core.Validation.InvokeResult<string>>>(obj => obj.AddSecretAsync(org, It.IsAny<string>()), Times.Never);
            _secureStorage.Verify<Task<Core.Validation.InvokeResult>>(obj => obj.RemoveSecretAsync(org, It.IsAny<string>()), Times.Never);

            Assert.IsNull(stream.AzureAccessKey);
            Assert.AreEqual(OLD_SECURE_ID_VALUE, stream.AzureAccessKeySecureId);
        }

        private async Task AzureReplaceKeyAsync(EntityHeader org, DataStreamTypes streamType)
        {
            var updatedAccesKey = "newlycreatedaccesskey";
            var stream = GetDataStream(streamType);
            stream.AzureAccessKeySecureId = OLD_SECURE_ID_VALUE;
            stream.AzureAccessKey = updatedAccesKey;
            var result = await _dataStreamManager.UpdateDataStreamAsync(stream, _org, _user);
            Assert.IsTrue(result.Successful);
            _secureStorage.Verify<Task<Core.Validation.InvokeResult<string>>>(obj => obj.AddSecretAsync(org, updatedAccesKey), Times.Once);
            _secureStorage.Verify<Task<Core.Validation.InvokeResult>>(obj => obj.RemoveSecretAsync(org, OLD_SECURE_ID_VALUE), Times.Once);
            Assert.IsNull(stream.AzureAccessKey);
            Assert.AreEqual(GENERATD_SECURE_ID_VALUE, stream.AzureAccessKeySecureId);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureBlob_Insert_Valid()
        {
              await AzureInsertAsync(_org, DataStreamTypes.AzureBlob);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureBlob_Update_Valid()
        {
            await AzureUpdateAsync(_org, DataStreamTypes.AzureBlob);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureBlob_Replace_Valid()
        {
            await AzureReplaceKeyAsync(_org, DataStreamTypes.AzureBlob);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureEventHub_Insert_Valid()
        {
            await AzureInsertAsync(_org, DataStreamTypes.AzureEventHub);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureEventHub_Update_Valid()
        {
            await AzureUpdateAsync(_org, DataStreamTypes.AzureEventHub);
        }


        [TestMethod]
        public async Task DataStream_Manager_AzureEventHub_Replaced_Valid()
        {
            await AzureReplaceKeyAsync(_org, DataStreamTypes.AzureEventHub);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureTableStorage_Insert_Valid()
        {
            await AzureInsertAsync(_org, DataStreamTypes.AzureTableStorage);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureEventTableStorage_Update_Valid()
        {
            await AzureUpdateAsync(_org, DataStreamTypes.AzureTableStorage);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureEventTableStorage_Replace_Valid()
        {
            await AzureReplaceKeyAsync(_org, DataStreamTypes.AzureTableStorage);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureTableStorageManaged_Insert_Valid()
        {
            await AzureInsertAsync(_org, DataStreamTypes.AzureTableStorage_Managed);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureEventTableStorageManaged_Update_Valid()
        {
            await AzureUpdateAsync(_org, DataStreamTypes.AzureTableStorage_Managed);
        }

        [TestMethod]
        public async Task DataStream_Manager_AzureEventTableStorageManaged_ReplaceKey_Valid()
        {
            await AzureReplaceKeyAsync(_org, DataStreamTypes.AzureTableStorage_Managed);
        }

        [TestMethod]
        public async Task DataStream_Manager_SQLServer_Insert_Valid()
        {
            var stream = GetDataStream(DataStreamTypes.SQLServer);
            var originalPassword = stream.DbPassword;
            var result = await _dataStreamManager.AddDataStreamAsync(stream, _org, _user);
            Assert.IsTrue(result.Successful);
            _secureStorage.Verify<Task<Core.Validation.InvokeResult<string>>>(obj => obj.AddSecretAsync(_org, originalPassword), Times.Once);
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

            _secureStorage.Verify<Task<Core.Validation.InvokeResult<string>>>(obj => obj.AddSecretAsync(_org, It.IsAny<string>()), Times.Never);
            _secureStorage.Verify<Task<Core.Validation.InvokeResult>>(obj => obj.RemoveSecretAsync(_org, It.IsAny<string>()), Times.Never);

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
            _secureStorage.Verify<Task<Core.Validation.InvokeResult<string>>>(obj => obj.AddSecretAsync(_org, updatedPassword), Times.Once);
            _secureStorage.Verify<Task<Core.Validation.InvokeResult>>(obj => obj.RemoveSecretAsync(_org, OLD_SECURE_ID_VALUE), Times.Once);
            Assert.IsNull(stream.DbPassword);
            Assert.AreEqual(GENERATD_SECURE_ID_VALUE, stream.DBPasswordSecureId);
        }

    }
}
