namespace PackingApp.Services.API;

public class APIURLs
{
    //users
    public static string Users_AllUsers { get; set; } = "Users/AllUsers";
    public static string Users_UserDetail { get; set; } = "Users/UserDetail";
    public static string Users_AddUser { get; set; } = "Users/AddUser";
    public static string Users_UpdateUser { get; set; } = "Users/UpdateUser";
    public static string Users_DeleteUser { get; set; } = "Users/DeleteUser";
    public static string Users_Login { get; set; } = "Users/Login";
    public static string Users_LoginByAccessToken { get; set; } = "Users/LoginByAccessToken";
    public static string Users_Logout { get; set; } = "Users/Logout";

    //categories
    public static string Categories_AllCategories { get; set; } = "Categories/AllCategories";
    public static string Categories_CategoryDetail { get; set; } = "Categories/CategoryDetail";
    public static string Categories_AddCategory { get; set; } = "Categories/AddCategory";
    public static string Categories_UpdateCategory { get; set; } = "Categories/UpdateCategory";
    public static string Categories_DeleteCategory { get; set; } = "Categories/DeleteCategory";

    //products
    public static string Products_AllProducts { get; set; } = "Products/AllProducts";
    public static string Products_ProductDetail { get; set; } = "Products/ProductDetail";
    public static string Products_AddProduct { get; set; } = "Products/AddProduct";
    public static string Products_UpdateProduct { get; set; } = "Products/UpdateProduct";
    public static string Products_DeleteProduct { get; set; } = "Products/DeleteProduct";
}
