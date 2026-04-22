using com.danliris.support.lib.ViewModel.Ceisa;
using com.danliris.support.lib.ViewModel.Ceisa.TPBViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static com.danliris.support.lib.Services.Ceisa.CeisaService;

namespace com.danliris.support.lib.Interfaces.Ceisa
{
    public interface ICeisaService
    {
        Task<List<RateValutaViewModel>> GetValutaRate(string kode, string token);
        Task<List<LartasViewModel>> GetLartas(string kode, string token);
        Task<ManifesBC11ViewModel> GetManifestBC11(string kodeKantor, string noHostBl, DateTime tglHostBl,string nama, string token);
        Task<List<ResponViewModel>> GetRespon(string kode, string token);
        Task<List<LartasViewModel>> GetTarifHS(string kode, string token);
        Task<List<PelabuhanViewModel>> GetPelabuhan(string kodeKantor, string token);
        Task<ResultLoginCeisa> Login();
        Task<ResultLoginCeisa> RefreshToken();
        Task<string> RefreshAccessToken();
        Task<object> PostingCeisa(object data);
        Task<byte[]> GetPdfFromExternalApi(string noAju);
        Task<byte[]> GetPdfDokFromExternalApi(string no ,string noAju);
        Task<TPBStatusResponViewModel> GetResponAll(string noAju);

    }
}
