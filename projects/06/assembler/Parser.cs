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
        while((line = fileToRead.ReadLine()) != null)
        {
            Console.WriteLine(line);
        }
        fileToRead.Close();        
    }
}
