using CefSharp;
using CefSharp.OffScreen;
using Newtonsoft;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationWorker
{
    public class BrowserWorker
    {
        private ChromiumWebBrowser _chrome;

        public async Task<string> GetSchedule()
        {
            var result = await MakeSession().ConfigureAwait(false);

            return result;
        }

        private async Task<string> MakeSession()
        {
            //쿠키 데이터 사용하는 방법
            CefSettings settings = new CefSettings();
            settings.CachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CEF";
            CefSharp.Cef.Initialize(settings);

            //웹 사이트 이동
            //var url = "https://gwa.ezcaretech.com";
            var url = "https://camp.xticket.kr/web/main?shopEncode=5f9422e223671b122a7f2c94f4e15c6f71cd1a49141314cf19adccb98162b5b0";
            _chrome = new ChromiumWebBrowser(url);
            //_chrome.async
            var loadResult = await _chrome.LoadUrlAsync(url).ConfigureAwait(false);
            if (loadResult.Success)
            {
                //_chrome.ExecuteScriptAsync("document.getElementById('userId').value = 'burn7';");
                //_chrome.ExecuteScriptAsync("document.getElementById('userPw').value = '!dlwlzpdjxpr1';");
                //_chrome.ExecuteScriptAsync("actionLogin();");

                _chrome.ExecuteScriptAsync("document.getElementById('login_id').value = 'burn7';");
                _chrome.ExecuteScriptAsync("document.getElementById('login_passwd').value = '79qkrqudcjf';");
                _chrome.ExecuteScriptAsync("model.login();");
                var result = await SearchCancelAppointmentSlot().ConfigureAwait(false);

                //var result = await _chrome.GetSourceAsync().ConfigureAwait(false);
                return result;
            }

            else
            {
                return "fail";
            }
            //_chrome = new ChromiumWebBrowser("https://gwa.ezcaretech.com");
            //한국어 설정
            //_chrome.BrowserSettings.AcceptLanguageList = "ko-KR";

            //페이지 로딩 완료 이벤트            
            //_chrome.LoadingStateChanged += OnLoadingStateChanged;

        }


        private async Task<string> SearchCancelAppointmentSlot()
        {
            var date = "20230401";
            var url = string.Format(@"https://camp.xticket.kr/Web/Book/GetBookProduct010001.json?" + 
                                        "product_group_code=0003&start_date={0}&end_date={0}&book_days=1&two_stay_days=0&shopCode=212820734901", 
                                    date);
            var httpClient = new HttpClient();

            try
            {
                var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                var result = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                using var streamReader = new StreamReader(result);
                using var jsonReader = new JsonTextReader(streamReader);

                return jsonReader.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
