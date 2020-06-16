using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ObjectStackAllocation.Core;
using ObjectStackAllocation.Tests.Data;
using Xunit;

namespace ObjectStackAllocation.Tests
{
    [Collection("Sync")]
    public class HeapAllocTests
    {
        public static SimpleObject SimpleObject;

        static HeapAllocTests()
        {
            //Pre JIT
            HeapAllocTests.SimpleObject = new SimpleObject();
        }

        [Fact]
        public void HeapAlloc()
        {
            long before = GC.GetTotalAllocatedBytes(precise: true);

            HeapAllocTests.SimpleObject = new SimpleObject();

            long after = GC.GetTotalAllocatedBytes(precise: true);

            Assert.Equal(SimpleObject.TypeLayout.FullSize, after - before);
        }
    }
}
