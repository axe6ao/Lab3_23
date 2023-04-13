using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ImageProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Bitmap> images = LoadImagesFromDirectory(@"C:\Images"); // завантаження зображень з директорії
            List<Func<Bitmap, Bitmap>> filters = new List<Func<Bitmap, Bitmap>>(); // список функцій-фільтрів
            filters.Add(InvertColors);
            filters.Add(Grayscale);

            foreach (Bitmap image in images)
            {
                Console.WriteLine("Original image:");
                DisplayImage(image); // відображення оригінального зображення

                foreach (var filter in filters)
                {
                    Bitmap filteredImage = filter(image); // застосування фільтру до зображення
                    Console.WriteLine($"Filtered image using {filter.Method.Name}:");
                    DisplayImage(filteredImage); // відображення обробленого зображення
                }
            }

            Console.ReadLine();
        }

        static Bitmap InvertColors(Bitmap image)
        {
            Bitmap result = new Bitmap(image.Width, image.Height);

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color color = image.GetPixel(x, y);
                    Color invertedColor = Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
                    result.SetPixel(x, y, invertedColor);
                }
            }

            return result;
        }

        static Bitmap Grayscale(Bitmap image)
        {
            Bitmap result = new Bitmap(image.Width, image.Height);

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color color = image.GetPixel(x, y);
                    int gray = (int)(color.R * 0.299 + color.G * 0.587 + color.B * 0.114);
                    result.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }

            return result;
        }

        static void DisplayImage(Bitmap image)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                Console.WriteLine(Convert.ToBase64String(memoryStream.ToArray()));
            }
        }

        static List<Bitmap> LoadImagesFromDirectory(string path)
        {
            List<Bitmap> images = new List<Bitmap>();

            foreach (string file in Directory.GetFiles(path))
            {
                images.Add(new Bitmap(file));
            }

            return images;
        }
    }
}
