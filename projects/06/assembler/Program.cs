using System;

namespace assembler
{
    class Program
    {
        static void Main(string[] args)
        {
            SymbolTable mySymbolTable = new SymbolTable();
            if (args == null || args.Length == 0)
            {
                System.Console.WriteLine("Need assembly source code file as argument.");
                System.Environment.Exit(1);   
            }

            System.IO.StreamReader file = new System.IO.StreamReader(args[0]);
            Parser myParser = new Parser(file);
            foreach(var line in myParser.Parse())
            {
                System.Console.WriteLine(line);
            };
        }
    }
}
