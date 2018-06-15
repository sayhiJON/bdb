using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btex {
	public class PixelFormat {
		public int Size {
			get => this.p_Size;
			set => this.p_Size = value;
		}

		public int Flags {
			get => this.p_Flags;
			set => this.p_Flags = value;
		}

		public int FourCC {
			get => this.p_FourCC;
			set => this.p_FourCC = value;
		}

		public int RGBBitCount {
			get => this.p_RGBBitCount;
			set => this.p_RGBBitCount = value;
		}

		public int RBitMask {
			get => this.p_RBitMask;
			set => this.p_RBitMask = value;
		}

		public int GBitMask {
			get => this.p_GBitMask;
			set => this.p_GBitMask = value;
		}

		public int BBitMask {
			get => this.p_BBitMask;
			set => this.p_BBitMask = value;
		}

		public int ABitMask {
			get => this.p_ABitMask;
			set => this.p_ABitMask = value;
		}

		private int p_Size			= 32,
					p_Flags			= (int)PixelFormatFlags.FourCC,
					p_FourCC		= 808540228,	//* DX10
					p_RGBBitCount	= 0,
					p_RBitMask		= 0,
					p_GBitMask		= 0,
					p_BBitMask		= 0,
					p_ABitMask		= 0;
	}

	/*
	DDPF_ALPHAPIXELS	Texture contains alpha data; dwRGBAlphaBitMask contains valid data.			0x1
	DDPF_ALPHA			Used in some older DDS files for alpha channel only uncompressed data 
						(dwRGBBitCount contains the alpha channel bitcount; dwABitMask contains 
						valid data)																	0x2
	DDPF_FOURCC			Texture contains compressed RGB data; dwFourCC contains valid data.			0x4
	DDPF_RGB			Texture contains uncompressed RGB data; dwRGBBitCount and the RGB masks 
						(dwRBitMask, dwGBitMask, dwBBitMask) contain valid data.					0x40
	DDPF_YUV			Used in some older DDS files for YUV uncompressed data (dwRGBBitCount 
						contains the YUV bit count; dwRBitMask contains the Y mask, dwGBitMask 
						contains the U mask, dwBBitMask contains the V mask)						0x200
	DDPF_LUMINANCE		Used in some older DDS files for single channel color uncompressed data 
						(dwRGBBitCount contains the luminance channel bit count; dwRBitMask 
						contains the channel mask). Can be combined with DDPF_ALPHAPIXELS for a two 
						channel DDS file.															0x20000
	*/
	[Flags]
	public enum PixelFormatFlags : uint {
		AlphaPixels = 0x01,
		Alpha = 0x02,
		FourCC = 0x04,
		RGB = 0x40,
		YUV = 0x200,
		Luminance = 0x20000
	}
}
