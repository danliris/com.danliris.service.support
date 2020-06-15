using com.danliris.support.lib.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace com.danliris.support.test.library.Models
{
    public class FactItemMutationTest
    {
        [Fact]
        public void Should_Success_Instantiate_FactItemMutation()
        {
            var now = DateTime.Now;
            FactItemMutation model = new FactItemMutation()
            {
                Id ="1",
                Type = "Type",
                ItemName = "ItemName",
                ItemCode = "ItemCode",
                UnitQtyName = "UnitQtyName",
                Date =now,
                Poid = "Poid",
                Ro = "Ro",
                UnitCode = "UnitCode",
                UnitName = "UnitName",
                Quantity =1,
                UnitQtyCode= "UnitQtyCode",
                ClassificationCode = "ClassificationCode",
                ClassificationName = "ClassificationName",
                StockId =1
            };

            Assert.Equal(1, model.StockId);
            Assert.Equal("1", model.Id);
            Assert.Equal("Type", model.Type);
            Assert.Equal("ItemName", model.ItemName);
            Assert.Equal("ItemCode", model.ItemCode);
            Assert.Equal("UnitQtyName", model.UnitQtyName);
            Assert.Equal(now, model.Date);
            Assert.Equal("Poid", model.Poid);
            Assert.Equal("Ro", model.Ro);
            Assert.Equal("UnitCode", model.UnitCode);
            Assert.Equal("UnitName", model.UnitName);
            Assert.Equal(1, model.Quantity);
            Assert.Equal("UnitQtyCode", model.UnitQtyCode);
            Assert.Equal("ClassificationCode", model.ClassificationCode);
            Assert.Equal("ClassificationName", model.ClassificationName);
        }
    }
}
