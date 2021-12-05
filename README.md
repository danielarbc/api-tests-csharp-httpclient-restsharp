# API Tests with Restsharp and HttpClient 


## About 
This is a automated tests project using HttpClient and RestSharp frameworks. 

We are using [Go Rest APIs](https://gorest.co.in/) for the tests. 


## You will need
- Visual Studio Community with C# or similar

## Starting
- Open and build project 
- Install NuGet packages 


### Create autentication file 
Inside Helper folder, add a class called AuthToken

```c#
namespace MicroserviceAutomation.Helper
{
    public class AuthToken
    {
        public static string Auth { get; } = "Bearer ACCESS_TOKEN";
    }
}
```

Insted of `ACCESS_TOKEN`, put your own token generated at [Go Rest API`s site](https://gorest.co.in/).  

Then, build the project again and execute tests 
