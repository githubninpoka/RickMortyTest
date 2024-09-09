
namespace MyWebApiNamespace
{
    public interface IWebApiReader
    {
        Task<string> ReadStringAsync(
            string baseurl, 
            string endpoint, 
            string requestparameters = "");
    }
}