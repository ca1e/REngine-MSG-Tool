using System;
using System.IO;
using System.Text;
using Gibbed.IO;

namespace REngine.MsgTool
{
	static class Util
	{
		public static void Decrypt(byte[] pBuffer, int dwSize)
		{
			byte b = 0;
			int num = 0;
			int num2 = 0;
			do
			{
				byte b2 = b;
				b = pBuffer[num2];
				int num3 = num++ & 15;
				pBuffer[num2] = (byte)(b2 ^ b ^ Util.m_Key[num3]);
				num2 = num;
			}
			while (num < dwSize);
		}

		public static void Encrypt(byte[] pBuffer, int dwSize)
		{
			byte b = 0;
			int num = 0;
			int num2 = 0;
			do
			{
				byte b2 = pBuffer[num2];
				int num3 = num++ & 15;
				pBuffer[num2] = (byte)(b2 ^ b ^ Util.m_Key[num3]);
				b = pBuffer[num2];
				num2 = num;
			}
			while (num < dwSize);
		}

		public static string ReadStringZ(long dataBase, Stream data, long offset)
		{
			long position = data.Position;
			data.Position = offset - dataBase;
			string result = data.ReadStringZ(Encoding.Unicode);
			data.Position = position;
			return result;
		}

		private static readonly byte[] m_Key = new byte[]
		{
			0xCF, 0xCE, 0xFB, 0xF8, 0xEC, 0x0A, 0x33, 0x66,
			0x93, 0xA9, 0x1D, 0x93, 0x50, 0x39, 0x5F, 0x09
		};
	}
}
