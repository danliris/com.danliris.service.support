using com.danliris.support.lib.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xunit;

namespace com.danliris.support.test.library.Services
{
     public  class ExcelTest
    {
        [Fact]
        public void Should_Success_Instantiate_Excel()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Dosage", typeof(int));
            table.Columns.Add("Drug", typeof(string));
            table.Columns.Add("Patient", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));

            // Step 3: here we add 5 rows.
            table.Rows.Add(25, "Indocin", "David", DateTime.Now);
            table.Rows.Add(50, "Enebrel", "Sam", DateTime.Now);
            table.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Now);
            table.Rows.Add(21, "Combivent", "Janet", DateTime.Now);
            table.Rows.Add(100, "Dilantin", "Melanie", DateTime.Now);

            var datasource = new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(table, "Territory") };
            var result = Excel.CreateExcel(datasource, false);
            Assert.NotNull(result);
        }

        }
}
