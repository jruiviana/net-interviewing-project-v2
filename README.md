# Net Interview Project José Rui
## Task 1
- The issue is in the  If statement of the insurance calculation. We need to move the if that verify the product type to out side the else statement:

```c#
if (toInsure.SalesPrice < 500)
    toInsure.InsuranceValue = 0;
else
{
    if (toInsure.SalesPrice > 500 && toInsure.SalesPrice < 2000)
        if (toInsure.ProductTypeHasInsurance)
            toInsure.InsuranceValue += 1000;
    if (toInsure.SalesPrice >= 2000)
        if (toInsure.ProductTypeHasInsurance)
            toInsure.InsuranceValue += 2000;
}
if (toInsure.ProductTypeName == "Laptops" || toInsure.ProductTypeName == "Smartphones" && toInsure.ProductTypeHasInsurance)
    toInsure.InsuranceValue += 500;
```
## Task 2
### Assumption/Decision Made
- 1 - Migrate the project to .Net 5
- 2 - Split the solution in 4 projects.
### Reason
- 1 - I'm using Mac and in my local environment I have only .Net 5 and .Net 6 installed. Another reason is that I think that we need to update to a new version everytime we havea oprotunity
- 2 - In my opinion this is better to separate the boundaries of the application.
### Insurance.Api Project
#### Assumption/Decision Made
- 1 - I change the existing endpoint to to be a get instead of post
- 2 - I moved all the business logic to the BusinessRule project
- 3 - I change the name of the controller from HomeController to InsuranceController
- 4 - I Added a swagger documentaion to the API
- 5 - I Added a Authentication based on Api Key and added this key in the appsetings
- 6 - I added a ErrorHandling middleware.
- 7 - I added a Serilog library and configuration
- 8 - I added a appseting for production
- 9 - I added Dependency Injection configuration in the APi
- 10 -I added  Polly and Polly.Extensions.Http libraries https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly
- 11- I Configured the Api to get the configuration from the appseting files and removed thr ProductApi const from the homeController
#### Reason
- 1 - I prefere this approach, so the Api is more restfull in my opinion
- 2 - The api is not the correct place for the business logic. In my opinion The Api should have only the the endpoint.
- 3 - I think HomeController do not express the function of the controller
- 4 - The swagger documentation will help us our customer to consume our Api
- 5 - the Api was open for everyone. I think this not secury. So I added a APi Key Authentication. for this key is in the appsettings, but a better approach is move this to a database.
- 6 - The Error Handling middleware is responsible to catch the exceptions and format in a better way for the consumer.
- 7 - I thin that the Api should have a log system, so we can verify the issues in production if we need. For now it is send the log to console, but the library permit we send the log for an Elasticsearch for instance.
- 8 - Now it is possible we have diferent confoguration when we run the app in a production environment
- 9 - Before the Api was not prepered to work with Dependency injection. Dependency Inversion  is on of the SOLID principles
- 10 - With this libraries I can have a retry patern for the HttpClient. So if the ProductApi is down or have error, the Httpclient will try two times befor  throw a error. This is good because the call issue could be because a network latency problem.
- 11 - This type of configiguration should be in the appsettings file
### Insurance.BusinessRule Project
#### Assumption/Decision Made
- 1 - Today in my company we develop our applications using CQRS (commands and queries), I not used this approach here I prefere use the service approach.
- 2 - I split the business logic in two service, ProductApiService ans InsuranceService
- 3 - I moved the Models adn exceptions classes to the Insurance.Core project
- 4 - I creates interfaces for the services
- 5 - In the ProductapiService I used the IHttpClientFactory to generate the Httpclient
- 6 - I change all methods from the services to use asynchoneous
#### Reason
- 1 - The reason for this is that I already change to much of the project and I think here is fine not use CQRS.
- 2 - The reason for is that is used the principle of single responsability, The ProductAPiService is responsible only to get the product from the Product Api and generate a InsuranceDto. The Insurance service is responsible for the Insurance calculation logic
- 3 - This way the models and exceptions are not dependent of the Business Project.
- 4 - This way I can have a Dependency Inversion for the services. If I need I can mock this service in my unit tests
- 5 - With IHttpClientFactory I can configure the HttpClient instance in the Api startup file adn inject this instance in the service.
- 6 - This way the call will no block the thread and the process will be free to process other threads. This is good for Application with high load.
### Insurance.Core Project
#### Assumption/Decision Made
- I created a this project to have the Models and Exceptions and other classes or helpes that cpould be shared between the BusinessRule projec and the Api of the appliation.
#### Reason
- The reason for is a good pratice in all projects that I work. This way we have a separation of responsibility from the projects.

