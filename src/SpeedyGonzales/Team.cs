using System;

namespace SpeedyGonzales
{
    public record Team(
        string Naam)
    {
        public static readonly Team A = new Team("A");
        public static readonly Team B = new Team("B");


        public static Team Parse(string input)
            => input switch
            {
                "0" => A,
                "1" => B,
                _ => throw new InvalidOperationException($"Ongeldig team {input}")
            };
    };
}
