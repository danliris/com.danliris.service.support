using com.danliris.support.lib.Helpers;
using com.danliris.support.lib.Interfaces.Ceisa;
using com.danliris.support.lib.Models.Ceisa.TPB;
using com.danliris.support.lib.ViewModel.Ceisa;
using com.danliris.support.lib.ViewModel.Ceisa.TPBViewModel;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace com.danliris.support.lib.Services.Ceisa
{
    public class CeisaService : ICeisaService
    {
        private readonly IMemoryCache _cache;
        public CeisaService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public class ApiResponse<T>
        {
            public bool Status { get; set; }
            public string Message { get; set; }
            public T Data { get; set; }
        }

        public class PelabuhanViewModel
        {
            public string KodePelabuhan { get; set; }
            public string NamaPelabuhan { get; set; }
            public string KodeKantor { get; set; }
            public string NamaKantor { get; set; }
        }

        private void SetTokenToCache(ResultLoginCeisa viewModel)
        {
            var accessToken = viewModel?.item?.access_token;
            var refreshToken = viewModel?.item?.refresh_token;

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
                return;

            int expiredAccess = (viewModel.item.expires_in ?? 7200) - 60;
            int expiredRefresh = (viewModel.item.refresh_expires_in ?? 14400) - 60;

            _cache.Set("ceisa_access_token", accessToken,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromSeconds(expiredAccess)
                });

            _cache.Set("ceisa_refresh_token", refreshToken,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromSeconds(expiredRefresh)
                });
        }

        public async Task<ResultLoginCeisa> RefreshToken()
        {
            using (var client = new HttpClient())
            {
                var authCeisa = new AuthenticationHeaderValue("Bearer", TokenCeisa.refresh_token);
                client.DefaultRequestHeaders.Authorization = authCeisa;

                var content = new StringContent(JsonConvert.SerializeObject(new { }), Encoding.UTF8, "application/json");
                var response = client.PostAsync($"{APIEndpoint.HostToHost}nle-oauth/v1/user/update-token", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    var contentResp = response.Content.ReadAsStringAsync().Result;
                    ResultLoginCeisa viewModel = JsonConvert.DeserializeObject<ResultLoginCeisa>(contentResp);

                    TokenCeisa.refresh_token = viewModel.item.refresh_token;
                    TokenCeisa.token_ceisa = viewModel.item.access_token;

                    return viewModel;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<string> RefreshAccessToken()
        {
            if (!_cache.TryGetValue("ceisa_refresh_token", out string refreshToken)
                || string.IsNullOrEmpty(refreshToken))
            {
                var login = await Login();
                return login?.item?.access_token;
            }

            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", refreshToken);
                var content = new StringContent("{}", Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{APIEndpoint.HostToHost}nle-oauth/v1/user/update-token", content);
                if (!response.IsSuccessStatusCode)
                {
                    var login = await Login();
                    return login?.item?.access_token;
                }

                var contentResp = await response.Content.ReadAsStringAsync();
                var viewModel = JsonConvert.DeserializeObject<ResultLoginCeisa>(contentResp);

                var newAccessToken = viewModel?.item?.access_token;

                if (!string.IsNullOrEmpty(newAccessToken))
                {
                    int expiredAccess = (viewModel.item.expires_in ?? 7200) - 60;

                    _cache.Set("ceisa_access_token", newAccessToken,
                        new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow =
                                TimeSpan.FromSeconds(expiredAccess)
                        });
                }
                return newAccessToken;
            }
        }

        public async Task<string> GetValidAccessToken()
        {
            if (_cache.TryGetValue("ceisa_access_token", out string token)
                && !string.IsNullOrEmpty(token))
            {
                return token;
            }

            // Kalau access token tidak ada / expired → refresh
            var refreshedToken = await RefreshAccessToken();

            if (string.IsNullOrEmpty(refreshedToken))
                throw new Exception("Gagal mendapatkan access token CEISA");

            return refreshedToken;
        }


        public async Task<ResultLoginCeisa> Login()
        {
            using (var client = new HttpClient())
            {
                var payload = new
                {
                    username = CredentialCeisa.Username,
                    password = CredentialCeisa.Password
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json");

                var response = await client.PostAsync(
                    $"{APIEndpoint.HostToHost}nle-oauth/v1/user/login",
                    content);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Login CEISA gagal");

                var contentResp = await response.Content.ReadAsStringAsync();

                var viewModel = JsonConvert.DeserializeObject<ResultLoginCeisa>(contentResp);

                SetTokenToCache(viewModel);
                return viewModel;
            }
        }

        public async Task<List<RateValutaViewModel>> GetValutaRate(string kode, string token)
        {

            using (var client = new HttpClient())
            {
                var authtoken = await GetValidAccessToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authtoken);
                client.DefaultRequestHeaders.Add("beacukai-api-key", APIEndpoint.APIKeyHostToHost);

                var response = client.GetAsync($"{APIEndpoint.HostToHost}v2/openapi/kurs/{kode}").Result;
                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<ApiResponse<List<RateValutaViewModel>>>(json);

                    if (result?.Data != null)
                    {
                        foreach (var item in result.Data)
                        {
                            item.kodeValuta = kode; // isi manual
                        }
                    }
                    return result?.Data ?? new List<RateValutaViewModel>();
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<LartasViewModel>> GetLartas(string kode, string token)
        {

            using (var client = new HttpClient())
            {
                var authtoken = await GetValidAccessToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authtoken);

                var response = client.GetAsync($"{APIEndpoint.HostToHost}v2/openapi/hs-lartas?kodeHs={kode}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                    List<LartasViewModel> viewModel = JsonConvert.DeserializeObject<List<LartasViewModel>>(result.GetValueOrDefault("data").ToString()); ;
                    return viewModel;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<LartasViewModel>> GetTarifHS(string kode, string token)
        {

            using (var client = new HttpClient())
            {
                var dateNow = DateTime.Now.ToString("yyyy-MM-dd");
                var authCeisa = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Authorization = authCeisa;
                var response = client.GetAsync($"{APIEndpoint.HostToHost}openapi/tarif-hs?kodeHs={kode}&tanggal={dateNow}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                    List<LartasViewModel> viewModel = JsonConvert.DeserializeObject<List<LartasViewModel>>(result.GetValueOrDefault("posTarif").ToString()); ;
                    return viewModel;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<ManifesBC11ViewModel> GetManifestBC11(string kodeKantor, string noHostBl, DateTime tglHostBl, string namaImportir, string token)
        {

            using (var client = new HttpClient())
            {
                var tglManifes = tglHostBl.ToString("dd-MM-yyyy");
                var authtoken = await GetValidAccessToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authtoken);
                client.DefaultRequestHeaders.Add("beacukai-api-key", APIEndpoint.APIKeyHostToHost);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = client.GetAsync($"{APIEndpoint.HostToHost}openapi/manifes-bc11?kodeKantor={kodeKantor}&noHostBl={noHostBl}&tglHostBl={tglManifes}&nama={Uri.EscapeDataString(namaImportir ?? "")}").Result;
                if (response.IsSuccessStatusCode)
                {
                    //var content = response.Content.ReadAsStringAsync().Result;
                    ManifesBC11ViewModel viewModel = JsonConvert.DeserializeObject<ManifesBC11ViewModel>(response.Content.ReadAsStringAsync().Result);
                    //Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                    //ManifesBC11ViewModel viewModel = JsonConvert.DeserializeObject<ManifesBC11ViewModel>(result.GetValueOrDefault("data").ToString()); ;
                    return viewModel;
                }
                else
                {
                    throw new Exception("err");
                }
            }
        }

        public async Task<List<PelabuhanViewModel>> GetPelabuhan(string kodeKantor, string token)
        {
            using (var client = new HttpClient())
            {
                var authtoken = await GetValidAccessToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authtoken);
                client.DefaultRequestHeaders.Add("beacukai-api-key", APIEndpoint.APIKeyHostToHost);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await client.GetAsync(
                    $"{APIEndpoint.HostToHost}v2/openapi/referensi/pelabuhan-dalam-negeri/{kodeKantor}");

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ApiResponse<List<PelabuhanViewModel>>>(json);

                return result?.Data ?? new List<PelabuhanViewModel>();
            }
        }

        public class ResponViewModel
        {
            public string nomorAju { get; set; }
            public string nomorDaftar { get; set; }
            public DateTime? tanggalDaftar { get; set; }
            public string kodeProses { get; set; }
            public DateTime? waktuStatus { get; set; }
            public string keterangan { get; set; }
            public string kodeDokumen { get; set; }
        
        }
        public async Task<List<ResponViewModel>> GetRespon(string kode, string token)
        {

            using (var client = new HttpClient())
            {
                var authtoken = await GetValidAccessToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authtoken);

                var response = client.GetAsync($"{APIEndpoint.HostToHost}openapi/status/{kode}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                    if (result != null && result.TryGetValue("status", out var statusObj))
                    {
                        var status = statusObj?.ToString();

                        if (string.Equals(status, "failed", StringComparison.OrdinalIgnoreCase))
                        {
                            string message = "Terjadi kesalahan";

                            if (result.TryGetValue("message", out var msgObj))
                            {
                                if (msgObj is Newtonsoft.Json.Linq.JArray arr)
                                {
                                    message = string.Join(", ", arr.ToObject<List<string>>());
                                }
                                else
                                {
                                    message = msgObj?.ToString();
                                }
                            }

                            throw new Exception(message);
                        }
                    }

                    List<ResponViewModel> viewModel = JsonConvert.DeserializeObject<List<ResponViewModel>>(result.GetValueOrDefault("dataRespon").ToString());
                    return viewModel;
                }
                else
                {
                    return null;
                }
            }
        }
        public async Task<object> PostingCeisa(object data)
        {
            using (var client = new HttpClient())
            {
                var authtoken = await GetValidAccessToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authtoken);
                client.DefaultRequestHeaders.Add("beacukai-api-key", APIEndpoint.APIKeyHostToHost);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var jsonString = data.ToString();

                var content = new StringContent(
                    jsonString,
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync(
                    $"{APIEndpoint.HostToHost}v2/openapi/document", content);
                var json = await response.Content.ReadAsStringAsync();
                return json;
            }
        }

        public async Task<byte[]> GetPdfFromExternalApi(string noAju)
        {
            using (var client = new HttpClient())
            {
                var authtoken = await GetValidAccessToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authtoken);
                client.DefaultRequestHeaders.Add("beacukai-api-key", APIEndpoint.APIKeyHostToHost);
                client.DefaultRequestHeaders.Add("Accept", "application/json");


                var response = await client.GetAsync(
                    $"{APIEndpoint.HostToHost}v2/openapi/respon/cetak-formulir/final?nomorAju={noAju}");
                return await response.Content.ReadAsByteArrayAsync();
            }
        }

        public async Task<byte[]> GetPdfDokFromExternalApi(string no, string noAju)
        {
            using (var client = new HttpClient())
            {
                var authtoken = await GetValidAccessToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authtoken);
                client.DefaultRequestHeaders.Add("beacukai-api-key", APIEndpoint.APIKeyHostToHost);
                client.DefaultRequestHeaders.Add("Accept", "application/json");


                var response = await client.GetAsync(
                    $"{APIEndpoint.HostToHost}v2/openapi/respon/pdf?kodeRespon={no}&nomorAju={noAju}");
                return await response.Content.ReadAsByteArrayAsync();
            }
        }

        public async Task<TPBStatusResponViewModel> GetResponAll(string noAju)
        {
            using (var client = new HttpClient())
            {
                var authtoken = await GetValidAccessToken();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", authtoken);

                string kodeAju = (!string.IsNullOrEmpty(noAju) && noAju.Length >= 7)
                    ? noAju.Substring(4, 2)
                    : string.Empty;

                var response = await client.GetAsync($"{APIEndpoint.HostToHost}openapi/status/{noAju}");

                if (!response.IsSuccessStatusCode)
                    return new TPBStatusResponViewModel();

                var content = await response.Content.ReadAsStringAsync();

                JObject result;
                try
                {
                    result = JObject.Parse(content);
                }
                catch
                {
                    throw new Exception("Format response tidak valid");
                }

                var status = result["status"]?.ToString();

                if (string.Equals(status, "failed", StringComparison.OrdinalIgnoreCase))
                {
                    var msgToken = result["message"];

                    string message = "Terjadi kesalahan";

                    if (msgToken is JArray arr)
                    {
                        message = string.Join(", ", arr.ToObject<List<string>>() ?? new List<string>());
                    }
                    else if (msgToken != null)
                    {
                        message = msgToken.ToString();
                    }

                    throw new Exception(message);
                }

                var dataStatusArr = result["dataStatus"] as JArray ?? new JArray();
                var dataResponArr = result["dataRespon"] as JArray ?? new JArray();

                var lastStatus = dataStatusArr
                    .OrderByDescending(x =>
                    {
                        return DateTime.TryParse(x["waktuStatus"]?.ToString(), out var dt)
                            ? dt
                            : DateTime.MinValue;
                    })
                    .FirstOrDefault();

                string kodeProses = lastStatus?["kodeProses"]?.ToString();
                string statusProses = lastStatus?["keterangan"]?.ToString();

                JToken dokumenUtama = null;

                if (new[] { "23", "40", "27", "26" }.Contains(kodeAju))
                {
                    dokumenUtama = dataResponArr.FirstOrDefault(x =>
                        x["keterangan"]?.ToString() == "SPPB" ||
                        x["keterangan"]?.ToString() == "SPPKB");
                }
                else if (kodeAju == "25")
                {
                    dokumenUtama = dataResponArr.FirstOrDefault(x =>
                        x["keterangan"]?.ToString() == "BILLING");
                }
                else if (kodeAju == "30")
                {
                    dokumenUtama = dataResponArr.FirstOrDefault(x =>
                        x["keterangan"]?.ToString() == "NPE");
                }

                string kodeDokumenUtama = dokumenUtama?["kodeRespon"]?.ToString();
                string namaDokumenUtama = dokumenUtama?["keterangan"]?.ToString();
                string nomorDokumenUtama = dokumenUtama?["nomorRespon"]?.ToString();

                var dokumenPendukung = dataResponArr
                    .Where(x =>
                    {
                        var ket = x["keterangan"]?.ToString();

                        if (dokumenUtama != null)
                            return !JToken.DeepEquals(x, dokumenUtama);

                        return true;
                    })
                    .ToList();

                var firstPendukung = dokumenPendukung.FirstOrDefault();

                string kodeDokumenPendukung = firstPendukung?["kodeRespon"]?.ToString();
                string namaDokumenPendukung = firstPendukung?["keterangan"]?.ToString();
                string nomorDokumenPendukung = firstPendukung?["nomorRespon"]?.ToString();

                var vm = new TPBStatusResponViewModel
                {
                    noAju = noAju,
                    kodeProses = kodeProses,
                    statusProses = statusProses,

                    kodeDokumenUtama = kodeDokumenUtama,
                    namaDokumenUtama = namaDokumenUtama,
                    nomorDokumenUtama = nomorDokumenUtama,

                    kodeDokumenPendukung = kodeDokumenPendukung,
                    namaDokumenPendukung = namaDokumenPendukung,
                    nomorDokumenPendukung = nomorDokumenPendukung
                };

                return vm;
            }
        }


        public class TPSGudangViewModel
        {
            public string kodeGudang { get; set; }
            public string namaGudang { get; set; }
            public string kodeKantor { get; set; }
        }

        public async Task<List<TPSGudangViewModel>> getTPS(string kodeKantor)
        {
            using (var client = new HttpClient())
            {
                var authtoken = await GetValidAccessToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authtoken);
                client.DefaultRequestHeaders.Add("beacukai-api-key", APIEndpoint.APIKeyHostToHost);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var response = await client.GetAsync(
                    $"{APIEndpoint.HostToHost}v2/openapi/referensi/tps-gudang/{kodeKantor}");
                response.EnsureSuccessStatusCode();
                var content = response.Content.ReadAsStringAsync().Result;
                Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                List<TPSGudangViewModel> viewModel = JsonConvert.DeserializeObject<List<TPSGudangViewModel>>(result.GetValueOrDefault("data").ToString());

                return viewModel ?? new List<TPSGudangViewModel>();
            }
        }

        public async Task<List<PelabuhanViewModel>> GetPelabuhanLuar(string kodePelabuhan)
        {
            using (var client = new HttpClient())
            {
                var authtoken = await GetValidAccessToken();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authtoken);
                client.DefaultRequestHeaders.Add("beacukai-api-key", APIEndpoint.APIKeyHostToHost);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                
                var response = await client.GetAsync(
                    $"{APIEndpoint.HostToHost}v2/openapi/referensi/pelabuhan-luar-negeri/{kodePelabuhan}");
                var content = response.Content.ReadAsStringAsync().Result;
                Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                List<PelabuhanViewModel> viewModel = JsonConvert.DeserializeObject<List<PelabuhanViewModel>>(result.GetValueOrDefault("data").ToString());

                return viewModel ?? new List<PelabuhanViewModel>();
            }
        }
    }
}
