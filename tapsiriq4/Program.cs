using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== METN ANALIZATORU ===");

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        Console.Write("\nOxunacaq .txt faylinin adini ve ya yolunu daxil edin (mes: metn.txt): ");
        string inputPath = Console.ReadLine()?.Trim();

        string filePath;
        if (Path.IsPathRooted(inputPath))
        {
            filePath = inputPath;
        }
        else
        {
            filePath = Path.Combine(desktopPath, inputPath);
        }

        if (!File.Exists(filePath))
        {
            string sampleText = "Bu bir sinaq metnidir.\nMetn analizatoru fali oxuyur ve analiz edir.\nSinaq ucun yazilmis metn fayli istifade olunur.";
            File.WriteAllText(filePath, sampleText);
            Console.WriteLine($"Fayl tapilmadi, sınaq ucun Masaustunde yaradildi: {filePath}");
        }

        string[] lines = File.ReadAllLines(filePath);
        string fullText = File.ReadAllText(filePath);

        int lineCount = lines.Length;
        int charCount = fullText.Length;

        char[] delimiters = new char[] { ' ', '\r', '\n', '\t', '.', ',', '!', '?', ';', ':', '-', '(', ')', '"' };
        string[] words = fullText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

        int wordCount = words.Length;

        string longestWord = "";
        Dictionary<string, int> wordCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (string word in words)
        {
            if (word.Length > longestWord.Length)
            {
                longestWord = word;
            }

            string lowerWord = word.ToLower();
            if (wordCounts.ContainsKey(lowerWord))
            {
                wordCounts[lowerWord]++;
            }
            else
            {
                wordCounts[lowerWord] = 1;
            }
        }

        var top3Words = wordCounts.OrderByDescending(x => x.Value).Take(3);

        string reportPath = Path.Combine(desktopPath, "hesabat.txt");

        using (StreamWriter writer = new StreamWriter(reportPath, false))
        {
            writer.WriteLine("=== METN ANALIZI HESABATI ===");
            writer.WriteLine($"Setir sayi: {lineCount}");
            writer.WriteLine($"Soz sayi: {wordCount}");
            writer.WriteLine($"Simvol sayi: {charCount}");
            writer.WriteLine($"En uzun soz: {longestWord}");
            writer.WriteLine("\nEn cox tekrarlanan 3 soz:");

            foreach (var pair in top3Words)
            {
                writer.WriteLine($"- {pair.Key}: {pair.Value} defe");
            }
        }

        Console.WriteLine("\n--- ANALIZ TAMAMLANDI ---");
        Console.WriteLine($"Setir sayi: {lineCount}");
        Console.WriteLine($"Soz sayi: {wordCount}");
        Console.WriteLine($"Simvol sayi: {charCount}");
        Console.WriteLine($"En uzun soz: {longestWord}");
        Console.WriteLine($"Neticeler Masaustundeki 'hesabat.txt' faylina yazildi.");
    }
}