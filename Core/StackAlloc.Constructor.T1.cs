using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using ObjectStackAllocation.Core.Extensions;

namespace ObjectStackAllocation.Core
{
    public static partial class StackAlloc<T> where T : class
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T Alloc<T1>(Span<byte> buffer, T1 value) => StackAlloc<T>.Alloc(buffer.AsPointer(), value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T Alloc<T1>(byte* buffer, T1 value)
        {
            T returnObject = StackAlloc<T>.AllocInternal((IntPtr*)buffer);

            Constructor<T1>.Invoke(returnObject, value);

            return returnObject;
        }

        private static unsafe class Constructor<T1>
        {
            private static readonly delegate*<T, T1, void> ConstructorHandle;

            static Constructor()
            {
                Type type = typeof(T);

                ConstructorInfo constructorInfo = type.GetConstructor(new[]
                {
                    typeof(T1)
                });

                Constructor<T1>.ConstructorHandle = (delegate*<T, T1, void>)constructorInfo.MethodHandle.GetFunctionPointer();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static void Invoke(T returnObject, T1 value)
            {
                Constructor<T1>.ConstructorHandle(returnObject, value);
            }
        }
    }
}
