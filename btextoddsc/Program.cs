using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//* non-default
using System.IO;
using btex;

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
			Console.ReadKey();
		}
	}
}
