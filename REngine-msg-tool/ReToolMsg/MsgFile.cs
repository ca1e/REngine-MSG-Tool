using Gibbed.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace REngine.MsgTool
{

    public class MsgFile
	{
		public Language LANG_CODE { get; set; }

		private static readonly uint GMSG = 1196641607U;
		private static readonly string STRI = "<string>";
		private static readonly string NLF = "<lf>";
		private static readonly string CRLF = "\r\n";

		private SubEntry[] subs;
		private TypeEntry[] types;
		private MemoryStream data;

		private uint magic;
		private uint version;
		private long headerOffset;
		private uint subCount;
		private uint typeCount;
		private uint langCount;
		private uint zero;
		private long data1Offset;
		private long data2Offset;
		private long langOffset;
		private long typeOffset;
		private long typenameOffset;
		private long[] subOffsets;
		private long zeroOffset;
		private int[] langs;
		private int[] typeIds;
		private byte[] garbage;
		private long[] typenamesStr;
		private byte[] dataArr;
		private string[] typeNames;

		public string[] Export()
		{
			string[] array = new string[this.subCount];
			for (uint num = 0U; num < this.subCount; num += 1U)
			{
				array[(int)num] = Encode(this.subs[(int)num].Refs[(int)this.LANG_CODE]);
			}
			return array;
		}

		public void Import(string[] lines)
		{
			if (lines.Length != this.subCount)
			{
				throw new Exception($"Unexpected the number of lines in text file! lines.Length({lines.Length}) != subCount({this.subCount})");
			}
			this.UpdateTexts(lines);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				this.UpdateOffsets(this.CalculateOffsets(this.StringPool(), memoryStream));
				this.dataArr = memoryStream.ToArray();
			}
		}

		public void Read(Stream input, MsgVersion version = MsgVersion.V14)
		{
			this.version = input.ReadValueU32();
			this.magic = input.ReadValueU32();
			this.headerOffset = input.ReadValueS64();
			this.subCount = input.ReadValueU32();
			this.typeCount = input.ReadValueU32();
			this.langCount = input.ReadValueU32();
			this.zero = input.ReadValueU32();
			this.Check(this.zero == 0U && this.magic == GMSG && this.version == (uint)version, "Header");
			this.data1Offset = input.ReadValueS64();
			this.data2Offset = input.ReadValueS64();
			this.langOffset = input.ReadValueS64();
			this.typeOffset = input.ReadValueS64();
			this.typenameOffset = input.ReadValueS64();
			long position = input.Position;
			input.Seek(this.data2Offset, SeekOrigin.Begin);
			this.Check(input.ReadValueS64() == 0L, "Data2Offset");
			input.Seek(this.data1Offset, SeekOrigin.Begin);
			int num = (int)(input.Length - this.data1Offset);
			byte[] array = input.ReadBytes(num);
			Util.Decrypt(array, num);
			input.Position = position;
			using (this.data = new MemoryStream(array))
			{
				this.subOffsets = new long[this.subCount];
				this.subs = new SubEntry[this.subCount];
				for (uint num2 = 0U; num2 < this.subCount; num2 += 1U)
				{
					this.subOffsets[(int)num2] = input.ReadValueS64();
					this.subs[(int)num2] = new SubEntry();
					this.subs[(int)num2].Read(this.langCount, this.typeCount, this.subOffsets[(int)num2], input, this.data1Offset, this.data);
				}
				this.zeroOffset = input.ReadValueS64();
				this.Check(this.zeroOffset == 0L, "Data2Offset");
				this.langs = new int[this.langCount];
				this.Check(input.Position == this.langOffset, "LangOffet");
				for (uint num3 = 0U; num3 < this.langCount; num3 += 1U)
				{
					this.langs[(int)num3] = input.ReadValueS32();
				}
				this.typeIds = new int[this.typeCount];
				this.Check(input.Position == this.typeOffset, "TypeOffset");
				for (uint num4 = 0U; num4 < this.typeCount; num4 += 1U)
				{
					this.typeIds[(int)num4] = input.ReadValueS32();
				}
				this.garbage = input.ReadBytes((int)(this.typenameOffset - input.Position));
				this.typenamesStr = new long[this.typeCount];
				this.typeNames = new string[this.typeCount];
				this.Check(input.Position == this.typenameOffset, "TypeNameOffset diff is " + (this.typenameOffset - input.Position).ToString());
				for (uint num5 = 0U; num5 < this.typeCount; num5 += 1U)
				{
					this.typenamesStr[(int)num5] = input.ReadValueS64();
					this.typeNames[(int)num5] = Util.ReadStringZ(this.data1Offset, this.data, this.typenamesStr[(int)num5]);
				}
				this.types = new TypeEntry[this.subCount];
				for (uint num6 = 0U; num6 < this.subCount; num6 += 1U)
				{
					this.types[(int)num6] = this.subs[(int)num6].Type;
				}
			}
		}

		public void Write(Stream output)
		{
			output.WriteValueU32(this.version);
			output.WriteValueU32(this.magic);
			output.WriteValueS64(this.headerOffset);
			output.WriteValueU32(this.subCount);
			output.WriteValueU32(this.typeCount);
			output.WriteValueU32(this.langCount);
			output.WriteValueU32(this.zero);
			output.WriteValueS64(this.data1Offset);
			output.WriteValueS64(this.data2Offset);
			output.WriteValueS64(this.langOffset);
			output.WriteValueS64(this.typeOffset);
			output.WriteValueS64(this.typenameOffset);
			for (uint num = 0U; num < this.subCount; num += 1U)
			{
				output.WriteValueS64(this.subOffsets[(int)num]);
			}
			output.WriteValueS64(this.zeroOffset);
			for (uint num2 = 0U; num2 < this.langCount; num2 += 1U)
			{
				output.WriteValueS32(this.langs[(int)num2]);
			}
			for (uint num3 = 0U; num3 < this.typeCount; num3 += 1U)
			{
				output.WriteValueS32(this.typeIds[(int)num3]);
			}
			output.WriteBytes(this.garbage);
			for (uint num4 = 0U; num4 < this.typeCount; num4 += 1U)
			{
				output.WriteValueS64(this.typenamesStr[(int)num4]);
			}
			for (uint num5 = 0U; num5 < this.subCount; num5 += 1U)
			{
				this.subs[(int)num5].Write(output, this.langCount);
			}
			for (uint num6 = 0U; num6 < this.subCount; num6 += 1U)
			{
				this.types[(int)num6].Write(output, this.typeCount);
			}
			Util.Encrypt(this.dataArr, this.dataArr.Length);
			output.WriteBytes(this.dataArr);
		}

		private void Check(bool cond, string reason = "")
		{
			if (!cond)
			{
				throw new Exception($"[{reason}]The file has an unknown header.");
			}
		}

		private static string Encode(string str)
		{
			return STRI + str.Replace(CRLF, NLF);
		}

		private static string Decode(string str)
		{
			return str.Substring(STRI.Length).Replace(NLF, CRLF);
		}

		private void UpdateTexts(string[] lines)
		{
			for (uint num = 0U; num < this.subCount; num += 1U)
			{
				this.subs[(int)num].Refs[(int)this.LANG_CODE] = Decode(lines[(int)num]);
			}
		}

		private ISet<string> StringPool()
		{
			SortedSet<string> sortedSet = new SortedSet<string>();
			for (uint num = 0U; num < this.typeCount; num += 1U)
			{
				sortedSet.Add(this.typeNames[(int)num]);
			}
			for (uint num2 = 0U; num2 < this.subCount; num2 += 1U)
			{
				sortedSet.Add(this.subs[(int)num2].First);
				for (uint num3 = 0U; num3 < this.langCount; num3 += 1U)
				{
					sortedSet.Add(this.subs[(int)num2].Refs[(int)num3]);
				}
				for (uint num4 = 0U; num4 < this.typeCount; num4 += 1U)
				{
					if (this.types[(int)num2].Type[(int)num4] != null)
					{
						sortedSet.Add(this.types[(int)num2].Type[(int)num4]);
					}
				}
			}
			return sortedSet;
		}

		private Dictionary<string, long> CalculateOffsets(ISet<string> pool, MemoryStream dataOut)
		{
			var dictionary = new Dictionary<string, long>(pool.Count);
			foreach (string text in pool)
			{
				dictionary.Add(text, dataOut.Position + this.data1Offset);
				dataOut.WriteStringZ(text, Encoding.Unicode);
			}
			return dictionary;
		}

		private void UpdateOffsets(Dictionary<string, long> offsets)
		{
			for (uint num = 0U; num < this.typeCount; num += 1U)
			{
				this.typenamesStr[(int)num] = offsets[this.typeNames[(int)num]];
			}
			for (uint num2 = 0U; num2 < this.subCount; num2 += 1U)
			{
				this.subs[(int)num2].FirstOffset = offsets[this.subs[(int)num2].First];
				for (uint num3 = 0U; num3 < this.langCount; num3 += 1U)
				{
					this.subs[(int)num2].StrOffsets[(int)num3] = offsets[this.subs[(int)num2].Refs[(int)num3]];
				}
				for (uint num4 = 0U; num4 < this.typeCount; num4 += 1U)
				{
					if (this.types[(int)num2].Type[(int)num4] != null)
					{
						this.types[(int)num2].TypeStrOffset[(int)num4] = offsets[this.types[(int)num2].Type[(int)num4]];
					}
				}
			}
		}
	}

	public enum MsgVersion
    {
		V14 = 14,
		V15,
		V17 = 17,
    }

	public enum Language// 14, 15, 17
	{
		ja,
		en,
		fr,
		it,
		de,
		es,
		ru,
		pl,
		ptBR = 10,
		ko,
		zhTW,
		zhCN,
		// for msg.17
		ar = 21,
		th = 26
	}
}
