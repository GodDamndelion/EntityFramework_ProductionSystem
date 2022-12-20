using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_3_EntityFramework_ProductionSystem
{
    class MainDbContext : DbContext
    {
        public MainDbContext() : base("DbConnection") { }
        //public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }
        public DbSet<Type_of_product> Types_of_products { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Use_in> Used_in { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Production_machine> Production_machines { get; set; }
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Type_of_product>()
            //    .HasMany(x => x.Used_in);

            //builder.Entity<Use_in>()
            //    .HasKey(c => new { c.Type_of_product_Id, c.Recipe_Id });

        }
    }
    public class Type_of_product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
        public ICollection<Use_in> Used_in { get; set; }
    }
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public int Volume { get; set; }
        public int Type_Id { get; set; }
        public Type_of_product Type_of_product { get; set; }
    }
    //[PrimaryKey(nameof(Type_of_product_id), nameof(Recipe_id))]
    public class Use_in
    {
        //[Key]
        //[Column(Order=1)]
        [Key]
        public int Id { get; set; }
        public int Type_of_product_Id { get; set; }
        public Type_of_product Type_of_product { get; set; }
        public int Quantity { get; set; }
        public bool Is_output { get; set; }
        //[Key]
        //[Column(Order = 2)]
        public int Recipe_Id { get; set; }
        public Recipe Recipe { get; set; }
    }
    public class Recipe
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Use_in> Used_in { get; set; }
        public ICollection<Production_machine> Production_machines { get; set; }
    }
    public class Production_machine
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Recipe_Id { get; set; }
        public Recipe Recipe { get; set; }
    }
}
