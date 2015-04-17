using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Spark.Runtime
{
    public abstract class RuntimeInstruction
    {
        protected RuntimeInstruction() { }

        public abstract void Execute(Stream stream);
    }
}
