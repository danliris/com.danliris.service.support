using System;
using System.Collections.Generic;
using System.Text;

namespace com.danliris.support.lib.ViewModel.Ceisa.TPBViewModel
{
    public class TPBViewModelList
    {
        public long Id { get; set; }
        public string nomorAju { get; set; }
        public string kodeDokumen { get; set; }
        public string tanggalAju { get; set; }
        public string nomorDaftar { get; set; }
        public string tanggalDaftar { get; set; }
        public string namaPenerima { get; set; }
        public bool isPosted { get; set; }
        public string postedBy { get; set; }
        public string CreatedDate { get; set; }
        public DateTime? tanggalDatang { get; set; }
        public bool isBCTemps { get; set; }

        //StatusRespon
        public string kodeProses { get; set; }
        public string statusProses { get; set; }

        public string kodeDokumenUtama { get; set; }
        public string namaDokumenUtama { get; set; }
        public string nomorDokumenUtama { get; set; }

        public string kodeDokumenPendukung { get; set; }
        public string namaDokumenPendukung { get; set; }
        public string nomorDokumenPendukung { get; set; }
    }

    public class statusCeisa
    {
        public string nomorAju { get; set; }
        public string nomorDaftar { get; set; }
        public string tanggalDaftar { get; set; }
    }
}
