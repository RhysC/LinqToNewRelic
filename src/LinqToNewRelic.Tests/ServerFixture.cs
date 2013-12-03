using System.Linq;
using Xunit;

namespace LinqToNewRelic.Tests
{
    public class ServerFixture : IUseFixture<ConfigSetUp>
    {
        private ServerApi _api;
        private string _accountId;

        public void SetFixture(ConfigSetUp data)
        {
            _api = new ServerApi(data.ApiKey);
            _accountId = data.AccountId;
        }
        [Fact]
        public void CanGetAllApplicationServerSettings()
        {
            var settingsList = _api.GetServerSettings(_accountId);
            Assert.NotEmpty(settingsList);
            foreach (var settings in settingsList)
            {
                Assert.NotEqual(0, settings.ServerId);
                Assert.NotNull(settings.DisplayName);
                Assert.NotNull(settings.HostName);
                Assert.NotNull(settings.Url);
            }
        }
        [Fact]
        public void CanGetApplicationServerSettings()
        {
            var serverId = _api.GetServerSettings(_accountId).First().ServerId;
            var settings = _api.GetServerSettings(_accountId, serverId);
            Assert.NotEqual(0, settings.ServerId);
            Assert.NotNull(settings.DisplayName);
            Assert.NotNull(settings.HostName);
        }
        [Fact]
        public void CanGetApplicationServerThresholds()
        {
            var serverId = _api.GetServerSettings(_accountId).First().ServerId;
            var thresholds = _api.GetServerAlertThresholds(_accountId, serverId);

            foreach (var threshold in thresholds)
            {
                Assert.NotEqual(0, threshold.Id);
                Assert.NotNull(threshold.Type);
                Assert.NotNull(threshold.Url);
                Assert.NotEqual(0, threshold.CautionValue);
                Assert.NotEqual(0, threshold.CriticalValue);
            }

        }
        [Fact]
        public void CanGetApplicationServerThresholdById()
        {
            var serverId = _api.GetServerSettings(_accountId).First().ServerId;
            var thresholdId = _api.GetServerAlertThresholds(_accountId, serverId).First().Id;
            var threshold = _api.GetServerAlertThreshold(_accountId, serverId, thresholdId);
            Assert.NotEqual(0, threshold.Id);
            Assert.NotNull(threshold.Type);
            Assert.NotEqual(0, threshold.CautionValue);
            Assert.NotEqual(0, threshold.CriticalValue);
        }
    }
}