using LinqToNewRelic.Models;

namespace LinqToNewRelic
{
    public interface IServerQueries
    {
        ServerSettings[] GetServerSettings(string accountId);
        ServerSettings GetServerSettings(string accountId, int serverId);
        ServerThreshold[] GetServerAlertThresholds(string accountId, int serverId);
        ServerThreshold GetServerAlertThreshold(string accountId, int serverId, int thresholdId);
    }
}