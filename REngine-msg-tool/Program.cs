using REngine.MsgTool;
using System;
using System.IO;
using System.Text;

namespace REngine_msg_tool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine($"export: re2-msg-tool.exe msg_file_01.msg.<14|15|17>");
                Console.WriteLine($"import: re2-msg-tool.exe msg_file_01.msg.<14|15|17>.txt");
                return;
            }
            var txtFile = args[0];
            var msgFile = new MsgFile();

            var muchMsg = txtFile switch
            {
                var t when t.EndsWith(".msg.14")
                    || t.EndsWith(".msg.15")
                    || t.EndsWith(".msg.17") => true,
                _ => false,
            };
            var muchTxt= txtFile switch
            {
                var t when t.EndsWith(".msg.14.txt")
                    || t.EndsWith(".msg.15.txt")
                    || t.EndsWith(".msg.17.txt") => true,
                _ => false,
            };
            if(muchMsg)
            {
                using Stream stream = File.OpenRead(txtFile);
                msgFile.Read(stream);
                File.WriteAllLines(txtFile + ".txt", msgFile.Export(), Encoding.Unicode);
                return;
            }
            if (muchTxt)
            {
                string text = Path.ChangeExtension(txtFile, null);
                using Stream stream2 = File.OpenRead(text);
                using Stream stream3 = File.Open(text + ".new", FileMode.Create);
                msgFile.Read(stream2);
                msgFile.Import(File.ReadAllLines(text + ".txt", Encoding.Unicode));
                msgFile.Write(stream3);
                return;
            }
            Console.WriteLine("Invalid file format!");
        }
    }
}
