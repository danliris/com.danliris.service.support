using com.danliris.support.lib.Services;
using com.danliris.support.webapi.Controllers.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace com.danliris.support.test.webapi.Controllers.v1
{
   public class CustomsReportControllerTest
    {
        protected virtual CustomsReportController GetController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);


            Mock<ScrapService> scrapService = new Mock<ScrapService>();
            Mock<WIPService> wipService = new Mock<WIPService>();
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>();
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>();
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>();
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>();


            //serviceProvider
            //   .Setup(s => s.GetService(typeof(IIdentityService)))
            //   .Returns(new IdentityService() { TimezoneOffset = 1, Token = "token", Username = "username" });

            //var validateService = new Mock<IValidateService>();
            //serviceProvider
            //  .Setup(s => s.GetService(typeof(IValidateService)))
            //  .Returns(validateService.Object);

            CustomsReportController controller = new CustomsReportController(scrapService.Object,wipService.Object,factBeacukaiService.Object,factItemMutationService.Object,finishedGoodService.Object,machineMutationService.Object);
            controller.ControllerContext = new ControllerContext()
            {

                HttpContext = new DefaultHttpContext()
                {
                    User = user.Object,

                }
            };
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer unittesttoken";
            controller.ControllerContext.HttpContext.Request.Path = new PathString("/v1/unit-test");

            return controller;
        }
        protected virtual int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }


        //[Fact]
        //public void Post_Success_Created()
        //{

        //  //  var serviceProviderMock = new Mock<IServiceProvider>();

        //    //var unitServiceMock = new Mock<IUnitService>();
        //    //serviceProviderMock
        //    //   .Setup(serviceProvider => serviceProvider.GetService(typeof(IUnitService)))
        //    //   .Returns(unitServiceMock.Object);



        //    IActionResult response = GetController().Get(DateTime.Now, DateTime.Now.AddDays(1));
        //    int statusCode = this.GetStatusCode(response);
        //    Assert.Equal((int)HttpStatusCode.Created, statusCode);

        //}

    }
}
