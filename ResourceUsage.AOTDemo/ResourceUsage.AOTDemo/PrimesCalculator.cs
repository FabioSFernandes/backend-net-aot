using System;
using System.Collections.Generic;

namespace ResourceUsage.AOTDemo
{
    public class PrimeCalculator
    {
        public static List<double> CalculatePrimes(double upperLimit)
        {
            List<double> primes = new List<double>();
            bool isPrime;

            for (double i = 2; i <= upperLimit; i++)
            {
                isPrime = true;
                for (double j = 2; j <= Math.Sqrt(i); j++)
                {
                    if (i % j == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }
                if (isPrime)
                {
                    primes.Add(i);
                }
            }
            return primes;
        }

    }

}
