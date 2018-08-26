using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AntNet45;
using AntNet45Tests.Controller;
using AntNet45Tests.Helpers;
using Moq;
using Xunit;

namespace AntNet45Tests
{
    public class FlightsPnrControllerAuthorizationTests
    {
        private const string ReadRole = "read";
        private const string WriteRole = "write";
        private const string TestApiRoute = "http://localhost/api/reservations";
        //private readonly ITestOutputHelper output; can be used for debugging 


        private static ReservationsController GetPnrController(Mock<ISomeDependency> someServiceMock)
        {
            someServiceMock.Setup(s => s.Do());
            return new ReservationsController(someServiceMock.Object);
        }

        [Fact]
        public async Task Get_Unauthorized()
        {
            var managementService = new Mock<ISomeDependency>();
            var controller = GetPnrController(managementService);
            var resultStatus = await controller.Test()
                .GetAsync(TestApiRoute, r => r.StatusCode);
            Assert.True(resultStatus == HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Get_Authorized()
        {
            var managementService = new Mock<ISomeDependency>();
            var controller = GetPnrController(managementService);
            var resultStatus = await controller
                .Test(filters: AssignClaimsAuthenticationFilterAttribute.Parse(ReadRole))
                .GetAsync(TestApiRoute, r => r.StatusCode);
            Assert.True(resultStatus == HttpStatusCode.OK);
        }


        [Fact]
        public async Task Post_Unathorized()
        {
            var managementService = new Mock<ISomeDependency>();
            var controller = GetPnrController(managementService);
            var resultStatus = await controller.Test()
                .PostAsync(TestApiRoute, r => r.StatusCode);
            Assert.True(resultStatus == HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Post_Authorized()
        {
            var managementService = new Mock<ISomeDependency>();
            var controller = GetPnrController(managementService);
            var resultStatus = await controller
                .Test(ClaimsHandler.Parse(ReadRole, WriteRole))
                .HttpRequest(HttpMethod.Get, TestApiRoute, r => r.StatusCode);
            Assert.True(resultStatus == HttpStatusCode.OK);

        }

        [Fact]
        public async Task Get_ComplexRouting_DeriveVerbFromAttributes()
        {
            var managementService = new Mock<ISomeDependency>();
            var controller = GetPnrController(managementService);
            var resultStatus = await controller.Test()
                .BuildHttpRequest(() => controller.GetMoreComplexRoute(), r => r.StatusCode);
            Assert.True(resultStatus == HttpStatusCode.OK);

        }

        [Fact]
        public async Task Get_ComplexRouting_DeriveVerbFromName()
        {
            var managementService = new Mock<ISomeDependency>();
            var controller = GetPnrController(managementService);
            var resultStatus = await controller.Test()
                .BuildHttpRequest(() => controller.OptionsMoreComplexRoute(), r => r.StatusCode);
            Assert.True(resultStatus == HttpStatusCode.OK);

        }
    }
}
