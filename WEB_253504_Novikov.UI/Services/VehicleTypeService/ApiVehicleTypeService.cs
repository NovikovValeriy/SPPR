using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;
using WEB_253504_Novikov.Domain.Entities;
using WEB_253504_Novikov.Domain.Models;
using WEB_253504_Novikov.UI.Services.VehicleService;

namespace WEB_253504_Novikov.UI.Services.VehicleTypeService
{
    public class ApiVehicleTypeService : IVehicleTypeService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiVehicleService> _logger;

        public ApiVehicleTypeService(
            HttpClient client,
            ILogger<ApiVehicleService> logger)
        {
            _httpClient = client;
            _logger = logger;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
        public async Task<ResponseData<List<VehicleType>>> GetCategoryListAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}VehicleTypes");

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response
                    .Content
                    .ReadFromJsonAsync<ResponseData<List<VehicleType>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return ResponseData<List<VehicleType>>
                    .Error($"Ошибка: {ex.Message}");
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error {response.StatusCode.ToString()}");
            return ResponseData<List<VehicleType>>.Error($"Данные не получены от сервера. Error{response.StatusCode.ToString()}");
        }
    }
}
