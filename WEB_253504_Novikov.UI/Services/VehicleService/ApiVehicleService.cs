using System.Text;
using System.Text.Json;
using WEB_253504_Novikov.Domain.Entities;
using WEB_253504_Novikov.Domain.Models;

namespace WEB_253504_Novikov.UI.Services.VehicleService
{
    public class ApiVehicleService : IVehicleService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiVehicleService> _logger;

        public ApiVehicleService(
            HttpClient client,
            IConfiguration configuration,
            ILogger<ApiVehicleService> logger)
        {
            _httpClient = client;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }
        public async Task<ResponseData<Vehicle>> CreateProductAsync(Vehicle product, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Vehicles");

            var response = await _httpClient.PostAsJsonAsync(
                uri,
                product,
                _serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Vehicle>>(_serializerOptions);
                return data; // Vehicle;
            }
            _logger.LogError($"-----> object not created. Error:{ response.StatusCode.ToString()}");
            return ResponseData<Vehicle>.Error($"Объект не добавлен. Error:{response.StatusCode.ToString()}");
        }

        public async Task<ResponseData<Vehicle>> DeleteProductAsync(int id)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Vehicles/" + id.ToString());

            var response = await _httpClient.DeleteAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Vehicle>>(_serializerOptions);
                return data;
            }
            _logger.LogError($"-----> object not deleted. Error:{response.StatusCode.ToString()}");
            return ResponseData<Vehicle>.Error($"Объект не удален. Error:{response.StatusCode.ToString()}");
        }

        public async Task<ResponseData<Vehicle>> GetProductByIdAsync(int id)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Vehicles/" + id.ToString());

            var response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Vehicle>>(_serializerOptions);
                return data;
            }
            _logger.LogError($"-----> object not aquired. Error:{response.StatusCode.ToString()}");
            return ResponseData<Vehicle>.Error($"Объект не получен. Error:{response.StatusCode.ToString()}");
        }

        public async Task<ResponseData<ListModel<Vehicle>>> GetProductListAsync(string? categoryNormalizedName = null, int pageNo = 1)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Vehicles");
            var query = new StringBuilder(string.Empty);
            
            if (categoryNormalizedName != null) 
                query.Append("?vehicleType=" + categoryNormalizedName);
            if (pageNo > 1)
            {
                if(query.Length > 0) query.Append("&");
                else query.Append("?");
                query.Append("pageNo=" + pageNo.ToString());
            }

            urlString.Append(query.ToString());
           
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response
                    .Content
                    .ReadFromJsonAsync<ResponseData<ListModel<Vehicle>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return ResponseData<ListModel<Vehicle>>
                    .Error($"Ошибка: {ex.Message}");
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error { response.StatusCode.ToString()}");
            return ResponseData<ListModel<Vehicle>>.Error($"Данные не получены от сервера. Error{response.StatusCode.ToString()}");
        }

        public async Task<ResponseData<Vehicle>> UpdateProductAsync(int id, Vehicle product, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Vehicles/" + id.ToString());

            var response = await _httpClient.PutAsJsonAsync(
                uri, 
                product, 
                _serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Vehicle>>(_serializerOptions);
                return data;
            }
            _logger.LogError($"-----> object not deleted. Error:{response.StatusCode.ToString()}");
            return ResponseData<Vehicle>.Error($"Объект не удален. Error:{response.StatusCode.ToString()}");
        }
    }
}
