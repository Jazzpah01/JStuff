using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace JStuff.Randomness
{
    public static class RandomGenerator
    {
        private static int[] numbers;
        private static int index;

        /// <summary>
        /// Generate an array of numbers, in the interval [start;end].
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public static void GenerateNumbers(int start, int end, int amount)
        {
            numbers = new int[amount];
            index = 0;

            int m = 2147483647;
            int a = 16807;
            int c = 10211;
            int X = Mathf.Abs(DateTime.UtcNow.Second) % (m-1) + 1;

            for (int i = 0; i < amount; i++)
            {
                numbers[i] = (a * X + c) % m % (end - start) - start;
                X = numbers[i];
            }
        }

        public static int NextInteger()
        {
            int retval = numbers[index];
            if (numbers.Length <= index)
            {
                throw new System.Exception("Random numbers are exhausted!");
            }
            index++;
            return retval;
        }
    }
}