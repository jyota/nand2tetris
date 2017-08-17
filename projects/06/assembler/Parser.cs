using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
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
                Console.WriteLine(currentCommandString);
            }
        }
    }
}
