using AdminApp.Dtos;

namespace PackingApp.Services.API;

public interface IClientActions
{
    Task<APIResult> Get(string url);
    Task<APIResult> Post(string url, object data);
    Task<APIResult> Put(string url, object? data=null);
    Task<APIResult> Delete(string url);
}
