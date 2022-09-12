using System;
using System.Collections.Generic;

public class TTTSolver
{
            public static bool IsDraw(int[][] board)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (board[i][j] == 0) return false;
                    }
                }
                return true;
            }
            public static List<int[]> GetAvailableMove(int[][] board)
            {
                List<int[]> moves = new List<int[]>();
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (board[j][i] == 0)
                        {
                            moves.Add(new int[] { j, i });
                        }
                    }
                }
                return moves;
            }
            public static bool IsWinner(int[][] board, int ID)
            {
                if ((board[0][0] == ID && board[0][1] == ID && board[0][2] == ID) ||
                   (board[1][0] == ID && board[1][1] == ID && board[1][2] == ID) ||
                   (board[2][0] == ID && board[2][1] == ID && board[2][2] == ID) ||

                   (board[0][0] == ID && board[1][0] == ID && board[2][0] == ID) ||
                   (board[0][1] == ID && board[1][1] == ID && board[2][1] == ID) ||
                   (board[0][2] == ID && board[1][2] == ID && board[2][2] == ID) ||

                   (board[0][0] == ID && board[1][1] == ID && board[2][2] == ID) ||
                   (board[2][0] == ID && board[1][1] == ID && board[0][2] == ID)) return true;

                else return false;
            }
            public static int[] TurnMethod(int[][] board, int player)
            {

                bool aiTurn = true; 
                bool opponentTurn = false;
                int aiID = player;
                int opponentID = 2;

                Dictionary<(int[][], int, int, int, bool), int> cache = new Dictionary<(int[][], int, int, int, bool), int>();

                int Minimax(int[][]board, int depth, int alpha, int beta, bool IsAITurn)
                {
   
                    if (IsWinner(board, aiID)) return 1;
                    if (IsWinner(board, opponentID)) return -1;
                    if (IsDraw(board)) return 0;
                    int bestScore = int.MinValue;
                    if (IsAITurn)
                    {
                        if (cache.ContainsKey((board, depth, alpha, beta, aiTurn)))
                        {
                          return cache[(board, depth, alpha, beta, aiTurn)];
                        }   
                        bestScore = int.MinValue;
                        foreach (int[] i in GetAvailableMove(board))
                        {
                            board[i[0]][i[1]] = aiID;
                            int score = Minimax(board, depth++, alpha, beta, opponentTurn);
                            bestScore = Math.Max(score, bestScore);
                            board[i[0]][i[1]] = 0;
                            alpha = Math.Max(alpha, score);
                            if (beta <= alpha) break;
                                                    
                        }
                        cache[(board, depth, alpha, beta, aiTurn)] = bestScore;
                        return bestScore;
                    }
                    else
                    {

                        bestScore = int.MaxValue;
                        foreach (int[] i in GetAvailableMove(board))
                        {
                            board[i[0]][i[1]] = opponentID;
                            int score = Minimax(board, depth++, alpha, beta, aiTurn);
                            bestScore = Math.Min(score, bestScore);
                            beta = Math.Min(beta, score);
                            board[i[0]][i[1]] = 0;
                            if (beta <= alpha) break;
                        }
                        return bestScore;
                    }
                    
                }

                int[] GetPlayerMove(int[][] board)
                {
                    
                    int bestScore = int.MinValue;
                    int[] move = null;
                    foreach (int[] i in GetAvailableMove(board))
                    {
                        board[i[0]][i[1]] = aiID;
                        if (IsWinner(board, aiID)) return i;
                        board[i[0]][i[1]] = 0;
                    }
                    foreach (int[] i in GetAvailableMove(board))
                    {
                        board[i[0]][i[1]] = opponentID;
                        if (IsWinner(board, opponentID)) return i;
                        board[i[0]][i[1]] = 0;
                    }
                    if(GetAvailableMove(board).Count >= 7 && board[1][1] == 0) return new int[]{1,1}; 
                    foreach (int[] i in GetAvailableMove(board))
                    {
                        board[i[0]][i[1]] = aiID;
                        int score = Minimax(board, 0, int.MinValue, int.MaxValue, opponentTurn);
                        board[i[0]][i[1]] = 0;
                        if (score > bestScore)
                        {
                            bestScore = score;
                            move = i;
                        }
                    }
                    return move;
                }
                return (GetPlayerMove(board));
