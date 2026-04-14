using com.danliris.support.lib.Helpers;
using com.danliris.support.lib.Interfaces.Ceisa;
using com.danliris.support.lib.ViewModel.Ceisa;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

                    List<ResponViewModel> viewModel = JsonConvert.DeserializeObject<List<ResponViewModel>>(result.GetValueOrDefault("dataRespon").ToString()); ;
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
    }
}
