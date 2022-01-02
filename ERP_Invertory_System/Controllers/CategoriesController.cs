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
    public class CategoriesController : Controller
    {
        private readonly InventoryDbContext _db;
        public CategoriesController(InventoryDbContext db)
        {
            this._db = db;
        }
        public IActionResult Index(string search = "")
        {
            ViewBag.ProductTypes = _db.ProductTypes.ToList();
            ViewBag.Search = search;
            if (!string.IsNullOrEmpty(search))
            {
                var data = this._db.Categories
                    .Where(x => x.CategoryName.ToLower().StartsWith(search.ToLower()))
                    .Where(p => p.Is_deleted == 0)
                    .ToList();
                return View(data);
            }
            else
                return View(this._db.Categories.Where(p => p.Is_deleted == 0).ToList());
        }

    
        public IActionResult Create()
        {
            ViewBag.ProductTypes = _db.ProductTypes.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category._Key = utility.RandomString(32);
                category.Is_deleted = 0;
                _db.Categories.Add(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult DeleteSelected(int[] ids)
        {
            foreach (int i in ids)
            {
                var existing = _db.Categories.FirstOrDefault(x => x.Id == i);
                if (existing != null)
                    existing.Is_deleted = 1;
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int Id)
        {
            var existing = _db.Categories.FirstOrDefault(x => x.Id == Id);
            if (existing != null)
                existing.Is_deleted = 1;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

       
        public IActionResult Edit(int id, string _Key)
        {
            var data = _db.Categories.FirstOrDefault(x => x.Id == id);
            if (data == null)
                return NotFound();
            else
            {
                if (data._Key != _Key)
                    return BadRequest();
            }
            ViewBag.ProductTypes = _db.ProductTypes.ToList();
            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var data = _db.Categories.FirstOrDefault(x => x.Id == category.Id);
                if (data == null)
                    return NotFound();
                else
                {
                    if (data.Id != category.Id)
                        return BadRequest();
                    else
                         if (data._Key != category._Key)
                        return BadRequest();

                }
                _db.Entry(data).State = EntityState.Detached;
                _db.Entry(category).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductTypes = _db.ProductTypes.ToList();
            return View(category);
        }
    }
}
