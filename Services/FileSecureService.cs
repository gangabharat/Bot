using System.Security.Cryptography;
using System.Text;

namespace Bot.Services
{
    public interface IFileSecureService
    {
        public string? SecretKey { get; set; }
        public string? FilePath { get; set; }
        public string? FileExtension { get; set; }
        void Init(string? secretKey = null, string? filePath = null, string? fileExtension = null);
        string Encrypt(string fileName, string content);        
        string Decrypt(string fileName);
    }
    public class FileSecureService : IFileSecureService
    {
        public string? SecretKey { get; set; }
        public string? FilePath { get; set; } = Path.GetTempPath();
        public string? FileExtension { get; set; } = "bot";
        public void Init(string? secretKey = null, string? filePath = null, string? fileExtension = null)
        {
            SecretKey = secretKey ?? SecretKey;
            FilePath = filePath ?? FilePath;
            FileExtension = fileExtension ?? FileExtension;


            Console.WriteLine("File Secure Path {0} with '{1}' extension", FilePath, fileExtension);
        }

        /// <summary>
        /// Return Encrypted filename
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public string Encrypt(string fileName, string content)
        {
            var fileNamewithPath = Path.Combine(FilePath!, $"{fileName}.{FileExtension}");

            using (FileStream fileStream = new(fileNamewithPath, FileMode.OpenOrCreate))
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] key =
                    {
                        0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                        0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
                    };

                    if (SecretKey != null)
                    {
                        key = Encoding.UTF8.GetBytes(SecretKey);
                    }

                    aes.Key = key;

                    byte[] iv = aes.IV;
                    fileStream.Write(iv, 0, iv.Length);

                    using (CryptoStream cryptoStream = new(
                        fileStream,
                        aes.CreateEncryptor(),
                        CryptoStreamMode.Write))
                    {
                        // By default, the StreamWriter uses UTF-8 encoding.
                        // To change the text encoding, pass the desired encoding as the second parameter.
                        // For example, new StreamWriter(cryptoStream, Encoding.Unicode).
                        using (StreamWriter encryptWriter = new(cryptoStream))
                        {
                            //encryptWriter.
                            encryptWriter.WriteLine(content);
                        }
                    }
                }
            }
            return fileNamewithPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">encrypted file name</param>
        /// <returns>decrypted text</returns>
        public string Decrypt(string fileName)
        {
            using (FileStream fileStream = new(fileName, FileMode.Open))
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] iv = new byte[aes.IV.Length];
                    int numBytesToRead = aes.IV.Length;
                    int numBytesRead = 0;
                    while (numBytesToRead > 0)
                    {
                        int n = fileStream.Read(iv, numBytesRead, numBytesToRead);
                        if (n == 0) break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }

                    byte[] key =
                    {
                        0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                        0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
                    };

                    if (SecretKey != null)
                    {
                        key = Encoding.UTF8.GetBytes(SecretKey);
                    }

                    using (CryptoStream cryptoStream = new(
                       fileStream,
                       aes.CreateDecryptor(key, iv),
                       CryptoStreamMode.Read))
                    {
                        // By default, the StreamReader uses UTF-8 encoding.
                        // To change the text encoding, pass the desired encoding as the second parameter.
                        // For example, new StreamReader(cryptoStream, Encoding.Unicode).
                        using (StreamReader decryptReader = new(cryptoStream))
                        {
                            // Create a FileStream to write to the file
                            string decryptedMessage = decryptReader.ReadToEndAsync().Result;
                            //Console.WriteLine($"The decrypted original message: {decryptedMessage}");
                            return decryptedMessage;
                        }
                    }
                }
            }
        }
    }
}