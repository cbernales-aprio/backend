﻿using backend.api.Data.Generated;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace backend.api.Models.Generated
{
    public partial class Product : IProduct
    {
        [JsonIgnore]
        public ErrorModel ErrorModel { get; set; }

        private readonly FullstackDBContext fullstackDBContext;

        public Product()
        {
            ErrorModel = new ErrorModel();
            this.fullstackDBContext ??= new FullstackDBContext();
        }

        public Product(FullstackDBContext fullstackDBContext)
        {
            this.fullstackDBContext = fullstackDBContext;
            ErrorModel = new ErrorModel();
        }

        public IList<Product> GetAllProducts()
        {
            return [.. fullstackDBContext.Products];
        }

        public Product? GetProductDetails(int id)
        {
            IQueryable<Product> product = fullstackDBContext.Products.Where(x => x.Id == id);
            if (!product.Any())
            {
                ErrorModel.ErrorMessage = "Product not found.";
            }

            return product.First();
        }

        public void AddProduct(Product product)
        {
            fullstackDBContext.Products.Add(product);
            fullstackDBContext.SaveChanges();
        }

        public bool EditProduct(Product product)
        {
            int result = 0;
            try
            {
                fullstackDBContext.Products.Update(product);
                result = fullstackDBContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                ErrorModel.ErrorMessage = $"{exception.Message}";
            }

            return result > 0;
        }

        public bool DeleteProduct(int id)
        {
            int result = 0;
            IQueryable<Product> product = fullstackDBContext.Products.Where(x => x.Id == id);

            if (product.Any())
            {
                fullstackDBContext.Products.Remove(product.First());
                result = fullstackDBContext.SaveChanges();
            }
            else
            {
                ErrorModel.ErrorMessage = $"Failed to delete product with ID '{id}'. ID may not exist.";
            }

            return result > 0;
        }
    }
}