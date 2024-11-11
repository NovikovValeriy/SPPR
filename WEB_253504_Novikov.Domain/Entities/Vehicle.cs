namespace WEB_253504_Novikov.Domain.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? TypeId { get; set; }
        public double Cost { get; set; }
        public string? ImagePath { get; set; }
        public string? ImageMime { get; set; }
    }
}
