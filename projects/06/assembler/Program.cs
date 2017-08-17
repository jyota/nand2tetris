using System;

namespace assembler
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser myParser = new Parser();
            Code myCode = new Code();
            SymbolTable mySymbolTable = new SymbolTable();
            Console.WriteLine(myParser.CallMe() + myCode.CallMeToo() + mySymbolTable.CallMeThree());
        }
    }
}
