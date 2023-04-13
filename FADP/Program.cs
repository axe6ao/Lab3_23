using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        string path = "path/to/json/files"; // змініть цей шлях на свій
        List<Predicate<Product>> filters = new List<Predicate<Product>>();

        // Додайте критерії фільтрації, які потрібні користувачу
        filters.Add(p => p.Price < 10);
        filters.Add(p => p.Category == "Fruit");

        // Пройдіться по всіх JSON-файлах, від 1 до 10
        for (int i = 1; i <= 10; i++)
        {
            string filename = Path.Combine(path, $"{i}.json");
            if (File.Exists(filename))
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    string json = sr.ReadToEnd();
                    List<Product> products = JsonSerializer.Deserialize<List<Product>>(json);

                    // Фільтруйте продукти
                    List<Product> filteredProducts = FilterProducts(products, filters);

                    // Відображайте відфільтровані продукти
                    DisplayProducts(filteredProducts);
                }
            }
        }
    }

    static List<Product> FilterProducts(List<Product> products, List<Predicate<Product>> filters)
    {
        List<Product> filteredProducts = new List<Product>();

        foreach (Product product in products)
        {
            bool pass = true;
            foreach (Predicate<Product> filter in filters)
            {
                if (!filter(product))
                {
                    pass = false;
                    break;
                }
            }
            if (pass)
            {
                filteredProducts.Add(product);
            }
        }

        return filteredProducts;
    }

    static void DisplayProducts(List<Product> products)
    {
        foreach (Product product in products)
        {
            Console.WriteLine($"Name: {product.Name}, Category: {product.Category}, Price: {product.Price}");
        }
    }
}

class Product
{
    public string Name { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
}
