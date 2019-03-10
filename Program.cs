using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; 

namespace hoylo
{
    class Program
    {
        const int WAVE_RIFF_HEADER = 0x46464952; 

        static BinaryReader file;

        static int global_wav_idx = 0;

        static int Main(string[] args)
        {
      

            Console.WriteLine("HOYLO by XAYRGA");
            Console.WriteLine("http://www.xayr.ga/");

            if (args.Length > 0)
            {
                try
                {
                    file = new BinaryReader(File.OpenRead(args[0]));
                } catch
                {
                    Console.WriteLine("Cannot open file {0}", args[0]);
                }
                try
                {
                    Directory.CreateDirectory("out");
                }
                catch { }

                while (file.BaseStream.Position < file.BaseStream.Length) {
                    var rhead = file.ReadUInt32();
                    if (rhead == WAVE_RIFF_HEADER)
                    {
                        global_wav_idx++;
                        Console.WriteLine("Found wave at {0:X6}", file.BaseStream.Position);
                        // Here, we're already past the RIFF header. 
                        var size = file.ReadInt32() + 8; // we've read 8 bytes.
                        file.BaseStream.Position = file.BaseStream.Position - 8;
                        var name = String.Format("./out/0x{0:X6}.wav", file.BaseStream.Position); // generate filename
                        // now read the entire wav file
                        var wdata = file.ReadBytes(size);
                        File.WriteAllBytes(name, wdata);
                        
                        

                    } else
                    {
                        file.BaseStream.Position = file.BaseStream.Position - 3; // Advance 1 byte at a time.
                    }
                }



            }
            else
            {
                Console.WriteLine("No file specified.");
                return -1;
            }


            return 0;
        }
    }
}
