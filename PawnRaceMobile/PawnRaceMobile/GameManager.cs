﻿using PawnRaceMobile.Core;
using System;
using System.Collections.Generic;

namespace PawnRaceMobile
{
    internal class GameManager
    {
        private Game m_Game;
        public Board Board => m_Game.Board;
        public IPlayer CurrentPlayer => m_Game.CurrentPlayer;

        public event Action MoveMade;

        public GameManager(char whiteGap, char blackGap
            , Player player1, Player player2)
        {
            m_Game = new Game(whiteGap, blackGap, player1, player2);
            CurrentPlayer.MoveProduced += SelectMove;
            CurrentPlayer.TakeTurn();
        }

        public bool IsValidMove(Move move)
        {
            IList<Move> moves = CurrentPlayer.GetAvailableMovesForPawn(move.From);
            foreach (Move m in moves)
            {
                if (m.Equals(move))
                {
                    return true;
                }
            }

            return false;
        }

        public void SelectMove(IPlayer player, Move move)
        {
            if (CurrentPlayer == player && !m_Game.IsFinished)
            {
                CurrentPlayer.MoveProduced -= SelectMove;
                m_Game.ApplyMove(move);
                MoveMade?.Invoke();
                CurrentPlayer.MoveProduced += SelectMove;
                CurrentPlayer.TakeTurn();
            }
        }
    }
}