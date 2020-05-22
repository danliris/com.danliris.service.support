using com.danliris.support.lib;
using com.danliris.support.lib.Models;
using com.danliris.support.lib.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace com.danliris.support.test.library.Services
{
    public class FactBeacukaiServiceTest
    {
        private string _entity1 = "FactBeacukai";
        private string _entity2 = "FactBeacukaiViewModel";

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected string GetCurrentMethod(string entity)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", entity);
        }

        protected SupportDbContext DbContext(string testName)
        {
            DbContextOptionsBuilder<SupportDbContext> optionsBuilder = new DbContextOptionsBuilder<SupportDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            SupportDbContext dbContext = Activator.CreateInstance(typeof(SupportDbContext), optionsBuilder.Options) as SupportDbContext;

            return dbContext;
        }

        [Fact]
        public void ReadModelReturnSuccess()
        {
            var dbContext = DbContext(GetCurrentMethod(_entity1));
            FactBeacukaiService factBeacukaiService = new FactBeacukaiService(dbContext);
            factBeacukaiService.ReadModel(1);
        }

        [Fact]
        public void GenerateExcelIN_Return_Success()
        {
            var dbContext = DbContext(GetCurrentMethod(_entity2));
            dbContext.ViewFactBeacukai.Add(
                new ViewFactBeacukai()
                {
                    DetailshippingOrderId = "1",
                    BCId = "1",
                    BCType = "BC 262",
                    BCNo = "1",
                    BCDate = DateTime.Now,
                    BonDate = DateTime.Now,
                    BonNo = "1",
                    ItemCode = "ItemCode",
                    ItemName = "ItemName",
                    SupplierName = "SupplierName",
                    SupllierCode = "SupllierCode",
                    Quantity = 1,
                    Nominal = 1,
                    CurrencyCode = "RP",
                    UnitQtyName = "1"
                }
                );

            dbContext.SaveChanges();

            FactBeacukaiService factBeacukaiService = new FactBeacukaiService(dbContext);
            //string type = "BC 262";
            string type = "";
            DateTime dateFrom = DateTime.Now.AddDays(-1);
            DateTime dateTo = DateTime.Now.AddDays(1);
            int offset = 1;
            var result = factBeacukaiService.GenerateExcelIN(type, dateFrom, dateTo, offset);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetReportIN_Return_Success()
        {
            var dbContext = DbContext(GetCurrentMethod(_entity2));
            dbContext.ViewFactBeacukai.Add(
                new ViewFactBeacukai()
                {
                    DetailshippingOrderId = "1",
                    BCId = "1",
                    BCType = "BC 262",
                    BCNo = "1",
                    BCDate =DateTime.Now,
                    BonDate = DateTime.Now,
                    BonNo = "1",
                    ItemCode = "ItemCode",
                    ItemName = "ItemName",
                    SupplierName = "SupplierName",
                    Quantity =1,
                    Nominal =1,
                    CurrencyCode ="RP",
                    UnitQtyName = "1"
                }
                );
          
            dbContext.SaveChanges();
            FactBeacukaiService factBeacukaiService = new FactBeacukaiService(dbContext);

            string type = "BC 262";
            DateTime dateFrom = DateTime.Now.AddDays(-2);
            DateTime dateTo = DateTime.Now.AddDays(2);
            int offset = 1;
            int page = 1;
            int size = 25;
            
            //Create object
            var orderData = new
            {
                BCNo ="1",
            };
            string order = JsonConvert.SerializeObject(orderData);
            var result = factBeacukaiService.GetReportIN(type, dateFrom, dateTo, page,size,order, offset);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetReportOUTQuery_Return_Success()
        {
            var dbContext = DbContext(GetCurrentMethod(_entity2));
            dbContext.ViewFactBeacukai.Add(
                new ViewFactBeacukai()
                {
                    DetailshippingOrderId = "1",
                    BCId = "1",
                    BCType = "BC 2.6.1",
                    BCNo = "1",
                    BCDate = DateTime.Now,
                    BonDate = DateTime.Now,
                    BonNo = "1",
                    ItemCode = "ItemCode",
                    ItemName = "ItemName",
                    SupplierName = "SupplierName",
                    Quantity = 1,
                    Nominal = 1,
                    CurrencyCode = "RP",
                    UnitQtyName = "1"
                }
                );

            dbContext.SaveChanges();
            FactBeacukaiService factBeacukaiService = new FactBeacukaiService(dbContext);
            string type = "";
            DateTime dateFrom = DateTime.Now.AddDays(-2);
            DateTime dateTo = DateTime.Now.AddDays(2);
            int offset = 1;
           
            var result =  factBeacukaiService.GetReportOUTQuery(type, dateFrom,dateTo,offset);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetReportOUT_Return_Success()
        {
            var dbContext = DbContext(GetCurrentMethod(_entity2));
            dbContext.ViewFactBeacukai.Add(
                new ViewFactBeacukai()
                {
                    DetailshippingOrderId = "1",
                    BCId = "1",
                    BCType = "BC 2.6.1",
                    BCNo = "1",
                    BCDate = DateTime.Now,
                    BonDate = DateTime.Now,
                    BonNo = "1",
                    ItemCode = "ItemCode",
                    ItemName = "ItemName",
                    SupllierCode = "SupllierCode",
                    SupplierName = "SupplierName",
                    Quantity = 1,
                    Nominal = 1,
                    CurrencyCode = "RP",
                    UnitQtyName = "1"
                }
                );

            dbContext.SaveChanges();
            FactBeacukaiService factBeacukaiService = new FactBeacukaiService(dbContext);

            string type = "";
            DateTime dateFrom = DateTime.Now.AddDays(-2);
            DateTime dateTo = DateTime.Now.AddDays(2);
            int offset = 1;
            int page = 1;
            int size = 25;

            //Create object
            var orderData = new
            {
                BCNo = "1",
            };
            string order = JsonConvert.SerializeObject(orderData);
            var result = factBeacukaiService.GetReportOUT(type, dateFrom, dateTo, page, size, order, offset);
            Assert.NotNull(result);
            Assert.NotEqual(0, result.Item2);

        }


        [Fact]
        public void GenerateExcelOUT_Return_Success()
        {
            var dbContext = DbContext(GetCurrentMethod(_entity2));
            dbContext.ViewFactBeacukai.Add(
                new ViewFactBeacukai()
                {
                    DetailshippingOrderId = "1",
                    BCId = "1",
                    BCType = "BC 2.6.1",
                    BCNo = "1",
                    BCDate = DateTime.Now,
                    BonDate = DateTime.Now,
                    BonNo = "1",
                    ItemCode = "ItemCode",
                    ItemName = "ItemName",
                    SupllierCode = "SupllierCode",
                    SupplierName = "SupplierName",
                    Quantity = 1,
                    Nominal = 1,
                    CurrencyCode = "RP",
                    UnitQtyName = "1"
                }
                );

            dbContext.SaveChanges();
            FactBeacukaiService factBeacukaiService = new FactBeacukaiService(dbContext);

            string type = "";
            DateTime dateFrom = DateTime.Now.AddDays(-2);
            DateTime dateTo = DateTime.Now.AddDays(2);
            int offset = 1;
            
            var result = factBeacukaiService.GenerateExcelOUT(type, dateFrom, dateTo, offset);
            Assert.NotNull(result);
        }

    }
}
