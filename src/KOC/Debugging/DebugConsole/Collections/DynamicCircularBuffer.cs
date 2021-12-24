using System;
using Appalachia.Core.Objects.Root;

namespace Appalachia.Prototype.KOC.Debugging.DebugConsole.Collections
{
    public class DynamicCircularBuffer<T> : AppalachiaSimpleBase
    {
        public DynamicCircularBuffer(int initialCapacity = 2)
        {
            arr = new T[initialCapacity];
        }

        private int startIndex;
        private T[] arr;

        public int Count { get; private set; }

        public T this[int index]
        {
            get => arr[(startIndex + index) % arr.Length];
            set => arr[(startIndex + index) % arr.Length] = value;
        }

        public void Add(T value)
        {
            if (Count >= arr.Length)
            {
                var prevSize = arr.Length;
                var newSize =
                    prevSize > 0
                        ? prevSize * 2
                        : 2; // Size must be doubled (at least), or the shift operation below must consider IndexOutOfRange situations

                Array.Resize(ref arr, newSize);

                if (startIndex > 0)
                {
                    if (startIndex <= ((prevSize - 1) / 2))
                    {
                        // Move elements [0,startIndex) to the end
                        for (var i = 0; i < startIndex; i++)
                        {
                            arr[i + prevSize] = arr[i];
#if RESET_REMOVED_ELEMENTS
							arr[i] = default( T );
#endif
                        }
                    }
                    else
                    {
                        // Move elements [startIndex,prevSize) to the end
                        var delta = newSize - prevSize;
                        for (var i = prevSize - 1; i >= startIndex; i--)
                        {
                            arr[i + delta] = arr[i];
#if RESET_REMOVED_ELEMENTS
							arr[i] = default( T );
#endif
                        }

                        startIndex += delta;
                    }
                }
            }

            this[Count++] = value;
        }

        public T RemoveFirst()
        {
            var element = arr[startIndex];
#if RESET_REMOVED_ELEMENTS
			arr[startIndex] = default( T );
#endif

            if (++startIndex >= arr.Length)
            {
                startIndex = 0;
            }

            Count--;
            return element;
        }

        public T RemoveLast()
        {
            var element = arr[Count - 1];
#if RESET_REMOVED_ELEMENTS
			arr[Count - 1] = default( T );
#endif

            Count--;
            return element;
        }
    }
}
