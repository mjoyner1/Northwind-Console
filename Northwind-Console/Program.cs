using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NLog;
using NorthwindConsole.Models;

namespace NorthwindConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            try
            {
                string choice;
                do
                {
                    Console.WriteLine("1) Display Categories");
                    Console.WriteLine("2) Add Category");
                    Console.WriteLine("3) Edit Category");
                    Console.WriteLine("4) Display Category and related products");
                    Console.WriteLine("5) Display all Categories and their active products");
                    Console.WriteLine("6) Display specific Category and its active products");
                    Console.WriteLine("7) Add Product");
                    Console.WriteLine("8) Edit Product");
                    Console.WriteLine("9) Display Products");
                    Console.WriteLine("10) Display Specific Product");

                    Console.WriteLine("\"q\" to quit");
                    choice = Console.ReadLine();
                    Console.Clear();
                    logger.Info($"Option {choice} selected");
                    if (choice == "1")
                    {
                        var db = new NorthwindContext();
                        var query = db.Categories.OrderBy(p => p.CategoryName);

                        Console.WriteLine($"{query.Count()} records returned");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryName} - {item.Description}");
                        }
                    }
                    else if (choice == "2")
                    {
                        Category category = new Category();
                        Console.WriteLine("Enter Category Name:");
                        category.CategoryName = Console.ReadLine();
                        Console.WriteLine("Enter the Category Description:");
                        category.Description = Console.ReadLine();

                        ValidationContext context = new ValidationContext(category, null, null);
                        List<ValidationResult> results = new List<ValidationResult>();

                        var isValid = Validator.TryValidateObject(category, context, results, true);
                        if (isValid)
                        {
                            var db = new NorthwindContext();
                            // check for unique name
                            if (db.Categories.Any(c => c.CategoryName == category.CategoryName))
                            {
                                // generate validation error
                                isValid = false;
                                results.Add(new ValidationResult("Name exists", new string[] { "CategoryName" }));
                            }
                            else
                            {
                                logger.Info("Validation passed");
                                // TODO: save category to db
                                db.Categories.Add(category);
                                db.SaveChanges();
                            }
                        }
                        if (!isValid)
                        {
                            foreach (var result in results)
                            {
                                logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                            }
                        }
                    }
                    else if (choice == "3")
                    {
                        var db = new NorthwindContext();
                        var query = db.Categories.OrderBy(p => p.CategoryId);

                        Console.WriteLine("Select the category you want to edit:");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
                        }
                        int id = int.Parse(Console.ReadLine());
                        Console.Clear();
                        logger.Info($"CategoryId {id} selected");
                        Category category = db.Categories.FirstOrDefault(c => c.CategoryId == id);
                        Console.WriteLine("Would you like to edit Category Name?(y/n): ");
                        string editname = Console.ReadLine();

                        if (editname.ToLower() == "y")

                        {
                            Console.WriteLine("Enter Name:");
                            category.CategoryName = Console.ReadLine();
                        }
                        Console.WriteLine("Would you like to edit Category Description?(y/n): ");
                        string editDescription = Console.ReadLine();

                        if (editDescription.ToLower() == "y")

                        {
                            Console.WriteLine("Enter Description:");
                            category.Description = Console.ReadLine();
                        }
                        logger.Info("Edited Category Successfully");
                        db.SaveChanges();


                    }
                    else if (choice == "4")
                    {
                        var db = new NorthwindContext();
                        var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryName}");
                            foreach (Product p in item.Products)
                            {
                                Console.WriteLine($"\t{p.ProductName}");
                            }
                        }
                    }
                    else if (choice == "5")
                    {
                        var db = new NorthwindContext();
                        var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryName}");
                            foreach (Product p in item.Products)
                            {
                                if (p.Discontinued == false)

                                { Console.WriteLine($"\t{p.ProductName}"); }

                            }
                        }
                    }
                    else if (choice == "6")
                    {
                        var db = new NorthwindContext();
                        var query = db.Categories.OrderBy(p => p.CategoryId);

                        Console.WriteLine("Select the category whose products you want to display:");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
                        }
                        int id = int.Parse(Console.ReadLine());
                        Console.Clear();
                        logger.Info($"CategoryId {id} selected");
                        Category category = db.Categories.FirstOrDefault(c => c.CategoryId == id);
                        Console.WriteLine($"{category.CategoryName} - {category.Description}");
                        foreach (Product p in category.Products)
                        {
                            if (p.Discontinued == false)

                            { Console.WriteLine($"\t{p.ProductName}"); }
                        }
                    }
                    else if (choice == "7")
                    {
                        Product product = new Product();
                        Console.WriteLine("Enter Product Name:");
                        product.ProductName = Console.ReadLine();
                        Console.WriteLine("Enter Product Unit Price:");
                        product.UnitPrice = Decimal.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Number of Product Units in Stock:");
                        product.UnitsInStock = (short)Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Is This Product Discontinued? (y/n):");

                        string discontinued = Console.ReadLine();

                        if(discontinued.ToLower()=="y")
                        {
                            product.Discontinued = true;
                        }
                        else if(discontinued.ToLower()=="n")
                        {
                            product.Discontinued = false;
                        }
                      


                        ValidationContext context = new ValidationContext(product, null, null);
                        List<ValidationResult> results = new List<ValidationResult>();

                        var isValid = Validator.TryValidateObject(product, context, results, true);
                        if (isValid)
                        {
                            var db = new NorthwindContext();
                            // check for unique name
                            if (db.Products.Any(c => c.ProductName == product.ProductName))
                            {
                                // generate validation error
                                isValid = false;
                                results.Add(new ValidationResult("Name exists", new string[] { "ProductName" }));
                            }
                            else
                            {
                                logger.Info("Validation passed");
                                // TODO: save category to db
                                db.Products.Add(product);
                                db.SaveChanges();
                            }
                        }
                        if (!isValid)
                        {
                            foreach (var result in results)
                            {
                                logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                            }
                        }
                    }
                    else if (choice == "8")
                    {
                        var db = new NorthwindContext();
                        var query = db.Products.OrderBy(p => p.ProductID);

                        Console.WriteLine("Select the Product you want to edit:");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.ProductID}) {item.ProductName}");
                        }
                        int id = int.Parse(Console.ReadLine());
                        Console.Clear();
                        logger.Info($"ProductID {id} selected");
                        Product product = db.Products.FirstOrDefault(c => c.ProductID == id);
                        Console.WriteLine("Would you like to edit Product Name?(y/n): ");
                        string editname = Console.ReadLine();

                        if (editname.ToLower() == "y")

                        {
                            Console.WriteLine("Enter Name:");
                            product.ProductName = Console.ReadLine();
                        }
                        Console.WriteLine("Would you like to edit Product Unit Price?(y/n): ");
                        string editUnitPrice = Console.ReadLine();

                        if (editUnitPrice.ToLower() == "y")

                        {
                            Console.WriteLine("Enter Unit Price:");
                            product.UnitPrice = Decimal.Parse(Console.ReadLine());
                        }
                        Console.WriteLine("Would you like to edit Product Unit in Stock?(y/n): ");
                        string editUnitsinStock = Console.ReadLine();

                        if (editUnitsinStock.ToLower() == "y")

                        {
                            Console.WriteLine("Enter Unit Price:");
                            product.UnitsInStock = (short)Convert.ToInt32(Console.ReadLine());
                        }
                        logger.Info("Edited Products Successfully");
                        db.SaveChanges();


                    }
                    else if (choice == "9")
                    {
                        var db = new NorthwindContext();
                        var query = db.Products.OrderBy(p => p.ProductName);

                        Console.WriteLine($"{query.Count()} records returned");
                        Console.WriteLine("What Products would you like to see?(All, Active, Discontinued): ");
                        string all = Console.ReadLine();

                        if (all.ToLower() == "all")

                        {
                            Console.WriteLine("Product Name - Discontinued");
                            Console.WriteLine("---------------------------");
                            foreach (var item in query)
                            {
                                Console.WriteLine($"{item.ProductName} - {item.Discontinued}");
                            }
                        }
                        else if (all.ToLower() == "active")

                        {
                            foreach (var item in query)
                            {
                                if (item.Discontinued == false)
                                {
                                    Console.WriteLine($"{item.ProductName}");
                                }
                            }
                        }
                        else if (all.ToLower() == "discontinued")

                        {
                            foreach (var item in query)
                            {
                                if (item.Discontinued == true)
                                {
                                    Console.WriteLine($"{item.ProductName}");
                                }
                            }
                        }

                    }
                    else if (choice == "10")
                    {
                        var db = new NorthwindContext();
                        var query = db.Products.OrderBy(p => p.ProductID);

                        Console.WriteLine("Select the Product you want to display:");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.ProductID}) {item.ProductName}");
                        }
                        int id = int.Parse(Console.ReadLine());
                        Console.Clear();
                        logger.Info($"ProductId {id} selected");
                        Product product = db.Products.FirstOrDefault(c => c.ProductID == id);
                        Console.WriteLine($"Product Name: {product.ProductName}");
                        Console.WriteLine($"Supplier ID: {product.SupplierId}");
                        Console.WriteLine($"Category ID: {product.CategoryId}");
                        Console.WriteLine($"Quantity Per Unit: {product.QuantityPerUnit}");
                        Console.WriteLine($"Unit Price: {product.UnitPrice}");
                        Console.WriteLine($"Units In Stock: {product.UnitsInStock}");
                        Console.WriteLine($"Units on Order: {product.UnitsOnOrder}");
                        Console.WriteLine($"Reorder Level: {product.ReorderLevel}");
                        Console.WriteLine($"Discontinued: {product.Discontinued}");

                    }
                    Console.WriteLine();

                } while (choice.ToLower() != "q");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
    }
}
