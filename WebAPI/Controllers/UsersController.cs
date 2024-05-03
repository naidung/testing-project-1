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
public class UsersController : ControllerBase
{
    private readonly DatabaseContext db;
    private readonly ERoleName roleName;
    private readonly BCryptHelper bCryptHelper;
    private readonly IJwtHelper jwtHelper;

    public UsersController(DatabaseContext db, ERoleName roleName, BCryptHelper bCryptHelper, IJwtHelper jwtHelper)
    {
        this.db = db;
        this.roleName = roleName;
        this.bCryptHelper = bCryptHelper;
        this.jwtHelper = jwtHelper;
    }

    [HttpGet("AllUsers")]
    public APIResult AllUsers()
    {
        var users = db.Users.Where(e => e.IsDeleted == false)
            .Include(e => e.UserRoles)
            .Select(e => new UserDto
            {
                Id = e.Id,
                Email = e.Email,
                FullName = e.FullName,
                JoinDate = e.CreatedAt,
                Phone = e.Phone,
                Roles = e.UserRoles.Select(k => (int)k.RoleId!).ToList()
            })
            .ToList();
        return new APIResult { Status = EAPIStatus.Success, Data = users };
    }

    [HttpGet("UserDetail/{id}")]
    public APIResult UserDetail(long Id)
    {
        var user = db.Users.Where(e => e.IsDeleted == false && e.Id == Id)
            .Include(e => e.UserRoles)
            .Select(e => new UserDto
            {
                Id = e.Id,
                Email = e.Email,
                FullName = e.FullName,
                JoinDate = e.CreatedAt,
                Phone = e.Phone,
                Roles = e.UserRoles.Select(k => (int)k.RoleId!).ToList()
            })
            .FirstOrDefault();
        if (user == null) return new APIResult { Status = EAPIStatus.ErrorHasMsg, Msg = "Nhân viên không tồn tại" };
        return new APIResult { Status = EAPIStatus.Success, Data = user };
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("AddUser")]
    public APIResult AddUser(UserDto dto)
    {
        if (
            string.IsNullOrEmpty(dto.FullName) ||
            string.IsNullOrEmpty(dto.UserName) ||
            string.IsNullOrEmpty(dto.Password)
            )
        {
            return new APIResult { Status = EAPIStatus.ErrorHasMsg, Msg = "Vui lòng cung cấp đủ thông tin" };
        }

        List<UserRole> userRoles = new List<UserRole>();
        foreach (var item in dto.Roles)
        {
            if (item == (int)ERole.Admin || item == (int)ERole.Updater)
            {
                userRoles.Add(new UserRole { RoleId = item });
            }
        }

        User model = new User
        {
            FullName = dto.FullName,
            UserName = dto.UserName,
            CreatedAt = DateTime.UtcNow,
            Password = bCryptHelper.GenHashCode(dto.Password),
            Email = dto.Email,
            Phone = dto.Phone,
            UserRoles = userRoles,
            AccessToken = Guid.NewGuid().ToString()
        };

        db.Users.Add(model);

        db.SaveChanges();

        return new APIResult { Status = EAPIStatus.Success };
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("UpdateUser")]
    public APIResult UpdateUser(UserDto dto)
    {
        if (
            string.IsNullOrEmpty(dto.FullName) ||
            string.IsNullOrEmpty(dto.Email) ||
            string.IsNullOrEmpty(dto.Phone)
            )
        {
            return new APIResult { Status = EAPIStatus.ErrorHasMsg, Msg = "Vui lòng cung cấp đủ thông tin" };
        }

        var user = db.Users.Where(e=> e.Id == dto.Id).Include(e=> e.UserRoles).FirstOrDefault();
        if(user == null)
        {
            return new APIResult { Status = EAPIStatus.ErrorHasMsg, Msg = "Nhân viên không tồn tại" };
        }

        var myRoles = db.UserRoles.Where(e => e.UserId == user.Id).ToList();
        db.UserRoles.RemoveRange(myRoles);
        List<UserRole> userRoles = new List<UserRole>();
        foreach (var item in dto.Roles)
        {
            if(item == (int)ERole.Admin || item == (int)ERole.Updater)
            {
                userRoles.Add(new UserRole { RoleId = item });
            }
        }
        user.UserRoles = userRoles;

        user.FullName = dto.FullName;
        user.Email = dto.Email;
        user.Phone = dto.Phone;

        db.SaveChanges();

        return new APIResult { Status = EAPIStatus.Success };
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("DeleteUser/{id}")]
    public APIResult DeleteUser(int id)
    {
        int userId = int.Parse(jwtHelper.GetClaimValue("userid")!);

        if(userId == id)
        {
            return new APIResult { Status = EAPIStatus.ErrorHasMsg, Msg = "Bạn đừng xóa chính bạn chứ!!!" };
        }

        var user = db.Users.Find(id);
        if (user == null)
        {
            return new APIResult { Status = EAPIStatus.ErrorHasMsg, Msg = "Nhân viên không tồn tại" };
        }

        user.IsDeleted = true;
        user.ModifiedAt = DateTime.UtcNow;

        db.SaveChanges();

        return new APIResult { Status = EAPIStatus.Success };
    }


