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
    public Parser(System.IO.StreamReader file)
    {
        fileToRead = file;
        var cmdsEnumerator = GetCommands();
        commandsSet = cmdsEnumerator.GetEnumerator();
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
    public bool HasMoreCommands()
    {
        return(commandsSet.MoveNext());
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
            var regex = new Regex(@"([A-Za-z])=");
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
            var regex = new Regex(@"=([0-9A-Za-z\-\+]+)|([0-9A-Za-z\-\+]+);");
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
    public void Advance()
    {
        if(HasMoreCommands()){
            currentCommandString = commandsSet.Current;
        }else{
            currentCommandString = null;
        }
    }

    public void Parse()
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
                string builder = currentCommandString + " -- ";
                if(!(dest == null)){
                    builder += " -- " + dest;
                }
                if(!(symbol == null)){
                    builder += " -- " + symbol;
                }
                if(!(comp == null)){
                    builder += " -- " + comp;
                }
                if(!(jump == null)){
                    builder += " -- " + jump;
                }
                Console.WriteLine(builder);
            }
        }
    }
}
