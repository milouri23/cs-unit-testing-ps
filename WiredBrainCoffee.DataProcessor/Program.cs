using System;
using System.IO;
using WiredBrainCoffee.DataProcessor.Model;
using WiredBrainCoffee.DataProcessor.Parsing;
using WiredBrainCoffee.DataProcessor.Processing;

Console.WriteLine("---------------------------------------");
Console.WriteLine("  Wired Brain Coffee - Data Processor  ");
Console.WriteLine("---------------------------------------");
Console.WriteLine();

const string fileName = "CoffeeMachineData.csv";
string baseDirectory = AppContext.BaseDirectory;

string fullPath = Path.Combine(baseDirectory, fileName);

// Se usa sync ReadAllLines conscientemente: Script simple, único usuario, impacto de bloqueo negligible.
#pragma warning disable S6966 // Awaitable method should be used
string[] csvLines = File.ReadAllLines(fullPath);
#pragma warning restore S6966 // Awaitable method should be used

MachineDataItem[] machineDataItems = CsvLineParser.Parse(csvLines);

var machineDataProcessor = new MachineDataProcessor();
machineDataProcessor.ProcessItems(machineDataItems);

Console.WriteLine();
Console.WriteLine($"File {fileName} was successfully processed!");

Console.ReadLine();