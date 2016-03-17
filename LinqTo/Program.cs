using System;

namespace LinqTo
{
    class Program
    {
        static void Main(string[] args)
        {
            new LinqToXML().Test();
            new LinqToDataset().Test();
            Console.ReadLine();
        }
    }
}
