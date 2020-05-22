using com.danliris.support.lib;
using com.danliris.support.lib.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace com.danliris.support.test.library.Services
{
   public class ScrapServiceTest
    {
        private string _entity = "ViewScrap";

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

        //[Fact]
        //public void GetScrapReport_Return_Success()
        //{
        //    var dbContext = DbContext(GetCurrentMethod(_entity));
            
        //    ScrapService factBeacukaiService = new ScrapService(dbContext);

        //    DateTime dateFrom = DateTime.Now.AddDays(-1);
        //    DateTime dateTo = DateTime.Now.AddDays(1);
            
        //   factBeacukaiService.GetScrapReport(dateFrom, dateTo,1);
        //}



    }
}
