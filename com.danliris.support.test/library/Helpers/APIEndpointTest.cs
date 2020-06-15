using com.danliris.support.lib.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace com.danliris.support.test.library.Helpers
{
    public   class APIEndpointTest
    {
        [Fact]
        public void Should_Success_APIEndpoint()
        {
            APIEndpoint.Core = "Core";
            APIEndpoint.Inventory = "Inventory";
            APIEndpoint.ConnectionString = "ConnectionString";
            APIEndpoint.Production = "Production";

            Assert.Equal("Core", APIEndpoint.Core);
            Assert.Equal("Inventory", APIEndpoint.Inventory);
            Assert.Equal("ConnectionString", APIEndpoint.ConnectionString);
            Assert.Equal("Production", APIEndpoint.Production);
        }
             
    }
}
