using System;

namespace ObjectStackAllocation.Core
{
    public static partial class StackAlloc<T> where T : class
    {
        public static void Prepare()
        {
            GetMethodTablePointer();
            GetAllocAmount();
            GetObjectLength();

            static IntPtr GetMethodTablePointer() => StackAlloc<T>.MethodTablePointer;
            static int GetAllocAmount() => StackAlloc<T>.AllocAmount;
            static int GetObjectLength() => StackAlloc<T>.ObjectLength;
        }
    }
}
