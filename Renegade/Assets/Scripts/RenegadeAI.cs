using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public static class RenegadeAI
{
    private static Vector2Int nullVector2Int = new Vector2Int(-1, -1);
    public static Vector2Int BestMove = nullVector2Int;

    //White is the Minimizing Player
    //Black is the Maximizing Player;
    public static int Minimax(Renegade currentState, int depth, int alpha, int beta, bool maximizingPlayer) {
        if (depth == 0 || currentState.GameOver) {
            return currentState.StaticEvalutation();
        }

        Vector2Int localBestMove = nullVector2Int;

        if (maximizingPlayer) {
            int maxEval = int.MinValue;
            HashSet<Vector2Int> possibleMoves = currentState.PossibleMoves;
            foreach (Vector2Int possibleMove in possibleMoves) {
                Renegade stateAfterMove = currentState.GetStateAfterMove(possibleMove);
                int eval = Minimax(stateAfterMove, depth - 1, alpha, beta, false);
                if (eval >= maxEval) {
                    maxEval = eval;
                    localBestMove = possibleMove;
                }

                alpha = Math.Max(alpha, eval);
                if (beta <= alpha) {
                    break;
                }
            }

            BestMove = localBestMove;
            return maxEval;
        }
        else {
            int minEval = int.MaxValue;
            HashSet<Vector2Int> possibleMoves = currentState.PossibleMoves;
            foreach (Vector2Int possibleMove in possibleMoves) {
                Renegade stateAfterMove = currentState.GetStateAfterMove(possibleMove);
                int eval = Minimax(stateAfterMove, depth - 1, alpha, beta, true);
                if (eval <= minEval) {
                    minEval = eval;
                    localBestMove = possibleMove;
                }

                beta = Math.Max(beta, eval);
                if (beta <= alpha) {
                    break;
                }
            }

            BestMove = localBestMove;
            return minEval;
        }
    }
}
