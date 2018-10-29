using System;
using System.IO;
using System.Security.Cryptography;


public static class Cryptography
{
	public static byte[] EncryptBytes(byte[] bytes, string key, string iv)
	{
		using (var rijndael = new RijndaelManaged())
		{
			rijndael.Key = Convert.FromBase64String(key);
			rijndael.IV = Convert.FromBase64String(iv);

			using (var stream = new MemoryStream())
			{
				using (var encryptor = rijndael.CreateEncryptor())
				{
					using (var encrypt = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
					{
						encrypt.Write(bytes, 0, bytes.Length);
						encrypt.FlushFinalBlock();

						return stream.ToArray();
					}
				}
			}
		}
	}


	public static byte[] DecryptBytes(byte[] bytes, string key, string iv)
	{
		using (var rijndael = new RijndaelManaged())
		{
			rijndael.Key = Convert.FromBase64String(key);
			rijndael.IV = Convert.FromBase64String(iv);

			using (var stream = new MemoryStream())
			{
				using (var decryptor = rijndael.CreateDecryptor())
				{
					using (var encrypt = new CryptoStream(stream, decryptor, CryptoStreamMode.Write))
					{
						encrypt.Write(bytes, 0, bytes.Length);
						encrypt.FlushFinalBlock();

						return stream.ToArray();
					}
				}
			}
		}
	}
}
