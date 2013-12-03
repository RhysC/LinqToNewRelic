using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using Xunit;

namespace LinqToNewRelic.Tests
{
    public class ApplicationFixture
    {
        private readonly string _apiKey;
        private readonly string _accountId;
        private readonly int _applicationId;

        public ApplicationFixture()
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

        [Fact]
        public void CanGetApplication()
        {
            var api = new Api(_apiKey);
            var applications = api.GetApplications(_accountId).ToList();
            Assert.NotEmpty(applications);
            foreach (var application in applications)
            {
                Assert.NotEqual(0, application.Id);
                Assert.NotNull(application.Name);
                Assert.Equal(_accountId, application.AccountId.ToString(CultureInfo.InvariantCulture));
                if (application.Hidden) continue;
                Assert.NotNull(application.AgentSummary);
                Assert.NotEqual(0, application.HealthStatus);
            }
        }

        [Fact]
        public void CanGetApplicationServers()
        {
            var api = new Api(_apiKey);
            var servers = api.GetApplicationServers(_accountId, _applicationId);
            Assert.NotEmpty(servers);
            foreach (var server in servers)
            {
                Assert.NotEqual(0, server.Id);
                Assert.NotNull(server.HostName);
                Assert.NotNull(server.OverviewUrl);
            }
        }
        [Fact]
        public void CanGetAllApplicationServerSettings()
        {
            var api = new Api(_apiKey);
            var settingsList = api.GetServerSettings(_accountId);
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
            var api = new Api(_apiKey);
            var serverId = api.GetServerSettings(_accountId).First().ServerId;
            var settings = api.GetServerSettings(_accountId, serverId);
            Assert.NotEqual(0, settings.ServerId);
            Assert.NotNull(settings.DisplayName);
            Assert.NotNull(settings.HostName);
        } 
        [Fact]
        public void CanGetApplicationServerThresholds()
        {
            var api = new Api(_apiKey);
            var serverId = api.GetServerSettings(_accountId).First().ServerId;
            var thresholds = api.GetServerAlertThresholds(_accountId, serverId);

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
        public void CanGetAllApplicationsSettings()
        {
            var api = new Api(_apiKey);
            var settings = api.GetApplicationSettings(_accountId);
            Assert.NotEmpty(settings);
            foreach (var setting in settings)
            {
                Assert.NotEqual(0, setting.ApplicationId);
                Assert.NotNull(setting.Name);
                Assert.NotNull(setting.Url);
                Assert.True(setting.AppApdexT > -0.1);
                Assert.NotEqual(0, setting.RumApdexT);
            }
        }
        [Fact]
        public void CanGetAnApplicationsSettings()
        {
            var api = new Api(_apiKey);
            var setting = api.GetApplicationSettings(_accountId, _applicationId);
            Assert.NotEqual(0, setting.ApplicationId);
            Assert.NotNull(setting.Name);
            Assert.True(setting.AppApdexT > -0.1);
            Assert.NotEqual(0, setting.RumApdexT);
            Assert.Null(setting.Url);//Not returned in the query
        }

        [Fact]
        public void CanGetAnApplicationsAlertThresholds()
        {
            var api = new Api(_apiKey);
            var thresholds = api.GetApplicationAlertThresholds(_accountId, _applicationId);
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
        public void CanGetApplicationMetricDefinitions()
        {
            var api = new Api(_apiKey);
            var applicationMetricDefinitions = api.ApplicationMetricDefinitions(_accountId, _applicationId).ToList();
            Assert.NotEmpty(applicationMetricDefinitions);
            foreach (var applicationMetricDefinition in applicationMetricDefinitions)
            {
                Assert.NotNull(applicationMetricDefinition.Name);
                Assert.NotEmpty(applicationMetricDefinition.Fields);
            }
        }
        [Fact]
        public void CanGetApplicationMetricData()
        {
            var api = new Api(_apiKey);
            var metricDataSearchParams = new MetricDataSearchParams
                {
                    StartDate = DateTime.Now.AddDays(-1),
                    EndDate = DateTime.Now,
                    Metrics = new[] { "Custom/Info/Message_Loop_Mercury.Messages.Command_Delay" },
                    MetricField = "call_count"
                };
            var applicationMetricDefinitions = api.ApplicationMetricData(_accountId, _applicationId, metricDataSearchParams).ToList();
            Assert.NotEmpty(applicationMetricDefinitions);
            foreach (var applicationMetricDefinition in applicationMetricDefinitions)
            {
                Assert.NotNull(applicationMetricDefinition.Begin);
                Assert.NotNull(applicationMetricDefinition.End);
            }
        }

        [Fact]
        public void CanChainQueries()
        {
            var api = new Api(_apiKey);
            var query =
                from application in api.GetApplications(_accountId)
                where application.Hidden
                where application.Name.ToLower().Contains("command") &&
                      application.Name.ToLower().Contains("mediator")
                from metricDef in api.ApplicationMetricDefinitions(_accountId, application.Id)
                where metricDef.Name.ToLower().Contains("custom") &&
                      metricDef.Name.ToLower().Contains("delay")
                select api.ApplicationMetricData(_accountId, application.Id, new MetricDataSearchParams
                    {
                        StartDate = DateTime.Now.AddDays(-1),
                        EndDate = DateTime.Now,
                        Metrics = new[] { metricDef.Name },
                        MetricField = "call_count"
                    });

            var metricdata = query.ToList();

            Assert.NotEmpty(metricdata);
        }
    }
}
