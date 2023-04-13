using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TextAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Задайте шлях до текових файлів для аналізу
            string folderPath = @"C:\Users\Username\Documents\TextFiles";

            // Викличте метод AnalyzeText для аналізу текстових файлів і відображення статистичних даних
            AnalyzeText(folderPath, TokenizeText, CalculateWordFrequencies, DisplayWordFrequencies);
        }

        // Метод для аналізу текстових файлів і відображення статистичних даних
        static void AnalyzeText(string folderPath, Func<string, IEnumerable<string>> tokenizeText, Func<IEnumerable<string>, IDictionary<string, int>> calculateWordFrequencies, Action<IDictionary<string, int>> displayWordFrequencies)
        {
            // Отримайте всі текові файли з заданої теки
            string[] fileNames = Directory.GetFiles(folderPath, "*.txt");

            // Створіть словник для зберігання частот слів у файлах
            IDictionary<string, int> wordFrequencies = new Dictionary<string, int>();

            // Пройдіться по кожному файлу та аналізуйте його текст
            foreach (string fileName in fileNames)
            {
                // Відкрийте файл та прочитайте його текст
                string fileText = File.ReadAllText(fileName);

                // Розбийте текст на слова за допомогою заданого методу токенізації
                IEnumerable<string> words = tokenizeText(fileText);

                // Обчисліть частоти кожного слова за допомогою заданого методу розрахунку частот
                IDictionary<string, int> frequencies = calculateWordFrequencies(words);

                // Додайте частоти слів у загальний словник
                foreach (KeyValuePair<string, int> kvp in frequencies)
                {
                    if (wordFrequencies.ContainsKey(kvp.Key))
                    {
                        wordFrequencies[kvp.Key] += kvp.Value;
                    }
                    else
                    {
                        wordFrequencies[kvp.Key] = kvp.Value;
                    }
                }
            }

            // Відобразіть статистику частот слів за допомогою заданого методу відображення статистики
            displayWordFrequencies(wordFrequencies);
        }

        // Метод для токенізації тексту на слова
        static IEnumerable<string> TokenizeText(string text)
        {
            // Розбийте текст на слова з використанням пробілів та знаків пунктуації як роздільники
            char
