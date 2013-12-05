using LinqToNewRelic.Models;

namespace LinqToNewRelic.Implementations
{
    class UserApi : ApiBase, IUserQueries
    {
        public UserApi(string apiKey)
            : base(apiKey) { }

        public User[] GetAllUsers(string accountId)
        {
            return GetDeserializedResult<User[]>(GetNewRelicUrl(accountId, "users.json"));
        }

        public User GetUser(string accountId, int userId)
        {
            var userPath = string.Format("users/{0}.json", userId);
            return GetDeserializedResult<User>(GetNewRelicUrl(accountId, userPath));
        }

        public AlertSetting[] GetUsersAlertSettings(string accountId, int userId)
        {
            var userAlertPath = string.Format("users/{0}/alert_settings.json", userId);
            return GetDeserializedResult<AlertSetting[]>(GetNewRelicUrl(accountId, userAlertPath));
        }

        public AlertSetting GetUsersAlertSettings(string accountId, int userId, int applicationId)
        {
            var userAlertPath = string.Format("users/{0}/alert_settings/{1}.json", userId, applicationId);
            return GetDeserializedResult<AlertSetting>(GetNewRelicUrl(accountId, userAlertPath));
        }
    }
}