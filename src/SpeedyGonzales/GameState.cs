using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyGonzales
{
    public record GameState(
        Team Wij,
        Bord Bord,
        Kaart[] Kaarten,
        Move[] Moves)
    {
        //public GameState Play(Move move)
        //{
        //    var newBord = Bord.Clone();
        //    var movePlayer = this[move.Van];
        //}

        public static GameState Parse(Team wij, Func<string> inputLine)
        {
            var bordInput = Enumerable.Range(0, 5)
                .Select(_ => inputLine())
                .ToArray();
            var kaartInput = Enumerable.Range(0, 5)
                .Select(_ => inputLine())
                .ToArray();
            var aantalOptiesInput = inputLine();
            int aantalOpties = int.Parse(aantalOptiesInput);
            var optiesInput = Enumerable.Range(0, aantalOpties)
                .Select(_ => inputLine())
                .ToArray();

            var moves = optiesInput
                .Select(Move.Parse)
                .ToArray();

            var bord = Bord.Parse(bordInput);
            var kaarten = kaartInput
                .Select(Kaart.Parse)
                .ToArray();

            return new GameState(
                wij,
                bord,
                kaarten,
                moves);
        }
    }
}
