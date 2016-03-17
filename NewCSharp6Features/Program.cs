using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Do not use Console prefix
using static System.Console;

//Import specific class with extenion methos from a namespace
using static A.Extensions;

namespace NewCSharp6Features
{
    class Cat
    {
        public string Name {  get; set; }

        //Expression Bodied Auto-Properties
        public string NameToLower => Name.ToLower();

        //Auto property improvements
        public long Id { get; } = DateTime.Now.Ticks;
    }
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("aaa".Fun());

            //String Interpolation
            var cat = new Cat { Name = "Kaja" };
            WriteLine($"Cat {cat.Name} {cat.NameToLower}");

            var s = $"hello, {cat.Name}";
            System.IFormattable s2 = $"Hello, {cat.Name}";
            System.FormattableString  s3= $"Hello, {cat.Name}";


            //nameof
            Console.WriteLine(nameof(cat));
            Console.WriteLine(nameof(Program));
            Console.WriteLine(nameof(Cat));
            Console.WriteLine(nameof(cat.Name));
            Console.WriteLine(nameof(Cat.Name));

            //Null-Conditional Operator
            int[] array = null;
            cat = null;
            WriteLine(array?.Length);
            WriteLine(cat?.ToString());
            WriteLine(array?[0]);

            //Dictionary Initializer
            var dict = new Dictionary<int, string>
            {
                [1] = "a",
                [2] = "b"
            };

            //Exception filters

            try
            {
                throw new Exception("aaaa");
            }
            catch(Exception ex) when (ex.Message == "aaaa")
            {
                WriteLine(ex.Message);
            }


        

            Console.ReadLine();
        }
    }
}


namespace A
{
    public static class Extensions
    {
        //Expression Bodied Methods 
        public static string Fun(this string s) => "Fun" + s;

        
    }

    public static class Extensions2
    {
        //Expression Bodied Methods
        public static string Fun2(this string s) => "Fun2" + s;
    }
}

