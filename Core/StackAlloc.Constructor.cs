using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using ObjectStackAllocation.Core.Extensions;

namespace ObjectStackAllocation.Core
{
    public static partial class StackAlloc<T> where T : class
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T Alloc(Span<byte> buffer) => StackAlloc<T>.Alloc(buffer.AsPointer());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T Alloc(byte* buffer)
        {
            T returnObject = StackAlloc<T>.AllocInternal((IntPtr*)buffer);

            Constructor.Invoke(returnObject);

            return returnObject;
        }

        private static unsafe class Constructor
        {
            private static readonly delegate*<T, void> ConstructorHandle;

            static Constructor()
            {
                Type type = typeof(T);

                ConstructorInfo constructorInfo = type.GetConstructor(new Type[0]);

                Constructor.ConstructorHandle = (delegate*<T, void>)constructorInfo.MethodHandle.GetFunctionPointer();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static void Invoke(T returnObject)
            {
                Constructor.ConstructorHandle(returnObject);
            }
        }
    }
}
