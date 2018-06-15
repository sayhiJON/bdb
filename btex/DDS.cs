using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btex {
	public class DDS {
		public int Height {
			get => this.p_Height;
			set => this.p_Height = value;
		}

		public int Width {
			get => this.p_Width;
			set => this.p_Width = value;
		}

		public int Pitch {
			get => this.p_Pitch;
			set => this.p_Pitch = value;
		}

		public int Depth {
			get => this.p_Depth;
			set => this.p_Depth = value;
		}

		public int MipMapCount {
			get => this.p_MipMapCount;
			set => this.p_MipMapCount = value;
		}

		public int Flags {
			get => this.p_Flags;
			set => this.p_Flags = value;
		}

		public PixelFormat PixelFormat {
			get => this.p_SPF;
			set => this.p_SPF = value;
		}

		public DX10 DX10Header {
			get => this.p_DX10;
			set => this.p_DX10 = value;
		}

		private int			p_Size			= 124,
							p_Flags			= (int)DDSFlags.Texture,
							p_Height		= 0,
							p_Width			= 0,
							p_Pitch			= 0,
							p_Depth			= 0,
							p_MipMapCount	= 0,
							p_Caps			= (int)DDSCaps.Texture,
							p_Caps2			= 0,
							p_Caps3			= 0,
							p_Caps4			= 0,
							p_Reserved2		= 0;

		private int[]		p_Reserved		= new int[11];

		private PixelFormat p_SPF			= null;

		private DX10		p_DX10			= null;

		public byte[] ToArray(byte[] data) {
			byte[] output = null;

			using (MemoryStream outputStream = new MemoryStream()) {
				this.WriteToStream(outputStream, data);

				output = outputStream.ToArray();
			}

			return output;
		}

		public void Save(string path, string filename, byte[] data) {
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			using (Stream outputStream = File.Create(path + filename)) {
				this.WriteToStream(outputStream, data);
			}
		}

		private void WriteToStream(Stream stream, byte[] data) {
			using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, true)) {
				writer.Write(542327876);
				writer.Write(this.p_Size);
				writer.Write(this.p_Flags);
				writer.Write(this.p_Height);
				writer.Write(this.p_Width);
				writer.Write(this.p_Pitch);
				writer.Write(this.p_Depth);
				writer.Write(this.p_MipMapCount);
				writer.Write(0);
				writer.Write(0);
				writer.Write(0);
				writer.Write(0);
				writer.Write(0);
				writer.Write(0);
				writer.Write(0);
				writer.Write(0);
				writer.Write(0);
				writer.Write(0);
				writer.Write(0);
				writer.Write(this.p_SPF.Size);
				writer.Write(this.p_SPF.Flags);
				writer.Write(this.p_SPF.FourCC);
				writer.Write(this.p_SPF.RGBBitCount);
				writer.Write(this.p_SPF.RBitMask);
				writer.Write(this.p_SPF.GBitMask);
				writer.Write(this.p_SPF.BBitMask);
				writer.Write(this.p_SPF.ABitMask);
				writer.Write(this.p_Caps);
				writer.Write(this.p_Caps2);
				writer.Write(this.p_Caps3);
				writer.Write(this.p_Caps4);
				writer.Write(0);
				writer.Write(this.p_DX10.Format);
				writer.Write(this.p_DX10.ResourceDimension);
				writer.Write(this.p_DX10.MiscFlags);
				writer.Write(this.p_DX10.ArraySize);
				writer.Write(this.p_DX10.MiscFlags2);
				writer.Write(data);
			}
		}

		public static uint BTEXFormatToDX10Format(ushort format) {
			switch (format) {
				case 0x0b:
					return 28;
				case 0x19:
					return 74;
				case 0x1a:
					return 77;
				case 0x21:
					return 80;
				case 0x22:
					return 83;
				case 0x23:
					return 95;
				case 0x24:
					return 98;
				case 0x18:
				default:
					return 71;
			}
		}
	}

	[Flags]
	public enum DDSFlags : uint {
		None = 0x00,
		Caps = 0x01,
		Height = 0x02,
		Width = 0x04,
		Pitch = 0x08,
		PixelFormat = 0x1000,
		MipMapCount = 0x20000,
		LinearSize = 0x80000,
		Depth = 0x800000,
		Texture = Caps | Height | Width | PixelFormat
	}

	[Flags]
	public enum DDSCaps : uint {
		None = 0x00,
		Complex = 0x08,
		MipMap = 0x400000,
		Texture = 0x1000
	}
}
