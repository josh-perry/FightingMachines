using System;

namespace FightingMachines
{
    internal class Program
    {
        private static void Main()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new GenePool(100);

            Console.ReadKey();
        }
    }
}
