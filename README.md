# ant
A repository with helper classes and methods for aspnet webapi tests (System.Net.Http : ApiController) for now .net 4.5

[![contributions welcome](https://img.shields.io/badge/contributions-welcome-brightgreen.svg?style=flat)](https://github.com/jkone27/ant/issues)

![ant](https://raw.githubusercontent.com/jkone27/ant/master/Pics/ant-gammillian.png)



## How does it work?  

HTTP REQUEST  
--> DelegatingHandler (ant)  
--> AuthenticationFilter (ant)  
--> AuthorizationFilter (ant)  
---> ExceptionFilter (ant)  
--> ActionFilter (ant)  
--> Controller.  
  
anything can be ant'd.

## Basic Usage

```cs
class Program
{
    static void Main(string[] args)
    {
        //should be awaited, just for test
        var statusCode = new TestController().Test()
            .GetAsync("http://localhost/api/test", r => r.StatusCode).Result;

        //prints OK
        Console.WriteLine(statusCode);
        Task.Delay(TimeSpan.FromSeconds(5)).Wait();
    }
}

[RoutePrefix("api/test")]
public class TestController : ApiController
{
    [Route("")]
    [HttpGet]
    public IHttpActionResult Get()
    {
        return Ok();
    }
}
```

### Status
[![build status](https://img.shields.io/travis/jkone27/ant.svg)](https://travis-ci.org/jkone27/ant)

<!-- language: lang-none -->
     _  _  ,,                                                     ,, _  _
    (_)(_)-O                                                      0-(_)(_)
     L L L                                                          J J J
     `````--.._____..---`````---````----...._____....-`````--...__..``dwb`._


