using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu256.Simulator.Processor
{
    public class CPUTrace : List<string>
    {
        public const int TRACE_STEPS_MAX = 20000;
        public const int TRACE_STEPS_MIN = 10000;

        public void Add(CPU cpu)
        {
            AddStep(cpu.ToString());
        }

        void AddStep(string newStep)
        {
            // Prune the trace if it's gotten too long
            if (Count > TRACE_STEPS_MAX)
            {
                int n = Count - TRACE_STEPS_MIN;
                RemoveRange(0, n);
            }

            Add(newStep);
        }
    }
}
