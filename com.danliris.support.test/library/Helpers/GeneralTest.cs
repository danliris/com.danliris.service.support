using com.danliris.support.lib.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace com.danliris.support.test.library.Helpers
{
   public class GeneralTest
    {
        [Fact]
        public void BuildSearch_Return_Success()
        {
            List<string> SearchAttributes = new List<string>()
            {
                "key"
            };
            var result = General.BuildSearch(SearchAttributes);
        }

        [Fact]
        public void BuildSearch_behavior1_Return_Success()
        {
            List<string> SearchAttributes = new List<string>()
            {
                "."
            };
            var result = General.BuildSearch(SearchAttributes);
        }
    }
}
