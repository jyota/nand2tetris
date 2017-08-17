using System;

namespace assembler
{
    class Program
    {
        static void Main(string[] args)
        {
            Code myCode = new Code();
            SymbolTable mySymbolTable = new SymbolTable();
            if (args.Length == 0)
            {
                System.Console.WriteLine("Need assembly source code file as argument.");
            }

            System.IO.StreamReader file = new System.IO.StreamReader(args[0]);
            Parser myParser = new Parser(file);
            myParser.Parse();
        }
    }
}
