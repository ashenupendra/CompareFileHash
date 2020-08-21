using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CompareFileHash
{
    class Program
    {
        public static StringBuilder sb = new StringBuilder();
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter Path 1");
            string path1 = Console.ReadLine();
            Console.WriteLine("Please enter Path 2");
            string path2 = Console.ReadLine();
            sb.AppendLine($"Compare Source {path1}");
            sb.AppendLine($"Compare Target {path2}");
            sb.AppendLine();
            sb.AppendLine($"OriginalFile,TargetFile,OriginalFileHash,TargetFileHash,IsMatched");
            string[] fileEntries = Directory.GetFiles(path1, "*", SearchOption.AllDirectories);
            foreach (string fileName in fileEntries)
            {
                CheckFileHash(path1, fileName, path2);
            }
            WriteToFile(path1);
            Console.WriteLine("Completed, Press any key to continue...");
            Console.ReadLine();

        }

        private static void CheckFileHash(string path1, string originalFileName, string path2)
        {
            var originalHash = GetFileHash(originalFileName);
            var targetFileName = originalFileName.Replace(path1, path2);
            if (!File.Exists(targetFileName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"No files found for {path2}");
                return;
            }
            var targetFileHash = GetFileHash(targetFileName);
            Console.WriteLine($"Hash Matching started for {targetFileName}");
            bool isMatched = false;
            if (originalHash == targetFileHash)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{originalFileName}; is matched with target file");
                isMatched = true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{originalFileName}; is not matched with target file");
            }
            sb.AppendLine($"{originalFileName},{targetFileName},{originalHash},{targetFileHash},{isMatched}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static string GetFileHash(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public static void WriteToFile(string path1)
        {
            File.WriteAllText(path1 + $"/{DateTime.Now.ToString("ddmmyyyyHHMMss")}_HashCompare.csv", sb.ToString());
        }

    }
}



