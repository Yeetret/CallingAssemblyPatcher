using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Writer;

namespace CallingAssemblyPatcher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath;
            try
            {
                filePath = args[0];
            }
            catch
            {
                Console.WriteLine("Enter file path to patch:");
                filePath = Console.ReadLine();
            }

            ModuleDefMD module = ModuleDefMD.Load(filePath);

            new Patcher().Patch(module);

            var opts = new ModuleWriterOptions(module);
            opts.MetadataLogger = DummyLogger.NoThrowInstance; //Prevents errors if module is obfuscated
            string output = Path.GetFileNameWithoutExtension(filePath) + "_patched" + Path.GetExtension(filePath);
            module.Write(output, opts);
        }
    }
}
