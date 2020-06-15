using com.danliris.support.lib.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace com.danliris.support.test.library.ViewModel
{
    public class ScrapViewModelTest
    {
        [Fact]
        public void Should_Success_ScrapViewModel()
        {
            ScrapViewModel viewModel = new ScrapViewModel();
            viewModel.ClassificationId = "ClassificationId";
            viewModel.ClassificationCode = "ClassificationCode";
            viewModel.ClassificationName = "ClassificationName";
            viewModel.StockId = "StockId";
            viewModel.DestinationId = "DestinationId";
            viewModel.StockId = "StockId";
            viewModel.UnitQtyName = "UnitQtyName";
            viewModel.SaldoAwal = "SaldoAwal";
            viewModel.Pemasukan = "Pemasukan";
            viewModel.Pengeluaran = "Pengeluaran";
            viewModel.Penyesuaian = "Penyesuaian";
            viewModel.StockOpname = "StockOpname";
            viewModel.Selisih = "Selisih";
            viewModel.SaldoBuku = "SaldoBuku";

            Assert.Equal("ClassificationId", viewModel.ClassificationId);
            Assert.Equal("ClassificationCode", viewModel.ClassificationCode);
            Assert.Equal("ClassificationName", viewModel.ClassificationName);
            Assert.Equal("StockId", viewModel.StockId);
            Assert.Equal("DestinationId", viewModel.DestinationId);
            Assert.Equal("UnitQtyName", viewModel.UnitQtyName);
            Assert.Equal("SaldoAwal", viewModel.SaldoAwal);
            Assert.Equal("Pemasukan", viewModel.Pemasukan);
            Assert.Equal("Pengeluaran", viewModel.Pengeluaran);
            Assert.Equal("Penyesuaian", viewModel.Penyesuaian);
            Assert.Equal("StockOpname", viewModel.StockOpname);
            Assert.Equal("Selisih", viewModel.Selisih);
            Assert.Equal("SaldoBuku", viewModel.SaldoBuku);
        }
        }
}
