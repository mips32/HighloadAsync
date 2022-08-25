using System;
using System.IO.MemoryMappedFiles;

namespace highload.Helpers
{
    public static class StringHelper
    {
        public static unsafe string ReadString(this MemoryMappedViewAccessor accessor, long offset)
        {
            // MemoryMappedViewAccessor does not expose any methods for reading strings,
            // but it does allow us to obtain a pointer to the raw memory, which string
            // constructors accept as parameters.
            byte* ptrMemMap = (byte*)0;
            accessor.SafeMemoryMappedViewHandle.AcquirePointer(ref ptrMemMap);
            return new String((sbyte*)ptrMemMap + offset);
        }

    }
}