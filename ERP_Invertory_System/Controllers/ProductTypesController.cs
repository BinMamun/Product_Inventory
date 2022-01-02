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
    public class ProductTypesController : Controller
    {
        private readonly InventoryDbContext _db;
        public ProductTypesController(InventoryDbContext db)
        {
            this._db = db;
        }
        public IActionResult Index(string search = "")
        {
            ViewBag.Search = search;
            if (!string.IsNullOrEmpty(search))
            {
                var data = this._db.ProductTypes
                    .Where(x => x.ProductTypeName.ToLower().StartsWith(search.ToLower()))
                    .Where(p => p.Is_deleted == 0)
                    .ToList();
                return View(data);
            }
            else
                return View(this._db.ProductTypes.Where(p => p.Is_deleted == 0).ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Create(ProductType productType)
        {
            if (ModelState.IsValid)
            {
                productType._Key = utility.RandomString(32);
                productType.Is_deleted = 0;
                _db.ProductTypes.Add(productType);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productType);
        }

        public IActionResult DeleteSelected(int[] ids)
        {
            foreach (int i in ids)
            {
                var existing = _db.ProductTypes.FirstOrDefault(x => x.Id == i);
                if (existing != null)
                    existing.Is_deleted = 1;
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int Id)
        {
            var existing = _db.ProductTypes.FirstOrDefault(x => x.Id == Id);
            if (existing != null)
                existing.Is_deleted = 1;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id, string _Key)
        {
            var data = _db.ProductTypes.FirstOrDefault(x => x.Id == id);
            if (data == null)
                return NotFound();
            else
            {
                if (data._Key != _Key)
                    return BadRequest();
            }

            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(ProductType productType)
        {
            if (ModelState.IsValid)
            {
                var data = _db.ProductTypes.FirstOrDefault(x => x.Id == productType.Id);
                if (data == null)
                    return NotFound();
                else
                {
                    if (data.Id != productType.Id)
                        return BadRequest();
                    else
                         if (data._Key != productType._Key)
                        return BadRequest();

                }
                _db.Entry(data).State = EntityState.Detached;
                _db.Entry(productType).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productType);
        }
    }
}
