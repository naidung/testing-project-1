using Newtonsoft.Json;
using System.Text;
using AdminApp.Dtos;
using AdminApp.Enums;
using System.Net.Http;
using AdminApp.Helpers;
using AdminApp.Extensions;

namespace PackingApp.Services.API;

public class ClientActions : IClientActions
{
    public ClientActions()
    {

    }

    public static ClientActions Instance
    {
        get => new ClientActions();
    }

    private string domain { get; } = "http://localhost:7001/api";

    private HttpClient GenClient(string? url = "", double timeout = 3000)
    {
        var client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(timeout);
        if(LocalDB.User != null)
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {LocalDB.User.Jwt}");
        }
        return client;
    }

    private string GenFullUrl(string url)
    {
        return $"{domain}/{url}";
    }

    public async Task<APIResult> Get(string url)
    {
        try
        {
            var client = GenClient();
            url = GenFullUrl(url);
            HttpResponseMessage response = await client.GetAsync(url);
            int statusCode = (int)response.StatusCode;
            if (statusCode >= 401 && statusCode < 500)
            {
                return new APIResult
                {
                    Status = EAPIStatus.LoginRequest
                };
            }
            string responseBody = await response.Content.ReadAsStringAsync();
            APIResult? result = responseBody.DeserializeString<APIResult>();
            if(result != null)
            {
                return result;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return new APIResult
        {
            Status = EAPIStatus.ErrorHasMsg,
            Msg = "Xảy ra lỗi. Vui lòng kiểm tra và thử lại",
        };
    }

    public async Task<APIResult> Post(string url, object data)
    {
        try
        {
            var client = GenClient();
            string jsonData = JsonConvert.SerializeObject(data);
            HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            url = GenFullUrl(url);
            HttpResponseMessage response = await client.PostAsync(url, content);
            int statusCode = (int)response.StatusCode;
            if (statusCode >= 401 && statusCode < 500)
            {
                return new APIResult
                {
                    Status = EAPIStatus.LoginRequest
                };
            }
            string responseBody = await response.Content.ReadAsStringAsync();
            APIResult? result = responseBody.DeserializeString<APIResult>();
            if (result != null)
            {
                return result;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new APIResult
            {
                Status = EAPIStatus.ErrorHasMsg,
                Msg = ex.Message,
            };
        }
        return new APIResult
        {
            Status = EAPIStatus.ErrorHasMsg,
            Msg = "Xảy ra lỗi. Vui lòng kiểm tra và thử lại",
        };
    }

    public async Task<APIResult> Put(string url, object? data = null)
    {
        try
        {
            var client = GenClient();
            string jsonData = JsonConvert.SerializeObject(data);
            HttpContent? content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            url = GenFullUrl(url);
            HttpResponseMessage response = await client.PutAsync(url, content);
            int statusCode = (int)response.StatusCode;
            if (statusCode >= 401 && statusCode < 500)
            {
                return new APIResult
                {
                    Status = EAPIStatus.LoginRequest
                };
            }
            string responseBody = await response.Content.ReadAsStringAsync();
            APIResult? result = responseBody.DeserializeString<APIResult>();
            if (result != null)
            {
                return result;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return new APIResult
        {
            Status = EAPIStatus.ErrorHasMsg,
            Msg = "Xảy ra lỗi. Vui lòng kiểm tra và thử lại",
        };
    }

    public async Task<APIResult> Delete(string url)
    {
        try
        {
            var client = GenClient();
            url = GenFullUrl(url);
            HttpResponseMessage response = await client.DeleteAsync(url);
            int statusCode = (int)response.StatusCode;
            if (statusCode >= 401 && statusCode < 500)
            {
                return new APIResult
                {
                    Status = EAPIStatus.LoginRequest
                };
            }
            string responseBody = await response.Content.ReadAsStringAsync();
            APIResult? result = responseBody.DeserializeString<APIResult>();
            if (result != null)
            {
                return result;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return new APIResult
        {
            Status = EAPIStatus.ErrorHasMsg,
            Msg = "Xảy ra lỗi. Vui lòng kiểm tra và thử lại",
        };
    }
}
