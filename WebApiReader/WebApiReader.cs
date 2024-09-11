using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApiNamespace;

public class WebApiReader : IWebApiReader
{
    public WebApiReader()
    {
        
    }
    public async Task<string> ReadStringAsync(
        string baseurl, 
        string endpoint, 
        string requestparameters=""
        )
    {
        string request = endpoint + requestparameters;
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(baseurl);

        HttpResponseMessage response = await httpClient.GetAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();        
    }

    public void Dispose()
    {

    }
}
