using System;
using System.Runtime.CompilerServices;
using ObjectLayoutInspector;

[module: SkipLocalsInit]

namespace ObjectStackAllocation.Core
{
    public static partial class StackAlloc<T> where T : class
    {
        private static readonly TypeLayout TYPE_LAYOUT = TypeLayout.GetLayout<T>();
        private static readonly RuntimeTypeHandle RUNTIME_TYPE_HANDLE = typeof(T).TypeHandle;

        private static readonly IntPtr MethodTablePointer;

        public static readonly int AllocAmount;
        public static readonly int ObjectLength;

        static StackAlloc()
        {
            StackAlloc<T>.MethodTablePointer = StackAlloc<T>.RUNTIME_TYPE_HANDLE.Value;

            StackAlloc<T>.AllocAmount = StackAlloc<T>.TYPE_LAYOUT.FullSize;
            StackAlloc<T>.ObjectLength = StackAlloc<T>.AllocAmount - IntPtr.Size * 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe T AllocInternal(IntPtr* buffer)
        {
            //Zero the memory to be sure 
            StackAlloc<T>.ZeroMemory(buffer + 2);

            Unsafe.AsRef<IntPtr>(buffer) = IntPtr.Zero; //Object Header
            Unsafe.AsRef<IntPtr>(buffer + 1) = StackAlloc<T>.MethodTablePointer; //Method Table Pointer

            IntPtr dataPointer = (IntPtr)Unsafe.AsPointer(ref Unsafe.AsRef<IntPtr>(buffer +1 ));

            return Unsafe.As<IntPtr, T>(ref dataPointer); //Pointer to the method table pointer
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe void ZeroMemory(IntPtr* buffer)
        {
            Span<byte> span = new Span<byte>(buffer, StackAlloc<T>.ObjectLength);

            span.Clear();
        }
    }
}
