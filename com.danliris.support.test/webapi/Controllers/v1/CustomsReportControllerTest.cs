using com.danliris.support.lib;
using com.danliris.support.lib.Services;
using com.danliris.support.lib.ViewModel;
using com.danliris.support.webapi.Controllers.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace com.danliris.support.test.webapi.Controllers.v1
{
   public class CustomsReportControllerTest
    {
        protected virtual CustomsReportController GetController(Mock<ScrapService> scrapService, Mock<WIPService> wipService, Mock<FactBeacukaiService> factBeacukaiService, Mock<FactItemMutationService> factItemMutationService, Mock<FinishedGoodService> finishedGoodService, Mock<MachineMutationService> machineMutationService)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);

          


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

        protected SupportDbContext GetDbContext()
        {
            string databaseName = "testName";
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var optionsBuilder = new DbContextOptionsBuilder<SupportDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(databaseName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                 .UseInternalServiceProvider(serviceProvider);

            SupportDbContext DbContex = new SupportDbContext(optionsBuilder.Options);
            return DbContex;
        }

        protected virtual int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }

        [Fact]
        public void Post_Success_Created()
        {


            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            var scrapViewModels = new List<ScrapViewModel>()
            {
                new ScrapViewModel()
            }.AsQueryable();
            scrapService.Setup(s => s.GetScrapReport(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(scrapViewModels);

            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.Get(DateTime.Now, DateTime.Now.AddDays(1));
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);

        }
        [Fact]
        public void Post_Return_InternalServerError()
        {

            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            var scrapViewModels = new List<ScrapViewModel>()
            {
                new ScrapViewModel()
            }.AsQueryable();
            scrapService.Setup(s => s.GetScrapReport(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Throws(new Exception());

            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService,wipService,factBeacukaiService,factItemMutationService,finishedGoodService,machineMutationService);
            IActionResult response = controller.Get(DateTime.Now, DateTime.Now.AddDays(1));
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }


        [Fact]
        public void GetXlsScrap_Return_Success()
        {


            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            var scrapViewModels = new List<ScrapViewModel>()
            {
                new ScrapViewModel()
            }.AsQueryable();
            scrapService.Setup(s => s.GenerateExcel(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(new System.IO.MemoryStream());

            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetXlsScrap(DateTime.Now, DateTime.Now.AddDays(1));
            Assert.NotNull(response);
          
        }

        [Fact]
        public void GetXlsScrap_Throws_Exception()
        {


            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            var scrapViewModels = new List<ScrapViewModel>()
            {
                new ScrapViewModel()
            }.AsQueryable();
            scrapService.Setup(s => s.GenerateExcel(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Throws(new Exception());

            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetXlsScrap(DateTime.Now, DateTime.Now.AddDays(1));
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }


        [Fact]
        public void GetFinishedGoodReport_Return_Success()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            var scrapViewModels = new List<ScrapViewModel>()
            {
                new ScrapViewModel()
            }.AsQueryable();
           

            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            var finishedGoodViewModel = new List<FinishedGoodViewModel>()
            {
                new FinishedGoodViewModel()
            }.AsQueryable();

            finishedGoodService.Setup(s => s.GetFinishedGoodReport(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(finishedGoodViewModel);

            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetFinishedGood(DateTime.Now, DateTime.Now.AddDays(1));
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);

        }

        [Fact]
        public void GetFinishedGoodReport_Throws_Exception()
        {


            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            var scrapViewModels = new List<ScrapViewModel>()
            {
                new ScrapViewModel()
            }.AsQueryable();


            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            var finishedGoodViewModel = new List<FinishedGoodViewModel>()
            {
                new FinishedGoodViewModel()
            }.AsQueryable();

            finishedGoodService.Setup(s => s.GetFinishedGoodReport(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Throws(new Exception());

            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetFinishedGood(DateTime.Now, DateTime.Now.AddDays(1));
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }

        [Fact]
        public void GetXlsFinishedGood_Return_Success()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            var scrapViewModels = new List<ScrapViewModel>()
            {
                new ScrapViewModel()
            }.AsQueryable();


            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
           
            finishedGoodService.Setup(s => s.GenerateExcel(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(new System.IO.MemoryStream());

            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetXlsFinishedGood(DateTime.Now, DateTime.Now.AddDays(1));

            Assert.NotNull(response);

        }

        [Fact]
        public void GetXlsFinishedGood_Throws_Exception()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            var scrapViewModels = new List<ScrapViewModel>()
            {
                new ScrapViewModel()
            }.AsQueryable();


            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            var finishedGoodViewModel = new List<FinishedGoodViewModel>()
            {
                new FinishedGoodViewModel()
            }.AsQueryable();

            finishedGoodService.Setup(s => s.GenerateExcel(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Throws(new Exception());

            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetXlsFinishedGood(DateTime.Now, DateTime.Now.AddDays(1));
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }


        [Fact]
        public void GetMachineReport_Return_Success()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            var scrapViewModels = new List<ScrapViewModel>()
            {
                new ScrapViewModel()
            }.AsQueryable();


            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);

           

            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            Tuple<List<FinishedGoodViewModel>,int> tuple = new Tuple<List<FinishedGoodViewModel>,int>(new List<FinishedGoodViewModel>() { new FinishedGoodViewModel() },1);
            machineMutationService.Setup(s => s.GetMachineMutationReportData(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Returns(tuple);
            
            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetMachineReport(DateTime.Now, DateTime.Now.AddDays(1),1,1,"{}");

            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);

        }

        [Fact]
        public void GetMachineReport_Throws_InternalServerError()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            var scrapViewModels = new List<ScrapViewModel>()
            {
                new ScrapViewModel()
            }.AsQueryable();


            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);

            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            Tuple<List<FinishedGoodViewModel>, int> tuple = new Tuple<List<FinishedGoodViewModel>, int>(new List<FinishedGoodViewModel>() { new FinishedGoodViewModel() }, 1);
            machineMutationService.Setup(s => s.GetMachineMutationReportData(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Throws(new Exception());

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetMachineReport(DateTime.Now, DateTime.Now.AddDays(1), 1, 1, "{}");

            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }


        [Fact]
        public void GetXlsMachine_Return_Success()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);

            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);

            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            machineMutationService.Setup(s => s.GenerateExcel(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(new System.IO.MemoryStream());

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetXlsMachine(DateTime.Now, DateTime.Now.AddDays(1));
            Assert.NotNull(response);

        }

        [Fact]
        public void GetXlsMachine_Throws_InternalServerError()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);

            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);

            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            machineMutationService.Setup(s => s.GenerateExcel(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Throws(new Exception());

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetXlsMachine(DateTime.Now, DateTime.Now.AddDays(1));

            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }

        [Fact]
        public void GetWIP_Return_Success()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);

            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);

            var WIPViewModels = new List<WIPViewModel>()
            {
                new WIPViewModel()
            }.AsQueryable();

            wipService.Setup(s => s.GetWIPReport(It.IsAny<DateTime>(),  It.IsAny<int>())).Returns(WIPViewModels);
           
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetWIP(DateTime.Now);
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);

        }


        [Fact]
        public void GetWIP_Throws_InternalServerError()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);

            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);

           
            wipService.Setup(s => s.GetWIPReport(It.IsAny<DateTime>(), It.IsAny<int>())).Throws(new Exception());

            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetWIP(DateTime.Now);
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }

        [Fact]
        public void GetXlsWIP_Return_Success()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);

            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            wipService.Setup(s => s.GenerateExcel(It.IsAny<DateTime>(), It.IsAny<int>())).Returns(new System.IO.MemoryStream());
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);

            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetXlsWIP(DateTime.Now);
            Assert.NotNull(response);

        }




        [Fact]
        public void GetXlsWIP_Throws_InternalServerError()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);

            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);

            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            machineMutationService.Setup(s => s.GenerateExcel(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Throws(new Exception());

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetXlsWIP(DateTime.Now);
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }

        [Fact]
        public void GetBBUnit_Return_Success()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);

            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
           
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Tuple<List<FactMutationItemViewModel>, int> tuple = new Tuple<List<FactMutationItemViewModel>, int>(new List<FactMutationItemViewModel>() { new FactMutationItemViewModel() }, 1);
            factItemMutationService.Setup(s => s.GetReportBBUnit(It.IsAny<int>(),It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Returns(tuple);

            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);

            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);



            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetBBUnit(1,DateTime.Now, DateTime.Now.AddDays(1),1,1,"{}");
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);

        }

        [Fact]
        public void GetBBUnit_Throws_InternalServerError()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);

            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);

            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Tuple<List<FactMutationItemViewModel>, int> tuple = new Tuple<List<FactMutationItemViewModel>, int>(new List<FactMutationItemViewModel>() { new FactMutationItemViewModel() }, 1);
            factItemMutationService.Setup(s => s.GetReportBBUnit(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Throws(new Exception());

            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);

            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);



            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetBBUnit(1, DateTime.Now, DateTime.Now.AddDays(1), 1, 1, "{}");
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }

        [Fact]
        public void GetXlsBBUnit_Return_Success()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
           
            factItemMutationService.Setup(s => s.GenerateExcelBBUnit(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(new System.IO.MemoryStream());

            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetXlsBBUnit(1, DateTime.Now, DateTime.Now.AddDays(1));
            Assert.NotNull(response);

        }

        [Fact]
        public void GetXlsBBUnit_Throws_InternalServerError()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);

            factItemMutationService.Setup(s => s.GenerateExcelBBUnit(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Throws(new Exception());

            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetXlsBBUnit(1, DateTime.Now, DateTime.Now.AddDays(1));
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }


        [Fact]
        public void GetBPUnit_Return_Success()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Tuple<List<FactMutationItemViewModel>, int> tuple = new Tuple<List<FactMutationItemViewModel>, int>(new List<FactMutationItemViewModel>() { new FactMutationItemViewModel() }, 1);
            factItemMutationService.Setup(s => s.GetReportBPUnit(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Returns(tuple);

            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetBPUnit(1, DateTime.Now, DateTime.Now.AddDays(1),1,1,"{}");
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);

        }

        [Fact]
        public void GetBPUnit_Throws_InternalServerError()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Tuple<List<FactMutationItemViewModel>, int> tuple = new Tuple<List<FactMutationItemViewModel>, int>(new List<FactMutationItemViewModel>() { new FactMutationItemViewModel() }, 1);
            factItemMutationService.Setup(s => s.GetReportBPUnit(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Throws(new Exception());

            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetBPUnit(1, DateTime.Now, DateTime.Now.AddDays(1), 1, 1, "{}");
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }


        [Fact]
        public void GetXlsBPUnit_Return_Success()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
           
            factItemMutationService.Setup(s => s.GenerateExcelBPUnit(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(new System.IO.MemoryStream());

            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetXlsBPUnit(1, DateTime.Now, DateTime.Now.AddDays(1));
            Assert.NotNull(response);

        }

        [Fact]
        public void GetXlsBPUnit_Throws_InternalServerError()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);

            factItemMutationService.Setup(s => s.GenerateExcelBPUnit(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Throws(new Exception());

            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetXlsBPUnit(1, DateTime.Now, DateTime.Now.AddDays(1));
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }


        [Fact]
        public void GetBPCentral_Return_Success()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Tuple<List<FactMutationItemViewModel>, int> tuple = new Tuple<List<FactMutationItemViewModel>, int>(new List<FactMutationItemViewModel>() { new FactMutationItemViewModel() }, 1);
            factItemMutationService.Setup(s => s.GetReportBPCentral( It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Returns(tuple);

            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetBPCentral( DateTime.Now, DateTime.Now.AddDays(1),1,1,"{}");
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);

        }

        [Fact]
        public void GetXlsBBCentral_Return_Success()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            
            factItemMutationService.Setup(s => s.GenerateExcelBBCentral(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(new System.IO.MemoryStream());

            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetXlsBBCentral(DateTime.Now, DateTime.Now.AddDays(1));
            Assert.NotNull(response);

        }

        [Fact]
        public void GetXlsBBCentral_Throws_InternalServerError()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);

            factItemMutationService.Setup(s => s.GenerateExcelBBCentral(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Throws(new Exception());

            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetXlsBBCentral(DateTime.Now, DateTime.Now.AddDays(1));
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }

        [Fact]
        public void GetBPCentral_Throws_InternalServerError()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Tuple<List<FactMutationItemViewModel>, int> tuple = new Tuple<List<FactMutationItemViewModel>, int>(new List<FactMutationItemViewModel>() { new FactMutationItemViewModel() }, 1);
            factItemMutationService.Setup(s => s.GetReportBPCentral(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Throws(new Exception());

            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetBPCentral(DateTime.Now, DateTime.Now.AddDays(1), 1, 1, "{}");
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }

        [Fact]
        public void GetXlsBPCentral_Return_Success()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
           
            factItemMutationService.Setup(s => s.GenerateExcelBPCentral(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(new System.IO.MemoryStream());

            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetXlsBPCentral(DateTime.Now, DateTime.Now.AddDays(1));
            Assert.NotNull(response);

        }

        [Fact]
        public void GetXlsBPCentral_Return_InternalServerError()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);

            factItemMutationService.Setup(s => s.GenerateExcelBPCentral(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Throws(new Exception());

            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetXlsBPCentral(DateTime.Now, DateTime.Now.AddDays(1));
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }

        [Fact]
        public void GetBBCentral_Return_Success()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Tuple<List<FactMutationItemViewModel>, int> tuple = new Tuple<List<FactMutationItemViewModel>, int>(new List<FactMutationItemViewModel>() { new FactMutationItemViewModel() }, 1);
            factItemMutationService.Setup(s => s.GetReportBBCentral(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Returns(tuple);

            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetBBCentral(DateTime.Now, DateTime.Now.AddDays(1),1,1,"{}");
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);

        }

        [Fact]
        public void GetBBCentral_Return_InternalServerError()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);
            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Tuple<List<FactMutationItemViewModel>, int> tuple = new Tuple<List<FactMutationItemViewModel>, int>(new List<FactMutationItemViewModel>() { new FactMutationItemViewModel() }, 1);
            factItemMutationService.Setup(s => s.GetReportBBCentral(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Throws(new Exception());

            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetBBCentral(DateTime.Now, DateTime.Now.AddDays(1), 1, 1, "{}");
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }

        [Fact]
        public void GetIN_Return_Success()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);

            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);

            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Tuple<List<FactBeacukaiViewModel>, int> tuple = new Tuple<List<FactBeacukaiViewModel>, int>(new List<FactBeacukaiViewModel>() { new FactBeacukaiViewModel() }, 1);
            factBeacukaiService.Setup(s => s.GetReportIN(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Returns(tuple);


            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);

            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);



            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetIN("Type", DateTime.Now, DateTime.Now.AddDays(1), 1, 1, "{}");
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);

        }

        [Fact]
        public void GetIN_Throws_Exception()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);

            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);

            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Tuple<List<FactBeacukaiViewModel>, int> tuple = new Tuple<List<FactBeacukaiViewModel>, int>(new List<FactBeacukaiViewModel>() { new FactBeacukaiViewModel() }, 1);
            factBeacukaiService.Setup(s => s.GetReportIN(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Throws(new Exception());


            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);



            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetIN("Type", DateTime.Now, DateTime.Now.AddDays(1), 1, 1, "{}");
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }

        [Fact]
        public void GetXlsIN_Return_Success()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);

            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);

            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
           
            factBeacukaiService.Setup(s => s.GenerateExcelIN(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(new System.IO.MemoryStream());


            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);



            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetXlsIN("Type", DateTime.Now, DateTime.Now.AddDays(1));
           
            Assert.NotNull(response);

        }

        [Fact]
        public void GetXlsIN_Throws_InternalServerError()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);

            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);

            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);

            factBeacukaiService.Setup(s => s.GenerateExcelIN(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Throws(new Exception());


            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetXlsIN("Type", DateTime.Now, DateTime.Now.AddDays(1));

            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }

        [Fact]
        public void GetXlsOUT_Return_Success()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);

            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);

            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);

            factBeacukaiService.Setup(s => s.GenerateExcelOUT(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Returns(new System.IO.MemoryStream());


            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);



            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetXlsOUT("Type", DateTime.Now, DateTime.Now.AddDays(1));

            Assert.NotNull(response);

        }

        [Fact]
        public void GetXlsOUT_Throws_InternalServerError()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);

            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);

            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);

            factBeacukaiService.Setup(s => s.GenerateExcelOUT(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>())).Throws(new Exception());


            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);



            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetXlsOUT("Type", DateTime.Now, DateTime.Now.AddDays(1));

            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }

        [Fact]
        public void GetOUT_Return_Success()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);

            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);

            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            Tuple<List<FactBeacukaiViewModel>, int> tuple = new Tuple<List<FactBeacukaiViewModel>, int>(new List<FactBeacukaiViewModel>() { new FactBeacukaiViewModel() }, 1);
           // factBeacukaiService.Setup(s => s.GetReportOUT(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Returns(tuple);

            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetOUT("Type", DateTime.Now, DateTime.Now.AddDays(1),1,1,"{}");

            Assert.NotNull(response);

        }

        [Fact]
        public void GetOUT_Throws_InternalServerError()
        {
            var dbContext = GetDbContext();
            Mock<ScrapService> scrapService = new Mock<ScrapService>(dbContext);

            Mock<WIPService> wipService = new Mock<WIPService>(dbContext);
            Mock<FactBeacukaiService> factBeacukaiService = new Mock<FactBeacukaiService>(dbContext);
            
            factBeacukaiService.Setup(s => s.GetReportOUT(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Throws(new Exception());

            Mock<FactItemMutationService> factItemMutationService = new Mock<FactItemMutationService>(dbContext);
            Mock<FinishedGoodService> finishedGoodService = new Mock<FinishedGoodService>(dbContext);
            Mock<MachineMutationService> machineMutationService = new Mock<MachineMutationService>(dbContext);

            var controller = GetController(scrapService, wipService, factBeacukaiService, factItemMutationService, finishedGoodService, machineMutationService);
            IActionResult response = controller.GetOUT("Type", DateTime.Now, DateTime.Now.AddDays(1), 1, 1, "{}");

            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }

    }
}
