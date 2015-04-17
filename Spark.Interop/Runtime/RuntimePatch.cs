using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Spark.Interop.Runtime
{
    public sealed class RuntimePatch
    {
        #region Properties
        public string Name { get; set; }

        public IList<RuntimeInstruction> Instructions {get; private set;}
        #endregion

        public RuntimePatch()
        {
            this.Instructions = new List<RuntimeInstruction>();
        }

        public void Apply(Stream stream)
        {
            foreach (var instruction in this.Instructions)
                instruction.Execute(stream);
        }
    }
}
