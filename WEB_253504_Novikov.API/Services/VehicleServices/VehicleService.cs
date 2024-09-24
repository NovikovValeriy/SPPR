using Microsoft.EntityFrameworkCore;
using WEB_253504_Novikov.API.Data;
using WEB_253504_Novikov.Domain.Entities;
using WEB_253504_Novikov.Domain.Models;

namespace WEB_253504_Novikov.API.Services.VehicleServices
{
    public class VehicleService : IVehicleService
    {
        private readonly int _maxPageSize = 20;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VehicleService(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;   
        }

        public async Task<ResponseData<Vehicle>> CreateProductAsync(Vehicle product, IFormFile? formFile)
        {
            try
            {
                if (formFile != null)
                {
                    var path = "Images/" + formFile.FileName;
                    using (var fileStream = new FileStream(_webHostEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await formFile.CopyToAsync(fileStream);
                    }
                    product.ImagePath = path;  ///!!!!!!!!!!!!!!!!!
                }
                _context.Vehicles.Add(product);
                _context.SaveChanges();
                return ResponseData<Vehicle>.Success(product);
            }
            catch
            {
                return ResponseData<Vehicle>.Error("Error in creating new object");
            }
        }

        public async Task<ResponseData<Vehicle>> DeleteProductAsync(int id)
        {
            var response = new ResponseData<Vehicle>();
            try
            {
                var entity = _context.Vehicles.Find(id);
                if (entity == null) throw new Exception();
                _context.Vehicles.Remove(entity);
                _context.SaveChanges();
                response.Successfull = true;
                return response;
            }
            catch
            {
                response.Successfull= false;
                return response;
            }
        }

        public async Task<ResponseData<Vehicle>> GetProductByIdAsync(int id)
        {
            var response = new ResponseData<Vehicle>();
            try
            {
                var entity = _context.Vehicles.Find(id);
                if (entity == null) throw new Exception();
                return ResponseData<Vehicle>.Success(entity);
            }
            catch
            {
                return ResponseData<Vehicle>.Error("No such vehicle");
            }
        }

        public async Task<ResponseData<ListModel<Vehicle>>> GetProductListAsync(string? categoryNormalizedName = null, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;
            var query = _context.Vehicles.AsQueryable();
            var dataList = new ListModel<Vehicle>();
            query = query
                .Where(
                d => categoryNormalizedName == null 
                || 
                d.Type.NormalizedName.Equals(categoryNormalizedName))
                .Include(d => d.Type);

            // количество элементов в списке
            var count = await query.CountAsync(); //.Count();
            if (count == 0)
            {
                return ResponseData<ListModel<Vehicle>>.Success(dataList);
            }
            // количество страниц
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
                return ResponseData<ListModel<Vehicle>>.Error("No such page");
            dataList.Items = await query
            .OrderBy(d => d.Id)
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;
            return ResponseData<ListModel<Vehicle>>.Success(dataList);
        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            try
            {
                var entity = _context.Vehicles.Find(id);
                if (entity == null) throw new Exception();
                var path = "Images/" + formFile.FileName;
                using (var fileStream = new FileStream(_webHostEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }
                entity.ImagePath = path;
                _context.SaveChanges();
                return ResponseData<string>.Success(entity.ImagePath);
            }
            catch (Exception ex)
            {
                return ResponseData<string>.Error(ex.Message);
            }
        }

        public async Task<ResponseData<Vehicle>> UpdateProductAsync(int id, Vehicle product, IFormFile? formFile)
        {
            try
            {
                var entity = _context.Vehicles.Find(id);
                if (entity == null) throw new Exception();
                entity = product;
                entity.Id = id;
                return ResponseData<Vehicle>.Success(entity);
            }
            catch
            {
                return ResponseData<Vehicle>.Error("No such vehicle");
            }
        }
    }
}
