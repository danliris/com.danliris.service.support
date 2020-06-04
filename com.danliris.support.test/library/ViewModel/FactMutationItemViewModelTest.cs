using com.danliris.support.lib.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace com.danliris.support.test.library.ViewModel
{
   public class FactMutationItemViewModelTest
    {
        [Fact]
        public void Should_Success_FactMutationItemViewModel()
        {
            FactMutationItemViewModel viewModel = new FactMutationItemViewModel();
            viewModel.unitCode = "unitCode";
            viewModel.ItemCode = "ItemCode";
            viewModel.ItemName = "ItemName";
            viewModel.UnitQtyName = "UnitQtyName";
            viewModel.BeginQty = "BeginQty";
            viewModel.ReceiptQty = "ReceiptQty";
            viewModel.ExpenditureQty = "ExpenditureQty";
            viewModel.AdjustmentQty = "AdjustmentQty";
            viewModel.OpnameQty = "OpnameQty";
            viewModel.LastQty = "LastQty";
            viewModel.Diff = "Diff";

            Assert.Equal("unitCode", viewModel.unitCode);
            Assert.Equal("ItemCode", viewModel.ItemCode);
            Assert.Equal("ItemName", viewModel.ItemName);
            Assert.Equal("UnitQtyName", viewModel.UnitQtyName);
            Assert.Equal("BeginQty", viewModel.BeginQty);
            Assert.Equal("ReceiptQty", viewModel.ReceiptQty);
            Assert.Equal("ExpenditureQty", viewModel.ExpenditureQty);
            Assert.Equal("AdjustmentQty", viewModel.AdjustmentQty);
            Assert.Equal("OpnameQty", viewModel.OpnameQty);
            Assert.Equal("LastQty", viewModel.LastQty);
            Assert.Equal("Diff", viewModel.Diff);
        }
    }
}
