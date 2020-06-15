using com.danliris.support.lib.Models;
using com.danliris.support.lib.ViewModel;
using com.danliris.support.webapi.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace com.danliris.support.test.webapi.Helpers
{
    public class ResultFormatterTest
    {
        [Fact]
        public void OK_Return_Success()
        {
            string ApiVersion = "V1";
            int StatusCode = 200;
            string Message = "OK";
            ResultFormatter formatter = new ResultFormatter(ApiVersion, StatusCode, Message);
            
            var result = formatter.Ok();
            Assert.NotNull(result);
        }

        [Fact]
        public void OK_with_Model_Return_Success()
        {
            string ApiVersion = "V1";
            int StatusCode = 200;
            string Message = "OK";
            ResultFormatter formatter = new ResultFormatter(ApiVersion, StatusCode, Message);
            FactBeacukai model = new FactBeacukai();
            var result = formatter.Ok(model);
            Assert.NotNull(result);
        }

        [Fact]
        public void OK_withSelect_Return_Success()
        {
            string ApiVersion = "V1";
            int StatusCode = 200;
            string Message = "OK";
            ResultFormatter formatter = new ResultFormatter(ApiVersion, StatusCode, Message);
            FactBeacukai unit = new FactBeacukai();
         
            var data = new List<FactBeacukai>() {
                new FactBeacukai(){
                    Id =1,
                   
                }
            };
            Dictionary<string, string> Order = new Dictionary<string, string>();
            Order.Add("SupplierName", "asc");

            List<string> Select = new List<string>()
            {
                "SupplierName"
            };


            var result = formatter.Ok(data, 1, 1, 10, 10, Order, Select);
            Assert.NotNull(result);
        }

        [Fact]
        public void OK_withEmptySelect_Return_Success()
        {
            string ApiVersion = "V1";
            int StatusCode = 200;
            string Message = "OK";
            ResultFormatter formatter = new ResultFormatter(ApiVersion, StatusCode, Message);
            FactBeacukai unit = new FactBeacukai();

            var data = new List<FactBeacukai>() {
                new FactBeacukai(){
                    Id =1,

                }
            };
            Dictionary<string, string> Order = new Dictionary<string, string>();
            Order.Add("SupplierName", "asc");

            List<string> Select = new List<string>()
            {
                
            };


            var result = formatter.Ok(data, 1, 1, 10, 10, Order, Select);
            Assert.NotNull(result);
        }

        [Fact]
        public void OK_withMapping_Return_Success()
        {
            string ApiVersion = "V1";
            int StatusCode = 200;
            string Message = "OK";
            ResultFormatter formatter = new ResultFormatter(ApiVersion, StatusCode, Message);
            FactBeacukai unit = new FactBeacukai();

            var data = new List<FactBeacukai>() {
                new FactBeacukai(){
                    Id =1,

                }
            };
            Dictionary<string, string> Order = new Dictionary<string, string>();
            Order.Add("SupplierName", "asc");

            List<string> Select = new List<string>()
            {
                "SupplierName"
            };
            Func<FactBeacukai, FactBeacukai> MapToViewModel = (x) => x;

            var result = formatter.Ok(data, MapToViewModel, 1, 1, 10, 10, Order, Select);
            Assert.NotNull(result);
        }

        [Fact]
        public void OK_withMapping_emptySelect_Return_Success()
        {
            string ApiVersion = "V1";
            int StatusCode = 200;
            string Message = "OK";
            ResultFormatter formatter = new ResultFormatter(ApiVersion, StatusCode, Message);
            FactBeacukai unit = new FactBeacukai();

            var data = new List<FactBeacukai>() {
                new FactBeacukai(){
                    Id =1,

                }
            };
            Dictionary<string, string> Order = new Dictionary<string, string>();
            Order.Add("SupplierName", "asc");

            List<string> Select = new List<string>()
            {

            };
            Func<FactBeacukai, FactBeacukai> MapToViewModel = (x) => x;

            var result = formatter.Ok(data, MapToViewModel, 1, 1, 10, 10, Order, Select);
            Assert.NotNull(result);
        }

        [Fact]
        public void OK_withMapping2_Return_Success()
        {
            string ApiVersion = "V1";
            int StatusCode = 200;
            string Message = "OK";
            ResultFormatter formatter = new ResultFormatter(ApiVersion, StatusCode, Message);
            FactBeacukai unit = new FactBeacukai();

            var data = new List<FactBeacukai>() {
                new FactBeacukai(){
                    Id =1,
                    BCDate =DateTime.Now,
                    BCType ="BCType",
                    ItemCode ="ItemCode",
                    CurrencyCode ="CurrencyCode",
                    BonDate = DateTime.Now,
                    ItemName ="ItemName",
                    Nominal =1,
                    PricePo =1,
                    DetailshippingOrderId ="DetailshippingOrderId",
                    Quantity =1,
                    SupllierCode ="SupllierCode",
                    SupplierName ="SupplierName",
                    UnitQtyName ="UnitQtyName"
                }
            };
            
            Func<FactBeacukai, FactBeacukai> MapToViewModel = (x) => x;

            var result = formatter.Ok(data, MapToViewModel);
            Assert.NotNull(result);
        }


        [Fact]
        public void OK_withMapping3_Return_Success()
        {
            string ApiVersion = "V1";
            int StatusCode = 200;
            string Message = "OK";
            ResultFormatter formatter = new ResultFormatter(ApiVersion, StatusCode, Message);
            FactBeacukai unit = new FactBeacukai();

            var data = new FactBeacukai();

            Func<FactBeacukai, FactBeacukai> MapToViewModel = (x) => x;

            var result = formatter.Ok(data, MapToViewModel);
            Assert.NotNull(result);
        }


        [Fact]
        public void Fail_with_String_Return_Success()
        {
            string ApiVersion = "V1";
            int StatusCode = 200;
            string Message = "OK";
            ResultFormatter formatter = new ResultFormatter(ApiVersion, StatusCode, Message);
            var result = formatter.Fail("stringTest");
            Assert.NotNull(result);
        }

        [Fact]
        public void Fail_default_Return_Success()
        {
            string ApiVersion = "V1";
            int StatusCode = 200;
            string Message = "OK";
            ResultFormatter formatter = new ResultFormatter(ApiVersion, StatusCode, Message);
            var result = formatter.Fail();
            Assert.NotNull(result);
        }
    }
}
