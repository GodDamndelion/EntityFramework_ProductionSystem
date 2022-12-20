using System;
using System.Collections.Generic;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Internal;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using Microsoft.EntityFrameworkCore;

namespace lab_3_EntityFramework_ProductionSystem
{
    enum TableChoice { Exit = 0, Types_of_products = 1, Products = 2, Recipes = 3, Production_machines = 4 };
    enum ActionChoice { Cancel = 0, Insert = 1, Update = 2, Delete = 3, View_Data = 4 }
    class Program
    {
        static void Main(/*string[] args*/)
        {
            using (MainDbContext db = new MainDbContext())
            {
                /*SQL команды
                db.Database.ExecuteSqlCommand("ALTER TABLE dbo.Production_machine ADD CONSTRAINT Machines_Recipes FOREIGN KEY(Recipe_Id1) REFERENCES dbo.Recipes(Id) ON DELETE SET NULL");
                db.Database.ExecuteSqlCommand("ALTER TABLE dbo.Use_in ADD CONSTRAINT Used_in_Recipes FOREIGN KEY(Recipe_Id1) REFERENCES dbo.Recipes(Id) ON DELETE CASCADE");
                */
                bool ext = false;
                while (!ext)
                {
                    /*
                    Console.Beep(100, 500);
                    Console.Beep(200, 500);
                    Console.Beep(300, 500);
                    Console.Beep(400, 500);*/
                    Console.WriteLine(" Это начальная страница.");
                    Console.WriteLine(" Выберете таблицу, с которой хотите работать:");
                    Console.WriteLine();
                    for (int t = 1; t <= 4; ++t)
                    {
                        Console.WriteLine($" {t}. {(TableChoice)t}");
                    }
                    Console.WriteLine(" 0. Выход");
                    TableChoice tableChoice = (TableChoice)char.GetNumericValue(Console.ReadKey(true).KeyChar);
                    Console.Clear();
                    switch (tableChoice)
                    {
                        case TableChoice.Types_of_products:
                        case TableChoice.Products:
                        case TableChoice.Recipes:
                        case TableChoice.Production_machines:
                            bool correctAction = false;
                            while (!correctAction)
                            {
                                Console.WriteLine($" Выберете действие, которое хотите совершить с таблицей {tableChoice}:");
                                Console.WriteLine();
                                Console.WriteLine(" 1. Вставить элемент в таблицу");
                                Console.WriteLine(" 2. Изменить элемент таблицы");
                                Console.WriteLine(" 3. Удалить элемент таблицы");
                                Console.WriteLine(" 4. Посмотреть содержимое таблицы");
                                Console.WriteLine(" 0. Отмена");
                                ActionChoice actionChoice = (ActionChoice)char.GetNumericValue(Console.ReadKey(true).KeyChar);
                                Console.Clear();
                            
                                switch (actionChoice)
                                {
                                    case ActionChoice.Insert:
                                    case ActionChoice.Update:
                                    case ActionChoice.Delete:
                                    case ActionChoice.View_Data:
                                        correctAction = true;
                                        Console.WriteLine($" {tableChoice} {actionChoice}:");
                                        Console.WriteLine();
                                        //===============================================================================================================================
                                        switch (tableChoice)
                                        {
                                            case TableChoice.Types_of_products:
                                                switch (actionChoice)
                                                {
                                                    case ActionChoice.Insert:
                                                        {
                                                            //Эти скобочки необязательны, но просто необходимы
                                                            Type_of_product type_of_product = new Type_of_product();
                                                            Console.WriteLine(" Введите имя нового типа продуктов:");
                                                            Console.Write(Environment.NewLine + ' ');
                                                            type_of_product.Name = Console.ReadLine();
                                                            Console.WriteLine();

                                                            if ((from t in db.Types_of_products where t.Name == type_of_product.Name select t).Any())
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine(" Такой тип продукта уже существует");
                                                                Console.WriteLine();
                                                            }
                                                            else
                                                            {
                                                                bool correctSave = false;
                                                                while (!correctSave)
                                                                {
                                                                    Console.WriteLine($" Сохранить новый тип продукта {type_of_product.Name}?");
                                                                    Console.WriteLine();
                                                                    Console.WriteLine(" 1. Да");
                                                                    Console.WriteLine(" 0. Нет");
                                                                    switch (Console.ReadKey(true).KeyChar)
                                                                    {
                                                                        case '1':
                                                                            correctSave = true;
                                                                            db.Types_of_products.Add(type_of_product);
                                                                            db.SaveChanges();
                                                                            Console.WriteLine();
                                                                            Console.WriteLine(" Объекты успешно сохранены");
                                                                            Console.WriteLine();
                                                                            break;
                                                                        case '0':
                                                                            correctSave = true;
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Сохраненение объектов было отменено");
                                                                            Console.WriteLine();
                                                                            break;
                                                                        default:
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                            Console.WriteLine();
                                                                            break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        break;

                                                    case ActionChoice.Update:
                                                        {
                                                            var Types_of_products = db.Types_of_products;
                                                            if (!Types_of_products.Any())
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine(" В таблице нет ни одного элемента");
                                                                Console.WriteLine();
                                                            }
                                                            else
                                                            {
                                                                Type_of_product type_of_product = new Type_of_product();
                                                                int id = 0;
                                                                bool correctInput = false;
                                                                while (!correctInput)
                                                                {
                                                                    correctInput = true;
                                                                    foreach (var t in Types_of_products)
                                                                    {
                                                                        Console.WriteLine($" {t.Id}. {t.Name}");
                                                                    }
                                                                    Console.WriteLine();
                                                                    Console.WriteLine(" Выберете ID элемента для изменения:");
                                                                    Console.Write(Environment.NewLine + ' ');
                                                                    try
                                                                    {
                                                                        id = Convert.ToInt32(Console.ReadLine()); //Можно int.Parse(string) (то же самое)
                                                                    }
                                                                    catch
                                                                    {
                                                                        Console.Clear();
                                                                        Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                        Console.WriteLine();
                                                                        correctInput = false;
                                                                    }
                                                                    if (correctInput)
                                                                    {
                                                                        type_of_product = Types_of_products.Find(id);
                                                                        if (type_of_product == null)
                                                                        {
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Такого типа продуктов нет, попробуйте ещё раз!");
                                                                            Console.WriteLine();
                                                                            correctInput = false;
                                                                        }
                                                                    }
                                                                }
                                                                Console.WriteLine();
                                                                Console.WriteLine(" Введите новое имя:");
                                                                Console.Write(Environment.NewLine + ' ');
                                                                string name = Console.ReadLine();
                                                                Console.WriteLine();

                                                                correctInput = false;
                                                                while (!correctInput)
                                                                {
                                                                    Console.WriteLine($" Изменить тип продукта c {type_of_product.Name} на {name}?");
                                                                    Console.WriteLine();
                                                                    Console.WriteLine(" 1. Да");
                                                                    Console.WriteLine(" 0. Нет");
                                                                    switch (Console.ReadKey(true).KeyChar)
                                                                    {
                                                                        case '1':
                                                                            correctInput = true;
                                                                            type_of_product.Name = name;
                                                                            db.SaveChanges();
                                                                            Console.WriteLine();
                                                                            Console.WriteLine(" Объект успешно изменён");
                                                                            Console.WriteLine();
                                                                            break;
                                                                        case '0':
                                                                            correctInput = true;
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Изменение объекта было отменено");
                                                                            Console.WriteLine();
                                                                            break;
                                                                        default:
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                            Console.WriteLine();
                                                                            break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        break;

                                                    case ActionChoice.Delete:
                                                        {
                                                            var Types_of_products = db.Types_of_products;
                                                            if (!Types_of_products.Any())
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine(" В таблице нет ни одного элемента");
                                                                Console.WriteLine();
                                                            }
                                                            else
                                                            {
                                                                Type_of_product type_of_product = new Type_of_product();
                                                                int id = 0;
                                                                bool correctInput = false;
                                                                while (!correctInput)
                                                                {
                                                                    correctInput = true;
                                                                    foreach (var t in Types_of_products.Include(t => t.Products).Include(t => t.Used_in))
                                                                    {
                                                                        Console.WriteLine($" {t.Id}. {t.Name}");
                                                                    }
                                                                    Console.WriteLine();
                                                                    Console.WriteLine(" Выберете ID элемента для удаления:");
                                                                    Console.Write(Environment.NewLine + ' ');
                                                                    try
                                                                    {
                                                                        id = Convert.ToInt32(Console.ReadLine());
                                                                    }
                                                                    catch
                                                                    {
                                                                        Console.Clear();
                                                                        Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                        Console.WriteLine();
                                                                        correctInput = false;
                                                                    }
                                                                    if (correctInput)
                                                                    {
                                                                        type_of_product = Types_of_products.Find(id);
                                                                        if (type_of_product == null)
                                                                        {
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Такого типа продуктов нет, попробуйте ещё раз!");
                                                                            Console.WriteLine();
                                                                            correctInput = false;
                                                                        }
                                                                    }
                                                                }
                                                                
                                                                if (type_of_product.Products != null && type_of_product.Products.Any())
                                                                {
                                                                    Console.Clear();
                                                                    Console.WriteLine(" Вы не можете удалить этот тип продуктов, т.к. его экземпляры имеются на складе!");
                                                                    Console.WriteLine(" Сперва удалите их!");
                                                                    Console.WriteLine();
                                                                }
                                                                else
                                                                {
                                                                    if (type_of_product.Used_in != null && type_of_product.Used_in.Any())
                                                                    {
                                                                        Console.Clear();
                                                                        Console.WriteLine(" Вы не можете удалить этот тип продуктов, т.к. он нужен в рецептах!");
                                                                        Console.WriteLine(" Сперва удалите их!");
                                                                        Console.WriteLine();
                                                                    }
                                                                    else
                                                                    {
                                                                        Console.WriteLine();
                                                                        correctInput = false;
                                                                        while (!correctInput)
                                                                        {
                                                                            Console.WriteLine($" Удалить тип продукта {type_of_product.Name}?");
                                                                            Console.WriteLine();
                                                                            Console.WriteLine(" 1. Да");
                                                                            Console.WriteLine(" 0. Нет");
                                                                            switch (Console.ReadKey(true).KeyChar)
                                                                            {
                                                                                case '1':
                                                                                    correctInput = true;
                                                                                    db.Types_of_products.Remove(type_of_product);
                                                                                    db.SaveChanges();
                                                                                    Console.WriteLine();
                                                                                    Console.WriteLine(" Объект успешно удалён");
                                                                                    Console.WriteLine();
                                                                                    break;
                                                                                case '0':
                                                                                    correctInput = true;
                                                                                    Console.Clear();
                                                                                    Console.WriteLine(" Удаление объекта было отменено");
                                                                                    Console.WriteLine();
                                                                                    break;
                                                                                default:
                                                                                    Console.Clear();
                                                                                    Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                                    Console.WriteLine();
                                                                                    break;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        break;

                                                    case ActionChoice.View_Data:
                                                        {
                                                            var Types_of_products = db.Types_of_products;
                                                            if (!Types_of_products.Any())
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine(" В таблице нет ни одного элемента");
                                                                Console.WriteLine();
                                                            }
                                                            else
                                                            {
                                                                foreach (var t in Types_of_products)
                                                                {
                                                                    Console.WriteLine($" {t.Id}. {t.Name}");
                                                                }
                                                                Console.WriteLine();
                                                            }
                                                        }
                                                        break;
                                                }
                                                break;
                                            //-------------------------------------------------------------------------------------------------------------------------
                                            case TableChoice.Products:
                                                switch (actionChoice)
                                                {
                                                    case ActionChoice.Insert:
                                                        {
                                                            Product product = new Product();
                                                            var Types_of_products = db.Types_of_products;
                                                            if (!Types_of_products.Any())
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine(" Сначала добавьте хотя бы один тип продукта");
                                                                Console.WriteLine();
                                                            }
                                                            else
                                                            {
                                                                int id = 0;
                                                                bool correctInput = false;
                                                                while (!correctInput)
                                                                {
                                                                    correctInput = true;
                                                                    foreach (var t in Types_of_products)
                                                                    {
                                                                        Console.WriteLine($" {t.Id}. {t.Name}");
                                                                    }
                                                                    Console.WriteLine();
                                                                    Console.WriteLine(" Введите ID типа продуктов:");
                                                                    Console.Write(Environment.NewLine + ' ');
                                                                    try
                                                                    {
                                                                        id = Convert.ToInt32(Console.ReadLine());
                                                                    }
                                                                    catch
                                                                    {
                                                                        Console.Clear();
                                                                        Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                        Console.WriteLine();
                                                                        correctInput = false;
                                                                    }
                                                                    if (correctInput)
                                                                    {
                                                                        if (Types_of_products.Find(id) == null)
                                                                        {
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Такого типа продуктов нет, попробуйте ещё раз!");
                                                                            Console.WriteLine();
                                                                            correctInput = false;
                                                                        }
                                                                    }
                                                                }
                                                                product.Type_Id = id;
                                                                product.Type_of_product = Types_of_products.Find(id);

                                                                int vol = 0;
                                                                correctInput = false;
                                                                while (!correctInput)
                                                                {
                                                                    correctInput = true;
                                                                    Console.WriteLine();
                                                                    Console.WriteLine($" Введите количество продукта {product.Type_of_product.Name}:");
                                                                    Console.Write(Environment.NewLine + ' ');
                                                                    try
                                                                    {
                                                                        vol = Convert.ToInt32(Console.ReadLine());
                                                                    }
                                                                    catch
                                                                    {
                                                                        Console.Clear();
                                                                        Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                        Console.WriteLine();
                                                                        correctInput = false;
                                                                    }
                                                                    if (correctInput)
                                                                    {
                                                                        if (vol < 0)
                                                                        {
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Количество продуктов не может быть отрицательным! Попробуйте ещё раз!");
                                                                            Console.WriteLine();
                                                                            correctInput = false;
                                                                        }
                                                                    }
                                                                }
                                                                product.Volume = vol;
                                                                Console.WriteLine();

                                                                //var currentProduct = (from p in db.Products where p.Type_Id == product.Type_Id select p).FirstOr(null);
                                                                var currentProduct = db.Products.FirstOr(p => p.Type_Id == product.Type_Id, null);
                                                                if (currentProduct != null)
                                                                {
                                                                    correctInput = false;
                                                                    while (!correctInput)
                                                                    {
                                                                        Console.WriteLine($" Изменить количество продукта {currentProduct.Type_of_product.Name} с {currentProduct.Volume} шт. на {currentProduct.Volume + product.Volume} шт.?");
                                                                        Console.WriteLine();
                                                                        Console.WriteLine(" 1. Да");
                                                                        Console.WriteLine(" 0. Нет");
                                                                        switch (Console.ReadKey(true).KeyChar)
                                                                        {
                                                                            case '1':
                                                                                correctInput = true;
                                                                                currentProduct.Volume += product.Volume;
                                                                                db.SaveChanges();
                                                                                Console.WriteLine();
                                                                                Console.WriteLine(" Объект успешно сохранён");
                                                                                Console.WriteLine();
                                                                                break;
                                                                            case '0':
                                                                                correctInput = true;
                                                                                Console.Clear();
                                                                                Console.WriteLine(" Сохраненение объекта было отменено");
                                                                                Console.WriteLine();
                                                                                break;
                                                                            default:
                                                                                Console.Clear();
                                                                                Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                                Console.WriteLine();
                                                                                break;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    correctInput = false;
                                                                    while (!correctInput)
                                                                    {
                                                                        Console.WriteLine($" Сохранить новый продукт {product.Type_of_product.Name} ({product.Volume} шт.)?");
                                                                        Console.WriteLine();
                                                                        Console.WriteLine(" 1. Да");
                                                                        Console.WriteLine(" 0. Нет");
                                                                        switch (Console.ReadKey(true).KeyChar)
                                                                        {
                                                                            case '1':
                                                                                correctInput = true;
                                                                                db.Products.Add(product);
                                                                                db.SaveChanges();
                                                                                Console.WriteLine();
                                                                                Console.WriteLine(" Объект успешно сохранён");
                                                                                Console.WriteLine();
                                                                                break;
                                                                            case '0':
                                                                                correctInput = true;
                                                                                Console.Clear();
                                                                                Console.WriteLine(" Сохраненение объекта было отменено");
                                                                                Console.WriteLine();
                                                                                break;
                                                                            default:
                                                                                Console.Clear();
                                                                                Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                                Console.WriteLine();
                                                                                break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        break;

                                                    case ActionChoice.Update:
                                                        {
                                                            //Тип продуктов на складе менять нельзя! Это нарушение логики
                                                            var Products = db.Products;
                                                            if (!Products.Any())
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine(" В таблице нет ни одного элемента");
                                                                Console.WriteLine();
                                                            }
                                                            else
                                                            {
                                                                Product product = new Product();
                                                                int id = 0;
                                                                bool correctInput = false;
                                                                while (!correctInput)
                                                                {
                                                                    correctInput = true;
                                                                    foreach (var t in Products.Include(p => p.Type_of_product))
                                                                    {
                                                                        Console.WriteLine($" {t.Id}. {t.Type_of_product.Name} ({t.Volume} шт.)");
                                                                    }
                                                                    Console.WriteLine();
                                                                    Console.WriteLine(" Выберете ID элемента для изменения:");
                                                                    Console.Write(Environment.NewLine + ' ');
                                                                    try
                                                                    {
                                                                        id = Convert.ToInt32(Console.ReadLine());
                                                                    }
                                                                    catch
                                                                    {
                                                                        Console.Clear();
                                                                        Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                        Console.WriteLine();
                                                                        correctInput = false;
                                                                    }
                                                                    if (correctInput)
                                                                    {
                                                                        product = Products.Find(id);
                                                                        if (product == null)
                                                                        {
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Такого продукта нет, попробуйте ещё раз!");
                                                                            Console.WriteLine();
                                                                            correctInput = false;
                                                                        }
                                                                    }
                                                                }

                                                                int vol = 0;
                                                                correctInput = false;
                                                                while (!correctInput)
                                                                {
                                                                    correctInput = true;
                                                                    Console.WriteLine();
                                                                    Console.WriteLine($" Введите новое количество продукта {product.Type_of_product.Name}:");
                                                                    Console.Write(Environment.NewLine + ' ');
                                                                    try
                                                                    {
                                                                        vol = Convert.ToInt32(Console.ReadLine());
                                                                    }
                                                                    catch
                                                                    {
                                                                        Console.Clear();
                                                                        Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                        Console.WriteLine();
                                                                        correctInput = false;
                                                                    }
                                                                    if (correctInput)
                                                                    {
                                                                        if (vol < 0)
                                                                        {
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Количество продуктов не может быть отрицательным! Попробуйте ещё раз!");
                                                                            Console.WriteLine();
                                                                            correctInput = false;
                                                                        }
                                                                    }
                                                                }
                                                                Console.WriteLine();

                                                                correctInput = false;
                                                                while (!correctInput)
                                                                {
                                                                    Console.WriteLine($" Изменить количество продукта {product.Type_of_product.Name} c {product.Volume} шт. на {vol} шт.?");
                                                                    Console.WriteLine();
                                                                    Console.WriteLine(" 1. Да");
                                                                    Console.WriteLine(" 0. Нет");
                                                                    switch (Console.ReadKey(true).KeyChar)
                                                                    {
                                                                        case '1':
                                                                            correctInput = true;
                                                                            product.Volume = vol;
                                                                            db.SaveChanges();
                                                                            Console.WriteLine();
                                                                            Console.WriteLine(" Объект успешно изменён");
                                                                            Console.WriteLine();
                                                                            break;
                                                                        case '0':
                                                                            correctInput = true;
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Изменение объекта было отменено");
                                                                            Console.WriteLine();
                                                                            break;
                                                                        default:
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                            Console.WriteLine();
                                                                            break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        break;

                                                    case ActionChoice.Delete:
                                                        {
                                                            var Products = db.Products;
                                                            if (!Products.Any())
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine(" В таблице нет ни одного элемента");
                                                                Console.WriteLine();
                                                            }
                                                            else
                                                            {
                                                                Product product = new Product();
                                                                int id = 0;
                                                                bool correctInput = false;
                                                                while (!correctInput)
                                                                {
                                                                    correctInput = true;
                                                                    foreach (var t in Products.Include(p => p.Type_of_product))
                                                                    {
                                                                        Console.WriteLine($" {t.Id}. {t.Type_of_product.Name} ({t.Volume} шт.)");
                                                                    }
                                                                    Console.WriteLine();
                                                                    Console.WriteLine(" Выберете ID элемента для удаления:");
                                                                    Console.Write(Environment.NewLine + ' ');
                                                                    try
                                                                    {
                                                                        id = Convert.ToInt32(Console.ReadLine());
                                                                    }
                                                                    catch
                                                                    {
                                                                        Console.Clear();
                                                                        Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                        Console.WriteLine();
                                                                        correctInput = false;
                                                                    }
                                                                    if (correctInput)
                                                                    {
                                                                        product = Products.Find(id);
                                                                        if (product == null)
                                                                        {
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Такого продукта нет, попробуйте ещё раз!");
                                                                            Console.WriteLine();
                                                                            correctInput = false;
                                                                        }
                                                                    }
                                                                }

                                                                Console.WriteLine();
                                                                correctInput = false;
                                                                while (!correctInput)
                                                                {
                                                                    Console.WriteLine($" Удалить продукт {product.Id}. {product.Type_of_product.Name} ({product.Volume} шт.)?");
                                                                    Console.WriteLine();
                                                                    Console.WriteLine(" 1. Да");
                                                                    Console.WriteLine(" 0. Нет");
                                                                    switch (Console.ReadKey(true).KeyChar)
                                                                    {
                                                                        case '1':
                                                                            correctInput = true;
                                                                            db.Products.Remove(product);
                                                                            db.SaveChanges();
                                                                            Console.WriteLine();
                                                                            Console.WriteLine(" Объект успешно удалён");
                                                                            Console.WriteLine();
                                                                            break;
                                                                        case '0':
                                                                            correctInput = true;
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Удаление объекта было отменено");
                                                                            Console.WriteLine();
                                                                            break;
                                                                        default:
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                            Console.WriteLine();
                                                                            break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        break;

                                                    case ActionChoice.View_Data:
                                                        {
                                                            var Products = db.Products;
                                                            if (!Products.Any())
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine(" В таблице нет ни одного элемента");
                                                                Console.WriteLine();
                                                            }
                                                            else
                                                            {
                                                                foreach (var t in Products.Include(p => p.Type_of_product))
                                                                {
                                                                    Console.WriteLine($" {t.Id}. {t.Type_of_product.Name} ({t.Volume} шт.)");
                                                                }
                                                                Console.WriteLine();
                                                            }
                                                        }
                                                        break;
                                                }
                                                break;
                                            //-------------------------------------------------------------------------------------------------------------------------
                                            case TableChoice.Recipes:
                                                switch (actionChoice)
                                                {
                                                    case ActionChoice.Insert:
                                                        {
                                                            var Types_of_products = db.Types_of_products;
                                                            if (!Types_of_products.Any())
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine(" Сначала добавьте хотя бы один тип продукта");
                                                                Console.WriteLine();
                                                            }
                                                            else
                                                            {
                                                                Recipe recipe = new Recipe();
                                                                Console.WriteLine(" Введите имя нового рецепта:");
                                                                Console.Write(Environment.NewLine + ' ');
                                                                recipe.Name = Console.ReadLine();
                                                                Console.WriteLine();

                                                                if ((from r in db.Recipes where r.Name == recipe.Name select r).Any())
                                                                {
                                                                    Console.Clear();
                                                                    Console.WriteLine(" Такой рецепт уже существует");
                                                                    Console.WriteLine();
                                                                }
                                                                else
                                                                {
                                                                    var Recipes = db.Recipes.Include(r => r.Used_in);
                                                                    var Used_in = db.Used_in.Include(u => u.Type_of_product);
                                                                    bool another_one = true;
                                                                    while (another_one)
                                                                    {
                                                                        Use_in use_in = new Use_in();
                                                                        int type_id = 0;
                                                                        bool correctInput = false;
                                                                        while (!correctInput)
                                                                        {
                                                                            correctInput = true;
                                                                            foreach (var t in Types_of_products)
                                                                            {
                                                                                Console.WriteLine($" {t.Id}. {t.Name}");
                                                                            }
                                                                            Console.WriteLine();
                                                                            Console.WriteLine($" Введите ID типа продуктов для рецепта {recipe.Name}:");
                                                                            Console.Write(Environment.NewLine + ' ');
                                                                            try
                                                                            {
                                                                                type_id = Convert.ToInt32(Console.ReadLine());
                                                                            }
                                                                            catch
                                                                            {
                                                                                Console.Clear();
                                                                                Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                                Console.WriteLine();
                                                                                correctInput = false;
                                                                            }
                                                                            if (correctInput)
                                                                            {
                                                                                if (Types_of_products.Find(type_id) == null)
                                                                                {
                                                                                    Console.Clear();
                                                                                    Console.WriteLine(" Такого типа продуктов нет, попробуйте ещё раз!");
                                                                                    Console.WriteLine();
                                                                                    correctInput = false;
                                                                                }
                                                                            }
                                                                        }
                                                                        use_in.Type_of_product_Id = type_id;
                                                                        use_in.Type_of_product = Types_of_products.Find(type_id);

                                                                        int quantity = 0;
                                                                        correctInput = false;
                                                                        while (!correctInput)
                                                                        {
                                                                            correctInput = true;
                                                                            Console.WriteLine();
                                                                            Console.WriteLine($" Введите количество продукта {use_in.Type_of_product.Name} для рецепта {recipe.Name}:");
                                                                            Console.Write(Environment.NewLine + ' ');
                                                                            try
                                                                            {
                                                                                quantity = Convert.ToInt32(Console.ReadLine());
                                                                            }
                                                                            catch
                                                                            {
                                                                                Console.Clear();
                                                                                Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                                Console.WriteLine();
                                                                                correctInput = false;
                                                                            }
                                                                            if (correctInput)
                                                                            {
                                                                                if (quantity < 0)
                                                                                {
                                                                                    Console.Clear();
                                                                                    Console.WriteLine(" Количество продуктов не может быть отрицательным! Попробуйте ещё раз!");
                                                                                    Console.WriteLine();
                                                                                    correctInput = false;
                                                                                }
                                                                            }
                                                                        }
                                                                        use_in.Quantity = quantity;
                                                                        Console.WriteLine();

                                                                        bool is_output = false;
                                                                        correctInput = false;
                                                                        while (!correctInput)
                                                                        {
                                                                            Console.WriteLine($" Продукт {use_in.Type_of_product.Name} в рецепте {recipe.Name} идёт на вход?");
                                                                            Console.WriteLine();
                                                                            Console.WriteLine(" 1. Да");
                                                                            Console.WriteLine(" 0. Нет");
                                                                            switch (Console.ReadKey(true).KeyChar)
                                                                            {
                                                                                case '1':
                                                                                    correctInput = true;
                                                                                    Console.WriteLine();
                                                                                    Console.WriteLine($" {use_in.Type_of_product.Name} идёт на вход");
                                                                                    Console.WriteLine();
                                                                                    break;
                                                                                case '0':
                                                                                    correctInput = true;
                                                                                    is_output = true;
                                                                                    Console.WriteLine();
                                                                                    Console.WriteLine($" {use_in.Type_of_product.Name} получается на выходе");
                                                                                    Console.WriteLine();
                                                                                    break;
                                                                                default:
                                                                                    Console.Clear();
                                                                                    Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                                    Console.WriteLine();
                                                                                    break;
                                                                            }
                                                                        }
                                                                        use_in.Is_output = is_output;
                                                                        //use_in.Recipe_Id = recipe.Id;
                                                                        use_in.Recipe = recipe;

                                                                        correctInput = false;
                                                                        while (!correctInput)
                                                                        {
                                                                            Console.WriteLine($" Сохранить {(use_in.Is_output ? "выходной" : "входной")} продукт {use_in.Type_of_product.Name} ({use_in.Quantity} шт.) для рецепта {recipe.Name}?");
                                                                            Console.WriteLine();
                                                                            Console.WriteLine(" 1. Да");
                                                                            Console.WriteLine(" 0. Нет");
                                                                            switch (Console.ReadKey(true).KeyChar)
                                                                            {
                                                                                case '1':
                                                                                    correctInput = true;
                                                                                    //db_tmp.Used_in.Add(use_in);
                                                                                    //use_in = db_tmp.Used_in.First(u => u.Type_of_product_Id == use_in.Type_of_product_Id && u.Quantity == use_in.Quantity && u.Is_output == use_in.Is_output);
                                                                                    if (recipe.Used_in == null)
                                                                                        recipe.Used_in = new List<Use_in>();
                                                                                    recipe.Used_in.Add(use_in);
                                                                                    Console.Clear();
                                                                                    Console.WriteLine(" Объект успешно сохранён");
                                                                                    Console.WriteLine();
                                                                                    break;
                                                                                case '0':
                                                                                    correctInput = true;
                                                                                    Console.Clear();
                                                                                    Console.WriteLine(" Сохраненение объекта было отменено");
                                                                                    Console.WriteLine();
                                                                                    break;
                                                                                default:
                                                                                    Console.Clear();
                                                                                    Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                                    Console.WriteLine();
                                                                                    break;
                                                                            }
                                                                        }

                                                                        if (recipe.Used_in == null || (recipe.Used_in != null && !recipe.Used_in.Any()))
                                                                        {
                                                                            Console.Clear();
                                                                            Console.WriteLine(" В рецепте должен использоваться хотя бы один продукт");
                                                                            Console.WriteLine();
                                                                        }
                                                                        else
                                                                        {
                                                                            correctInput = false;
                                                                            while (!correctInput)
                                                                            {
                                                                                Console.WriteLine($" Сохранить новый рецепт {recipe.Name}?");
                                                                                foreach (var u in recipe.Used_in.OrderByDescending(u => u.Is_output))
                                                                                {
                                                                                    Console.WriteLine($"\t{u.Type_of_product.Name} ({u.Quantity} шт.) {(u.Is_output ? "output" : "input")}");
                                                                                }
                                                                                Console.WriteLine();
                                                                                Console.WriteLine(" 1. Да");
                                                                                Console.WriteLine(" 2. Добавить ещё продукт");
                                                                                Console.WriteLine(" 0. Нет");
                                                                                switch (Console.ReadKey(true).KeyChar)
                                                                                {
                                                                                    case '1':
                                                                                        correctInput = true;
                                                                                        another_one = false;
                                                                                        db.Recipes.Add(recipe);
                                                                                        db.SaveChanges();
                                                                                        recipe = db.Recipes.First(r => r.Name == recipe.Name);
                                                                                        foreach (var u in recipe.Used_in)
                                                                                        {
                                                                                            u.Recipe_Id = u.Recipe.Id;
                                                                                        }
                                                                                        db.SaveChanges();
                                                                                        Console.WriteLine();
                                                                                        Console.WriteLine(" Объекты успешно сохранены");
                                                                                        Console.WriteLine();
                                                                                        break;
                                                                                    case '2':
                                                                                        correctInput = true;
                                                                                        Console.Clear();
                                                                                        break;
                                                                                    case '0':
                                                                                        correctInput = true;
                                                                                        another_one = false;
                                                                                        Console.Clear();
                                                                                        Console.WriteLine(" Сохраненение объектов было отменено");
                                                                                        Console.WriteLine();
                                                                                        break;
                                                                                    default:
                                                                                        Console.Clear();
                                                                                        Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                                        Console.WriteLine();
                                                                                        break;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        break;

                                                    case ActionChoice.Update:
                                                        {
                                                            var Recipes = db.Recipes;
                                                            var Used_in = db.Used_in;
                                                            if (!Recipes.Any())
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine(" В таблице нет ни одного элемента");
                                                                Console.WriteLine();
                                                            }
                                                            else
                                                            {
                                                                Recipe recipe = new Recipe();
                                                                int id = 0;
                                                                bool correctInput = false;
                                                                while (!correctInput)
                                                                {
                                                                    correctInput = true;
                                                                    foreach (var t in Recipes.Include(r => r.Used_in).ToArray())
                                                                    {
                                                                        Console.WriteLine($" {t.Id}. {t.Name}:");
                                                                        foreach (var u in Used_in.Include(u => u.Type_of_product).Where(u => u.Recipe_Id == t.Id).OrderByDescending(u => u.Is_output))
                                                                        {
                                                                            Console.WriteLine($"\t{u.Type_of_product.Name} ({u.Quantity} шт.) {(u.Is_output ? "output" : "input")}");
                                                                        }
                                                                    }
                                                                    Console.WriteLine();
                                                                    Console.WriteLine(" Выберете ID элемента для изменения:");
                                                                    Console.Write(Environment.NewLine + ' ');
                                                                    try
                                                                    {
                                                                        id = Convert.ToInt32(Console.ReadLine()); //Можно int.Parse(string) (то же самое)
                                                                    }
                                                                    catch
                                                                    {
                                                                        Console.Clear();
                                                                        Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                        Console.WriteLine();
                                                                        correctInput = false;
                                                                    }
                                                                    if (correctInput)
                                                                    {
                                                                        recipe = Recipes.Find(id);
                                                                        if (recipe == null)
                                                                        {
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Такого рецепта нет, попробуйте ещё раз!");
                                                                            Console.WriteLine();
                                                                            correctInput = false;
                                                                        }
                                                                    }
                                                                }
                                                                Console.Clear();

                                                                bool correctChoice = false;
                                                                while (!correctChoice)
                                                                {
                                                                    Console.WriteLine($" Как вы хотите изменить рецепт {recipe.Name}?");
                                                                    foreach (var u in recipe.Used_in.OrderByDescending(u => u.Is_output))
                                                                    {
                                                                        Console.WriteLine($"\t{u.Type_of_product.Name} ({u.Quantity} шт.) {(u.Is_output ? "output" : "input")}");
                                                                    }
                                                                    Console.WriteLine();
                                                                    Console.WriteLine(" 1. Поменять имя");
                                                                    Console.WriteLine(" 2. Изменить продукты");
                                                                    Console.WriteLine(" 0. Отмена");
                                                                    Console.WriteLine();
                                                                    switch (Console.ReadKey(true).KeyChar)
                                                                    {
                                                                        case '1':
                                                                            correctChoice = true;
                                                                            {
                                                                                Console.WriteLine(" Введите новое имя:");
                                                                                Console.Write(Environment.NewLine + ' ');
                                                                                string name = Console.ReadLine();
                                                                                Console.WriteLine();

                                                                                correctInput = false;
                                                                                while (!correctInput)
                                                                                {
                                                                                    Console.WriteLine($" Изменить имя рецепта c {recipe.Name} на {name}?");
                                                                                    Console.WriteLine();
                                                                                    Console.WriteLine(" 1. Да");
                                                                                    Console.WriteLine(" 0. Нет");
                                                                                    switch (Console.ReadKey(true).KeyChar)
                                                                                    {
                                                                                        case '1':
                                                                                            correctInput = true;
                                                                                            recipe.Name = name;
                                                                                            db.SaveChanges();
                                                                                            Console.WriteLine();
                                                                                            Console.WriteLine(" Объект успешно изменён");
                                                                                            Console.WriteLine();
                                                                                            break;
                                                                                        case '0':
                                                                                            correctInput = true;
                                                                                            Console.Clear();
                                                                                            Console.WriteLine(" Изменение объекта было отменено");
                                                                                            Console.WriteLine();
                                                                                            break;
                                                                                        default:
                                                                                            Console.Clear();
                                                                                            Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                                            Console.WriteLine();
                                                                                            break;
                                                                                    }
                                                                                }
                                                                            }
                                                                            break;
                                                                        case '2':
                                                                            correctChoice = true;
                                                                            {
                                                                                var Types_of_products = db.Types_of_products;
                                                                                bool another_one = true;
                                                                                while (another_one)
                                                                                {
                                                                                    bool correctTry = false;
                                                                                    while (!correctTry)
                                                                                    {
                                                                                        Console.WriteLine($" Что вы хотите сделать с продуктами в рецепте {recipe.Name}?");
                                                                                        foreach (var u in recipe.Used_in.OrderByDescending(u => u.Is_output))
                                                                                        {
                                                                                            Console.WriteLine($"\t{u.Type_of_product.Name} ({u.Quantity} шт.) {(u.Is_output ? "output" : "input")}");
                                                                                        }
                                                                                        Console.WriteLine();
                                                                                        Console.WriteLine(" 1. Добавить продукт");
                                                                                        Console.WriteLine(" 2. Удалить продукт");
                                                                                        Console.WriteLine(" 0. Сохранить и вернуться");
                                                                                        Console.WriteLine();
                                                                                        switch (Console.ReadKey(true).KeyChar)
                                                                                        {
                                                                                            case '1':
                                                                                                correctTry = true;
                                                                                                {
                                                                                                    Use_in use_in = new Use_in();
                                                                                                    int type_id = 0;
                                                                                                    correctInput = false;
                                                                                                    while (!correctInput)
                                                                                                    {
                                                                                                        correctInput = true;
                                                                                                        foreach (var t in Types_of_products)
                                                                                                        {
                                                                                                            Console.WriteLine($" {t.Id}. {t.Name}");
                                                                                                        }
                                                                                                        Console.WriteLine();
                                                                                                        Console.WriteLine($" Введите ID типа продуктов для рецепта {recipe.Name}:");
                                                                                                        Console.Write(Environment.NewLine + ' ');
                                                                                                        try
                                                                                                        {
                                                                                                            type_id = Convert.ToInt32(Console.ReadLine());
                                                                                                        }
                                                                                                        catch
                                                                                                        {
                                                                                                            Console.Clear();
                                                                                                            Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                                                            Console.WriteLine();
                                                                                                            correctInput = false;
                                                                                                        }
                                                                                                        if (correctInput)
                                                                                                        {
                                                                                                            if (Types_of_products.Find(type_id) == null)
                                                                                                            {
                                                                                                                Console.Clear();
                                                                                                                Console.WriteLine(" Такого типа продуктов нет, попробуйте ещё раз!");
                                                                                                                Console.WriteLine();
                                                                                                                correctInput = false;
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                    use_in.Type_of_product_Id = type_id;
                                                                                                    use_in.Type_of_product = Types_of_products.Find(type_id);
                                                                                                    Console.WriteLine();

                                                                                                    int quantity = 0;
                                                                                                    correctInput = false;
                                                                                                    while (!correctInput)
                                                                                                    {
                                                                                                        correctInput = true;
                                                                                                        Console.WriteLine($" Введите количество продукта {use_in.Type_of_product.Name} для рецепта {recipe.Name}:");
                                                                                                        Console.Write(Environment.NewLine + ' ');
                                                                                                        try
                                                                                                        {
                                                                                                            quantity = Convert.ToInt32(Console.ReadLine());
                                                                                                        }
                                                                                                        catch
                                                                                                        {
                                                                                                            Console.Clear();
                                                                                                            Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                                                            Console.WriteLine();
                                                                                                            correctInput = false;
                                                                                                        }
                                                                                                        if (correctInput)
                                                                                                        {
                                                                                                            if (quantity < 0)
                                                                                                            {
                                                                                                                Console.Clear();
                                                                                                                Console.WriteLine(" Количество продуктов не может быть отрицательным! Попробуйте ещё раз!");
                                                                                                                Console.WriteLine();
                                                                                                                correctInput = false;
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                    use_in.Quantity = quantity;
                                                                                                    Console.WriteLine();

                                                                                                    bool is_output = false;
                                                                                                    correctInput = false;
                                                                                                    while (!correctInput)
                                                                                                    {
                                                                                                        Console.WriteLine($" Продукт {use_in.Type_of_product.Name} в рецепте {recipe.Name} идёт на вход?");
                                                                                                        Console.WriteLine();
                                                                                                        Console.WriteLine(" 1. Да");
                                                                                                        Console.WriteLine(" 0. Нет");
                                                                                                        switch (Console.ReadKey(true).KeyChar)
                                                                                                        {
                                                                                                            case '1':
                                                                                                                correctInput = true;
                                                                                                                Console.WriteLine();
                                                                                                                Console.WriteLine($" {use_in.Type_of_product.Name} идёт на вход");
                                                                                                                Console.WriteLine();
                                                                                                                break;
                                                                                                            case '0':
                                                                                                                correctInput = true;
                                                                                                                is_output = true;
                                                                                                                Console.Clear();
                                                                                                                Console.WriteLine($" {use_in.Type_of_product.Name} получается на выходе");
                                                                                                                Console.WriteLine();
                                                                                                                break;
                                                                                                            default:
                                                                                                                Console.Clear();
                                                                                                                Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                                                                Console.WriteLine();
                                                                                                                break;
                                                                                                        }
                                                                                                    }
                                                                                                    use_in.Is_output = is_output;
                                                                                                    use_in.Recipe = recipe;
                                                                                                    use_in.Recipe_Id = recipe.Id;
                                                                                                    Console.WriteLine();

                                                                                                    correctInput = false;
                                                                                                    while (!correctInput)
                                                                                                    {
                                                                                                        Console.WriteLine($" Сохранить {(use_in.Is_output ? "выходной" : "входной")} продукт {use_in.Type_of_product.Name} ({use_in.Quantity} шт.) для рецепта {recipe.Name}?");
                                                                                                        Console.WriteLine();
                                                                                                        Console.WriteLine(" 1. Да");
                                                                                                        Console.WriteLine(" 0. Нет");
                                                                                                        switch (Console.ReadKey(true).KeyChar)
                                                                                                        {
                                                                                                            case '1':
                                                                                                                correctInput = true;
                                                                                                                if (recipe.Used_in == null)
                                                                                                                    recipe.Used_in = new List<Use_in>();
                                                                                                                recipe.Used_in.Add(use_in);
                                                                                                                db.SaveChanges();
                                                                                                                Console.Clear();
                                                                                                                Console.WriteLine(" Объект успешно сохранён");
                                                                                                                Console.WriteLine();
                                                                                                                break;
                                                                                                            case '0':
                                                                                                                correctInput = true;
                                                                                                                Console.Clear();
                                                                                                                Console.WriteLine(" Сохраненение объекта было отменено");
                                                                                                                Console.WriteLine();
                                                                                                                break;
                                                                                                            default:
                                                                                                                Console.Clear();
                                                                                                                Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                                                                Console.WriteLine();
                                                                                                                break;
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                break;
                                                                                            case '2':
                                                                                                correctTry = true;
                                                                                                {
                                                                                                    Use_in use_in = new Use_in();
                                                                                                    int use_in_id = 0;
                                                                                                    correctInput = false;
                                                                                                    while (!correctInput)
                                                                                                    {
                                                                                                        correctInput = true;
                                                                                                        Console.WriteLine($" Какой продукт из рецепта {recipe.Name} вы хотите удалить?");
                                                                                                        foreach (var u in recipe.Used_in.OrderByDescending(u => u.Is_output))
                                                                                                        {
                                                                                                            Console.WriteLine($"\t{u.Id}: {u.Type_of_product.Name} ({u.Quantity} шт.) {(u.Is_output ? "output" : "input")}");
                                                                                                        }
                                                                                                        Console.WriteLine();
                                                                                                        Console.WriteLine(" Выберете ID элемента для удаления:");
                                                                                                        Console.Write(Environment.NewLine + ' ');
                                                                                                        try
                                                                                                        {
                                                                                                            use_in_id = Convert.ToInt32(Console.ReadLine());
                                                                                                        }
                                                                                                        catch
                                                                                                        {
                                                                                                            Console.Clear();
                                                                                                            Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                                                            Console.WriteLine();
                                                                                                            correctInput = false;
                                                                                                        }
                                                                                                        if (correctInput)
                                                                                                        {
                                                                                                            use_in = Used_in.Find(use_in_id);
                                                                                                            if (use_in == null || use_in.Recipe_Id != recipe.Id)
                                                                                                            {
                                                                                                                Console.Clear();
                                                                                                                Console.WriteLine(" Такого продукта нет, попробуйте ещё раз!");
                                                                                                                Console.WriteLine();
                                                                                                                correctInput = false;
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                    Console.WriteLine();

                                                                                                    correctInput = false;
                                                                                                    while (!correctInput)
                                                                                                    {
                                                                                                        Console.WriteLine($" Удалить продукт {use_in.Type_of_product.Name} из рецепта {recipe.Name}?");
                                                                                                        Console.WriteLine();
                                                                                                        Console.WriteLine(" 1. Да");
                                                                                                        Console.WriteLine(" 0. Нет");
                                                                                                        switch (Console.ReadKey(true).KeyChar)
                                                                                                        {
                                                                                                            case '1':
                                                                                                                correctInput = true;
                                                                                                                recipe.Used_in.Remove(use_in);
                                                                                                                db.Used_in.Remove(use_in);
                                                                                                                db.SaveChanges();
                                                                                                                Console.WriteLine();
                                                                                                                Console.WriteLine(" Объект успешно удалён");
                                                                                                                Console.WriteLine();
                                                                                                                break;
                                                                                                            case '0':
                                                                                                                correctInput = true;
                                                                                                                Console.Clear();
                                                                                                                Console.WriteLine(" Удаление объекта было отменено");
                                                                                                                Console.WriteLine();
                                                                                                                break;
                                                                                                            default:
                                                                                                                Console.Clear();
                                                                                                                Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                                                                Console.WriteLine();
                                                                                                                break;
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                break;
                                                                                            case '0':
                                                                                                {
                                                                                                    if (recipe.Used_in == null || (recipe.Used_in != null && !recipe.Used_in.Any()))
                                                                                                    {
                                                                                                        Console.Clear();
                                                                                                        Console.WriteLine(" Рецепт не может быть пустым!");
                                                                                                        Console.WriteLine(" Пожалуйста, добавьте хотя бы один продукт");
                                                                                                        Console.WriteLine();
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        correctTry = true;
                                                                                                        another_one = false;
                                                                                                        Console.WriteLine();
                                                                                                        Console.WriteLine(" Объект успешно сохранён");
                                                                                                        Console.WriteLine();
                                                                                                    }
                                                                                                }
                                                                                                break;
                                                                                            default:
                                                                                                Console.Clear();
                                                                                                Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                                                Console.WriteLine();
                                                                                                break;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            break;
                                                                        case '0':
                                                                            correctChoice = true;
                                                                            Console.Clear();
                                                                            Console.WriteLine($" Изменение рецепта {recipe.Name} было отменено");
                                                                            Console.WriteLine();
                                                                            break;
                                                                        default:
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                            Console.WriteLine();
                                                                            break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        break;

                                                    case ActionChoice.Delete:
                                                        {
                                                            //Каскадное удаление Used_in! PS. SQL команда сработала на БД, но в EF от клиента удалений нет, замена на SET NULL. Придётся удалять ручками...
                                                            //При удалении у машин Set null! PS. Программные Recipe_Id придётся обнулять вручную!...
                                                            //(SQL команды)
                                                            var Recipes = db.Recipes;
                                                            var Used_in = db.Used_in;
                                                            if (!Recipes.Any())
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine(" В таблице нет ни одного элемента");
                                                                Console.WriteLine();
                                                            }
                                                            else
                                                            {
                                                                Recipe recipe = new Recipe();
                                                                int id = 0;
                                                                bool correctInput = false;
                                                                while (!correctInput)
                                                                {
                                                                    correctInput = true;
                                                                    foreach (var t in Recipes.Include(r => r.Used_in).ToArray())
                                                                    {
                                                                        Console.WriteLine($" {t.Id}. {t.Name}:");
                                                                        foreach (var u in Used_in.Include(u => u.Type_of_product).Where(u => u.Recipe_Id == t.Id).OrderByDescending(u => u.Is_output))
                                                                        {
                                                                            Console.WriteLine($"\t{u.Type_of_product.Name} ({u.Quantity} шт.) {(u.Is_output ? "output" : "input")} [id: {u.Id}]");
                                                                        }
                                                                    }
                                                                    Console.WriteLine();
                                                                    Console.WriteLine(" Выберете ID элемента для удаления:");
                                                                    Console.Write(Environment.NewLine + ' ');
                                                                    try
                                                                    {
                                                                        id = Convert.ToInt32(Console.ReadLine()); //Можно int.Parse(string) (то же самое)
                                                                    }
                                                                    catch
                                                                    {
                                                                        Console.Clear();
                                                                        Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                        Console.WriteLine();
                                                                        correctInput = false;
                                                                    }
                                                                    if (correctInput)
                                                                    {
                                                                        recipe = Recipes.Find(id);
                                                                        if (recipe == null)
                                                                        {
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Такого рецепта нет, попробуйте ещё раз!");
                                                                            Console.WriteLine();
                                                                            correctInput = false;
                                                                        }
                                                                    }
                                                                }
                                                                Console.WriteLine();

                                                                correctInput = false;
                                                                while (!correctInput)
                                                                {
                                                                    Console.WriteLine($" Удалить рецепт {recipe.Name}?");
                                                                    foreach (var u in Used_in.Include(u => u.Type_of_product).Where(u => u.Recipe_Id == recipe.Id).OrderByDescending(u => u.Is_output))
                                                                    {
                                                                        Console.WriteLine($"\t{u.Type_of_product.Name} ({u.Quantity} шт.) {(u.Is_output ? "output" : "input")} [id: {u.Id}]");
                                                                    }
                                                                    Console.WriteLine();
                                                                    Console.WriteLine(" 1. Да");
                                                                    Console.WriteLine(" 0. Нет");
                                                                    switch (Console.ReadKey(true).KeyChar)
                                                                    {
                                                                        case '1':
                                                                            correctInput = true;
                                                                            foreach (var u in Used_in.Where(u => u.Recipe_Id == recipe.Id))
                                                                            {
                                                                                db.Used_in.Remove(u);
                                                                            }
                                                                            foreach (var m in db.Production_machines.Where(m => m.Recipe_Id == recipe.Id))
                                                                            {
                                                                                m.Recipe_Id = 0;
                                                                            }
                                                                            db.Recipes.Remove(recipe);
                                                                            db.SaveChanges();
                                                                            Console.WriteLine();
                                                                            Console.WriteLine(" Объект успешно удалён");
                                                                            Console.WriteLine();
                                                                            break;
                                                                        case '0':
                                                                            correctInput = true;
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Удаление объекта было отменено");
                                                                            Console.WriteLine();
                                                                            break;
                                                                        default:
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                            Console.WriteLine();
                                                                            break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        break;

                                                    case ActionChoice.View_Data:
                                                        {
                                                            var Recipes = db.Recipes;
                                                            var Used_in = db.Used_in;
                                                            if (!Recipes.Any())
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine(" В таблице нет ни одного элемента");
                                                                Console.WriteLine();
                                                            }
                                                            else
                                                            {
                                                                foreach (var t in Recipes.Include(r => r.Used_in).ToArray())
                                                                {
                                                                    Console.WriteLine($" {t.Id}. {t.Name}:");
                                                                    foreach (var u in Used_in.Include(u => u.Type_of_product).Where(u => u.Recipe_Id == t.Id).OrderByDescending(u => u.Is_output))
                                                                    {
                                                                        Console.WriteLine($"\t{u.Type_of_product.Name} ({u.Quantity} шт.) {(u.Is_output ? "output" : "input")} [id: {u.Id}]");
                                                                    }
                                                                }
                                                                Console.WriteLine();
                                                            }
                                                        }
                                                        break;
                                                }
                                                break;
                                            //-------------------------------------------------------------------------------------------------------------------------
                                            case TableChoice.Production_machines:
                                                switch (actionChoice)
                                                {
                                                    case ActionChoice.Insert:
                                                        {
                                                            Production_machine production_machine = new Production_machine();
                                                            var Production_machines = db.Production_machines.Include(m => m.Recipe);
                                                            /*
                                                            //if (!db.Recipes.Any())
                                                            //{
                                                            //    Console.Clear();
                                                            //    Console.WriteLine(" Сначала добавьте хотя бы один рецепт");
                                                            //    Console.WriteLine();
                                                            //}
                                                            //else
                                                            //{
                                                            */
                                                            Console.WriteLine(" Введите имя новой производственной машины:");
                                                            Console.Write(Environment.NewLine + ' ');
                                                            production_machine.Name = Console.ReadLine();
                                                            Console.WriteLine();

                                                            int recipe_id = 0;
                                                            bool correctInput = false;
                                                            while (!correctInput)
                                                            {
                                                                correctInput = true;
                                                                foreach (var r in db.Recipes)
                                                                {
                                                                    Console.WriteLine($" {r.Id}. {r.Name}");
                                                                }
                                                                Console.WriteLine(" 0. Оставить без рецепта");
                                                                Console.WriteLine();
                                                                Console.WriteLine(" Введите ID рецепта:");
                                                                Console.Write(Environment.NewLine + ' ');
                                                                try
                                                                {
                                                                    recipe_id = Convert.ToInt32(Console.ReadLine());
                                                                }
                                                                catch
                                                                {
                                                                    Console.Clear();
                                                                    Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                    Console.WriteLine();
                                                                    correctInput = false;
                                                                }
                                                                if (correctInput)
                                                                {
                                                                    if (recipe_id != 0 && db.Recipes.Find(recipe_id) == null)
                                                                    {
                                                                        Console.Clear();
                                                                        Console.WriteLine(" Такого рецепта нет, попробуйте ещё раз!");
                                                                        Console.WriteLine();
                                                                        correctInput = false;
                                                                    }
                                                                }
                                                            }
                                                            production_machine.Recipe_Id = recipe_id;
                                                            if (recipe_id != 0)
                                                            {
                                                                production_machine.Recipe = db.Recipes.Find(recipe_id);
                                                            }
                                                            Console.WriteLine();

                                                            correctInput = false;
                                                            while (!correctInput)
                                                            {
                                                                Console.WriteLine($" Сохранить новую производственную машину {production_machine.Name} [{(production_machine.Recipe != null ? production_machine.Recipe.Name : "NULL")}]?");
                                                                Console.WriteLine();
                                                                Console.WriteLine(" 1. Да");
                                                                Console.WriteLine(" 0. Нет");
                                                                Console.WriteLine();
                                                                switch (Console.ReadKey(true).KeyChar)
                                                                {
                                                                    case '1':
                                                                        correctInput = true;
                                                                        db.Production_machines.Add(production_machine);
                                                                        db.SaveChanges();
                                                                        Console.WriteLine(" Объект успешно сохранён");
                                                                        Console.WriteLine();
                                                                        break;
                                                                    case '0':
                                                                        correctInput = true;
                                                                        Console.Clear();
                                                                        Console.WriteLine(" Сохраненение объекта было отменено");
                                                                        Console.WriteLine();
                                                                        break;
                                                                    default:
                                                                        Console.Clear();
                                                                        Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                        Console.WriteLine();
                                                                        break;
                                                                }
                                                            }
                                                            //}
                                                        }
                                                        break;

                                                    case ActionChoice.Update:
                                                        {
                                                            var Production_machines = db.Production_machines.Include(m => m.Recipe);
                                                            if (!Production_machines.Any())
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine(" В таблице нет ни одного элемента");
                                                                Console.WriteLine();
                                                            }
                                                            else
                                                            {
                                                                Production_machine production_machine = new Production_machine();
                                                                Production_machine production_machine2 = new Production_machine();
                                                                int id = 0;
                                                                bool correctInput = false;
                                                                while (!correctInput)
                                                                {
                                                                    correctInput = true;
                                                                    foreach (var m in Production_machines)
                                                                    {
                                                                        Console.WriteLine($" {m.Id}. {m.Name} [{(m.Recipe != null ? m.Recipe.Name : "NULL")}]");
                                                                    }
                                                                    Console.WriteLine();
                                                                    Console.WriteLine(" Выберете ID элемента для изменения:");
                                                                    Console.Write(Environment.NewLine + ' ');
                                                                    try
                                                                    {
                                                                        id = Convert.ToInt32(Console.ReadLine()); //Можно int.Parse(string) (то же самое)
                                                                    }
                                                                    catch
                                                                    {
                                                                        Console.Clear();
                                                                        Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                        Console.WriteLine();
                                                                        correctInput = false;
                                                                    }
                                                                    if (correctInput)
                                                                    {
                                                                        production_machine = db.Production_machines.Find(id);
                                                                        if (production_machine == null)
                                                                        {
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Такой производственной машины нет, попробуйте ещё раз!");
                                                                            Console.WriteLine();
                                                                            correctInput = false;
                                                                        }
                                                                    }
                                                                }
                                                                Console.WriteLine();
                                                                production_machine2.Name = production_machine.Name;
                                                                production_machine2.Recipe = production_machine.Recipe;
                                                                production_machine2.Recipe_Id = production_machine.Recipe_Id;

                                                                correctInput = false;
                                                                while (!correctInput)
                                                                {
                                                                    Console.WriteLine($" Будете менять имя машины {production_machine2.Name} [{(production_machine2.Recipe != null ? production_machine2.Recipe.Name : "NULL")}]?");
                                                                    Console.WriteLine();
                                                                    Console.WriteLine(" 1. Да");
                                                                    Console.WriteLine(" 0. Нет");
                                                                    switch (Console.ReadKey(true).KeyChar)
                                                                    {
                                                                        case '1':
                                                                            correctInput = true;
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Введите новое имя:");
                                                                            Console.Write(Environment.NewLine + ' ');
                                                                            production_machine2.Name = Console.ReadLine();
                                                                            Console.WriteLine();
                                                                            break;
                                                                        case '0':
                                                                            correctInput = true;
                                                                            Console.Clear();
                                                                            Console.WriteLine($" Имя машины {production_machine2.Name} останется прежним");
                                                                            Console.WriteLine();
                                                                            break;
                                                                        default:
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                            Console.WriteLine();
                                                                            break;
                                                                    }
                                                                }

                                                                bool correctAnswer = false;
                                                                while (!correctAnswer)
                                                                {
                                                                    Console.WriteLine($" Будете менять рецепт машины {production_machine2.Name} [{(production_machine2.Recipe != null ? production_machine2.Recipe.Name : "NULL")}]?");
                                                                    Console.WriteLine();
                                                                    Console.WriteLine(" 1. Да");
                                                                    Console.WriteLine(" 0. Нет");
                                                                    switch (Console.ReadKey(true).KeyChar)
                                                                    {
                                                                        case '1':
                                                                            correctAnswer = true;
                                                                            {
                                                                                Console.Clear();
                                                                                int recipe_id = 0;
                                                                                correctInput = false;
                                                                                while (!correctInput)
                                                                                {
                                                                                    correctInput = true;
                                                                                    foreach (var r in db.Recipes)
                                                                                    {
                                                                                        Console.WriteLine($" {r.Id}. {r.Name}");
                                                                                    }
                                                                                    Console.WriteLine(" 0. Оставить без рецепта");
                                                                                    Console.WriteLine();
                                                                                    Console.WriteLine(" Введите ID рецепта:");
                                                                                    Console.Write(Environment.NewLine + ' ');
                                                                                    try
                                                                                    {
                                                                                        recipe_id = Convert.ToInt32(Console.ReadLine());
                                                                                    }
                                                                                    catch
                                                                                    {
                                                                                        Console.Clear();
                                                                                        Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                                        Console.WriteLine();
                                                                                        correctInput = false;
                                                                                    }
                                                                                    if (correctInput)
                                                                                    {
                                                                                        if (recipe_id != 0 && db.Recipes.Find(recipe_id) == null)
                                                                                        {
                                                                                            Console.Clear();
                                                                                            Console.WriteLine(" Такого рецепта нет, попробуйте ещё раз!");
                                                                                            Console.WriteLine();
                                                                                            correctInput = false;
                                                                                        }
                                                                                    }
                                                                                }
                                                                                production_machine2.Recipe_Id = recipe_id;
                                                                                if (recipe_id != 0)
                                                                                {
                                                                                    production_machine2.Recipe = db.Recipes.Find(recipe_id);
                                                                                }
                                                                                Console.WriteLine();
                                                                            }
                                                                            break;
                                                                        case '0':
                                                                            correctAnswer = true;
                                                                            Console.Clear();
                                                                            Console.WriteLine($" Рецепт машины {production_machine2.Name} [{(production_machine2.Recipe != null ? production_machine2.Recipe.Name : "NULL")}] останется прежним");
                                                                            Console.WriteLine();
                                                                            break;
                                                                        default:
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                            Console.WriteLine();
                                                                            break;
                                                                    }
                                                                }

                                                                correctInput = false;
                                                                while (!correctInput)
                                                                {
                                                                    Console.WriteLine($" Изменить производственную машину с {production_machine.Name} [{(production_machine.Recipe != null ? production_machine.Recipe.Name : "NULL")}] на {production_machine2.Name} [{(production_machine2.Recipe != null ? production_machine2.Recipe.Name : "NULL")}]?");
                                                                    Console.WriteLine();
                                                                    Console.WriteLine(" 1. Да");
                                                                    Console.WriteLine(" 0. Нет");
                                                                    Console.WriteLine();
                                                                    switch (Console.ReadKey(true).KeyChar)
                                                                    {
                                                                        case '1':
                                                                            correctInput = true;
                                                                            production_machine.Name = production_machine2.Name;
                                                                            production_machine.Recipe = production_machine2.Recipe;
                                                                            production_machine.Recipe_Id = production_machine2.Recipe_Id;
                                                                            db.SaveChanges();
                                                                            Console.WriteLine(" Объект успешно сохранён");
                                                                            Console.WriteLine();
                                                                            break;
                                                                        case '0':
                                                                            correctInput = true;
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Сохраненение объекта было отменено");
                                                                            Console.WriteLine();
                                                                            break;
                                                                        default:
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                            Console.WriteLine();
                                                                            break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        break;

                                                    case ActionChoice.Delete:
                                                        {
                                                            var Production_machines = db.Production_machines.Include(m => m.Recipe);
                                                            if (!Production_machines.Any())
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine(" В таблице нет ни одного элемента");
                                                                Console.WriteLine();
                                                            }
                                                            else
                                                            {
                                                                Production_machine production_machine = new Production_machine();
                                                                int id = 0;
                                                                bool correctInput = false;
                                                                while (!correctInput)
                                                                {
                                                                    correctInput = true;
                                                                    foreach (var m in Production_machines)
                                                                    {
                                                                        Console.WriteLine($" {m.Id}. {m.Name} [{(m.Recipe != null ? m.Recipe.Name : "NULL")}]");
                                                                    }
                                                                    Console.WriteLine();
                                                                    Console.WriteLine(" Выберете ID элемента для удаления:");
                                                                    Console.Write(Environment.NewLine + ' ');
                                                                    try
                                                                    {
                                                                        id = Convert.ToInt32(Console.ReadLine());
                                                                    }
                                                                    catch
                                                                    {
                                                                        Console.Clear();
                                                                        Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                        Console.WriteLine();
                                                                        correctInput = false;
                                                                    }
                                                                    if (correctInput)
                                                                    {
                                                                        production_machine = db.Production_machines.Find(id);
                                                                        if (production_machine == null)
                                                                        {
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Такой производственной машины нет, попробуйте ещё раз!");
                                                                            Console.WriteLine();
                                                                            correctInput = false;
                                                                        }
                                                                    }
                                                                }
                                                                Console.WriteLine();

                                                                correctInput = false;
                                                                while (!correctInput)
                                                                {
                                                                    Console.WriteLine($" Удалить производственную машину {production_machine.Name} [{(production_machine.Recipe != null ? production_machine.Recipe.Name : "NULL")}]?");
                                                                    Console.WriteLine();
                                                                    Console.WriteLine(" 1. Да");
                                                                    Console.WriteLine(" 0. Нет");
                                                                    switch (Console.ReadKey(true).KeyChar)
                                                                    {
                                                                        case '1':
                                                                            correctInput = true;
                                                                            db.Production_machines.Remove(production_machine);
                                                                            db.SaveChanges();
                                                                            Console.WriteLine();
                                                                            Console.WriteLine(" Объект успешно удалён");
                                                                            Console.WriteLine();
                                                                            break;
                                                                        case '0':
                                                                            correctInput = true;
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Удаление объекта было отменено");
                                                                            Console.WriteLine();
                                                                            break;
                                                                        default:
                                                                            Console.Clear();
                                                                            Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                                                            Console.WriteLine();
                                                                            break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        break;

                                                    case ActionChoice.View_Data:
                                                        {
                                                            var Production_machines = db.Production_machines.Include(m => m.Recipe);
                                                            if (!Production_machines.Any())
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine(" В таблице нет ни одного элемента");
                                                                Console.WriteLine();
                                                            }
                                                            else
                                                            {
                                                                foreach (var m in Production_machines)
                                                                {
                                                                    Console.WriteLine($" {m.Id}. {m.Name} [{(m.Recipe != null ? m.Recipe.Name : "NULL")}]");
                                                                }
                                                                Console.WriteLine();
                                                            }
                                                        }
                                                        break;
                                                }
                                                break;
                                        }
                                        //===============================================================================================================================
                                        break;

                                    case ActionChoice.Cancel:
                                        correctAction = true;
                                        break;

                                    default:
                                        Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                                        Console.WriteLine();
                                        break;
                                }
                            }
                            break;

                        case TableChoice.Exit:
                            Console.WriteLine(" Уверены, что хотите выйти?");
                            Console.WriteLine();
                            Console.WriteLine(" 1. Да");
                            Console.WriteLine(" 0. Нет");
                            switch (Console.ReadKey(true).KeyChar)
                            {
                                case '1':
                                    ext = true;
                                    break;
                                default:
                                    break;

                            }
                            Console.Clear();
                            break;

                        default:
                            Console.WriteLine(" Неверный формат ввода, попробуйте ещё раз!");
                            Console.WriteLine();
                            break;
                    }
                }
                Console.WriteLine(" Выход...");

                /*Добавление
                //Type_of_product product1 = new Type_of_product { Name = "iron_plate" };
                //Type_of_product product2 = new Type_of_product { Name = "iron_ore" };

                //Type_of_product product2 = new Type_of_product { Id = 2, Name = "copper_cable" }; // Id вручную не задаётся! Теперь оно = 7...
                //Type_of_product product3 = new Type_of_product { Id = 3, Name = "electronic_circuit" };

                //db.Types_of_products.Add(product1);
                //db.Types_of_products.Add(product2);
                //db.SaveChanges();
                */

                /*Изменение
                //Type_of_product product1 = db.Types_of_products.FirstOrDefault();
                //if (product1 != null)
                //{
                //    product1.Name = "iron_plate";
                //    db.SaveChanges();
                //}
                //Type_of_product product2 = db.Types_of_products.Find(2);
                //if (product2 != null)
                //{
                //    product2.Name = "aboba";
                //    db.SaveChanges();
                //}
                //Type_of_product product2 = db.Types_of_products.Find(6);
                //if (product2 != null)
                //{
                //    product2.Id = 2; //The property 'Id' is part of the object's key information and cannot be modified.
                //    db.SaveChanges();
                //}
                */

                /*Удаление
                //Type_of_product product2 = db.Types_of_products.Find(2);//Только primary key
                //if (product2 != null)
                //{
                //    db.Types_of_products.Remove(product2);
                //    db.SaveChanges();
                //}
                */

                /*Просмотр информации
                //Console.WriteLine("Объекты успешно сохранены");

                //var typestypes = db.Types_of_products;
                //Console.WriteLine("Список типов:");
                //foreach (Type_of_product t in typestypes)
                //{
                //    Console.WriteLine("{0}: {1}", t.Id, t.Name);
                //}
                */
            }
        }
    }
}
