using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class Parser
{
    private System.IO.StreamReader fileToRead;
    private String currentCommandString;
    private IEnumerator<string> commandsSet;
    private int programCounter;
    SymbolTable symbolTable = new SymbolTable();

    public Parser(string filename)
    {
        System.IO.StreamReader fileFirstPass = new System.IO.StreamReader(filename);

        System.IO.StreamReader file = new System.IO.StreamReader(filename);

        fileToRead = fileFirstPass;
        var cmdsEnumerator = GetCommands();
        commandsSet = cmdsEnumerator.GetEnumerator();
        programCounter = 0;
        ParseForSymbols();
        fileFirstPass.Close();
        fileToRead = file;
        cmdsEnumerator = GetCommands();
        commandsSet = cmdsEnumerator.GetEnumerator();
    }

    public bool HasMoreCommands()
    {
        return(commandsSet.MoveNext());
    }

    public void Advance()
    {
        if(HasMoreCommands()){
            currentCommandString = commandsSet.Current;
        }else{
            currentCommandString = null;
        }
    }

    public IEnumerable<string> GetCommands()
    {
        String line;
        String whitespaceRemovedLine;
        while(!fileToRead.EndOfStream)
        {
            line = fileToRead.ReadLine();
            whitespaceRemovedLine = Regex.Replace(line, @"\s+", "");
            if(!(whitespaceRemovedLine.Length == 0 || 
                 (whitespaceRemovedLine.Length >=2 && whitespaceRemovedLine.Substring(0, 2) == "//")))
            {
                currentCommandString = whitespaceRemovedLine;
                yield return whitespaceRemovedLine;
            }
        }
        fileToRead.Close();
    }

    bool IsDigitsOnly(string str)
    {
        foreach (char c in str)
        {
            if (c < '0' || c > '9')
                return false;
        }

        return true;
    }

    public string CommandType()
    {
        if(currentCommandString == null){
            return null;
        }else{
            switch(currentCommandString[0])
            {
                case '@': 
                    return("A_COMMAND");
                case '(':
                    return("L_COMMAND");
                default:
                    return("C_COMMAND");
            }
        }
    }

    public string Symbol()
    {
        List<string> validCommands = new List<string>();
        validCommands.Add("A_COMMAND");
        validCommands.Add("L_COMMAND");

        if(validCommands.Any(s=> s == CommandType())){
            return Regex.Replace(currentCommandString, @"[\@\(\)]", "");
        }else{
            return null;
        }
    }

    public string Dest()
    {
        List<string> validCommands = new List<string>();
        validCommands.Add("C_COMMAND");
        if(validCommands.Any(s=> s == CommandType())){
            var regex = new Regex(@"([A-Za-z]+)=");
            var match = regex.Match(currentCommandString);
            if(match.Success){
                return match.Groups[1].Value;
            }else{
                return null;
            }
        }else{
            return null;
        }
    }

    public string Comp()
    {
        List<string> validCommands = new List<string>();
        validCommands.Add("C_COMMAND");
        if(validCommands.Any(s=> s == CommandType())){
            var regex = new Regex(@"=([0-9A-Za-z\-\+\!\|\&]+)");
            var regexJmp = new Regex(@"([0-9A-Za-z\-\+\!\|\&]+);.*");
            var match = regex.Match(currentCommandString);
            var matchJump = regexJmp.Match(currentCommandString);
            if(match.Success){
                return match.Groups[1].Value;
            }else if (matchJump.Success){
                return matchJump.Groups[1].Value;
            }else{
                return null;
            }
        }else{
            return null;
        }
    }

    public string Jump()
    {
        List<string> validCommands = new List<string>();
        validCommands.Add("C_COMMAND");
        if(validCommands.Any(s=> s == CommandType())){
            var regex = new Regex(@";([0-9A-Za-z]+)");
            var match = regex.Match(currentCommandString);
            if(match.Success){
                return match.Groups[1].Value;
            }else{
                return null;
            }
        }else{
            return null;
        }
    }

    public void ParseForSymbols()
    {
        bool firstRun=true;

        while((firstRun && currentCommandString == null) || (!firstRun && currentCommandString != null)){
            Advance();
            firstRun=false;
            if(currentCommandString != null){
                string dest = Dest();
                string symbol = Symbol();
                string comp = Comp();
                string jump = Jump();
                string currentCommandType = CommandType();
                if(currentCommandType == "A_COMMAND"){
                    ++programCounter;
                }else if(currentCommandType == "C_COMMAND"){
                    ++programCounter;
                }else if(currentCommandType == "L_COMMAND"){
                    symbolTable.AddEntry(symbol, programCounter);
                }
            }
        }
    }

    public IEnumerable<string> Parse()
    {
        Code codeGenerator = new Code();
        bool firstRun=true;

        while((firstRun && currentCommandString == null) || (!firstRun && currentCommandString != null)){
            Advance();
            firstRun=false;
            if(currentCommandString != null){
                string dest = Dest();
                string symbol = Symbol();
                string comp = Comp();
                string jump = Jump();
                string currentCommandType = CommandType();
                if(currentCommandType == "A_COMMAND"){
                    if(!IsDigitsOnly(symbol)){
                        if(!symbolTable.Contains(symbol)){
                            int RAMAddr = symbolTable.GetUsableRAMLocation();
                            symbolTable.AddEntry(symbol, RAMAddr);
                            yield return codeGenerator.Ainstruction(RAMAddr);
                        }else{
                            yield return codeGenerator.Ainstruction(symbolTable.GetAddress(symbol));
                        }
                    }else{
                        yield return codeGenerator.Ainstruction(Int16.Parse(symbol));
                    }
                }else if(currentCommandType == "C_COMMAND"){
                    string cInstruction = codeGenerator.Comp(comp);
                    string dInstruction = codeGenerator.Dest(dest);
                    string jInstruction = codeGenerator.Jump(jump);
                    string totalInstruction = cInstruction + dInstruction + jInstruction;
                    yield return totalInstruction.PadLeft(16, '1');
                }
            }
        }
    }
}
