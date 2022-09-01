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
        public GameState Play(Move move)
        {
            var onzeKaarten = Kaarten.Where(x => x.Owner != null && x.Owner == Wij).ToArray();
            var hunKaarten = Kaarten.Where(x => x.Owner != null && x.Owner != Wij).ToArray();
            var stackKaart = Kaarten.First(x => x.Owner == null);

            var speelKaart = onzeKaarten
                .FirstOrDefault(x => x.Id == move.KaartId)
                ?? throw new InvalidOperationException("Kaart is niet beschikbaar");
            var onzeAndereKaart = onzeKaarten
                .Where(x => x.Id != move.KaartId)
                .First();
            var newBord = Bord.Clone();
            var van = newBord[move.Van];
            var naar = newBord[move.Naar];
            if (van.Renner == null || van.Renner.Team != SpelerAanZet)
            {
                throw new InvalidOperationException($"Speler aan zet is {SpelerAanZet}, en proberen move {move}, maar het bord is {Bord}");
            }
            naar.Renner = van.Renner;
            van.Renner = null;

            speelKaart = speelKaart.MoveToStack();
            stackKaart = stackKaart.ChangeOwner(SpelerAanZet);
            var volgendeSpeler = SpelerAanZet.TegenStander;
            var kaarten = new[]
                {
                    stackKaart,
                    speelKaart,
                    onzeAndereKaart,
                    hunKaarten[0],
                    hunKaarten[1]
                };
            var positiesVanSpeler = newBord.GetRennerPosities(volgendeSpeler);
            var moves = hunKaarten
                .SelectMany(k => positiesVanSpeler.SelectMany(
                    spelerPos => MogelijkeMoves(newBord, spelerPos, k)))
                .ToArray();
            return new GameState(
                Wij,
                volgendeSpeler,
                newBord,
                kaarten,
                moves);
        }

        private IEnumerable<Move> MogelijkeMoves(Bord bord, Positie positieRenner, Kaart kaart)
        {
            foreach (var vector in kaart.Bewegingen)
            {
                var nieuwePos = positieRenner.Add(vector);
                if (nieuwePos.IsOnBoard)
                {
                    var targetVeld = bord[nieuwePos];
                    if (targetVeld.Renner == null ||
                        targetVeld.Renner.Team != kaart.Owner)
                    {
                        yield return new Move(kaart.Id, positieRenner, nieuwePos);
                    }
                }
            }
        }

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
