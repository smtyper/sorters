using System.Collections.Generic;
using System.Linq;

namespace SortingResearch.Sorters
{
    public class QuickSorter : Sorter
    {
        protected override T[] Sort<T>(T[] array)
        {
            var stack = new Stack<int>();

            stack.Push(0);
            stack.Push(array.Length - 1);

            while (stack.Any())
            {
                var rightIndexOfSubset = stack.Pop();
                var leftIndexOfSubSet = stack.Pop();

                var rightIndex = rightIndexOfSubset;
                var leftIndex = leftIndexOfSubSet + 1;
                var pivotIndex = leftIndexOfSubSet;

                if (leftIndex > rightIndex)
                    continue;

                while (leftIndex < rightIndex)
                {
                    while (leftIndex <= rightIndex && array[leftIndex].CompareTo(array[pivotIndex]) <= 0)
                        leftIndex++;

                    while (leftIndex <= rightIndex && array[rightIndex].CompareTo(array[pivotIndex]) >= 0)
                        rightIndex--;

                    if (rightIndex >= leftIndex)
                        Swap(array, leftIndex, rightIndex);
                }

                if (pivotIndex <= rightIndex)
                    if (array[pivotIndex].CompareTo(array[rightIndex]) > 0)
                        Swap(array, pivotIndex, rightIndex);

                if (leftIndexOfSubSet < rightIndex)
                {
                    stack.Push(leftIndexOfSubSet);
                    stack.Push(rightIndex - 1);
                }

                if (rightIndexOfSubset > rightIndex)
                {
                    stack.Push(rightIndex + 1);
                    stack.Push(rightIndexOfSubset);
                }
            }

            return array;
        }
    }
}
