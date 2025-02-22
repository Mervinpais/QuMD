﻿namespace QuMD_Converter;

class Program
{
    static (string, string) SplitIntoInputAndOutput(string[] args) {
        int splitter = args.ToList().IndexOf("-o");
        string inputFile = string.Join(" ", args[0..splitter]);
        string outputFile = string.Join(" ", args[splitter..^1]);
        return (inputFile, outputFile);
    }
    static void Main(string[] args)
    {
        if (args.Length > 0) {
            if (args.Contains("-o")) {
                SplitIntoInputAndOutput(args);
            }
        }
        else {
            bool exit = false;
            while (!exit) {
                Console.Write("\nEnter file path to convert back to Markdown (h for help)> ");
                string? input = Console.ReadLine();
                switch (input) {
                    case "":
                        continue;
                    case "q":
                        exit = true;
                        break;
                    case "h":
                        break;
                    case string i when input.Contains(" -o "):
                        (string inputFile, string outputFile) = SplitIntoInputAndOutput(input.Split(" "));
                        Convert(inputFile, outputFile);
                        break;
                    case string i when File.Exists(input) && !input.Contains(" -o "):
                        Convert(input);
                        break;
                    default:
                        Console.WriteLine("Command does not exist!!!");
                        break;
                }
            }
            Console.WriteLine("Program exiting...");
                Console.WriteLine("Exit code: " + Environment.ExitCode); //debug purposes
        }
    }

    static void Convert(string fileName, string outputFile = "") {

        if (!File.Exists(fileName)) throw new FileNotFoundException($"\'{fileName}\' is not found!");

        string baseFileName = fileName.Split("/")[^1];
        if (outputFile == "") {
            if (OperatingSystem.IsLinux()) {
                outputFile = $"~/Downloads/{baseFileName}.md";
            }
            else {
                string docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                outputFile = $"{docsPath}{baseFileName}";
            }
        }

        List<string> outputFileLines = new List<string>();

        foreach (string line in File.ReadAllLines(fileName)) {
            char[] beginningCode = new char[4]; //xxxx abcdefg (first 4 chars)
            beginningCode = line[0..4].ToCharArray(); //shit ahh code
            int index = 0;
            char[] beginningCode_copy = beginningCode;
            foreach (char a in beginningCode_copy) {
                if (a == ' ') {
                    beginningCode = beginningCode_copy[0..index];
                    break;
                }
                index++;
            }
            string newLine = line.Substring(index).TrimStart(); //fixed ahh code
            outputFileLines.Add(newLine);
        }

        foreach (string ln in outputFileLines) {
            Console.WriteLine(ln);
        }
        Console.WriteLine(outputFile);
        File.WriteAllLines(outputFile, outputFileLines);
    }
}