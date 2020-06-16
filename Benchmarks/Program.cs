using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;
using ObjectStackAllocation.Benchmarks.Tests;

namespace ObjectStackAllocation.Benchmarks
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run(typeof(StackAllocateBenchmarks));
        }
    }
}
