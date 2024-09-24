using WEB_253504_Novikov.Domain.Entities;
using WEB_253504_Novikov.Domain.Models;

namespace WEB_253504_Novikov.API.Services.VehicleTypeServices
{
    public interface IVehicleTypeService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<List<VehicleType>>> GetCategoryListAsync();
    }
}
