using ObjectStackAllocation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ObjectStackAllocation.Tests.Data;
using Xunit;

namespace ObjectStackAllocation.Tests
{
    [Collection("Sync")]
    public class StackAllocTests
    {
        public const long SimpleDataObjectValue = 0x123456789;

        public static SimpleObject SimpleObject;
        public static SimpleDataObject SimpleDataObject;

        static StackAllocTests()
        {
            //Pre JIT
            StackAllocTests.SimpleObject = StackAlloc<SimpleObject>.Alloc(stackalloc byte[StackAlloc<SimpleObject>.AllocAmount]);
            StackAllocTests.SimpleDataObject = StackAlloc<SimpleDataObject>.Alloc(stackalloc byte[StackAlloc<SimpleDataObject>.AllocAmount], 0L);
        }

        [Fact]
        public void NoHeapAlloc()
        {
            long before = GC.GetTotalAllocatedBytes(precise: true);

            StackAllocTests.SimpleObject = StackAlloc<SimpleObject>.Alloc(stackalloc byte[StackAlloc<SimpleObject>.AllocAmount]);

            long after = GC.GetTotalAllocatedBytes(precise: true);

            Assert.Equal(before, after);
        }

        [Fact]
        public void NoHeapAllocConstructor()
        {
            long before = GC.GetTotalAllocatedBytes(precise: true);

            StackAllocTests.SimpleDataObject = StackAlloc<SimpleDataObject>.Alloc(stackalloc byte[StackAlloc<SimpleDataObject>.AllocAmount], StackAllocTests.SimpleDataObjectValue);

            long after = GC.GetTotalAllocatedBytes(precise: true);

            Assert.Equal(before, after);
            Assert.Equal(StackAllocTests.SimpleDataObject.Value, StackAllocTests.SimpleDataObject.Value);
        }
    }
}
