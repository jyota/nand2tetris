using System;

class Parser
{
    private System.IO.StreamReader fileToRead;
    public Parser(System.IO.StreamReader file)
    {
        fileToRead = file;
    }

    public void Parse()
    {
        String line;
        while(!fileToRead.EndOfStream)
        {
            line = fileToRead.ReadLine();
            Console.WriteLine(line);
        }
        fileToRead.Close();        
    }
}
