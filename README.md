LinqToNewRelic
==============

Query your Application metrics in NewRelic

See : https://docs.newrelic.com/docs/features/getting-started-with-the-new-relic-rest-api

To get the Tests to run you will need to your appropriate details into the test project application config.

This project does not currently deal with any non query API calls. 

This project is also poorly named Linq does not feature in it at all. See the ApplicationFixture.CanChainQueries test to how you could compose your queries using linq, however no deferred execution or anything smart is done here.