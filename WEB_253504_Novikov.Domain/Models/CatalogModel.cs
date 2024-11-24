using WEB_253504_Novikov.Domain.Entities;

namespace WEB_253504_Novikov.Domain.Models
{
    public class CatalogModel
    {
        public CatalogModel(
           ResponseData<ListModel<Vehicle>> productsResponse,
           ResponseData<List<VehicleType>> categoriesResponse)
        {
            ProductsResponse = productsResponse;
            CategoriesResponse = categoriesResponse;
        }

        public string CurrentVehicleTypeNormalizedName { get; set; }
        public ResponseData<ListModel<Vehicle>> ProductsResponse { get; set; }
        public ResponseData<List<VehicleType>> CategoriesResponse { get; set; }
    }
}
