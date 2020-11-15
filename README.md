Infotrack SEO Analytics App
=================

Shows App is an API that returns SEO Analytics.
The CEO is very interested in SEO and how this can improve Sales. Every morning he logs in to his favourite search engine and types in the same key words "online title search" and counts down to see where and how many times their company, https://www.infotrack.com.au sits on the list.
To make this task less tedious a small web-based application for him that will automatically perform this operation and return the result to the screen. 

### To Run API:

* cd InfoTrackSEOAnalytics folder 
* start .sln solution in visual studio.
* Deployed Swagger: https://infotrackseoanalyticsweb20201116075507.azurewebsites.net/swagger
* Deployed API: https://infotrackseoanalyticsweb20201116075507.azurewebsites.net/


### To Run SPA:

* cd InfoTrackSEOAnalyticsSPA
* npm i
* npm start
* Deployed SPA: https://seotrackerblobs.z19.web.core.windows.net/

### To Run tests:

* Build and run all tests in Visual Studio.


### Architecture:

* WebAPI used in .NET core 3.1.
* DI used for IOC.
* xUnit used as test framework.
* Shouldly used to write test conditions in BDD fashion.
* Moq used as mocking framework.

### Known issues:

* SearchMultiplePagesTest test fails as mocked IHttpClientFactory does not support multiple instances of HttpClient to be generated yet.

### Possible Improvements:

* Setup Dev, Staging, PROD environments (config changes, CI/CD) 
* Integration tests
* React test/snapshots
* Make models for paginated results + DTOs
* Automated UAT (selenium/fluent automation)
* BDD-FY test cases using BDDFY to make test cases more readable.
* Replace dynamic typing with stronger typing if request field types can be determined.
* More detailed error handling.
