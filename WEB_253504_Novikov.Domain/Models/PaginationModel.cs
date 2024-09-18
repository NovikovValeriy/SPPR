namespace WEB_253504_Novikov.Domain.Models
{
    public class PaginationModel
    {
        public PaginationModel() { }
        public string? CurrentVehicleTypeNormalizedName { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
