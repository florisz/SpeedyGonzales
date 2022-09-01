// See https://aka.ms/new-console-template for more information
using SpeedyGonzales;
using System;
using System.Diagnostics;
using System.Linq;

Debugger.Launch();

string InputLine()
{
    var l = Console.ReadLine();
    Console.Error.WriteLine($"INPUT: {l}");
    return l ?? throw new InvalidOperationException("End of input");
}

void ReturnLine(string output)
{
    Console.Error.WriteLine($"OUTPUT: {output}");
    Console.WriteLine(output);
}

var wijInput = InputLine();
var wij = Team.Parse(wijInput);

while (true)
{
    var bordInput = Enumerable.Range(0, 5)
        .Select(_ => InputLine())
        .ToArray();
    var kaartInput = Enumerable.Range(0, 5)
        .Select(_ => InputLine())
        .ToArray();
    var aantalOptiesInput = InputLine();
    int aantalOpties = int.Parse(aantalOptiesInput);
    var optiesInput = Enumerable.Range(0, aantalOpties)
        .Select(_ => InputLine())
        .ToArray();

    var moves = optiesInput
        .Select(Move.Parse)
        .ToArray();

    var bord = Bord.Parse(bordInput);
    var kaarten = kaartInput
        .Select(Kaart.Parse)
        .ToArray();

    ReturnLine(moves[0].ToString());
}