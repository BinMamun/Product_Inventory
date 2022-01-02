using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_Invertory_System.Models
{
    public class ProductType
    {

        public ProductType()
        {
            this.Categories = new List<Category>();
        }

        //Properties
        public int Id { get; set; }

        [Required, StringLength(50), Display(Name ="Product Type")]
        public string ProductTypeName { get; set; }

        [Required, StringLength(300)]
        public string Description { get; set; }

        [StringLength(32)]
        public string _Key { get; set; }
        public int Is_deleted { get; set; }


        //Navigation
        public virtual ICollection<Category> Categories { get; set; }
    }

    public class Category
    {

        public Category()
        {
            this.Products = new List<Product>();
        }

        //Properties
        public int Id { get; set; }

        [Required,StringLength(50), Display(Name ="Category")]
        public string CategoryName { get; set; }

        [Required, StringLength(300)]
        public string Description { get; set; }

        [StringLength(32)]
        public string _Key { get; set; }
        public int  Is_deleted { get; set; }


        //Foreign key
        [Required(ErrorMessage ="Product Type is Required"), ForeignKey("ProductType")]
        public int Product_Type_Id { get; set; }

        //Navigations
        public virtual ProductType ProductType { get; set; }
        public ICollection<Product> Products { get; set; }
    }

    public class Product
    {

        //Properties
        public int Id { get; set; }

        [Required, StringLength(50), Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required, StringLength(300)]
        public string Description { get; set; }

        [ StringLength(32)]
        public string _Key { get; set; }
        public int Is_deleted { get; set; }


        //Foreign key
        [Required, ForeignKey("Category")]
        public int Category_Id { get; set; }

        //Navigation
        public virtual Category Category { get; set; }
    }

    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
