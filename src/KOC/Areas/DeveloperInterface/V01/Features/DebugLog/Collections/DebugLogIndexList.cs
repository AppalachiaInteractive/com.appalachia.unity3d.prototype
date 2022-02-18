using System;
using Appalachia.Core.Objects.Root;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog.Collections
{
    [Serializable]
    public class DebugLogIndexList : AppalachiaSimpleBase
    {
        public DebugLogIndexList()
        {
            indices = new int[64];
            size = 0;
        }

        #region Fields and Autoproperties

        private int size;
        private int[] indices;

        #endregion

        public int Count => size;

        public int this[int index] => indices[index];

        public void Add(int index)
        {
            if (size == indices.Length)
            {
                Array.Resize(ref indices, size * 2);
            }

            indices[size++] = index;
        }

        public void Clear()
        {
            size = 0;
        }

        public int IndexOf(int index)
        {
            return Array.IndexOf(indices, index);
        }
    }
}
