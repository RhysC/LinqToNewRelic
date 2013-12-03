using System;
using System.Globalization;
using System.Linq;
using Xunit;

namespace LinqToNewRelic.Tests
{
    public class ApplicationFixture : IUseFixture<ConfigSetUp>
    {
        private ApplicationApi _api;
        private string _accountId;
        private int _applicationId;

        public void SetFixture(ConfigSetUp data)
        {
            _api = new ApplicationApi(data.ApiKey);
            _accountId = data.AccountId;
            _applicationId = data.ApplicationId;
        }

        [Fact]
        public void CanGetApplication()
        {
            var applications = _api.GetApplications(_accountId).ToList();
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
        public void CanGetApplicationInstances()
        {
            var instances = _api.GetApplicationInstances(_accountId, _applicationId).ToList();
            Assert.NotEmpty(instances);
            foreach (var instance in instances)
            {
                Assert.NotNull(instance.Id);
                Assert.NotNull(instance.Name);
                Assert.NotNull(instance.OverviewUrl);
            }
        }

        [Fact]
        public void CanGetApplicationServers()
        {
            var servers = _api.GetApplicationServers(_accountId, _applicationId);
            Assert.NotEmpty(servers);
            foreach (var server in servers)
            {
                Assert.NotEqual(0, server.Id);
                Assert.NotNull(server.HostName);
                Assert.NotNull(server.OverviewUrl);
            }
        }

        [Fact]
        public void CanGetAllApplicationsSettings()
        {
            var settings = _api.GetApplicationSettings(_accountId);
            Assert.NotEmpty(settings);
            foreach (var setting in settings)
            {
                Assert.NotEqual(0, setting.ApplicationId);
                Assert.NotNull(setting.Name);
                Assert.NotNull(setting.Url);
                Assert.True(setting.AppApdexT > 0);
                Assert.True(setting.RumApdexT > 0);
            }
        }
        [Fact]
        public void CanGetAnApplicationsSettings()
        {
            var setting = _api.GetApplicationSettings(_accountId, _applicationId);
            Assert.NotEqual(0, setting.ApplicationId);
            Assert.NotNull(setting.Name);
            Assert.True(setting.AppApdexT > 0);
            Assert.True(setting.RumApdexT > 0);
            Assert.Null(setting.Url);//Not returned in the query
        }

        [Fact]
        public void CanGetAnApplicationsAlertThresholds()
        {
            var thresholds = _api.GetApplicationAlertThresholds(_accountId, _applicationId);
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
            var applicationMetricDefinitions = _api.ApplicationMetricDefinitions(_accountId, _applicationId).ToList();
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
            var metricDataSearchParams = new MetricDataSearchParams
                {
                    StartDate = DateTime.Now.AddDays(-1),
                    EndDate = DateTime.Now,
                    Metrics = new[] { "Custom/Info/Message_Loop_Mercury.Messages.Command_Delay" },
                    MetricField = "call_count"
                };
            var applicationMetricDefinitions = _api.ApplicationMetricData(_accountId, _applicationId, metricDataSearchParams).ToList();
            Assert.NotEmpty(applicationMetricDefinitions);
            foreach (var applicationMetricDefinition in applicationMetricDefinitions)
            {
                Assert.NotNull(applicationMetricDefinition.Begin);
                Assert.NotNull(applicationMetricDefinition.End);
            }
        }

        [Fact]
        public void CanGetApplicationSummaryMetrics()
        {
             var thresholds = _api.ApplicationSummaryMetricData(_accountId, _applicationId);
            Assert.NotNull(thresholds);
            Assert.NotEmpty(thresholds.Values);
            foreach (var threshold in thresholds.Values)
            {
                Assert.NotNull(threshold.Name);
                Assert.NotNull(threshold.FormattedMetricValue);
                Assert.NotNull(threshold.BeginTime);
                Assert.NotNull(threshold.EndTime);
            }
        }

        [Fact]
        public void CanChainQueries()
        {
            var query =
               from application in _api.GetApplications(_accountId)
               where application.Hidden
               where application.Name.ToLower().Contains("command") &&
                     application.Name.ToLower().Contains("mediator")
               from metricDef in _api.ApplicationMetricDefinitions(_accountId, application.Id)
               where metricDef.Name.ToLower().Contains("custom") &&
                     metricDef.Name.ToLower().Contains("delay")
               select _api.ApplicationMetricData(_accountId, application.Id, new MetricDataSearchParams
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
