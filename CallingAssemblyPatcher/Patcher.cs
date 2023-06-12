using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace CallingAssemblyPatcher
{
    internal class Patcher
    {
        public void Patch(ModuleDefMD module)
        {
            int dc = 0;
            foreach (TypeDef type in module.GetTypes().Where(t => t.HasMethods))
            {
                foreach (MethodDef method in type.Methods.Where(m => m.HasBody && m.Body.HasInstructions))
                {
                    var instr = method.Body.Instructions;
                    
                    foreach (var t in instr)
                    {
                        if (t.OpCode.Code == Code.Call &&
                            t.Operand.ToString().Contains("GetCallingAssembly"))
                        {
                            dc++;
                            t.Operand = module.Import(typeof(Assembly).GetMethod("GetExecutingAssembly"));
                        }
                    }
                }
            }

            if (dc == 0)
            {
                Console.WriteLine("No instructions to be patched");
                return;
            }
            else
            {
                Console.WriteLine($"Patched: {dc}");
            }
        }
    }
}
