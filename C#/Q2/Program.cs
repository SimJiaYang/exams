using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.CodeDom;

namespace Q2
{
    class Program
    {
        /*
         * Simple file program with txt
         */
        static void Main(string[] args)
        {
            const string directoryName = "Q2 Folder";
            string basePath = Path.Combine(Directory.GetCurrentDirectory(), directoryName);

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
                Console.WriteLine($"Directory '{directoryName}' created.");
            }
            else
            {
                Console.WriteLine($"Directory '{directoryName}' already exists.");
            }

            while (true)
            {
                Console.WriteLine("\nSimple File System Menu:");
                Console.WriteLine("1. Create a file");
                Console.WriteLine("2. Write to a file");
                Console.WriteLine("3. Read from a file");
                Console.WriteLine("4. Delete a file");
                Console.WriteLine("5. Exit");
                Console.Write("Select an option (1-5): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateFile(basePath);
                        break;
                    case "2":
                        WriteToFile(basePath);
                        break;
                    case "3":
                        ReadFile(basePath);
                        break;
                    case "4":
                        DeleteFile(basePath);
                        break;
                    case "5":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        /*
         * Create file
         * 
         * @Param
         * - File base path
         * 
         * Process
         * 1. Let the user enter the filename
         * 2. Get the filename without extension
         * 3. Check filepath exist or not
         * 4. Create the file
         */
        static void CreateFile(string path)
        {
            Console.Write("Enter file name to create: ");
            string userInput = Console.ReadLine();

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(userInput);
            string fileName = fileNameWithoutExtension + ".txt";

            string fullPath = Path.Combine(path, fileName);

            if (File.Exists(fullPath))
            {
                Console.WriteLine("File already exists.");
            }
            else
            {
                try
                {
                    File.Create(fullPath).Close();
                    Console.WriteLine($"File '{fileName}' created successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while creating the file: {ex.Message}");
                }
            }
        }

        /*
         * Write File
         * 
         * @Param
         * - File base path
         * 
         * Process
         * 1. Let the user enter the filename to write
         * 2. Check the file is used by other program or not
         * 3. Let the user enter the file content want to replace
         * 4. Save the file content
         */
        static void WriteToFile(string path)
        {
            Console.Write("Enter file name to write to: ");
            string fileName = Console.ReadLine();
            string fullPath = Path.Combine(path, fileName);

            if (!File.Exists(fullPath))
            {
                Console.WriteLine("File does not exist.");
                return;
            }

            if (IsFileLocked(fullPath))
            {
                Console.WriteLine("The file is currently in use and cannot be edited.");
                return;
            }

            try
            {
                Console.Write("Enter text to write (existing content will be replaced): ");
                string content = Console.ReadLine();

                File.WriteAllText(fullPath, content);
                Console.WriteLine("✅ Text written and saved to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ An error occurred while writing to the file: {ex.Message}");
            }
        }

        /*
         * Read File
         * 
         * @Param
         * - File base path
         * 
         * Process
         * 1. Let the user enter the filename to read
         * 2. Check the file is used by other program or not
         * 3. Display the file content to the user
         */
        static void ReadFile(string path)
        {
            Console.Write("Enter file name to read: ");
            string fileName = Console.ReadLine();
            string fullPath = Path.Combine(path, fileName);

            if (!File.Exists(fullPath))
            {
                Console.WriteLine("File does not exist.");
                return;
            }

            string content = File.ReadAllText(fullPath);
            Console.WriteLine("\nFile Content:\n" + content);
        }

        /*
         * Delete File
         * 
         * @Param
         * - File base path
         * 
         * Process
         * 1. Let the user enter the filename
         * 2. Check the file is used by other program or not
         * 3. Delete the file
         */
        static void DeleteFile(string path)
        {
            Console.Write("Enter file name to delete: ");
            string fileName = Console.ReadLine();
            string fullPath = Path.Combine(path, fileName);

            if (!File.Exists(fullPath))
            {
                Console.WriteLine("File does not exist.");
                return;
            }

            if (IsFileLocked(fullPath))
            {
                Console.WriteLine("The file is currently in use and cannot be deleted.");
                return;
            }

            try
            {
                File.Delete(fullPath);
                Console.WriteLine("File deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the file: {ex.Message}");
            }
        }

        /*
         * Helper function to check the file has been used by other program or not
         * 
         * @Param
         * - File path
         */
        static bool IsFileLocked(string filePath)
        {
            FileStream stream = null;
            try
            {
                stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                stream?.Close();
            }

            return false;
        }

    }
}

