using System;
using System.Configuration;

namespace LinqToNewRelic.Tests
{
    public class ConfigSetUp
    {
        private readonly string _apiKey;
        private readonly string _accountId;
        private readonly int _applicationId;

        public ConfigSetUp()
        {
            if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["NewRelicApiKey"]) ||
                string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["NewRelicAccountId"]) ||
                string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["NewRelicApplicationId"]))
            {
                throw new InvalidOperationException("You need to assign the app setting in the test appconfig of the test project with your details. See https://docs.newrelic.com/docs/features/getting-started-with-the-new-relic-rest-api");
            }

            _apiKey = ConfigurationManager.AppSettings["NewRelicApiKey"];
            _accountId = ConfigurationManager.AppSettings["NewRelicAccountId"];
            _applicationId = int.Parse(ConfigurationManager.AppSettings["NewRelicApplicationId"]);
        }

        public string ApiKey
        {
            get { return _apiKey; }
        }

        public string AccountId
        {
            get { return _accountId; }
        }

        public int ApplicationId
        {
            get { return _applicationId; }
        }
    }
}