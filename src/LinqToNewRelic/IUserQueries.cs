using LinqToNewRelic.Models;

namespace LinqToNewRelic
{
    public interface IUserQueries
    {
        User[] GetAllUsers(string accountId);
        User GetUser(string accountId, int userId);
        AlertSetting[] GetUsersAlertSettings(string accountId, int userId);
        AlertSetting GetUsersAlertSettings(string accountId, int userId, int applicationId);
    }
}