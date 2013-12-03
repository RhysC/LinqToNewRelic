using LinqToNewRelic.Models;

namespace LinqToNewRelic
{
    public class ServerApi : ApiBase
    {
        public ServerApi(string apiKey) : base(apiKey) { }

        public ServerSettings[] GetServerSettings(string accountId)
        {
            return GetDeserializedResult<ServerSettings[]>(GetNewRelicUrl(accountId, "server_settings.json"));
        }
        public ServerSettings GetServerSettings(string accountId, int serverId)
        {
            var settingsPath = string.Format("server_settings/{0}.json", serverId);
            return GetDeserializedResult<ServerSettings>(GetNewRelicUrl(accountId, settingsPath));
        }
        public ServerThreshold[] GetServerAlertThresholds(string accountId, int serverId)
        {
            var thresholdPath = string.Format("servers/{0}/thresholds.json", serverId);
            return GetDeserializedResult<ServerThreshold[]>(GetNewRelicUrl(accountId, thresholdPath));
        }
        public ServerThreshold GetServerAlertThreshold(string accountId, int serverId, int thresholdId)
        {
            var thresholdPath = string.Format("servers/{0}/thresholds/{1}.json", serverId, thresholdId);
            return GetDeserializedResult<ServerThreshold>(GetNewRelicUrl(accountId, thresholdPath));
        }
    }
}