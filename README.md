# Object Stack Allocation
This is a pure hack to allow allocating objects in the stack which contains only blittable field types. The "point" is to allow stack allocation without modifications to the CLR. Reference type fields are not supported in any way as GC write barriers (optimization by the GC) tries to write to invalid memory location. This could be fixed by changing the CLR to use guarded write barries, however this has negative impact on the overall performance.

## Warning
By using the object's in any wrong way **YOU WILL** crash the CLR! Do not use this in production. Do not use the objects outside of the stack. Do not by any means assign the object to a field. You have been warned!

## CoreCLR
CoreCLR is actually already able to perform scalar replacement for objects in simple cases where they do not escape the stack. However this is disabled by default! You can track the progress of this optimization from here [runtime#11192](https://github.com/dotnet/runtime/issues/11192). Even tho its in progress, you can actually test the current behavior by settings COMPlus_JitObjectStackAllocation=1. Its pretty cool!

## Performance
```
|              Method |     Mean |     Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------------- |---------:|----------:|----------:|-------:|------:|------:|----------:|
|  HeapAllocateSimple | 5.170 ns | 0.0752 ns | 0.0667 ns | 0.0076 |     - |     - |      24 B |
|    HeapAllocateData | 5.472 ns | 0.0696 ns | 0.0581 ns | 0.0077 |     - |     - |      24 B |
| StackAllocateSimple | 4.851 ns | 0.0365 ns | 0.0341 ns |      - |     - |     - |         - |
|   StackAllocateData | 5.294 ns | 0.1077 ns | 0.0899 ns |      - |     - |     - |         - |
```

## Usage
As you may expect, the usage syntax looks ugly.

```csharp
StackAlloc<SimpleObject>.Alloc(stackalloc byte[StackAlloc<SimpleObject>.AllocAmount]);
```

You can also pass arguments by using.

```csharp
StackAlloc<SimpleDataObject>.Alloc(stackalloc byte[StackAlloc<SimpleDataObject>.AllocAmount], 0L);
```

## Have fun hacking!