using ERP_Invertory_System.Models;
using ERP_Invertory_System.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_Invertory_System.Controllers
{
    public class ProductsController : Controller
    {
        private readonly InventoryDbContext _db;
        public ProductsController(InventoryDbContext db)
        {
            this._db = db;
        }
        public IActionResult Index(string search = "")
        {
            
            ViewBag.Categories = _db.Categories.ToList();
            ViewBag.Search = search;
            if (!string.IsNullOrEmpty(search))
            {
                var data = this._db.Products
                    .Where(x => x.ProductName.ToLower().StartsWith(search.ToLower()))
                    .Where(p => p.Is_deleted == 0)
                    .ToList();
                return View(data);
            }
            else
                return View(this._db.Products.Where(p => p.Is_deleted == 0).ToList());
        }


        public IActionResult Create()
        {
            
            ViewBag.Categories = _db.Categories.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product._Key = utility.RandomString(32);
                product.Is_deleted = 0;
                _db.Products.Add(product);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public IActionResult DeleteSelected(int[] ids)
        {
            foreach (int i in ids)
            {
                var existing = _db.Products.FirstOrDefault(x => x.Id == i);
                if (existing != null)
                    existing.Is_deleted = 1;
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int Id)
        {
            var existing = _db.Products.FirstOrDefault(x => x.Id == Id);
            if (existing != null)
                existing.Is_deleted = 1;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id, string _Key)
        {
            var data = _db.Products.FirstOrDefault(x => x.Id == id);
            if (data == null)
                return NotFound();
            else
            {
                if (data._Key != _Key)
                    return BadRequest();
            }
            
            ViewBag.Categories = _db.Categories.ToList();
            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var data = _db.Products.FirstOrDefault(x => x.Id == product.Id);
                if (data == null)
                    return NotFound();
                else
                {
                    if (data.Id != product.Id)
                        return BadRequest();
                    else
                         if (data._Key != product._Key)
                        return BadRequest();

                }
                _db.Entry(data).State = EntityState.Detached;
                _db.Entry(product).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categories = _db.Categories.ToList();

            return View(product);
        }
    }
}
