﻿namespace Bigtoria.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ShowStore {  get; set; }
        public decimal Price { get; set; }
        public decimal Stock {  get; set; }
        public decimal Discount {  get; set; }
        public bool Status {  get; set; }
        public string? ImagePath {  get; set; }

        //Relaciones
        public int CategoryId {  get; set; }
        public Category Category { get; set; }

        public IEnumerable<SaleDetail> SaleDetail { get; set; }
    }
}
