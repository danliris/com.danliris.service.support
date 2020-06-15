using com.danliris.support.lib.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace com.danliris.support.test.library.Models
{
    public class BEACUKAI_TEMP_TEST
    {
        [Fact]
        public void Should_Success_Instantiate_BEACUKAI_TEMP()
        {
            var now = DateTime.Now;
            BEACUKAI_TEMP model = new BEACUKAI_TEMP()
            {
                Barang = "Barang",
                BCId = "BCId",
                BCNo = "BCNo",
                Bruto =1,
                CIF =1,
                CIF_Rupiah =1,
                Hari =now,
                ID = "ID",
                IDHeader = 1,
                JenisBC = "JenisBC",
                JenisDokumen = "JenisDokumen",
                JumlahBarang =1,
                JumlahSatBarang =1,
                KodeBarang = "KodeBarang",
                KodeDokumenPabean = "KodeDokumenPabean",
                KodeKemasan = "KodeKemasan",
                KodeSupplier = "KodeSupplier",
                NamaKemasan = "NamaKemasan",
                NamaSupplier = "NamaSupplier",
                Netto =1,
                NoAju = "NoAju",
                NomorDokumen = "NomorDokumen",
                Sat = "Sat",
                TanggalDokumen = "TanggalDokumen",
                TglBCNo =now,
                TglDaftarAju =now,
                TglDatang =now,
                Valuta = "Valuta"

            };

            Assert.Equal("Barang", model.Barang);
            Assert.Equal("BCId", model.BCId);
            Assert.Equal("BCNo", model.BCNo);
            Assert.Equal(1, model.Bruto);
            Assert.Equal(1, model.CIF);
            Assert.Equal(1, model.CIF_Rupiah);
            Assert.Equal(now, model.Hari);
            Assert.Equal("ID", model.ID);
            Assert.Equal(1, model.IDHeader);
            Assert.Equal("JenisBC", model.JenisBC);
            Assert.Equal("JenisDokumen", model.JenisDokumen);
            Assert.Equal(1, model.JumlahBarang);
            Assert.Equal(1, model.JumlahSatBarang);
            Assert.Equal("KodeBarang", model.KodeBarang);
            Assert.Equal("KodeDokumenPabean", model.KodeDokumenPabean);
            Assert.Equal("KodeKemasan", model.KodeKemasan);
            Assert.Equal("NamaSupplier", model.NamaSupplier);
            Assert.Equal(1, model.Netto);
            Assert.Equal("NoAju", model.NoAju);
            Assert.Equal("NomorDokumen", model.NomorDokumen);
            Assert.Equal("Sat", model.Sat);
            Assert.Equal("TanggalDokumen", model.TanggalDokumen);
            Assert.Equal(now, model.TglBCNo);
            Assert.Equal(now, model.TglDaftarAju);
            Assert.Equal(now, model.TglDatang);
            Assert.Equal("Valuta", model.Valuta);
            Assert.Equal("KodeSupplier", model.KodeSupplier);
            Assert.Equal("NamaKemasan", model.NamaKemasan);
        }
        }
}
