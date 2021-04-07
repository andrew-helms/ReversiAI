using GameAI.GamePlaying.Core;
using GameAI.GamePlaying.ExampleAI;
using System.Collections.Generic;

namespace GameAI.GamePlaying
{
    public class StudentAI : Behavior
    {
        // TODO: Methods go here
        public ComputerMove Run(int color, Board board, int lookAheadDepth)
        {
            return MinMax(color, board, lookAheadDepth, int.MinValue, int.MaxValue);
        }

        private ComputerMove MinMax(int color, Board board, int lookAheadDepth, int alpha, int beta)
        {
            ComputerMove bestMove = null;
            Board newState = new Board();
            List<ComputerMove> moves = new List<ComputerMove>();

            for (int row = 0; row < Board.Height; row++)
                for (int col = 0; col < Board.Width; col++)
                    if (board.IsValidMove(color, row, col))
                        moves.Add(new ComputerMove(row, col));

            foreach (ComputerMove move in moves)
            {
                newState.Copy(board);
                newState.MakeMove(color, move.row, move.column);

                if (newState.IsTerminalState() || lookAheadDepth == 0)
                    move.rank = Evaluate(newState);
                else
                    move.rank = MinMax((newState.HasAnyValidMove(color * -1) ? color * -1 : color), newState, lookAheadDepth - 1, alpha, beta).rank;

                if (bestMove == null || move.rank * color > bestMove.rank * color)
                {
                    bestMove = move;
                    if (color == 1 && bestMove.rank > alpha)
                        alpha = bestMove.rank;
                    else if (color == -1 && bestMove.rank < beta)
                        beta = bestMove.rank;
                    if (alpha >= beta)
                        break;
                }
            }

            return bestMove;
        }

        private int Evaluate(Board board)
        {
            int score = 0;

            for (int row = 0; row < Board.Height; row++)
            {
                for (int col = 0; col < Board.Width; col++)
                {
                    if (row % (Board.Height - 1) == 0 && col % (Board.Width- 1) == 0)
                        score += 100 * board.GetTile(row, col);
                    else if (row % (Board.Height - 1) == 0 || col % (Board.Width - 1) == 0)
                        score += 10 * board.GetTile(row, col);
                    else
                        score += board.GetTile(row, col);
                }
            }

            if (board.IsTerminalState())
                score += (score > 0 ? 10000 : -10000);

            return score;
        }
    }
}
