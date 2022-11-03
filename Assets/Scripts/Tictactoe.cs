using System;
using System.Collections.Generic;
using UnityEngine;

public class Tictactoe
{
    private static Tictactoe instancce;

    public static Tictactoe Instance
    {
        get
        {
            if (instancce == null)
            {
                instancce = new Tictactoe();
            }
            return instancce;
        }
    }
    
    public const int Player = 1;
    public const int Robot = 2; 
    public int[] pRows;
    public int[] pCols;
    public int[] rRows;
    public int[] rCols;
    public int[] hills;
    public int[] dales;
    public int n;
    public int remain;
    public int[] boards;
    public Stack<int> commands;

    public Tictactoe()
    {
        commands = new Stack<int>();
        pRows = new int[Global.MaxSize];
        pCols = new int[Global.MaxSize];
        rRows = new int[Global.MaxSize];
        rCols = new int[Global.MaxSize];
        hills = new int[2];
        dales = new int[2];
        boards = new int[Global.MaxSize * Global.MaxSize];
    }

    public void Reset(int size)
    {
        n = size;
        remain = n * n;
        Array.Clear(pRows, 0, pRows.Length);
        Array.Clear(pCols, 0, pCols.Length);
        Array.Clear(rRows, 0, rRows.Length);
        Array.Clear(rCols, 0, rCols.Length);
        Array.Clear(hills, 0, hills.Length);
        Array.Clear(dales, 0, dales.Length);
        Array.Clear(boards, 0, boards.Length);
        commands.Clear();
    }
    
    public int Move(int row, int col, int player)
    {
        int[] rows = player == Player ? pRows : rRows;
        int[] cols = player == Player ? pCols : rCols;
        rows[row]++;
        cols[col]++;
        int hillIndex = player == Player ? 0 : 1;
        int daleIndex = player == Player ? 0 : 1;
        if (row == col)
        {
            hills[hillIndex]++;
        }
        if (row + col == n - 1)
        {
            dales[daleIndex]++;
        }
        remain--;
        boards[row * n + col] = player;
        commands.Push((row<<n)+col);
        bool gameOver = rows[row] == n || cols[col] == n || (row == col && hills[hillIndex] == n) ||
                        (row + col == n - 1 && dales[daleIndex] == n);
        return gameOver ? player : 0;
    }

    public bool CanPlace(int row, int col)
    {
        return boards[row * n + col] == 0;
    }

    public bool IsFull()
    {
        return remain == 0;
    }

    public bool IsEmpty()
    {
        return remain == n * n;
    }
    
    public Vector2Int Last()
    {
        var v = commands.Peek();
        int row = v >> n;
        int col = v & ((1 << n) - 1);
        return new Vector2Int(col, row);
    }
    
    public int PlayerRound(int row, int col)
    {
        return Move(row, col, Player);
    }
    
    public int RobotRound()
    {
        int rRow, rCol, pRow, pCol;
        rRow = rCol = pRow = pCol = -1;
        int maxRobot, maxPlayer;
        maxRobot = maxPlayer = 0;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (boards[i * n + j] == 0)
                {
                    if (rRow == -1)
                    {
                        rRow = pRow = i;
                        rCol = pCol = j;
                    }
                    if (pRows[i] <= pRows[pRow] && rRows[i] > maxRobot)
                    {
                        rRow = i;
                        rCol = j;
                        maxRobot = rRows[i];
                    }
                    if (pCols[j] <= pCols[rCol] && rCols[j] > maxRobot)
                    {
                        rRow = i;
                        rCol = j;
                        maxRobot = rCols[j];
                    }
                    if (rRows[i] <= rRows[pRow] && pRows[i] > maxPlayer)
                    {
                        pRow = i;
                        pCol = j;
                        maxPlayer = pRows[i];
                    }
                    if (rCols[j] <= rCols[pCol] && pCols[j] > maxPlayer)
                    {
                        pCol = j;
                        pRow = i;
                        maxPlayer = pCols[j];
                    }
                    if (i == j)
                    {
                        if (hills[0]==0 && hills[1] > maxRobot)
                        {
                            rRow = i;
                            rCol = j;
                            maxRobot = hills[1];
                        }
                        if (hills[1]==0 && hills[0] > maxPlayer)
                        {
                            pRow = i;
                            pCol = j;
                            maxPlayer = hills[0];
                        }
                    }
                    if (i + j == n - 1)
                    {
                        if (dales[0] == 0 && dales[1] > maxRobot)
                        {
                            rRow = i;
                            rCol = j;
                            maxRobot = dales[1];
                        }
                        if (dales[1]==0 && dales[0] > maxPlayer)
                        {
                            pRow = i;
                            pCol = j;
                            maxPlayer = dales[0];
                        }
                    }
                }
            }
        }

        if (maxPlayer > maxRobot)
        {
            return Move(pRow, pCol, Robot);
        }
        else
        {
            return Move(rRow, rCol, Robot);
        }
    }

    public bool CanRollBack()
    {
        if (IsEmpty()) return false;
        return commands.Count > 0 && commands.Count % 2 == 0;
    }
    
    public void RollBack()
    {
        if (commands.Count > 0)
        {
            int v = commands.Pop();
            int row = v >> n;
            int col = v & ((1 << n) - 1);
            int player = boards[row * n + col];
            int[] rows = player == Player ? pRows : rRows;
            int[] cols = player == Player ? pCols : rCols;
            rows[row]--;
            cols[col]--;
            int hillIndex = player == Player ? 0 : 1;
            int daleIndex = player == Player ? 0 : 1;
            if (row == col)
            {
                hills[hillIndex]--;
            }
            if (row + col == n - 1)
            {
                dales[daleIndex]--;
            }
            remain++;
            boards[row * n + col] = 0;
        }
    }
}