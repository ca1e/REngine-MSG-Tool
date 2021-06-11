using Gibbed.IO;
using System.IO;

namespace REngine.MsgTool
{
    class SubEntry
    {
		private byte[] id;

		private int index;

		private long firstOffset;

		private long typeOffset;

		private long[] strOffsets;

		private string first;

		private TypeEntry type;

		private string[] refs;

		public string[] Refs
		{
			get
			{
				return this.refs;
			}
			set
			{
				this.refs = value;
			}
		}

		public string First
		{
			get
			{
				return this.first;
			}
			set
			{
				this.first = value;
			}
		}

		public TypeEntry Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		public long FirstOffset
		{
			get
			{
				return this.firstOffset;
			}
			set
			{
				this.firstOffset = value;
			}
		}

		public long[] StrOffsets
		{
			get
			{
				return this.strOffsets;
			}
			set
			{
				this.strOffsets = value;
			}
		}

		public void Read(uint langCount, uint typeCount, long offset, Stream input, long dataBase, Stream data)
		{
			long position = input.Position;
			input.Position = offset;
			this.id = input.ReadBytes(20);
			this.index = input.ReadValueS32();
			this.firstOffset = input.ReadValueS64();
			this.typeOffset = input.ReadValueS64();
			this.strOffsets = new long[langCount];
			for (uint num = 0U; num < langCount; num += 1U)
			{
				this.strOffsets[(int)num] = input.ReadValueS64();
			}
			input.Seek(this.typeOffset, SeekOrigin.Begin);
			this.Type = new TypeEntry();
			this.Type.Read(typeCount, input, dataBase, data);
			this.first = Util.ReadStringZ(dataBase, data, this.firstOffset);
			this.refs = new string[langCount];
			for (uint num2 = 0U; num2 < langCount; num2 += 1U)
			{
				this.refs[(int)num2] = Util.ReadStringZ(dataBase, data, this.strOffsets[(int)num2]);
			}
			input.Position = position;
		}

		public void Write(Stream output, uint langCount)
		{
			output.WriteBytes(this.id);
			output.WriteValueS32(this.index);
			output.WriteValueS64(this.firstOffset);
			output.WriteValueS64(this.typeOffset);
			for (uint num = 0U; num < langCount; num += 1U)
			{
				output.WriteValueS64(this.strOffsets[(int)num]);
			}
		}
	}
}
