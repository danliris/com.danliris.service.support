using com.danliris.support.lib.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace com.danliris.support.test.library.ViewModel
{
    public class FinishedGoodViewModelTest
    {
        [Fact]
        public void Should_Success_FinishedGoodViewModel()
        {
            FinishedGoodViewModel viewModel = new FinishedGoodViewModel();
            viewModel.KodeBarang = "KodeBarang";
            viewModel.NamaBarang = "NamaBarang";
            viewModel.UnitQtyName = "UnitQtyName";
            viewModel.SaldoAwal = "SaldoAwal";
            viewModel.Pemasukan = "Pemasukan";
            viewModel.Pengeluaran = "Pengeluaran";
            viewModel.Penyesuaian = "Penyesuaian";
            viewModel.StockOpname = "StockOpname";
            viewModel.Selisih = "Selisih";
            viewModel.SaldoBuku = "SaldoBuku";

            Assert.Equal("KodeBarang", viewModel.KodeBarang);
            Assert.Equal("NamaBarang", viewModel.NamaBarang);
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
