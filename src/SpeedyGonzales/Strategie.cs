using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyGonzales
{
    public class Strategie
    {
        public record State(
            GameState InitialState,
            Move Move,
            int Depth)
        {
            private GameState? _stateAfterMove;
            private Score? _cachedScoreForSpelerAanZet;
            private State[]? _nextStates;
            private State? _bestSubMove;

            public GameState StateAfterMove
            {
                get
                {
                    if (_stateAfterMove == null)
                    {
                        _stateAfterMove = InitialState.Play(Move);
                    }
                    return _stateAfterMove;
                }
            }

            public Score ScoreForSpelerAanZet
            {
                get
                {
                    if (_cachedScoreForSpelerAanZet == null)
                    {
                        _cachedScoreForSpelerAanZet = StateAfterMove.GetScore(InitialState.SpelerAanZet);
                    }
                    return _cachedScoreForSpelerAanZet.Value;
                }
            }

            public Score ScoreForUs
                => (InitialState.Wij == InitialState.SpelerAanZet)
                ? ScoreForSpelerAanZet
                : ScoreForSpelerAanZet.Inverse();

            public IEnumerable<State> TakeInNextLevel()
            {
                if (_nextStates == null)
                {
                    _nextStates = StateAfterMove.Moves
                        .Select(m => new State(StateAfterMove, m, Depth + 1))
                        .ToArray();

                    _bestSubMove = _nextStates
                        .OrderByDescending(x => x.ScoreForSpelerAanZet.AsNumber)
                        .FirstOrDefault();
                }
                return _nextStates;
            }

            public int ScoreForUsAsCombinedNumber
            {
                get 
                {
                    if (_bestSubMove == null) 
                    {
                        return ScoreForUs.AsNumber;
                    }
                    return (ScoreForUs.AsNumber * 3 + _bestSubMove.ScoreForUsAsCombinedNumber * 2) / 5;
                }
            }
        }

        public class Walker
        {
            private Queue<State> _backLog = new();

            public Walker(IEnumerable<State> states)
            {
                _backLog = new(states);
            }
            
            public void Walk(int maxDepth, TimeSpan maxDuration)
            {
                var sw = Stopwatch.StartNew();
                while (_backLog.TryDequeue(out var nextState))
                {
                    foreach (var subState in nextState.TakeInNextLevel())
                    {
                        if (nextState.Depth < maxDepth)
                        {
                            _backLog.Enqueue(subState);
                        }
                    }
                    if (sw.Elapsed > maxDuration)
                    {
                        break;
                    }
                }
            }
        }

        public Move? GetBestMove(GameState gameState)
        {
            var initialStates = gameState.Moves
                .Select(m => new State(gameState, m, 0))
                .ToArray();

            var walker = new Walker(initialStates);
            walker.Walk(3, TimeSpan.FromMilliseconds(90));

            return initialStates
                .OrderByDescending(x => x.ScoreForUsAsCombinedNumber)
                .FirstOrDefault()
                ?.Move;
        }
    }
}
