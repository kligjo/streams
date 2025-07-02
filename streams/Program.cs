using System;
using System.IO;
using System.Text;

namespace FileStreamBasics
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== BASIC FILESTREAM EXAMPLES ===\n");

            // Example 1: Write text to file
            WriteTextToFile();

            // Example 2: Read text from file
            ReadTextFromFile();

            // Example 3: Write binary data
            WriteBinaryData();

            // Example 4: Read binary data
            ReadBinaryData();

            // Example 5: Append to file
            AppendToFile();

            // Example 6: Copy file
            CopyFile();

            Console.WriteLine("\nAll examples completed!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void WriteTextToFile()
        {
            Console.WriteLine("=== EXAMPLE 1: WRITE TEXT TO FILE ===");

            string fileName = "example1.txt";
            string message = "Hello, this is my first FileStream example!";

            // Convert text to bytes
            byte[] data = Encoding.UTF8.GetBytes(message);

            // Write to file using FileStream
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                fs.Write(data, 0, data.Length);
            }

            Console.WriteLine($"✓ Written to file: {fileName}");
            Console.WriteLine($"  Message: {message}");
            Console.WriteLine($"  Bytes written: {data.Length}");
            Console.WriteLine();
        }

        static void ReadTextFromFile()
        {
            Console.WriteLine("=== EXAMPLE 2: READ TEXT FROM FILE ===");

            string fileName = "example1.txt";

            if (File.Exists(fileName))
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    // Create buffer to hold data
                    byte[] buffer = new byte[fs.Length];

                    // Read all data
                    fs.Read(buffer, 0, buffer.Length);

                    // Convert bytes back to text
                    string message = Encoding.UTF8.GetString(buffer);

                    Console.WriteLine($"📖 Read from file: {fileName}");
                    Console.WriteLine($"  Message: {message}");
                    Console.WriteLine($"  Bytes read: {buffer.Length}");
                }
            }
            else
            {
                Console.WriteLine($"❌ File not found: {fileName}");
            }
            Console.WriteLine();
        }

        static void WriteBinaryData()
        {
            Console.WriteLine("=== EXAMPLE 3: WRITE BINARY DATA ===");

            string fileName = "numbers.dat";

            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                // Write different types of numbers

                // Write an integer (4 bytes)
                int number = 12345;
                byte[] intBytes = BitConverter.GetBytes(number);
                fs.Write(intBytes, 0, intBytes.Length);

                // Write a decimal (16 bytes)
                decimal price = 99.99m;
                int[] decimalBytes = decimal.GetBits(price);
                foreach (int part in decimalBytes)
                {
                    byte[] partBytes = BitConverter.GetBytes(part);
                    fs.Write(partBytes, 0, partBytes.Length);
                }

                // Write a boolean (1 byte)
                bool isActive = true;
                byte[] boolBytes = BitConverter.GetBytes(isActive);
                fs.Write(boolBytes, 0, boolBytes.Length);
            }

            Console.WriteLine($"✓ Binary data written to: {fileName}");
            Console.WriteLine($"  Integer: {12345}");
            Console.WriteLine($"  Decimal: {99.99m}");
            Console.WriteLine($"  Boolean: {true}");
            Console.WriteLine();
        }

        static void ReadBinaryData()
        {
            Console.WriteLine("=== EXAMPLE 4: READ BINARY DATA ===");

            string fileName = "numbers.dat";

            if (File.Exists(fileName))
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    // Read integer (4 bytes)
                    byte[] intBuffer = new byte[4];
                    fs.Read(intBuffer, 0, 4);
                    int number = BitConverter.ToInt32(intBuffer, 0);

                    // Read decimal (16 bytes)
                    byte[] decimalBuffer = new byte[16];
                    fs.Read(decimalBuffer, 0, 16);
                    int[] decimalParts = new int[4];
                    for (int i = 0; i < 4; i++)
                    {
                        decimalParts[i] = BitConverter.ToInt32(decimalBuffer, i * 4);
                    }
                    decimal price = new decimal(decimalParts);

                    // Read boolean (1 byte)
                    byte[] boolBuffer = new byte[1];
                    fs.Read(boolBuffer, 0, 1);
                    bool isActive = BitConverter.ToBoolean(boolBuffer, 0);

                    Console.WriteLine($"📖 Binary data read from: {fileName}");
                    Console.WriteLine($"  Integer: {number}");
                    Console.WriteLine($"  Decimal: {price}");
                    Console.WriteLine($"  Boolean: {isActive}");
                }
            }
            else
            {
                Console.WriteLine($"❌ File not found: {fileName}");
            }
            Console.WriteLine();
        }

        static void AppendToFile()
        {
            Console.WriteLine("=== EXAMPLE 5: APPEND TO FILE ===");

            string fileName = "log.txt";
            string newEntry = $"[{DateTime.Now}] New log entry added\n";

            // Convert to bytes
            byte[] data = Encoding.UTF8.GetBytes(newEntry);

            // Append to file (creates file if it doesn't exist)
            using (FileStream fs = new FileStream(fileName, FileMode.Append))
            {
                fs.Write(data, 0, data.Length);
            }

            Console.WriteLine($"✓ Appended to file: {fileName}");
            Console.WriteLine($"  Entry: {newEntry.Trim()}");

            // Show file contents
            if (File.Exists(fileName))
            {
                string content = File.ReadAllText(fileName);
                Console.WriteLine("📄 Current file contents:");
                Console.WriteLine(content);
            }
            Console.WriteLine();
        }

        static void CopyFile()
        {
            Console.WriteLine("=== EXAMPLE 6: COPY FILE ===");

            string sourceFile = "example1.txt";
            string destinationFile = "example1_copy.txt";

            if (File.Exists(sourceFile))
            {
                using (FileStream source = new FileStream(sourceFile, FileMode.Open))
                using (FileStream destination = new FileStream(destinationFile, FileMode.Create))
                {
                    // Copy in chunks
                    byte[] buffer = new byte[1024]; // 1KB buffer
                    int bytesRead;

                    while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        destination.Write(buffer, 0, bytesRead);
                    }
                }

                Console.WriteLine($"✓ Copied file: {sourceFile} → {destinationFile}");

                // Verify copy
                string originalContent = File.ReadAllText(sourceFile);
                string copiedContent = File.ReadAllText(destinationFile);

                if (originalContent == copiedContent)
                {
                    Console.WriteLine("✅ Copy verified - files are identical");
                }
                else
                {
                    Console.WriteLine("❌ Copy failed - files are different");
                }
            }
            else
            {
                Console.WriteLine($"❌ Source file not found: {sourceFile}");
            }
            Console.WriteLine();
        }
    }

    // ===== BONUS: UTILITY CLASS FOR COMMON FILESTREAM OPERATIONS =====

    public static class FileStreamHelper
    {
        // Simple write text to file
        public static void WriteText(string fileName, string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);

            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                fs.Write(data, 0, data.Length);
            }

            Console.WriteLine($"✓ Text written to: {fileName}");
        }

        // Simple read text from file
        public static string ReadText(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"❌ File not found: {fileName}");
                return string.Empty;
            }

            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer);
            }
        }

        // Write number to file
        public static void WriteNumber(string fileName, int number)
        {
            byte[] data = BitConverter.GetBytes(number);

            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                fs.Write(data, 0, data.Length);
            }

            Console.WriteLine($"✓ Number {number} written to: {fileName}");
        }

        // Read number from file
        public static int ReadNumber(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"❌ File not found: {fileName}");
                return 0;
            }

            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                byte[] buffer = new byte[4]; // int is 4 bytes
                fs.Read(buffer, 0, 4);
                return BitConverter.ToInt32(buffer, 0);
            }
        }

        // Get file size
        public static long GetFileSize(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return -1;
            }

            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                return fs.Length;
            }
        }

        // Check if file exists and show info
        public static void ShowFileInfo(string fileName)
        {
            if (File.Exists(fileName))
            {
                FileInfo info = new FileInfo(fileName);
                Console.WriteLine($"📄 File: {fileName}");
                Console.WriteLine($"   Size: {info.Length} bytes");
                Console.WriteLine($"   Created: {info.CreationTime}");
                Console.WriteLine($"   Modified: {info.LastWriteTime}");
            }
            else
            {
                Console.WriteLine($"❌ File not found: {fileName}");
            }
        }
    }

    // ===== EXAMPLE USAGE OF HELPER CLASS =====

    public static class HelperExamples
    {
        public static void RunHelperExamples()
        {
            Console.WriteLine("=== USING FILESTREAM HELPER ===");

            // Write and read text
            FileStreamHelper.WriteText("helper_test.txt", "Hello from helper!");
            string text = FileStreamHelper.ReadText("helper_test.txt");
            Console.WriteLine($"Read text: {text}");

            // Write and read number
            FileStreamHelper.WriteNumber("number.dat", 42);
            int number = FileStreamHelper.ReadNumber("number.dat");
            Console.WriteLine($"Read number: {number}");

            // Show file info
            FileStreamHelper.ShowFileInfo("helper_test.txt");
            FileStreamHelper.ShowFileInfo("number.dat");

            Console.WriteLine();
        }
    }
}