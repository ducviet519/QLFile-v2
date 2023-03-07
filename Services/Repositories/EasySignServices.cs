using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebTools.Models.Entities;
using WebTools.Services.Interface;

namespace WebTools.Services.Repositories
{
    public class EasySignServices : IEasySignServices
    {
        #region API Client
        private readonly string _serial = "540110fc546beb3ae60c61317cde2335";
        private readonly string _pin = "123456";
        private readonly string _baseURL = "http://es.bvta.vn:8080";
        private HttpClient Get_HttpClient(string token = null)
        {
            var _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseURL);
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if(!String.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //_httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }    
            return _httpClient;
        }
        private async Task<string> GetAPI_Token()
        {
            string token = String.Empty;
            try
            {
                var account = new AccountRequest
                {
                    username = "test-cts",
                    password = "123456a@",
                    rememberMe = false
                };
                var requestContent = System.Text.Json.JsonSerializer.Serialize(account);
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, $"/api/authenticate");
                httpRequestMessage.Content = new StringContent(requestContent, Encoding.UTF8, "application/json");
                httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await Get_HttpClient().SendAsync(httpRequestMessage);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    token = JsonConvert.DeserializeObject<TokenResponse>(responseContent).id_token;
                }
                return token;
            }
            catch (Exception ex)
            {
                string Msg = ex.Message;
                return token;
            }
        }
        private async Task<string> GetAPI_Data(HttpMethod method, string uri, string jsonContent)
        {
            string token = await GetAPI_Token();
            try
            {
                var httpRequestMessage = new HttpRequestMessage(method, uri);
                httpRequestMessage.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await Get_HttpClient(await GetAPI_Token()).SendAsync(httpRequestMessage);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return string.Empty;
                }                
            }
            catch (Exception ex)
            {
                string Msg = ex.Message;
                return string.Empty;
            }
        }
        #endregion
        public async Task<InviciableSignFileResponse> InvisiableSignaturePDF(List<FilePDFInviciablContents> fileData)
        {
            InviciableSignFileResponse data = new InviciableSignFileResponse();
            try
            {
                var tokenInfo = new TokenInfo() { serial = _serial, pin = _pin };
                var fileContent = new FilePDFInviciableRequest
                {
                    signingRequestContents = fileData,
                    tokenInfo = tokenInfo
                };
                data = JsonConvert.DeserializeObject<InviciableSignFileResponse>(await GetAPI_Data(HttpMethod.Post, $"/api/sign/invisiblePdf", System.Text.Json.JsonSerializer.Serialize(fileContent)));
                return data;
            }
            catch (Exception ex)
            {
                string Msg = ex.Message;
                return data;
            }
        }
        public async Task<EasySignResponse> DigitalSignaturePDF(List<SigningRequestContents> fileData)
        {
            EasySignResponse data = new EasySignResponse();
            try
            {
                var tokenInfo = new TokenInfo() { serial = _serial, pin = _pin };
                var fileContent = new EasySignRequest
                {
                    signingRequestContents = fileData,
                    tokenInfo = tokenInfo
                };
                var requestContent = System.Text.Json.JsonSerializer.Serialize(fileContent);
                var responeData = await GetAPI_Data(HttpMethod.Post, $"/api/sign/pdf", requestContent);
                data = JsonConvert.DeserializeObject<EasySignResponse>(responeData);
                return data;
            }
            catch (Exception ex)
            {
                string Msg = ex.Message;
                return data;
            }
        }
        public async Task<EasySignResponse> GetImageSignature()
        {
            EasySignResponse responseData = new EasySignResponse();
            try
            {
                var queryParameters = new Dictionary<string, string>
                {
                    { "serial", _serial },
                    { "pin", _pin}
                };
                var dictFormUrlEncoded = new FormUrlEncodedContent(queryParameters);
                var requestContent = await dictFormUrlEncoded.ReadAsStringAsync();
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"/api/certificate/getImage");
                httpRequestMessage.Content = new StringContent(requestContent, Encoding.UTF8, "application/x-www-form-urlencoded");
                HttpResponseMessage response = await Get_HttpClient(await GetAPI_Token()).SendAsync(httpRequestMessage);
                if (response.IsSuccessStatusCode)
                {
                    responseData = JsonConvert.DeserializeObject<EasySignResponse>(await response.Content.ReadAsStringAsync());
                }
                return responseData;
            }
            catch (Exception ex)
            {
                string Msg = ex.Message;
                return responseData;
            }
        }
    }
}
