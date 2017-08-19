using System;
using System.Collections.Generic;

class Code
{
    private Dictionary<string, string> jumpLookup = new Dictionary<string, string>();
    private Dictionary<string, string> cmpLookup = new Dictionary<string, string>();

    public Code()
    {
        cmpLookup["0"] =   "0101010";
        cmpLookup["1"] =   "0111111";
        cmpLookup["-1"] =  "0111010";
        cmpLookup["D"] =   "0001100";
        cmpLookup["A"] =   "0110000";
        cmpLookup["M"] =   "1110000";
        cmpLookup["!D"] =  "0001101";
        cmpLookup["!A"] =  "0110001";
        cmpLookup["!M"] =  "1110001";
        cmpLookup["-D"] =  "0001111";
        cmpLookup["-A"] =  "0110011";
        cmpLookup["-M"] =  "1110011";
        cmpLookup["D+1"] = "0011111";
        cmpLookup["A+1"] = "0110111";
        cmpLookup["M+1"] = "1110111";
        cmpLookup["D-1"] = "0001110";
        cmpLookup["A-1"] = "0110010";
        cmpLookup["M-1"] = "1110010";
        cmpLookup["D+A"] = "0000010";
        cmpLookup["D+M"] = "1000010";
        cmpLookup["D-A"] = "0010011";
        cmpLookup["D-M"] = "1010011";
        cmpLookup["A-D"] = "0000111";
        cmpLookup["M-D"] = "1000111";
        cmpLookup["A-D"] = "0000111";
        cmpLookup["D&A"] = "0000000";
        cmpLookup["D&M"] = "1000000";
        cmpLookup["D|A"] = "0010101";
        cmpLookup["D|M"] = "1010101";

        jumpLookup["JGT"] = "001";
        jumpLookup["JEQ"] = "010";
        jumpLookup["JGE"] = "011";
        jumpLookup["JLT"] = "100";
        jumpLookup["JNE"] = "101";
        jumpLookup["JLE"] = "110";
        jumpLookup["JMP"] = "111";

    }

    public string Dest(String destMnemonic)
    {
        char[] converted = new char[]{ '0', '0', '0' };

        if(destMnemonic == null){
            destMnemonic = "";
        };
        if(destMnemonic.IndexOf('A') >= 0)
        {
            converted[0] = '1';
        };
        if(destMnemonic.IndexOf('D') >= 0)
        {
            converted[1] = '1';
        };
        if(destMnemonic.IndexOf('M') >= 0)
        {
            converted[2] = '1';
        };

        return(new string(converted));
    }

    public string Comp(String compMnemonic)
    {
        return(cmpLookup[compMnemonic]);
    }

    public string Jump(String jumpMnemonic)
    {
        if(jumpMnemonic == null)
        {
            return "000";
        }else{
            return(jumpLookup[jumpMnemonic]);
        }
    }

    public string Ainstruction(int value)
    {
        string preparedValue = Convert.ToString(value, 2).PadLeft(16, '0');
        return(preparedValue);
    }
}
