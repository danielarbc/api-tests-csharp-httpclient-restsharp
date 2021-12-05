# API Tests with Restsharp and HttpClient 


## About 
Esse é um projeto de testes automatizados de API utilizando as ferramentas HttpClient e RestSharp. 

Para rodar os testes utilizamos a [Go Rest API](https://gorest.co.in/)


## You will need
- Visual Studio Community with C# or similar

## Starting
- Open and build project 
- Install NuGet packages 


### Create autentication file 
At Helper folder, add a class called AuthToken

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



