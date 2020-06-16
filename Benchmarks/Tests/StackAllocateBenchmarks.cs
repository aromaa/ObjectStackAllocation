using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using ObjectStackAllocation.Core;

[module: SkipLocalsInit]

namespace ObjectStackAllocation.Benchmarks.Tests
{
    [MemoryDiagnoser]
    public class StackAllocateBenchmarks
    {
        public Simple SimpleObj;
        public SimpleData SimpleDataObj;

        static StackAllocateBenchmarks()
        {
            StackAlloc<Simple>.Prepare();
        }

        [Benchmark]
        public void HeapAllocateSimple()
        {
            this.SimpleObj = new Simple();
        }

        [Benchmark]
        public void HeapAllocateData()
        {
            this.SimpleDataObj = new SimpleData(50u);
        }

        [Benchmark]
        public void StackAllocateSimple()
        {
            this.SimpleObj = StackAlloc<Simple>.Alloc(stackalloc byte[StackAlloc<Simple>.AllocAmount]);
        }

        [Benchmark]
        public void StackAllocateData()
        {
            this.SimpleDataObj = StackAlloc<SimpleData>.Alloc(stackalloc byte[StackAlloc<SimpleData>.AllocAmount], 50u);
        }

        public class Simple
        {
            public uint Value;
        }

        public class SimpleData
        {
            public uint Value;

            public SimpleData(uint value)
            {
                this.Value = value;
            }
        }
    }
}
