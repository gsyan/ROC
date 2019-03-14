using UnityEngine;
using System;
using System.IO;
using System.Collections;
using ICSharpCode.SharpZipLib.Zip;


public static class ZipUtility
{
	private const int BUFFER_SIZE = 1024 * 1024 * 2;


	public class ExtractState
	{
		public byte[] buffer;
		public string targetDir;
		public ZipFile zipFile;
		public int fileIndex;
		public Stream streamInput;
		public Stream streamWrite;
		public Callback<int, int, string> progress;
		public float startTime;

		public ExtractState(int buffSize)
		{
			buffer = new byte[buffSize];
		}
	}


	public static bool Compression(string zipPath, string baseDir, string[] fileNames, CallbackResult<bool, string, int, int> progress = null)
	{
		DateTime startTime = DateTime.Now;

		try
		{
			string dirName = Path.GetDirectoryName(zipPath);
			if (!Directory.Exists(dirName))
			{
				Directory.CreateDirectory(dirName);
			}

			using (ZipOutputStream zos = new ZipOutputStream(File.Create(zipPath)))
			{
				byte[] buffer = new byte[BUFFER_SIZE];

				for (int i = 0; i < fileNames.Length; i++)
				{
					using (FileStream fs = File.OpenRead(baseDir + "/" + fileNames[i]))
					{
						ZipEntry entry = new ZipEntry(fileNames[i]);
						entry.DateTime = DateTime.Now;
						entry.Size = fs.Length;
						zos.PutNextEntry(entry);

						int readBytes = 0;
						int totalReadBytes = 0;

						while ((readBytes = fs.Read(buffer, 0, buffer.Length)) > 0)
						{
							zos.Write(buffer, 0, readBytes);
							totalReadBytes += readBytes;
						}

						if (totalReadBytes != fs.Length)
						{
							return false;
						}
					}

					if (progress != null && progress(zipPath, i, fileNames.Length))
					{
						return false;
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.Log(string.Format("Exception during processing {0}", ex));
			return false;
		}

		Debug.Log("compression elapsedTime=" + (DateTime.Now - startTime));

		return true;
	}


	public static IEnumerator Decompression(string zipPath, string targetDir, int startPos, Callback<int, int, string> progress = null)
	{
		ZipFile zipFile = new ZipFile(File.Open(zipPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
		if (zipFile != null)
		{
			if (!Directory.Exists(targetDir))
			{
				Directory.CreateDirectory(targetDir);
			}

			byte[] buffer = new byte[BUFFER_SIZE];

			for (int i = startPos; i < zipFile.Size; i++)
			{
				ZipEntry entry = zipFile[i];

				if (!entry.IsDirectory)
				{
					progress(i, zipFile.Size, entry.Name);

					yield return null;

					using (Stream stream = zipFile.GetInputStream(i))
					{
						using (FileStream fs = new FileStream(string.Format("{0}/{1}", targetDir, entry.Name), FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
						{
							int readBytes;
							while ((readBytes = stream.Read(buffer, 0, buffer.Length)) > 0)
							{
								fs.Write(buffer, 0, readBytes);
							}
						}
					}
				}
			}

			zipFile.Close();
		}
	}
}
