using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tk7texturevanilla
{
    public static class ArrayExtensions
    {
        static readonly int[] Empty = new int[0];

        public static byte[] GetBytes(this byte[] bytes, int start, int length)
        {

            byte[] newBytes = new byte[length];

            //Array.Copy(bytes, start, newBytes, 0, length);

            Buffer.BlockCopy(bytes, start, newBytes, 0, length);

            return newBytes;
        }

        public static string GetNullTerminatedString(this byte[] array, int startPosition)
        {
            int length = 0;
            while(array[startPosition + length] != 0x00)
            {
                length++;
            }

            return Encoding.ASCII.GetString(array.GetBytes(startPosition, length));
        }

        static bool IsEmptyLocate(byte[] array, byte[] candidate)
        {
            return array == null
                || candidate == null
                || array.Length == 0
                || candidate.Length == 0
                || candidate.Length > array.Length;
        }

        static bool IsMatch(byte[] array, int position, byte[] candidate)
        {
            if (candidate.Length > (array.Length - position))
                return false;

            for (int i = 0; i < candidate.Length; i++)
                if (array[position + i] != candidate[i])
                    return false;

            return true;
        }

        public static int[] Locate(this byte[] self, byte[] candidate)
        {
            if (IsEmptyLocate(self, candidate))
                return Empty;

            var list = new List<int>();

            for (int i = 0; i < self.Length; i++)
            {
                if (!IsMatch(self, i, candidate))
                    continue;

                list.Add(i);
            }

            return list.Count == 0 ? Empty : list.ToArray();
        }
    }
}
