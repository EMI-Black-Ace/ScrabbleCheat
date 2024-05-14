namespace WordLookupCore
{
    public static class Permutations
    {
        public static IEnumerable<T[]> GetPermutations<T>(T[] values, int fromInd = 0)
        {
            if (fromInd + 1 == values.Length)
                yield return values;
            else
            {
                foreach (var v in GetPermutations(values, fromInd + 1))
                    yield return v;

                for (var i = fromInd + 1; i < values.Length; i++)
                {
                    SwapValues(values, fromInd, i);
                    foreach (var v in GetPermutations(values, fromInd + 1))
                        yield return v;
                    SwapValues(values, fromInd, i);
                }
            }
        }

        public static IEnumerable<T?[]> GetSubsets<T>(T[] source)
        {
            int max = 1 << source.Length;
            for (int i = 0; i < max; i++)
            {
                T?[] combination = new T[source.Length];

                for (int j = 0; j < source.Length; j++)
                {
                    int tailIndex = source.Length - j - 1;
                    if ((i & (1 << j)) == 0)
                    {
                        combination[tailIndex] = default;
                    }
                    else
                    {
                        combination[tailIndex] = source[tailIndex];
                    }
                }

                yield return combination;
            }
        }

        private static void SwapValues<T>(T[] values, int pos1, int pos2)
        {
            if (pos1 != pos2)
            {
                T tmp = values[pos1];
                values[pos1] = values[pos2];
                values[pos2] = tmp;
            }
        }
    }
}