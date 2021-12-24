// #define RESET_REMOVED_ELEMENTS

using Appalachia.Core.Objects.Root;

namespace Appalachia.Prototype.KOC.Debugging.DebugConsole.Collections
{
    public class CircularBuffer<T> : AppalachiaSimpleBase
    {
        public CircularBuffer(int capacity)
        {
            arr = new T[capacity];
        }

        private readonly T[] arr;
        private int startIndex;

        public int Count { get; private set; }

        public T this[int index] => arr[(startIndex + index) % arr.Length];

        // Old elements are overwritten when capacity is reached
        public void Add(T value)
        {
            if (Count < arr.Length)
            {
                arr[Count++] = value;
            }
            else
            {
                arr[startIndex] = value;
                if (++startIndex >= arr.Length)
                {
                    startIndex = 0;
                }
            }
        }
    }
}
