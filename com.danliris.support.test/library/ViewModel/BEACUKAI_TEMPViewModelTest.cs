using com.danliris.support.lib.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace com.danliris.support.test.library.ViewModel
{
    public class BEACUKAI_TEMPViewModelTest
    {
        [Fact]
        public void Should_Success_BEACUKAI_TEMPViewModel()
        {
            BEACUKAI_TEMPViewModel viewModel = new BEACUKAI_TEMPViewModel();

            var now = DateTime.Now;

            viewModel.ID = "ID";
            viewModel.BCId = "BCId";
            viewModel.BCNo = "BCNo";
            viewModel.Barang = "Barang";
            viewModel.Bruto = 1;
            viewModel.CIF = 1;
            viewModel.CIF_Rupiah = 1;
            viewModel.KodeDokumenPabean = "KodeDokumenPabean";
            viewModel.JumlahSatBarang = 1;
            viewModel.KodeBarang = "KodeBarang";
            viewModel.KodeKemasan = "KodeKemasan";
            viewModel.NamaKemasan = "NamaKemasan";
            viewModel.Netto = 1;
            viewModel.NoAju = "NoAju";
            viewModel.Netto = 1;
            viewModel.NoAju = "NoAju";
            viewModel.TglDaftarAju = now;
            viewModel.TglBCNo = now; 
            viewModel.NamaSupplier = "NamaSupplier";
            viewModel.NamaSupplier = "NamaSupplier";

            Assert.Equal("ID", viewModel.ID);
            Assert.Equal("BCId", viewModel.BCId);
            Assert.Equal("BCNo", viewModel.BCNo);
            Assert.Equal("Barang", viewModel.Barang);
            Assert.Equal(1, viewModel.Bruto);
            Assert.Equal(1, viewModel.CIF);
            Assert.Equal(1, viewModel.CIF_Rupiah);
            Assert.Equal("KodeDokumenPabean", viewModel.KodeDokumenPabean);
            Assert.Equal(1, viewModel.JumlahSatBarang);
            Assert.Equal("KodeBarang", viewModel.KodeBarang);
            Assert.Equal("KodeKemasan", viewModel.KodeKemasan);
            Assert.Equal("NamaKemasan", viewModel.NamaKemasan);
            Assert.Equal("KodeBarang", viewModel.KodeBarang);
            Assert.Equal(1, viewModel.Netto);
            Assert.Equal("NoAju", viewModel.NoAju);
            Assert.Equal(now, viewModel.TglDaftarAju);
            Assert.Equal(now, viewModel.TglBCNo);
            Assert.Equal("NoAju", viewModel.NoAju);
            


        }
        }
}
