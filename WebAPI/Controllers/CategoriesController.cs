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
public class CategoriesController : ControllerBase
{
    private readonly DatabaseContext db;
    private readonly ERoleName roleName;
    private readonly BCryptHelper bCryptHelper;
    private readonly IJwtHelper jwtHelper;

    public CategoriesController(DatabaseContext db, ERoleName roleName, BCryptHelper bCryptHelper, IJwtHelper jwtHelper)
    {
        this.db = db;
        this.roleName = roleName;
        this.bCryptHelper = bCryptHelper;
        this.jwtHelper = jwtHelper;
    }

    [HttpGet("AllCategories")]
    public APIResult AllCategories()
    {
        var cates = db.Categories.Where(e => e.IsDeleted == false)
            .ToList();
        return new APIResult { Status = EAPIStatus.Success, Data = cates };
    }

    [HttpGet("CategoryDetail/{id}")]
    public APIResult CategoryDetail(int id)
    {
        var cate = db.Categories.Where(e => e.IsDeleted == false && e.Id == id)
            .FirstOrDefault();
        if(cate == null)
        {
            return new APIResult { Status = EAPIStatus.ErrorHasMsg, Msg = "Không tồn tại nhóm này" };
        }
        return new APIResult { Status = EAPIStatus.Success, Data = cate };
    }

    [Authorize(Roles = "Admin,Updater")]
    [HttpPost("AddCategory")]
    public APIResult AddCategory(CategoryDto dto)
    {
        //get user id of user who requesting
        int userId = int.Parse(jwtHelper.GetClaimValue("userid")!);

        if (
            string.IsNullOrEmpty(dto.Name))
        {
            return new APIResult { Status = EAPIStatus.ErrorHasMsg, Msg = "Vui lòng cung cấp tên nhóm sản phẩm" };
        }

        if(dto.ParentId != null)
        {
            var parent = db.Categories.Find(dto.ParentId);
            if(parent == null)
            {
                return new APIResult { Status = EAPIStatus.ErrorHasMsg, Msg = "Nhóm sản phẩm cha không tồn tại" };
            }
        }

        Category model = new Category { Name = dto.Name, ParentId = dto.ParentId, CreatedById = userId, CreatedAt = DateTime.UtcNow };

        db.Categories.Add(model);

        db.SaveChanges();

        return new APIResult { Status = EAPIStatus.Success };
    }


    [Authorize(Roles = "Admin,Updater")]
    [HttpPut("UpdateCategory")]
    public APIResult UpdateCategory(CategoryDto dto)
    {
        if (
            string.IsNullOrEmpty(dto.Name))
        {
            return new APIResult { Status = EAPIStatus.ErrorHasMsg, Msg = "Vui lòng cung cấp tên nhóm sản phẩm" };
        }

        var model = db.Categories.Find(dto.Id);
        if(model == null)
        {
            return new APIResult { Status = EAPIStatus.ErrorHasMsg, Msg = "Không tồn tại nhóm sản phẩm" };
        }

        model.Name = dto.Name;
        model.ParentId = dto.ParentId;
        model.ModifiedAt = DateTime.UtcNow;

        db.SaveChanges();

        return new APIResult { Status = EAPIStatus.Success };
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("DeleteCategory/{id}")]
    public APIResult DeleteCategory(int id)
    {
        long userId = long.Parse(jwtHelper.GetClaimValue("userid")!);

        var model = db.Categories.Find(id);
        if (model == null)
        {
            return new APIResult { Status = EAPIStatus.ErrorHasMsg, Msg = "Nhóm sản phẩm không tồn tại" };
        }

        model.IsDeleted = true;
        model.ModifiedAt = DateTime.UtcNow;

        db.SaveChanges();

        return new APIResult { Status = EAPIStatus.Success };
    }

}
