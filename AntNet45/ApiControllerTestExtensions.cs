using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace AntNet45
{
    public static class ApiControllerTestExtensions
    {
        public static ApiControllerTest<T> Test<T>(
            this T controller, 
            IEnumerable<DelegatingHandler> messageHandlers = null, 
            IEnumerable<IFilter> filters = null, 
            Action<HttpConfiguration> customSetup = null)
        where T : ApiController
        {
            return ApiControllerTest<T>.Get(controller, messageHandlers, filters, customSetup);
        }

    }
    
}