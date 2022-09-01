using System;

namespace SpeedyGonzales
{
    public record Team(
        string Naam,
        int Number,
        Positie WinPositie)
    {
        public static readonly Team A = new Team("A", 0, new Positie(2, 4));
        public static readonly Team B = new Team("B", 1, new Positie(2, 0));

        public Team TegenStander
            => this == A
            ? B
            : A;

        public static Team Parse(string input)
            => input switch
            {
                "0" => A,
                "1" => B,
                _ => throw new InvalidOperationException($"Ongeldig team {input}")
            };
    }}
