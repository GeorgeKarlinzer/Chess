﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.Logic.PieceColor;
using static Chess.Logic.Positions;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Chess.Logic.Moves;
using Chess.Logic.Pieces;
using System.Drawing;
using System.Xml.Serialization;

namespace Chess.Logic
{
    internal class Board
    {
        private int currentPieceId;
        private Dictionary<string, int> repeatPositionMap;

        public PieceColor CurrentPlayer { get; private set; }

        public List<Move> Moves { get; }
        public Dictionary<Vector2, Piece> PiecesMap { get; }

        public bool IsCheck { get; private set; }
        public bool IsEnd { get; private set; }

        public Board()
        {
            currentPieceId = 0;
            repeatPositionMap = new();
            PiecesMap = new();
            Moves = new();
            CurrentPlayer = White;

            GeneratePieces();

            CalculateAvailibleMoves();
        }

        public void MovePiece(Piece piece, Vector2 targetPos, Piece attackedPiece = null)
        {
            PiecesMap.Remove(piece.Position);

            if (attackedPiece is not null)
                PiecesMap.Remove(attackedPiece.Position);

            PiecesMap[targetPos] = piece;
            piece.Position = targetPos;
            piece.IsMoved = true;
        }

        public void MakeMove(Move move)
        {
            CurrentPlayer = CurrentPlayer == White ? Black : White;

            move.MakeMove();

            CalculateAvailibleMoves();

            IsEnd = !PiecesMap.Values.Where(x => x.Color == CurrentPlayer)
                .Any(x => x.PossibleMoves.Count > 0);
        }

        public void CalculateFenCode()
        {
            var str = "";
            for (int y = 7; y >= 0; y--)
            {
                var emptyCells = 0;
                for (int x = 0; x < 8; x++)
                {
                    if (PiecesMap.TryGetValue(new(x, y), out var piece))
                    {
                        str += piece.FENCode;

                        if (emptyCells != 0)
                            str += emptyCells.ToString();

                        emptyCells = 0;
                    }
                    else
                        emptyCells++;
                }
                if (emptyCells != 0)
                    str += emptyCells.ToString();

                str += "/";
            }

            if (!repeatPositionMap.ContainsKey(str))
                repeatPositionMap[str] = 0;
            else
                repeatPositionMap[str]++;
        }

        #region Moves calculating

        public void CalculateAvailibleMoves()
        {
            PiecesMap.Values.Foreach(x => x.CalculateMoves());

            var king = PiecesMap.Values
                .First(x => x.GetType() == typeof(King) && x.Color == CurrentPlayer);

            var kingPos = king.Position;

            foreach (var enemyPiece in PiecesMap.Values.Where(x => x.Color != CurrentPlayer))
            {
                var type = enemyPiece.GetType();
                var enemyPos = enemyPiece.Position;
                var deltaPos = kingPos - enemyPos;

                if ((type != typeof(Bishop) && type != typeof(Rook) && type != typeof(Queen))
                    || !enemyPos.IsSameLine(kingPos))
                    continue;

                Piece coverPiece = null;

                deltaPos = deltaPos.ToUnitDirection();

                var step = enemyPos;
                while ((step += deltaPos) != kingPos)
                {
                    if (PiecesMap.TryGetValue(step, out var p))
                    {
                        if (p.Color == enemyPiece.Color || coverPiece != null)
                        {
                            coverPiece = null;
                            break;
                        }
                        else
                        {
                            coverPiece = p;
                        }
                    }
                }

                if (coverPiece != null)
                {
                    coverPiece.PossibleMoves
                        .Where(x => !x.TargetPos.IsBetween(kingPos, enemyPos) && x.TargetPos != enemyPos)
                        .ToList()
                        .Foreach(x => coverPiece.PossibleMoves.Remove(x));
                }
            }

            var attackedCells = PiecesMap.Values
                .Where(x => x.Color != CurrentPlayer)
                .SelectMany(x => x.PossibleAttacks)
                .Distinct()
                .ToList();

            king.PossibleMoves.Where(x => attackedCells.Contains(x.TargetPos))
                .ToList()
                .Foreach(x => king.PossibleMoves.Remove(x));

            var kingCastle = king.PossibleMoves.FirstOrDefault(x => x.GetType() == typeof(KingCastle));
            var queenCastle = king.PossibleMoves.FirstOrDefault(x => x.GetType() == typeof(QueenCastle));

            var breakKingCastlePoss = king.Color == White ? new[] { E1, F1 } : new[] { E8, F8 };
            var breakQueenCastlePoss = king.Color == White ? new[] { E1, D1 } : new[] { E8, D8 };

            if (kingCastle is not null && attackedCells.Any(x => breakKingCastlePoss.Contains(x)))
            {
                king.PossibleMoves.Remove(kingCastle);
            }

            if (queenCastle is not null && attackedCells.Any(x => breakQueenCastlePoss.Contains(x)))
            {
                king.PossibleMoves.Remove(queenCastle);
            }

            var attackedPieces = PiecesMap.Values
                .Where(x => x.Color != CurrentPlayer && x.KingAttacks.Count != 0)
                .ToList();

            IsCheck = attackedPieces.Any();

            if (attackedPieces.Count == 1)
            {
                var attackedPiece = attackedPieces.First();

                foreach (var piece in PiecesMap.Values.Where(x => x.Color == CurrentPlayer && x != king))
                {
                    piece.PossibleMoves.Where(x => !attackedPiece.KingAttacks.Contains(x.TargetPos))
                        .ToList()
                        .Foreach(x => piece.PossibleMoves.Remove(x));
                }
            }

        }

        public bool CanMove(Vector2 targetPos)
        {
            return targetPos.IsValidChessPos() && !PiecesMap.ContainsKey(targetPos);
        }

        public bool CanBeat(Vector2 targetPos, PieceColor color, out Piece attackedPiece)
        {
            var canBeat = PiecesMap.TryGetValue(targetPos, out var piece) && piece.Color != color;
            attackedPiece = canBeat ? piece : null;
            return canBeat;
        }
        #endregion

        #region Generating
        private void GeneratePieces()
        {
            var pawnPos = new[] { A2, B2, C2, D2, E2, F2, G2, H2 };

            foreach (var pos in pawnPos)
            {
                AddPiece<Pawn>(White, pos);
                AddPiece<Pawn>(Black, new(pos.X, pos.Y + 5));
            }

            AddPiece<Rook>(White, A1);
            AddPiece<Rook>(White, H1);
            AddPiece<Rook>(Black, A8);
            AddPiece<Rook>(Black, H8);

            AddPiece<Knight>(White, B1);
            AddPiece<Knight>(White, G1);
            AddPiece<Knight>(Black, B8);
            AddPiece<Knight>(Black, G8);

            AddPiece<Bishop>(White, C1);
            AddPiece<Bishop>(White, F1);
            AddPiece<Bishop>(Black, C8);
            AddPiece<Bishop>(Black, F8);

            AddPiece<Queen>(White, D1);
            AddPiece<Queen>(Black, D8);

            AddPiece<King>(White, E1);
            AddPiece<King>(Black, E8);
        }

        public T AddPiece<T>(PieceColor color, Vector2 position) where T : Piece
        {
            var piece = (T)Activator.CreateInstance(typeof(T), new object[] { color, position, currentPieceId++, this });
            PiecesMap[piece.Position] = piece;

            return piece;
        }
        #endregion
    }
}

// TODO: Add pawn metomorphosis
// TDOD: Add Timer
// TODO: Handle end of the game