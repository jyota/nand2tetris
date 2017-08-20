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

            System.IO.StreamWriter outFile = new System.IO.StreamWriter("Prog.hack");

            Parser myParser = new Parser(args[0]);
            foreach(var line in myParser.Parse())
            {
                outFile.WriteLine(line);
            };
            outFile.Close();
        }
    }
}
