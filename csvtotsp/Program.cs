using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        string path = "transactions.csv"; // шлях до файлу CSV
        string dateFormat = "dd.MM.yyyy"; // формат дати

        Func<string, DateTime> getDate = (line) =>
        {
            string[] values = line.Split(',');
            return DateTime.ParseExact(values[0], dateFormat, null);
        }; // делегат для отримання дати з рядка CSV

        Func<string, double> getAmount = (line) =>
        {
            string[] values = line.Split(',');
            return double.Parse(values[1]);
        }; // делегат для отримання суми з рядка CSV

        Action<DateTime, double> printTotal = (date, total) =>
        {
            Console.WriteLine("{0}: {1}", date.ToString(dateFormat), total);
        }; // делегат для виведення загальної суми витрат за день

        int count = 0;
        DateTime currentDate = DateTime.MinValue;
        double currentTotal = 0;

        using (var reader = new StreamReader(path))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                DateTime date = getDate(line);
                double amount = getAmount(line);

                if (date != currentDate)
                {
                    if (count > 0)
                    {
                        printTotal(currentDate, currentTotal);
                        currentTotal = 0;
                    }

                    currentDate = date;
                    count = 0;
                }

                currentTotal += amount;
                count++;

                if (count == 10)
                {
                    var lines = File.ReadAllLines(path);
                    lines = lines.Skip(count).ToArray();
                    File.WriteAllLines(path, lines);
                    count = 0;
                }
            }

            if (count > 0)
            {
                printTotal(currentDate, currentTotal);
            }
        }
    }
}
