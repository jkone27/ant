# Antenna (ant)
A repository with helper classes and methods for aspnet webapi tests (System.Net.Http : ApiController) for .net 4.5

[![contributions welcome](https://img.shields.io/badge/contributions-welcome-brightgreen.svg?style=flat)](https://github.com/jkone27/ant/issues)

![ant](https://raw.githubusercontent.com/jkone27/ant/master/Pics/ant-gammillian.png)

## WebApi 2 Pipeline 

HTTP REQUEST  
--> DelegatingHandler (ant)  
--> AuthenticationFilter (ant)  
--> AuthorizationFilter (ant)  
---> ExceptionFilter (ant)  
--> ActionFilter (ant)  
--> Controller.  
  
anything can be ant'd.

## Usage

Let's try it with a simple `TestController`. we will add some route attributes to make it feel "more custom".
We will use this sample controller, to see the various features of the library.

```cs
[RoutePrefix("api/test")]
public class TestController : ApiController
{
    [Route("")]
    [HttpGet]
    public IHttpActionResult Get()
    {
        return Ok();
    }

    [Route("another/more/complex/route")]
    public IHttpActionResult MoreComplexPost([FromBody] string test)
    {
        return Ok();
    }
}
```
## HttpRequest

Let's make a call to the GET route, specifying the verb and the complete http route.

```cs
var testController = new TestController();
var responseStatus = await new TestController().Test().HttpRequest(HttpMethod.Get, TestApiRoute, r => r.StatusCode);
Assert.True(resultStatus == HttpStatusCode.OK);
```

To abbreviate usage, with the most common methods GET and POST, also GetAsync and PostAsync overloads are provided  
they simply "hide" the explicit HttpMethod, no magic (yet!).

## BuildHttpRequest (antenna mode on)

Now things become really interesting here!
Using **magic ant antennas** (ok, it's just .NET reflection), we are able to infer both the http *request Uri* and the *Http verb* used,
using the `.BuildHttpRequest` extension method. The verb is first inferred from any HttpMethodAttribute,  
and then, if not present, from the method name (convention based).

```cs
 var statusCode = await new testController.Test()
    .BuildHttpRequest(() => testController.MoreComplexPost(null)), r => r.StatusCode);

```

## Cusomizations in Test seupt and Request

*The `.Test` extension method accepts different customizations: e.g. custom DelegatingHandlers, Filters, and a custom  
`Action\<HttpConfiguration\>` if a special setup is needed.

*The `.HttpRequest` and `BuildHttpRequest` methods in its different variants, takes also an additional parameters to add custom actions on HttpRequest and HttpResponse messages within the pipeline (in case Handlers and Filters aren't enough for some particular case).

### Status
[![build status](https://img.shields.io/travis/jkone27/ant.svg)](https://travis-ci.org/jkone27/ant)

<!-- language: lang-none -->
     _  _  ,,                                                     ,, _  _
    (_)(_)-O                                                      0-(_)(_)
     L L L                                                          J J J
     `````--.._____..---`````---````----...._____....-`````--...__..``dwb`._


