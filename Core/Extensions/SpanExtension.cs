using System;
using System.Runtime.CompilerServices;

namespace ObjectStackAllocation.Core.Extensions
{
    internal static class SpanExtension
    {
        internal static unsafe byte* AsPointer(this ref Span<byte> span) => (byte*)Unsafe.AsPointer(ref span.GetPinnableReference());
    }
}
