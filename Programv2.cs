using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace REngine
{
	// Token: 0x02000004 RID: 4
	internal class Program
	{
		// Token: 0x06000017 RID: 23 RVA: 0x00003360 File Offset: 0x00001560
		private static void Main(string[] args)
		{
			Console.WriteLine("Resident Evil msg tool.       Version 1.5 from 09.05.2021");
			Console.WriteLine();
			try
			{
				bool flag = args.Length != 1 && args.Length != 2;
				if (flag)
				{
					Console.WriteLine();
					Console.WriteLine("export:");
					Console.WriteLine(Program.GetExecutableName() + " File.msg.* en");
					Console.WriteLine();
					Console.WriteLine("import:");
					Console.WriteLine(Program.GetExecutableName() + " File.txt en");
					Console.WriteLine();
					Console.WriteLine();
					Console.WriteLine(" default language: en");
					Console.WriteLine(" language: ID = Japanese: ja, English: en, French: fr, Italian: it, German: de, Spanish: es, Russian: ru");
					Console.WriteLine(" language: ID = Polish: pl, Portuguese Brasilian: ptBR, Korean: ko, Chinese (traditional): zhTW");
					Console.WriteLine(" language: ID = Chinese (Simplified): zhCN, Arabic: ar, Thai: th");
					Console.WriteLine();
					Console.WriteLine();
					Console.WriteLine(" Supported games:");
					Console.WriteLine();
					Console.WriteLine(" Resident Evil 2 Remake");
					Console.WriteLine(" Resident Evil 3 Remake");
					Console.WriteLine(" Resident Evil Village");
					Console.WriteLine();
					Environment.Exit(1);
				}
				bool flag2 = args[0].EndsWith(".msg.14");
				if (flag2)
				{
					bool flag3 = args.Length == 2;
					if (flag3)
					{
						TextMSG.LANG_CODE = (uint)Tools.ParseLanguage14(args[1]);
					}
					TextMSG textMSG = new TextMSG();
					using (Stream stream = File.OpenRead(args[0]))
					{
						textMSG.Read14(stream);
						File.WriteAllLines(args[0] + ".txt", textMSG.Export(), Encoding.Unicode);
						Console.WriteLine("Done!");
						Console.WriteLine();
					}
				}
				else
				{
					bool flag4 = args[0].EndsWith(".msg.14.txt");
					if (flag4)
					{
						bool flag5 = args[0].EndsWith(".msg.14");
						if (flag5)
						{
							Console.WriteLine(" ERROR:  File .msg.14 not found.");
							Console.WriteLine();
							Environment.Exit(0);
						}
						bool flag6 = args.Length == 2;
						if (flag6)
						{
							TextMSG.LANG_CODE = (uint)Tools.ParseLanguage14(args[1]);
						}
						string text = Path.ChangeExtension(args[0], null);
						TextMSG textMSG2 = new TextMSG();
						using (Stream stream2 = File.OpenRead(text))
						{
							using (Stream stream3 = File.Open(text + ".new", FileMode.Create))
							{
								textMSG2.Read14(stream2);
								textMSG2.Import(File.ReadAllLines(text + ".txt", Encoding.Unicode));
								textMSG2.Write(stream3);
								Console.WriteLine("Done!");
								Console.WriteLine();
							}
						}
					}
				}
				bool flag7 = args[0].EndsWith(".msg.15");
				if (flag7)
				{
					bool flag8 = args.Length == 2;
					if (flag8)
					{
						TextMSG.LANG_CODE = (uint)Tools.ParseLanguage15(args[1]);
					}
					TextMSG textMSG3 = new TextMSG();
					using (Stream stream4 = File.OpenRead(args[0]))
					{
						textMSG3.Read15(stream4);
						File.WriteAllLines(args[0] + ".txt", textMSG3.Export(), Encoding.Unicode);
						Console.WriteLine("Done!");
						Console.WriteLine();
					}
				}
				else
				{
					bool flag9 = args[0].EndsWith(".msg.15.txt");
					if (flag9)
					{
						bool flag10 = args[0].EndsWith(".msg.15");
						if (flag10)
						{
							Console.WriteLine(" ERROR:  File .msg.15 not found.");
							Console.WriteLine();
							Environment.Exit(0);
						}
						bool flag11 = args.Length == 2;
						if (flag11)
						{
							TextMSG.LANG_CODE = (uint)Tools.ParseLanguage15(args[1]);
						}
						string text2 = Path.ChangeExtension(args[0], null);
						TextMSG textMSG4 = new TextMSG();
						using (Stream stream5 = File.OpenRead(text2))
						{
							using (Stream stream6 = File.Open(text2 + ".new", FileMode.Create))
							{
								textMSG4.Read15(stream5);
								textMSG4.Import(File.ReadAllLines(text2 + ".txt", Encoding.Unicode));
								textMSG4.Write(stream6);
								Console.WriteLine("Done!");
								Console.WriteLine();
							}
						}
					}
				}
				bool flag12 = args[0].EndsWith(".msg.17");
				if (flag12)
				{
					bool flag13 = args.Length == 2;
					if (flag13)
					{
						TextMSG.LANG_CODE = (uint)Tools.ParseLanguage17(args[1]);
					}
					TextMSG textMSG5 = new TextMSG();
					using (Stream stream7 = File.OpenRead(args[0]))
					{
						textMSG5.Read17(stream7);
						File.WriteAllLines(args[0] + ".txt", textMSG5.Export(), Encoding.Unicode);
						Console.WriteLine("Done!");
						Console.WriteLine();
					}
				}
				else
				{
					bool flag14 = args[0].EndsWith(".msg.17.txt");
					if (flag14)
					{
						bool flag15 = args[0].EndsWith(".msg.17");
						if (flag15)
						{
							Console.WriteLine(" ERROR:  File .msg.17 not found.");
							Console.WriteLine();
							Environment.Exit(0);
						}
						bool flag16 = args.Length == 2;
						if (flag16)
						{
							TextMSG.LANG_CODE = (uint)Tools.ParseLanguage17(args[1]);
						}
						string text3 = Path.ChangeExtension(args[0], null);
						TextMSG textMSG6 = new TextMSG();
						using (Stream stream8 = File.OpenRead(text3))
						{
							using (Stream stream9 = File.Open(text3 + ".new", FileMode.Create))
							{
								textMSG6.Read17(stream8);
								textMSG6.Import(File.ReadAllLines(text3 + ".txt", Encoding.Unicode));
								textMSG6.Write(stream9);
								Console.WriteLine("Done!");
								Console.WriteLine();
							}
						}
					}
				}
			}
			catch (ArgumentException)
			{
				Console.WriteLine(" WARNING:   (" + Tools.ParseLanguage17(args[1]).ToString() + ") language not found in file.");
				Console.WriteLine();
				Environment.Exit(0);
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine();
				Console.WriteLine(" ERROR:  File .msg.* not found.");
				Console.WriteLine();
				Environment.Exit(0);
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00003A70 File Offset: 0x00001C70
		private static string GetExecutableName()
		{
			return Path.GetFileName(Assembly.GetExecutingAssembly().Location);
		}
	}
}
