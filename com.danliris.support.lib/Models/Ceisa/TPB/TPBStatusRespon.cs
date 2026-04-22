using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace com.danliris.support.lib.Models.Ceisa.TPB
{
    public class TPBStatusRespon : StandardEntity<long>
    {
        public long IdHeader { get; set; }
        [ForeignKey("IdHeader")]
        public virtual TPBHeader TPBHeader { get; set; }

        public string kodeProses { get; set; }
        public string statusProses { get; set; }
        
        public string kodeDokumenUtama { get; set; }
        public string namaDokumenUtama { get; set; }
        public string nomorDokumenUtama { get; set; }

        public string kodeDokumenPendukung { get; set; }
        public string namaDokumenPendukung { get; set; }
        public string nomorDokumenPendukung { get; set; }
    }
}
