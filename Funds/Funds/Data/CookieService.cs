using Microsoft.JSInterop;
using System.Globalization;

namespace Funds.Data
{

    public interface ICookieService
    {
        public Task CreateCookieAsync(string key, string value, DateTime expire);
        public Task<string?> ReadCookieAsync(string key);
        public Task<bool> DeleteCookieAsync(string key);
    }
    public class CookieService : ICookieService
    {
        private readonly IJSRuntime _jSRuntime;

        public CookieService(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
        }

        public async Task CreateCookieAsync(string key, string value, DateTime expire)
        {
            //await _jSRuntime.InvokeVoidAsync("eval", $"document.cookie = \"{key}={value}; expires={expire.ToString("ddd, d MMM yyyy HH:mm:ss UTC", new CultureInfo("en-US"))}; SameSite=Strict; Secure=true; path=/\"");
        }

        public async Task<bool> DeleteCookieAsync(string key)
        {
            //var check = await ReadCookieAsync(key);
            //if (check is null) 
            //    return false;
            //await CreateCookieAsync(key, "", DateTime.Now.AddDays(-7));
            return true;
        }

        public async Task<string?> ReadCookieAsync(string key)
        {
            //var cookies = await _jSRuntime.InvokeAsync<string>("eval", $"document.cookie");
            //if (string.IsNullOrEmpty(cookies))
            //    return null;

            //var vals = cookies.Split(';');
            //foreach (var val in vals)
            //    if (!string.IsNullOrEmpty(val) && val.IndexOf('=') > 0)
            //        if (val[..val.IndexOf('=')].Trim().Equals(key, StringComparison.OrdinalIgnoreCase))
            //            return val[(val.IndexOf('=') + 1)..];
            return null;
        }
    }
}
