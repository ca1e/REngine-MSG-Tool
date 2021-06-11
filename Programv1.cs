using System;
using System.IO;
using System.Text;

namespace re2_msg
{
	// Token: 0x02000004 RID: 4
	internal class Program
	{
		// Token: 0x06000013 RID: 19 RVA: 0x000029E0 File Offset: 0x00000BE0
		private static void Main(string[] args)
		{
			Console.WriteLine("Resident Evil 2 Remake -- msg tool -- celikeins -- 2019.02.18");
			if (args.Length != 1)
			{
				Console.WriteLine("export: re2-msg-tool.exe mes_file_01.msg.14");
				Console.WriteLine("import: re2-msg-tool.exe mes_file_01.msg.14.txt");
				Environment.Exit(1);
			}
			if (args[0].EndsWith(".msg.14") || args[0].EndsWith(".msg.17"))
			{
				var msgFile = new MsgFile();
				using (Stream stream = File.OpenRead(args[0]))
				{
					msgFile.Read(stream);
					File.WriteAllLines(args[0] + ".txt", msgFile.Export(), Encoding.Unicode);
					goto IL_FA;
				}
			}
			if (args[0].EndsWith(".msg.14.txt"))
			{
				string text = Path.ChangeExtension(args[0], null);
				MsgFile msgFile2 = new MsgFile();
				using (Stream stream2 = File.OpenRead(text))
				{
					using (Stream stream3 = File.Open(text + ".new", FileMode.Create))
					{
						msgFile2.Read(stream2);
						msgFile2.Import(File.ReadAllLines(text + ".txt", Encoding.Unicode));
						msgFile2.Write(stream3);
					}
				}
			}
			IL_FA:
			Console.WriteLine("Done!");
		}
	}
}
