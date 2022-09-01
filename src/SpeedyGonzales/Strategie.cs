using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyGonzales
{
    public class Strategie
    {
        private int? ScoreMove(GameState gameState, Move move, int recurseLeft)
        {
            var newState = gameState.Play(move);
            var score = newState.GetScore(gameState.SpelerAanZet);

            if (recurseLeft == 0 || score.IsGameOver)
            {
                return score.AsNumber;
            }
            var bestMoveAfter = GetBestMove(newState, recurseLeft - 1);
            if (bestMoveAfter == null)
            {
                return null;
            }
            return (score.AsNumber * 3 + bestMoveAfter.Value.Item2) / 4; 
        }

        Random _rnd = new Random();

        private (Move, int)? GetBestMove(GameState gameState, int recurseLeft)
        {
            var moveResults = gameState.Moves
                .Select(move => new
                {
                    Move = move,
                    Score = ScoreMove(gameState, move, recurseLeft),
                    Random = _rnd.Next()
                })
                .Where(x => x.Score != null)
                .OrderByDescending(x => x.Score).ThenBy(x => x.Random)
                .ToList();
            return moveResults
                .Select(x => (x.Move, x.Score!.Value))
                .FirstOrDefault();
        }

        public Move? GetBestMove(GameState gameState)
        {
            var bestMove = GetBestMove(gameState, 3);
            return bestMove?.Item1;
        }
    }
}
