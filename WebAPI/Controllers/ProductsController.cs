using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Dtos;
using WebAPI.Enums;
using WebAPI.Helpers;
using WebAPI.Helpers.JwtHelpers;
using WebAPI.Models;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly DatabaseContext db;
    private readonly ERoleName roleName;
    private readonly BCryptHelper bCryptHelper;
    private readonly IJwtHelper jwtHelper;

    public ProductsController(DatabaseContext db, ERoleName roleName, BCryptHelper bCryptHelper, IJwtHelper jwtHelper)
    {
        this.db = db;
        this.roleName = roleName;
        this.bCryptHelper = bCryptHelper;
        this.jwtHelper = jwtHelper;
    }

    [HttpGet("AllProducts")]
    public APIResult AllProducts(string? cateIds, int pageNo = 1, int pageSize = 50)
    {
        List<int>? categoryIds = null;
        if(cateIds != null)
        {
            try
            {
                categoryIds = cateIds.Split(',').Select(e => int.Parse(e)).ToList();
            }
            catch 
            {
                categoryIds = new();
            }
        }
        var totalItems = db.Products.Where(e => e.IsDeleted == false)
            .Include(e => e.Tags)
            .Include(e => e.CategoriesProductsMappings)
            .ThenInclude(e => e.Category)
            .Where(e =>
                categoryIds == null ||
                (e.CategoriesProductsMappings.Select(k => k.CategoryId).Where(k => categoryIds.Contains((int)k!)).FirstOrDefault() != null)
            )
            .Select(e=>1)
            .Count();
        //avoid error caculation of computer (6 / 2 = 3, but sometime the computer result may be 2.999)
        var totalPages = totalItems == 0 ? 0 : (int)Math.Floor(totalItems / (pageSize + 0.1)) + 1;

        var products = db.Products.Where(e => e.IsDeleted == false)
            .Include(e => e.Tags)
            .Include(e => e.CategoriesProductsMappings)
            .ThenInclude(e => e.Category)
            .Include(e => e.CategoriesProductsMappings)
            .ThenInclude(e=> e.Category)
            .Where(e =>
                categoryIds == null ||
                (e.CategoriesProductsMappings.Select(k => k.CategoryId).Where(k => categoryIds.Contains((int)k!)).FirstOrDefault() != null)
            )
            .Select(e => new ProductDto
            {
                Id = e.Id,
                Name = e.Name,
                ShortDescription = e.ShortDescription,
                Description = e.Description,
                Price = e.Price,
                Tags = e.Tags.Select(k => k.TagValue).ToList()!,
                CreatedAt = e.CreatedAt,
                CategoryIds = e.CategoriesProductsMappings
                    .Where(k=> k.Category!.IsDeleted == false)
                    .Select(k => (int)k.CategoryId!)
                    .ToList()
            })
            .Skip(pageSize * (pageNo - 1))
            .Take(pageSize)
            .ToList();
        return new APIResult { Status = EAPIStatus.Success, Data = new ProductQueryDto { Products = products, TotalPages = totalPages } };
    }


    [HttpGet("ProductDetail/{id}")]
    public APIResult ProductDetail(int id)
    {

        var product = db.Products.Where(e => e.IsDeleted == false && e.Id == id)
            .Include(e => e.Tags)
            .Include(e => e.CategoriesProductsMappings)
            .ThenInclude(e => e.Category)
            .Include(e => e.CategoriesProductsMappings)
            .ThenInclude(e => e.Category)
            .Select(e => new ProductDto
            {
                Id = e.Id,
                Name = e.Name,
                ShortDescription = e.ShortDescription,
                Description = e.Description,
                Price = e.Price,
                Tags = e.Tags.Select(k => k.TagValue).ToList()!,
                CreatedAt = e.CreatedAt,
                CategoryIds = e.CategoriesProductsMappings
                    .Where(k => k.Category!.IsDeleted == false)
                    .Select(k => (int)k.CategoryId!)
                    .ToList()
            })
            .FirstOrDefault();
        if(product == null)
        {
            return new APIResult { Status = EAPIStatus.ErrorHasMsg, Msg = "Sản phẩm không tồn tại" };
        }
        return new APIResult { Status = EAPIStatus.Success, Data = product };
    }


    [Authorize(Roles = "Admin,Updater")]
    [HttpPost("AddProduct")]
    public APIResult AddProduct(ProductDto dto)
    {
        int userId = int.Parse(jwtHelper.GetClaimValue("userid")!);

        if (
            string.IsNullOrEmpty(dto.Name)
            )
        {
            return new APIResult { Status = EAPIStatus.ErrorHasMsg, Msg = "Vui lòng cung cấp đủ thông tin" };
        }

        Product model = new Product
        {
            Name = dto.Name,
            CreatedAt = DateTime.UtcNow,
            CreatedById = userId,
            Description = dto.Description,
            Price = dto.Price,
            ShortDescription = dto.ShortDescription,
            Tags = dto.Tags.Select(e => new Tag { TagValue = e }).ToList(),
            CategoriesProductsMappings = dto.CategoryIds.Select(e=> new CategoriesProductsMapping { CategoryId = e}).ToList()
        };

        db.Products.Add(model);

        db.SaveChanges();

        return new APIResult { Status = EAPIStatus.Success };
    }


    [Authorize(Roles = "Admin,Updater")]
    [HttpPut("UpdateProduct")]
    public APIResult UpdateProduct(ProductDto dto)
    {
        if (
             string.IsNullOrEmpty(dto.Name)
             )
        {
            return new APIResult { Status = EAPIStatus.ErrorHasMsg, Msg = "Vui lòng cung cấp đủ thông tin" };
        }

        Product? model = db.Products.Find(dto.Id);
        if (model == null)
        {
            return new APIResult { Status = EAPIStatus.ErrorHasMsg, Msg = "Không tồn tại sản phẩm" };
        }

        model.Name = dto.Name;
        model.ModifiedAt = DateTime.UtcNow;
        model.ShortDescription = dto.ShortDescription;
        model.Description = dto.Description;
        model.Price = dto.Price;
        var myTags = db.Tags.Where(e => e.ProductId == model.Id).ToList();
        db.Tags.RemoveRange(myTags);
        model.Tags = dto.Tags.Select(e => new Tag { TagValue = e }).ToList();
        var myCates = db.CategoriesProductsMappings.Where(e => e.ProductId == model.Id).ToList();
        db.CategoriesProductsMappings.RemoveRange(myCates);
        model.CategoriesProductsMappings = dto.CategoryIds.Select(e => new CategoriesProductsMapping { CategoryId = e }).ToList();

        db.SaveChanges();

        return new APIResult { Status = EAPIStatus.Success };
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("DeleteProduct/{id}")]
    public APIResult DeleteProduct(int id)
    {
        var product = db.Products.Find(id);
        if (product == null)
        {
            return new APIResult { Status = EAPIStatus.ErrorHasMsg, Msg = "Sản phẩm không tồn tại" };
        }

        product.IsDeleted = true;
        product.ModifiedAt = DateTime.UtcNow;

        db.SaveChanges();

        return new APIResult { Status = EAPIStatus.Success };
    }

}
