using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DES_Cryptography
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1. Шифрування");
            Console.WriteLine("2. Розшифрування");
            Console.Write("Виберіть дію (1 або 2): ");
            int choice = int.Parse(Console.ReadLine());

            string filePath = @"d:\test.txt";
            string key = "ABCDEFGH"; // Ключ для DES (8 символів)
            string iv = "ABCDEFGH"; // Вектор ініціалізації (8 символів)

            if (choice == 1)
            {
                Console.Write("Введіть текст для шифрування: ");
                string plainText = Console.ReadLine();
                EncryptData(plainText, filePath, key, iv);
                Console.WriteLine("Дані зашифровані та збережені у файл.");
            }
            else if (choice == 2)
            {
                string decryptedText = DecryptData(filePath, key, iv);
                Console.WriteLine("Розшифровані дані: " + decryptedText);
            }
            else
            {
                Console.WriteLine("Неправильний вибір.");
            }
        }

        static void EncryptData(string plainText, string filePath, string key, string iv)
        {
            try
            {
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    des.Key = Encoding.ASCII.GetBytes(key);
                    des.IV = Encoding.ASCII.GetBytes(iv);
                    des.Mode = CipherMode.CBC;

                    using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                    using (CryptoStream cs = new CryptoStream(fs, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] data = Encoding.ASCII.GetBytes(plainText);
                        cs.Write(data, 0, data.Length);
                        Console.WriteLine("Шифрування завершено.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка під час шифрування: " + ex.Message);
            }
        }

        static string DecryptData(string filePath, string key, string iv)
        {
            try
            {
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    des.Key = Encoding.ASCII.GetBytes(key);
                    des.IV = Encoding.ASCII.GetBytes(iv);
                    des.Mode = CipherMode.CBC;

                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    using (CryptoStream cs = new CryptoStream(fs, des.CreateDecryptor(), CryptoStreamMode.Read))
                    using (StreamReader reader = new StreamReader(cs))
                    {
                        string decryptedText = reader.ReadToEnd();
                        Console.WriteLine("Розшифрування завершено.");
                        return decryptedText;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка під час розшифрування: " + ex.Message);
                return null;
            }
        }
    }
}
