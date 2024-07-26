using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Multithreading;

public static class FileHasher
{
    public static string GetCheckSum(string filePath)
    {
        if (!File.Exists(filePath)) 
            throw new FileNotFoundException("File not found", filePath);

        return ToSha256(File.ReadAllText(filePath));
    }


private static string ToSha256(string str)

    {

        SHA256 sha256 = SHA256.Create();

        // Convert the input string to a byte array and compute the hash.

        byte[] bytes = Encoding.UTF8.GetBytes(str);

        byte[] hashBytes = sha256.ComputeHash(bytes);

        sha256.Dispose();

        // Convert the byte array to a hexadecimal string.

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < hashBytes.Length; i++)

        {

            sb.Append(hashBytes[i].ToString("x2"));

        }

        return sb.ToString();

    }

}
