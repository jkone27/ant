# ant
A repository with helper classes and methods for aspnet webapi tests (System.Net.Http : ApiController) for now .net 4.5

<!-- language: lang-none -->
     _  _  ,,                                                     ,, _  _
    (_)(_)-O                                                      0-(_)(_)
     L L L                                                          J J J
     `````--.._____..---`````---````----...._____....-`````--...__..``dwb`._

## How does it work ?

HTTP REQUEST   
--> DelegatingHandler (ant)  
--> AuthenticationFilter (ant)  
--> AuthorizationFilter (ant)  
---> ExceptionFilter (ant)  
--> ActionFilter (ant)  
--> Controller.  
  
anything can be ant'd.


