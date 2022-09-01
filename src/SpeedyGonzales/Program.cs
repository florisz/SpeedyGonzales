// See https://aka.ms/new-console-template for more information
using SpeedyGonzales;
using System.Diagnostics;

Debugger.Launch();

string InputLine()
{
    var l = Console.ReadLine();
    Console.Error.WriteLine($"INPUT: {l}");
    return l;
}

var bordInput = Enumerable.Range(0, 5)
    .Select(_ => InputLine())
    .ToArray();
var cards = Enumerable.Range(0, 5)
    .Select(_ => InputLine())
    .ToArray();

var board = Bord.Parse(bordInput);
Console.WriteLine("Hello, World!");