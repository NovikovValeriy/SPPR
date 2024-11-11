﻿using System.Text;
using System.Text.Json;
using WEB_253504_Novikov.Domain.Entities;
using WEB_253504_Novikov.Domain.Models;
using WEB_253504_Novikov.UI.Services.FileService;

namespace WEB_253504_Novikov.UI.Services.VehicleService
{
    public class ApiVehicleService : IVehicleService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pageSize;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly ILogger<ApiVehicleService> _logger;
        private readonly IFileService _fileService;

        public ApiVehicleService(
            HttpClient client,
            IConfiguration configuration,
            ILogger<ApiVehicleService> logger,
            IFileService fileService)
        {
            _httpClient = client;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _fileService = fileService;
        }

        //POST Vehicle
        public async Task<ResponseData<Vehicle>> CreateProductAsync(Vehicle product, IFormFile? formFile)
        {
            product.ImagePath = "https://localhost:7002/Images/no-image.png";

            if(formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                if (!string.IsNullOrEmpty(imageUrl))
                    product.ImagePath = imageUrl;
            }

            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Vehicles");

            var multipartContent = new MultipartFormDataContent
            {
                { new StringContent(product.Name), "Name" },
                { new StringContent(product.Description), "Description" },
                { new StringContent(product.TypeId.ToString()), "TypeId" },
                { new StringContent(product.Cost.ToString()), "Cost" },
                { new StringContent(product.ImagePath), "ImagePath" }
            };


            var response = await _httpClient.PostAsync(
                uri,
                multipartContent);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Vehicle>>(_serializerOptions);
                return data; // Vehicle;
            }
            _logger.LogError($"-----> object not created. Error:{response.StatusCode.ToString()}");
            return ResponseData<Vehicle>.Error($"Объект не добавлен. Error:{response.StatusCode.ToString()}");
        }

        //DELETE Vehicle
        public async Task<ResponseData<Vehicle>> DeleteProductAsync(int id)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Vehicles/" + id.ToString());

            var productResponse = await _httpClient.GetAsync(uri);
            if (!productResponse.IsSuccessStatusCode)
            {
                _logger.LogError($"-----> object not deleted. Error:{productResponse.StatusCode.ToString()}");
                return ResponseData<Vehicle>.Error($"Объект не удален. Error:{productResponse.StatusCode.ToString()}");
            }
            var product = await productResponse.Content.ReadFromJsonAsync<ResponseData<Vehicle>>(_serializerOptions);

            await _fileService.DeleteFileAsync(product.Data.ImagePath);

            var response = await _httpClient.DeleteAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<Vehicle>>(_serializerOptions);
                return data;
            }
            _logger.LogError($"-----> object not deleted. Error:{response.StatusCode.ToString()}");
            return ResponseData<Vehicle>.Error($"Объект не удален. Error:{response.StatusCode.ToString()}");
        }

        //GET Vehicle
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

        //GET Vehicle List
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

        //UPDATE
        public async Task<ResponseData<Vehicle>> UpdateProductAsync(int id, Vehicle product, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Vehicles/" + id.ToString());

            if (formFile != null)
            {
                await _fileService.DeleteFileAsync(product.ImagePath);
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                if (!string.IsNullOrEmpty(imageUrl))
                    product.ImagePath = imageUrl;
            }

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
