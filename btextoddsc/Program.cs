using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//* non-default
using System.IO;
using btex;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace btextoddsc {
	class Program {
		static void Main(string[] args) {
			Console.WriteLine("BTEX to DDS by Jonathan Armstrong");
			Console.WriteLine("Website: sayhijon.com");
			Console.WriteLine();

			List<string> files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.btex", SearchOption.AllDirectories).ToList();

			Console.WriteLine(files.Count.ToString() + " .btex found. Beginning conversion.");

			int left = Console.CursorLeft,
				top = Console.CursorTop,
				count = 0;

			Console.Write(count.ToString() + " of " + files.Count.ToString() + " completed.");

			foreach (string file in files) {
				byte[] data = null;

				using (Stream inputStream = File.OpenRead(file)) {
					using (BinaryReader reader = new BinaryReader(inputStream)) {
						data = reader.ReadBytes((int)inputStream.Length);
					}
				}

				string path = file.Substring(0, file.LastIndexOf("\\") + 1);
				string filename = file.Substring(file.LastIndexOf("\\") + 1);
				filename = filename.Remove(filename.LastIndexOf("."));
				filename = filename + ".dds";

				BTEX texture = BTEX.Deserialize(data);
				DDS dds = texture.ToDDS();
				dds.Save(path, filename, texture.Data);
				count++;

				Console.CursorTop = top;
				Console.CursorLeft = left;
				Console.Write(count.ToString() + " of " + files.Count.ToString() + " completed.");
			}

			Console.WriteLine();
			Console.WriteLine();

			Console.WriteLine("Thanks for using my software. If you'd like to support my work please visit sayhijon.com and buy me a coffee!");
			Console.WriteLine("Press any key to exit.");

			Test();

			Console.ReadKey();
		}

		private static void Test() {
			string path = @"H:\Downloads\Other_BTEX\";
			string filename = @"bmptest.bmp";

			byte[] data = null;

			using (Stream stream = File.OpenRead(path + filename)) {
				using (BinaryReader reader = new BinaryReader(stream)) {
					data = reader.ReadBytes((int)stream.Length);
				}
			}

			// Create bitmap.
			Bitmap bmp = new Bitmap(368, 288, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			// Lock the bitmap's bits.  
			Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
			BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

			// Get the address of the first line.
			IntPtr ptr = bmpData.Scan0;

			// Copy the RGB values back to the bitmap
			Marshal.Copy(data, 0, ptr, data.Length);

			// Unlock the bits.
			bmp.UnlockBits(bmpData);

			bmp.Save(path + "bmptestcon.bmp");
		}
	}
}
