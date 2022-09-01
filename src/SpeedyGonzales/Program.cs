// See https://aka.ms/new-console-template for more information
using SpeedyGonzales;
using System;
using System.Diagnostics;
using System.Linq;

var computerName = Environment.GetEnvironmentVariable("COMPUTERNAME") ?? "";
if (computerName == "SURFACEOFFLORIS")
{
    Debugger.Launch();
}

string InputLine()
{
    var l = Console.ReadLine();
    // Console.Error.WriteLine($"INPUT: {l}");
    return l ?? throw new InvalidOperationException("End of input");
}

void ReturnLine(string output)
{
    // Console.Error.WriteLine($"OUTPUT: {output}");
    Console.WriteLine(output);
}

var wijInput = InputLine();
var wij = Team.Parse(wijInput);

while (true)
{
    var gameState = GameState.Parse(wij, InputLine);
    if (gameState.Moves.Length == 0)
    {
        ReturnLine("PASS"); // Bump
    }
    else
    {
        var move = gameState.Moves[0];
        var newGameState = gameState.Play(move);
        newGameState.WriteTo(Console.Error.WriteLine);
        ReturnLine(move.ToString());
    }
}