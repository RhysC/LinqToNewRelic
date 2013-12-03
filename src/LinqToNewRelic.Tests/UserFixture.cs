using System.Linq;
using Xunit;

namespace LinqToNewRelic.Tests
{
    public class UserFixture : IUseFixture<ConfigSetUp>
    {
        private UserApi _api;
        private string _accountId;
        public void SetFixture(ConfigSetUp data)
        {
            _api = new UserApi(data.ApiKey);
            _accountId = data.AccountId;
        }

        [Fact]
        public void CanGetAllUsers()
        {
            var users = _api.GetAllUsers(_accountId);
            Assert.NotEmpty(users);
            foreach (var user in users)
            {
                Assert.NotEqual(0, user.Id);
                Assert.NotNull(user.Email);
                Assert.NotNull(user.AlertSettingUrl);
            }
        }
        [Fact]
        public void CanGetUserById()
        {
            var userId = _api.GetAllUsers(_accountId).First().Id;
            var user = _api.GetUser(_accountId, userId);
            Assert.NotEqual(0, user.Id);
            Assert.NotNull(user.Email);
            Assert.NotNull(user.AlertSettingUrl);
        }
        [Fact]
        public void CanGetUsersAlertSettings()
        {
            var userId = _api.GetAllUsers(_accountId).First().Id;
            var alertSettings = _api.GetUsersAlertSettings(_accountId, userId);
            Assert.NotEmpty(alertSettings);
            foreach (var setting in alertSettings)
            {
                Assert.NotEqual(0, setting.ApplicationId);
                Assert.NotNull(setting.ApplicationName);
                Assert.NotNull(setting.Url);
            }
        }
        [Fact]
        public void CanGetUsersAlertSettingsForSpecificaApplication()
        {
            var userId = _api.GetAllUsers(_accountId).First().Id;
            var applicationId = _api.GetUsersAlertSettings(_accountId, userId).First().ApplicationId;
            var setting = _api.GetUsersAlertSettings(_accountId, userId, applicationId);
            Assert.NotEqual(0, setting.ApplicationId);
            Assert.NotNull(setting.ApplicationName);
        }
    }
}