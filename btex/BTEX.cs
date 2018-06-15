using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btex {
    public class BTEX {
		private const uint SEDBBTEX_HEADER_SIZE = 128;

		public ushort Height {
			get => this.p_Height;
			set => this.p_Height = value;
		}

		public ushort Width {
			get => this.p_Width;
			set => this.p_Width = value;
		}

		public ushort Pitch {
			get => this.p_Pitch;
			set => this.p_Pitch = value;
		}

		public ushort ArraySize {
			get => this.p_ArraySize;
			set => this.p_ArraySize = value;
		}

		public ushort Format {
			get => this.p_Format;
			set => this.p_Format = value;
		}

		public byte Depth {
			get => this.p_Depth;
			set => this.p_Depth = value;
		}

		public byte MipMap {
			get => this.p_MipMap;
			set => this.p_MipMap = value;
		}

		public byte Dimension {
			get => this.p_Dimension;
			set => this.p_Dimension = value;
		}

		public byte[] Data {
			get => this.p_Data;
			set => this.p_Data = value;
		}

		private uint	p_FileSize = 0,
						p_ImageFileSize = 0,
						p_ImageHeaderOffset = 0,
						p_PlatformDataOffset = 0,
						p_SurfaceHeaderOffset = 0,
						p_NameOffset = 0,
						p_PlatformDataSize = 0,
						p_HighTextureDataSizeByte = 0,
						p_TileMode = 0,
						p_SurfaceOffset = 0,
						p_SurfaceSize = 0;

		private ushort	p_Version = 0,
						p_ImageCount = 0,
						p_ImageHeaderStride = 0,
						p_Width = 0,
						p_Height = 0,
						p_Pitch = 0,
						p_Format = 0,
						p_SurfaceCount = 0,
						p_SurfaceHeaderStride = 0,
						p_ArraySize = 0;

		private byte	p_Platform = 0,
						p_Flags = 0,
						p_MipMap = 0,
						p_Depth = 0,
						p_Dimension = 0,
						p_ImageFlags = 0,
						p_HighTextureMipLevels = 0;

		private byte[]	p_Data = null;

		public static BTEX Deserialize(byte[] data) {
			//* first we need to check and see if we're working with the full SEDBbtex or just the BTEX data
			string magic = Encoding.UTF8.GetString(data, 0, 4);

			if (magic == "SEDB")
				data = RemoveSEDBHeader(data);

			magic = Encoding.UTF8.GetString(data, 0, 4);

			if (magic != "BTEX")
				//* we don't know what we're working with now so return null
				throw new FormatException("Unknown data, cannot parse");

			BTEX btex = new BTEX();

			using (MemoryStream memory = new MemoryStream(data)) {
				memory.Seek(4, SeekOrigin.Begin);

				using (BinaryReader reader = new BinaryReader(memory)) {
					//* read btex header
					btex.p_FileSize = reader.ReadUInt32();
					btex.p_ImageFileSize = reader.ReadUInt32();
					btex.p_Version = reader.ReadUInt16();
					btex.p_Platform = reader.ReadByte();
					btex.p_Flags = reader.ReadByte();

					btex.p_ImageCount = reader.ReadUInt16();
					btex.p_ImageHeaderStride = reader.ReadUInt16();
					btex.p_ImageHeaderOffset = reader.ReadUInt32();

					//* move to and read image header
					memory.Seek(btex.p_ImageHeaderOffset, SeekOrigin.Begin);

					btex.p_Width = reader.ReadUInt16();
					btex.p_Height = reader.ReadUInt16();
					btex.p_Pitch = reader.ReadUInt16();
					btex.p_Format = reader.ReadUInt16();
					btex.p_MipMap = reader.ReadByte();
					btex.p_Depth = reader.ReadByte();
					btex.p_Dimension = reader.ReadByte();
					btex.p_ImageFlags = reader.ReadByte();
					btex.p_SurfaceCount = reader.ReadUInt16();
					btex.p_SurfaceHeaderStride = reader.ReadUInt16();

					btex.p_PlatformDataOffset = reader.ReadUInt32();
					btex.p_SurfaceHeaderOffset = reader.ReadUInt32();
					btex.p_NameOffset = reader.ReadUInt32();
					btex.p_PlatformDataSize = reader.ReadUInt32();

					btex.p_HighTextureMipLevels = reader.ReadByte();
					//* 3 bytes of padding
					memory.Seek(3, SeekOrigin.Current);
					btex.p_HighTextureDataSizeByte = reader.ReadUInt32();
					//* 8 bytes of padding
					memory.Seek(8, SeekOrigin.Current);

					btex.p_TileMode = reader.ReadUInt32();
					btex.p_ArraySize = reader.ReadUInt16();

					memory.Seek(btex.p_ImageHeaderOffset + btex.p_SurfaceHeaderOffset, SeekOrigin.Begin);
					//memory.Seek(0, SeekOrigin.Begin);
					btex.p_Data = reader.ReadBytes((int)(memory.Length - memory.Position));
				}
			}

			return btex;
		}

		public DDS ToDDS() {
			DDS dds = new DDS {
				Height = this.Height,
				Width = this.Width,
				Pitch = this.Pitch,
				Depth = this.Depth,
				MipMapCount = this.MipMap
			};

			dds.Flags |= (int)(DDSFlags.Pitch | DDSFlags.Depth | DDSFlags.MipMapCount);
			dds.PixelFormat = new PixelFormat();
			dds.DX10Header = new DX10() {
				ArraySize = this.ArraySize,
				Format = DDS.BTEXFormatToDX10Format(this.Format),
				ResourceDimension = this.Dimension + 1u
			};

			//* experimenting with stuff trying to get 0x1a to work correctly
			if (dds.DX10Header.Format == 77) {
				/*
				pf.dwRBitMask == 0xff0000) && \
   (pf.dwGBitMask == 0xff00) && \
   (pf.dwBBitMask == 0xff) && \
   (pf.dwAlphaBitMask == 0xff000000U))
				*/
				dds.PixelFormat.Flags |= (int)(PixelFormatFlags.RGB | PixelFormatFlags.Alpha);
				dds.PixelFormat.RGBBitCount = 32;
				dds.PixelFormat.BBitMask = 0xffffff;
				dds.PixelFormat.GBitMask = 0x00ff00;
				dds.PixelFormat.RBitMask = 0x0000ff;
				dds.PixelFormat.ABitMask = unchecked((int)0xff000000);
			}

			return dds;
		}

		private static byte[] RemoveSEDBHeader(byte[] data) {
			byte[] clean = new byte[data.Length - SEDBBTEX_HEADER_SIZE];

			Array.Copy(data, SEDBBTEX_HEADER_SIZE, clean, 0, clean.Length);

			return clean;
		}
    }
}
