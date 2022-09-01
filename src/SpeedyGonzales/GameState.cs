using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyGonzales
{
    public record GameState(
        Team Wij,
        Team SpelerAanZet,
        Bord Bord,
        Kaart[] Kaarten,
        Move[] Moves)
    {
        public Team Hullie => Wij.TegenStander;

        //public GameState Play(Move move)
        //{
        //    var onzeKaarten = Kaarten.Where(x => x.Owner != null && x.Owner == Wij).ToArray();
        //    var hunKaarten = Kaarten.Where(x => x.Owner != null && x.Owner != Wij).ToArray();
        //    var stackKaart = Kaarten.First(x => x.Owner == null);

        //    var onzeKaart = onzeKaarten
        //        .FirstOrDefault(x => x.Id == move.KaartId)
        //        ?? throw new InvalidOperationException("Kaart is niet beschikbaar");
        //    var onzeAndereKaart = onzeKaarten
        //        .Where(x => x.Id != move.KaartId)
        //        .First();
        //    var newBord = Bord.Clone();
        //    var van = newBord[move.Van];
        //    var naar = newBord[move.Naar];
        //    if (van.Renner == null || van.Renner.Team != Wij)
        //    {
        //        throw new InvalidOperationException($"Wij zijn {Wij}, en proberen move {move}, maar het bord is {Bord}");
        //    }
        //    naar.Renner = van.Renner;
        //    van.Renner = null;

        //    onzeKaart = onzeKaart.ChangeOwner(null);
        //    stackKaart = stackKaart.ChangeOwner(Wij);
        //    return new GameState(
        //        Wij,
        //        Hullie,
        //        newBord,
        //        xxx,
        //        xxx);
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
                wij,
                bord,
                kaarten,
                moves);
        }
    }
}