### Insurance.Data Project
#### Assumption/Decision Made
- I created this project to provide the Database context and entities
#### Reason
- this database context will be used in the Task 5

### Insurance.Tests Project
#### Assumption/Decision Made
- 1- This is the unit test project
- 2- I moved the ControllerTestFixture and StartUp to anothe folder
- 3 - I added the NBuilder library https://github.com/nbuilder/nbuilder
- 4 - I created a BaseTest class
- 5 - In the InsuranceTests class I Add the Moq https://github.com/moq/moq4
- 6 - I used the Theory atribute in some tests
#### Reason
- 1- The project already exists, I only did some changes on it
- 2 - Inside the Test file is not a correct place for this classes. In my current company we not do unit  tests for controllers, we prefere do this on a integration test with BDD. but here I decide not change the approach
- 3 -This library will help me generate products for the ProductApi. Before that the products are static.
- 4 - This way I can add methods that a recomon for the tests. In this case the DBContext Mock
- 5 - I need this Moq to mock the IHttpClientFactory
- 6 - This way we can run the same test many times for diferrents values

### Insurance.Specs Project
#### Assumption/Decision Made
- This project is a example test project using specflow https://docs.specflow.org/projects/getting-started/en/latest/index.html 
#### Reason
- This is a alternative test project. We could use this in a TDD aproach, writing all the BDD scenarios before beging the development.

## Task 3
#### Assumption/Decision Made
- 1 - I created a new endpoint, we the user can get the Insurace for the other passing a list of product ids.
- 2 - Another method in the InsuranceService was created GetOrderInsuranceAsync.
- -3 - a new unit test was created for this.
#### Reason
- 1 - In this I decided keep the get aproach for this, the product id list shpuld be passed as a string separeted by ",". I think this way we will keep the restfull api.
- 2 - This method do a foreach in the productid list and fill the insurance for each product. The method fill the total insurance for the oder too.
- 3 - I created a unit test before develop the the solution. Try to keep the TDD in mind.

## Task 4
#### Assumption/Decision Made
- 1 - I added the method AddExtraInsuranceforDigitalCameras in the end of the method GetOrderInsuranceAsync from the InsuranceService 
- 2 - I unit test was created to validate the change.
#### Reason
- 1 This method will verify if exists any digital Camera product type in the order, if so it will add the value 500 to the total insurance
- 2 - I created a unit test before develop the the solution. Try to keep the TDD in mind.

## Task 5
#### Assumption/Decision Made
- 1 - I created a new controller called SurchageRateController and a POST endpoint to the user be able to add new rates.
- 2 - to persiste the New Rate I Created a EF CORE Context and a new service calledSurchargeRateService.
- 3 - I added the SurchargRateService in the Insurance service and call a method AddSurchargeRate in the end of the CalculateInsuranceAsync
- 4 - I unit test was created to validate the change.
- 5 - In the unit test I'm using in memory database option.
#### Reason
- 1- I decided this because the InsuranceController is not responsible for this behaviour, so the correct action is create a a new one. The I decided by the post in this case becaus now the user want to add a new object to the api and is not tryin to get values from it.
- 2 - I could persist the rate ina MongoDB database, but I chose the SQL Serve in memory database to be easy for us run the project. The new service was created because is not responsability from the InsuranceService and ProductAPi service manage the CRUD for the Rate.
- 3 - This way we can claculate the new insurance for a product, if the exists any rate in the database.
- 4 - I created a unit test before develop the the solution. Try to keep the TDD in mind.
- 5 - This is a wasy way to mock the EF core context in unit tests.