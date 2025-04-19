using Bigtoria.ViewModels;

namespace Bigtoria.utils
{
    public static class SalesProduct
    {
        public static List<SaleProductViewModel> products { get; set; } = new List<SaleProductViewModel>();
        
        public static decimal getAmmount()
        {
            decimal total = 0;

            if (products.Count > 0)
            {
                foreach (var p in products)
                    total += ((p.Price * p.Quantity) - ((p.Price * p.Quantity) * p.Discount / 100));

                return Math.Round(total, 2);
            }

            return Math.Round(total, 2);
        }

        public static decimal getIgv()
        {
            decimal igv = getAmmount() * (decimal)0.10;
            return Math.Round(igv, 2);
        }

        public static decimal getSubTotal()
        {
            decimal sub = getAmmount() - getIgv();
            return Math.Round(sub, 2);
        }
    }
}
