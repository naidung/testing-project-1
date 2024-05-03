namespace AdminApp.Enums;

public enum ERole
{
    Admin = 1,
    Updater = 2
}

public class ERoleName
{
    public string Admin => "Admin";
    public string Updater => "Updater";

    public ERoleName()
    {

    }

    public string GetRoleName(ERole role)
    {
        switch (role)
        {
            case ERole.Admin:
                {
                    return Admin;
                }
            case ERole.Updater:
                {
                    return Updater;
                }
            default:
                {
                    return "";
                }
        }
    }
}