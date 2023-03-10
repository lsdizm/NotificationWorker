using CefSharp;
using CefSharp.OffScreen;

using System;
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
            var url = "https://gwa.ezcaretech.com";
            _chrome = new ChromiumWebBrowser(url);
            //_chrome.async
            var loadResult = await _chrome.LoadUrlAsync(url).ConfigureAwait(false);
            if (loadResult.Success)
            {
                //_chrome.ExecuteScriptAsync("document.getElementById('userId').value = 'burn7';");
                //_chrome.ExecuteScriptAsync("document.getElementById('userPw').value = '!dlwlzpdjxpr1';");
                //_chrome.ExecuteScriptAsync("actionLogin();");

                var result = await _chrome.GetSourceAsync().ConfigureAwait(false);
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



        private void OnLoadingStateChanged(object? sender, LoadingStateChangedEventArgs e)
        {
            _chrome.ExecuteScriptAsync("document.getElementById('userId').value = 'burn7';");
            _chrome.ExecuteScriptAsync("document.getElementById('userPw').value = '!dlwlzpdjxpr1';");
            _chrome.ExecuteScriptAsync("actionLogin();");

            // 소스 조회
            var resultHtml = string.Empty;
            _chrome.GetSourceAsync().ContinueWith(result =>
            {
                resultHtml = result.Result;
            });

            resultHtml = string.Empty;
        }
    }
}
