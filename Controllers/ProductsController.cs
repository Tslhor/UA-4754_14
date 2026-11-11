using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductsWithRouting.Models;
using ProductsWithRouting.Services;

namespace ProductsWithRouting.Controllers
{
    public class ProductsController : Controller
    {
        private List<Product> myProducts;

        public ProductsController(Data data)
        {
            myProducts = data.Products;
        }

        [HttpGet("products")]
        [HttpGet("products/index")]
        [HttpGet("items")]
        [HttpGet("items/index")]
        public IActionResult Index(int? filterId, string filtername)
        {
            IEnumerable<Product> products = myProducts;

            if (filterId.HasValue)
            {
                products = products.Where(product => product.Id == filterId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filtername))
            {
                products = products.Where(product => product.Name.Contains(filtername, StringComparison.OrdinalIgnoreCase));
            }

            return View(products.ToList());
        }

        [HttpGet("products/{id:int}")]
        public IActionResult View(int id)
        {
            var product = myProducts.FirstOrDefault(item => item.Id == id);
            return product == null ? ProductNotFound() : View(product);
        }

        [HttpGet("products/edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var product = myProducts.FirstOrDefault(item => item.Id == id);
            return product == null ? ProductNotFound() : View(product);
        }

        [HttpPost("products/edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            var existingProduct = myProducts.FirstOrDefault(item => item.Id == product.Id);
            if (existingProduct == null)
            {
                return ProductNotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("products/create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            product.Id = myProducts.Count == 0 ? 1 : myProducts.Max(item => item.Id) + 1;
            myProducts.Add(product);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("products/create")]
        [HttpGet("products/new")]
        public IActionResult Create()
        {
            return View(new Product());
        }

        [HttpGet("products/delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var product = myProducts.FirstOrDefault(item => item.Id == id);
            if (product == null)
            {
                return ProductNotFound();
            }

            myProducts.Remove(product);
            return RedirectToAction(nameof(Index));
        }

        private IActionResult ProductNotFound()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }
    }
}