    [HttpPut("Logout/{id}")]
    public APIResult Logout(int id)
    {
        int userId = int.Parse(jwtHelper.GetClaimValue("userid")!);

        var user = db.Users.Find(id);
        if (user == null)
        {
            return new APIResult { Status = EAPIStatus.ErrorHasMsg, Msg = "Nhân viên không tồn tại" };
        }

        user.AccessToken = Guid.NewGuid().ToString();

        db.SaveChanges();

        return new APIResult { Status = EAPIStatus.Success };
    }


    [HttpPost("Login")]
    public APIResult Login(UserDto dto)
    {
        var user = db.Users.Where(e => e.UserName == dto.UserName)
            .Include(e=> e.UserRoles)
            .FirstOrDefault();
        if (user == null)
        {
            return new APIResult { Status = EAPIStatus.ErrorHasMsg, Msg = "Tên đăng nhập không tồn tại. Vui lòng kiểm tra lại" };
        }
        var passwordValid = bCryptHelper.VerifyHashAndPlain(user.Password!, dto.Password!);
        if (!passwordValid)
        {
            return new APIResult { Status = EAPIStatus.ErrorHasMsg, Msg = "Sai mật khẩu. Vui lòng kiểm tra lại" };
        }

        //change access token to prevent login by current access_token on other devices
        user.AccessToken = Guid.NewGuid().ToString();
        db.SaveChanges();

        string jwt = jwtHelper.GenerateToken(user);

        return new APIResult
        {
            Status = EAPIStatus.Success,
            Data = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                JoinDate = user.CreatedAt,
                Phone = user.Phone,
                Roles = user.UserRoles.Select(k => (int)k.RoleId!).ToList(),
                AccessToken = user.AccessToken,
                Jwt = jwt
            }
        };
    }


    [HttpPost("LoginByAccessToken")]
    public APIResult LoginByAccessToken(UserDto dto)
    {

        var user = db.Users.Where(e => e.AccessToken == dto.AccessToken)
            .Include(e => e.UserRoles)
            .FirstOrDefault();
        if (user == null)
        {
            return new APIResult { Status = EAPIStatus.ErrorHasMsg, Msg = "Vui lòng đăng nhập lại" };
        }

        //change access token to prevent login by current access_token on other devices
        user.AccessToken = Guid.NewGuid().ToString();
        string jwt = jwtHelper.GenerateToken(user);

        return new APIResult
        {
            Status = EAPIStatus.Success,
            Data = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                JoinDate = user.CreatedAt,
                Phone = user.Phone,
                Roles = user.UserRoles.Select(k => (int)k.RoleId!).ToList(),
                AccessToken = user.AccessToken,
                Jwt = jwt
            }
        };
    }

}
