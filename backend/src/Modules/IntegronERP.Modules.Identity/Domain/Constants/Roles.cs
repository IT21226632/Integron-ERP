namespace IntegronERP.Modules.Identity.Domain.Constants;

public static class Roles
{
    public const string Owner = "Owner";
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string Cashier = "Cashier";
    public const string Accountant = "Accountant";

    public static readonly string[] All =
    {
        Owner,
        Admin,
        Manager,
        Cashier,
        Accountant
    };
}