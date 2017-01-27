using System;

namespace AgentFire.Performance.SystemClock.TestApp
{
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine($"Minimum: {Resolution.Min.Ticks} ticks.");
            Console.WriteLine($"Maximum: {Resolution.Max.Ticks} ticks.");

            Console.WriteLine($"Current: {Resolution.Current.Ticks} ticks.");
            ConsoleKeyInfo k;

            do
            {
                Console.Write("Enter desired resolution (ticks): ");
                int value = int.Parse(Console.ReadLine());
                Resolution.TrySet(TimeSpan.FromTicks(value));

                Console.WriteLine($"The API call was made and the resolution value is now {Resolution.Current.Ticks} ticks.");
                Console.WriteLine("Press any key to repeat, and 'q' to exit this process and hence revert the changes.");
                k = Console.ReadKey(true);
            }
            while (k.KeyChar != 'q');
        }
    }
}
