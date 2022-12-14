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
            var onzeKaarten = Kaarten.Where(x => x.Owner != null && x.Owner == SpelerAanZet).ToArray();
            var hunKaarten = Kaarten.Where(x => x.Owner != null && x.Owner != SpelerAanZet).ToArray();
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

        public void WriteTo(Action<string> writeLine)
        {
            Bord.WriteTo(writeLine);
            foreach (var kaart in Kaarten)
            {
                writeLine(kaart.ToString());
            }
            writeLine(Moves.Length.ToString());
            foreach (var move in Moves)
            {
                writeLine(move.ToString());
            }
        }

        public Score GetScore(Team voorTeam)
        {
            var hullie = voorTeam.TegenStander;
            var onzeRenners = Bord.GetRennerPosities(voorTeam).ToArray();
            var hunRenners = Bord.GetRennerPosities(hullie).ToArray();

            var rennerOpOnzeWinstPositie = Bord[voorTeam.WinPositie].Renner;
            var rennerOpHunWinstPositie = Bord[hullie.WinPositie].Renner;
            var winst = (rennerOpOnzeWinstPositie != null && rennerOpOnzeWinstPositie.IsLeader && rennerOpOnzeWinstPositie.Team == voorTeam);
            var verlies = (rennerOpHunWinstPositie != null && rennerOpHunWinstPositie.IsLeader && rennerOpHunWinstPositie.Team != voorTeam);

            if (!onzeRenners.Any(p => Bord[p].Renner!.IsLeader))
            {
                winst = false;
                verlies = true;
            }
            if (!hunRenners.Any(p => Bord[p].Renner!.IsLeader))
            {
                winst = true;
            }

            return new Score(
                Winst: winst,
                Verlies: verlies,
                Renners: onzeRenners.Length,
                Tegenstanders: hunRenners.Length);
        }
    }

    public record struct Score(
        bool Winst,
        bool Verlies,
        int Renners,
        int Tegenstanders) 
    {
        public bool IsGameOver => Winst || Verlies;

        public int AsNumber
        {
            get
            {
                if (Winst)
                {
                    return 10000;
                }
                if (Verlies)
                {
                    return -10000;
                }
                return Renners * 2 - Tegenstanders;
            }
        }

        public Score Inverse()
            => new Score(
                Winst: this.Verlies,
                Verlies: this.Winst,
                Renners: this.Tegenstanders,
                Tegenstanders: this.Renners);
    }
}
