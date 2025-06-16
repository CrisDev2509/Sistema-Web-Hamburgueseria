namespace Bigtoria.utils
{
    public static class States
    {
        public static bool IsChangePassword = false;

        public static Menu MenuSelect { get; set; }
    }

    public enum Menu
    {
        HOME = 1,
        SALE = 2,
        DELIVERY = 3,
        INVENTORY = 4,
        SUPPLIER = 5,
        REPORT = 6,
        ACCOUNT = 7,
        USERS = 8,
    }
}
