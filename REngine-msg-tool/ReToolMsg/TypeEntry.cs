using Gibbed.IO;
using System.IO;

namespace REngine.MsgTool
{
    class TypeEntry
	{
		private long[] typeStrOffset;

		private string[] type;

		public string[] Type
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

		public long[] TypeStrOffset
		{
			get
			{
				return this.typeStrOffset;
			}
			set
			{
				this.typeStrOffset = value;
			}
		}

		public void Read(uint typeCount, Stream input, long dataBase, Stream data)
		{
			this.typeStrOffset = new long[typeCount];
			this.Type = new string[typeCount];
			for (uint num = 0U; num < typeCount; num += 1U)
			{
				this.typeStrOffset[(int)num] = input.ReadValueS64();
				this.Type[(int)num] = ((this.typeStrOffset[(int)num] < dataBase) ? null : Util.ReadStringZ(dataBase, data, this.typeStrOffset[(int)num]));
			}
		}

		public void Write(Stream output, uint typeCount)
		{
			for (uint num = 0U; num < typeCount; num += 1U)
			{
				output.WriteValueS64(this.typeStrOffset[(int)num]);
			}
		}
	}
}
