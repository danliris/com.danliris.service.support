using com.danliris.support.lib.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace com.danliris.support.test.library.Models
{
    public class FactBeacukaiTest
    {
        [Fact]
        public void Should_Success_Instantiate_FactItemMutation()
        {
            var now = DateTime.Now;
            FactBeacukai model = new FactBeacukai()
            {
                DetailshippingOrderId ="1",
                BCId ="1",
                BCNo = "BCNo",
                BCType = "BCType",
                UnitQtyName = "UnitQtyName",
                Quantity =1,
                BCDate =now,
                BonDate =now,
                SupllierCode = "SupllierCode",
                ItemCode= "ItemCode",
                ItemName = "ItemName",
                SupplierName = "SupplierName",
                BonNo = "BonNo",
                PricePo= 1,
                Rate =1,
                Nominal =1,
                CurrencyCode = "CurrencyCode"
            };

            Assert.Equal("1", model.DetailshippingOrderId);
            Assert.Equal("1", model.BCId);
            Assert.Equal("BCNo", model.BCNo);
            Assert.Equal("BCType", model.BCType);
            Assert.Equal("UnitQtyName", model.UnitQtyName);
            Assert.Equal(now, model.BCDate);
            Assert.Equal("BonNo", model.BonNo);
            Assert.Equal(now, model.BonDate);
            Assert.Equal("SupllierCode", model.SupllierCode);
            Assert.Equal("SupplierName", model.SupplierName);
            Assert.Equal("ItemCode", model.ItemCode);
            Assert.Equal("ItemName", model.ItemName);
            Assert.Equal("UnitQtyName", model.UnitQtyName);
            Assert.Equal(1, model.Quantity);
            Assert.Equal(1, model.PricePo);
            Assert.Equal(1, model.Rate);
            Assert.Equal(1, model.Nominal);
            Assert.Equal("CurrencyCode", model.CurrencyCode);
        }
    }
}
