using com.danliris.support.lib.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace com.danliris.support.test.library.ViewModel
{
    public class WIPViewModelTest
    {
        [Fact]
        public void Should_Success_Instantiate_WIPViewModel()
        {
            WIPViewModel wIPViewModel = new WIPViewModel()
            {
                Kode ="Kode",
                Comodity = "Comodity",
                UnitQtyName = "UnitQtyName",
                WIP ="WIP"
            };

            Assert.Equal("Kode", wIPViewModel.Kode);
            Assert.Equal("Comodity", wIPViewModel.Comodity);
            Assert.Equal("UnitQtyName", wIPViewModel.UnitQtyName);
            Assert.Equal("WIP", wIPViewModel.WIP);

        }
    }
}
