using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btex {
	public class DX10 {
		public uint Format {
			get => this.p_Format;
			set => this.p_Format = value;
		}

		public uint ResourceDimension {
			get => this.p_ResourceDimension;
			set => this.p_ResourceDimension = value;
		}

		public uint MiscFlags {
			get => this.p_miscFlag;
			set => this.p_miscFlag = value;
		}

		public uint ArraySize {
			get => this.p_ArraySize;
			set => this.p_ArraySize = value;
		}

		public uint MiscFlags2 {
			get => this.p_miscFlags2;
			set => this.p_miscFlags2 = value;
		}

		private uint	p_Format = 0,
						p_ResourceDimension = 0,
						p_miscFlag = 0,
						p_ArraySize = 0,
						p_miscFlags2 = 0;
	}

	public enum DX10MiscFlags2 : uint {
		Unknown = 0x0,
		Straight = 0x01,
		Premultiplied = 0x02,
		Opaque = 0x03,
		Custom = 0x04
	}
}
