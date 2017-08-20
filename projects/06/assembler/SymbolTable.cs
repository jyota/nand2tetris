using System;
using System.Collections.Generic;

class SymbolTable
{
    private Dictionary<string, int> symbolLookup = new Dictionary<string, int>();
    private int currentRAMlocation;

    public SymbolTable()
    {
        symbolLookup["SP"] = 0;
        symbolLookup["LCL"] = 1;
        symbolLookup["ARG"] = 2;
        symbolLookup["THIS"] = 3;
        symbolLookup["THAT"] = 4;
        symbolLookup["R0"] = 0;
        symbolLookup["R1"] = 1;
        symbolLookup["R2"] = 2;
        symbolLookup["R3"] = 3;
        symbolLookup["R4"] = 4;
        symbolLookup["R5"] = 5;
        symbolLookup["R6"] = 6;
        symbolLookup["R7"] = 7;
        symbolLookup["R8"] = 8;
        symbolLookup["R9"] = 9;
        symbolLookup["R10"] = 10;
        symbolLookup["R11"] = 11;
        symbolLookup["R12"] = 12;
        symbolLookup["R13"] = 13;
        symbolLookup["R14"] = 14;
        symbolLookup["R15"] = 15;
        symbolLookup["SCREEN"] = 16384;
        symbolLookup["KBD"] = 24576;
        currentRAMlocation = 16;
    }

    public void AddEntry(string symbol, int address)
    {
        symbolLookup[symbol] = address;
    }

    public int GetUsableRAMLocation()
    {
        int usable = currentRAMlocation;
        ++currentRAMlocation;
        return usable;
    }
    public bool Contains(string symbol)
    {
        return symbolLookup.ContainsKey(symbol);
    }

    public int GetAddress(string symbol)
    {
        return symbolLookup[symbol];
    }
}
