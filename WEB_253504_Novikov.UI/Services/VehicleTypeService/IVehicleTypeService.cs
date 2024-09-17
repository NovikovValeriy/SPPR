using WEB_253504_Novikov.Domain.Models;
using WEB_253504_Novikov.Domain.Entities;

namespace WEB_253504_Novikov.UI.Services.VehicleTypeService
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
