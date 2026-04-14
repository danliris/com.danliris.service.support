using System;
using System.Collections.Generic;
using System.Text;

namespace com.danliris.support.lib.ViewModel.Ceisa
{
    public class Kontainer
    {
        public string jenisKontainer { get; set; }
        public string noKontainer { get; set; }
        public string tipeKontainer { get; set; }
        public string ukuranKontainer { get; set; }
    }
    public class ManifesBC11ViewModel
    {
        public string bendera { get; set; }
        public string caraPengangkutan { get; set; }
        public string idManifesDetail { get; set; }
        public string kodeGudang { get; set; }
        public List<Kontainer> listContainer { get; set; }
        public string namaPemilik { get; set; }
        public string namaPenerima { get; set; }
        public string namaSaranaPengangkut { get; set; }
        public string nilaiSimi { get; set; }
        public string noBc11 { get; set; }
        public string noPos { get; set; }
        public string noVoyage { get; set; }
        public string npwpPemilik { get; set; }
        public string npwpPemilk { get; set; }
        public string npwpPenerima { get; set; }
        public string pelAsal { get; set; }
        public string pelBongkar { get; set; }
        public string pelTransit { get; set; }
        public string respon { get; set; }
        public string tglBc11 { get; set; }
        public string tglTiba { get; set; }
    }
}
