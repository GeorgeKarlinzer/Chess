using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.Logic.PieceType;
using static Chess.Logic.PieceColor;
using static Chess.Logic.PositionEnum;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;

namespace Chess.Logic
{
    public class Match
    {
        private readonly List<Move> moves;

        public Dictionary<Piece, List<PositionEnum>> AvailibleMovesMap { get; }
        public Dictionary<PositionEnum, Piece> PiecesMap { get; }

        public Match()
        {
            PiecesMap = new();
            AvailibleMovesMap = new();

            moves = new List<Move>();

            GeneratePieces();

            foreach (var piece in PiecesMap.Values)
            {
                AvailibleMovesMap[piece] = new List<PositionEnum>();
            }

            AvailibleMovesMap.Where(x => x.Key.Color == White)
                .Foreach(x => CalculateAvailibleMoves(x.Key));
        }

        public void MakeMove(Piece piece, int availibleMoveId, object parameter)
        {
            var targetPos = AvailibleMovesMap[piece][availibleMoveId];

            var move = new Move(moves.Count, piece, piece.Position, targetPos);
            moves.Add(move);

            AvailibleMovesMap.Where(x => x.Key.Color != piece.Color)
                .Foreach(x => CalculateAvailibleMoves(x.Key));
        }

        #region Moves calculating
        private void CalculateAvailibleMoves(Piece piece)
        {
            AvailibleMovesMap[piece].Clear();

            switch (piece.Type)
            {
                case Pawn: AddPawnMoves(piece); break;
                case Bishop: AddBishopMoves(piece); break;
                case Knight: AddKnightMoves(piece); break;
                case Rook: AddRookMoves(piece); break;
                case Queen: AddQueenMoves(piece); break;
                case King: AddKingMoves(piece); break;
            }

            var king = PiecesMap.Values
                .First(x => x.Type == King && x.Color == piece.Color);

            var kingPos = king.Position;

            foreach (var enemyPiece in PiecesMap.Values.Where(x => x.Color != piece.Color))
            {
                var type = enemyPiece.Type;
                var pos = enemyPiece.Position;
                var deltaPos = kingPos - pos;

                if (type != Bishop || type != Rook || type != Queen || !pos.IsSameLine(kingPos))
                    continue;

                Piece coverPiece = null;

                while ((pos += deltaPos) != kingPos)
                {
                    if (PiecesMap.TryGetValue(pos, out var p))
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
                    AvailibleMovesMap[coverPiece]
                        .Where(x => !x.IsSameLine(pos))
                        .ToList()
                        .Foreach(x => AvailibleMovesMap[coverPiece].Remove(x));
                }

                if (pos == kingPos)
                {
                    foreach (var p in AvailibleMovesMap
                        .Where(x => x.Key.Color == piece.Color && x.Key.Type != King))
                    {
                        p.Value.Where(x => !x.IsBetween(kingPos, pos))
                            .ToList()
                            .Foreach(x => p.Value.Remove(x));
                    }
                }
            }
        }

        private void AddPawnMoves(Piece piece)
        {
            var startPosY = piece.Color == White ? 1 : 6;
            var oneStepUp = piece.Color == White ? 1 : -1;
            var twoStepsUp = oneStepUp * 2;
            var rightAttack = piece.Color == White ? 9 : 7;

            var moves = AvailibleMovesMap[piece];
            var pos = piece.Position;
            var leftAttack = piece.Color == White ? -7 : -9;

            if (CanMove(pos + oneStepUp))
                moves.Add(pos + oneStepUp);

            if (pos.ToTuple().y == startPosY && CanMove(pos + twoStepsUp))
                moves.Add(pos + twoStepsUp);

            var deltas = new[] { rightAttack, leftAttack };

            foreach (var delta in deltas)
            {
                if (CanBeat(pos + delta, piece.Color))
                    moves.Add(pos + delta);
                else
                {
                    if (this.moves.Count == 0)
                        continue;

                    var lastMove = this.moves.Last();

                    if (pos.ToTuple().y == 4
                        && lastMove.Piece.Type == Pawn
                        && (Math.Abs(lastMove.TargetPos.GetX() - piece.Position.GetX()) == 1)
                        && lastMove.SourcePos - lastMove.TargetPos == 2)
                    {
                        moves.Add(pos + delta);
                    }
                }
            }
        }

        private void AddBishopMoves(Piece piece)
        {
            AddContinuousMove(piece, 9);
            AddContinuousMove(piece, -9);
            AddContinuousMove(piece, 7);
            AddContinuousMove(piece, -7);
        }

        private void AddKnightMoves(Piece piece)
        {
            var deltaPositions = new[] { 10, 17, 15, 6, -10, -17, -15, -6 };

            foreach (var deltaPos in deltaPositions)
            {
                var pos = piece.Position + deltaPos;

                if (CanMove(pos) || CanBeat(pos, piece.Color))
                    AvailibleMovesMap[piece].Add(pos);
            }
        }

        private void AddRookMoves(Piece piece)
        {
            AddContinuousMove(piece, 1);
            AddContinuousMove(piece, -1);
            AddContinuousMove(piece, 8);
            AddContinuousMove(piece, -8);
        }

        private void AddQueenMoves(Piece piece)
        {
            AddBishopMoves(piece);
            AddRookMoves(piece);
        }

        private void AddKingMoves(Piece piece)
        {
            var pos = piece.Position;
            var deltas = new[] { 1, 9, 8, 7, -1, -9, -8, -7 };
            foreach (var delta in deltas)
            {
                if (CanMove(pos + delta) || CanBeat(pos + delta, piece.Color))
                    AvailibleMovesMap[piece].Add(pos + delta);
            }
        }

        private void AddContinuousMove(Piece piece, int deltaPos)
        {
            var pos = piece.Position;

            while (true)
            {
                pos += deltaPos;
                if (CanMove(pos))
                {
                    AvailibleMovesMap[piece].Add(pos);
                }
                else
                {
                    if (CanBeat(pos, piece.Color))
                        AvailibleMovesMap[piece].Add(pos);

                    return;
                }
            }
        }

        private bool CanMove(PositionEnum targetPos)
        {
            return targetPos.IsValid() && !PiecesMap.ContainsKey(targetPos);
        }

        private bool CanBeat(PositionEnum targetPos, PieceColor color)
        {
            return PiecesMap.TryGetValue(targetPos, out var piece) && piece.Color != color;
        }
        #endregion

        #region Generating
        private void GeneratePieces()
        {
            for (var pos = a2; pos < h8; pos += 8)
            {
                AddPiece(Pawn, White, pos);
                AddPiece(Pawn, Black, pos + 5);
            }

            AddPiece(Rook, White, a1);
            AddPiece(Rook, White, h1);
            AddPiece(Rook, Black, a8);
            AddPiece(Rook, Black, h8);

            AddPiece(Knight, White, b1);
            AddPiece(Knight, White, g1);
            AddPiece(Knight, Black, b8);
            AddPiece(Knight, Black, g8);

            AddPiece(Bishop, White, c1);
            AddPiece(Knight, White, f1);
            AddPiece(Knight, Black, c8);
            AddPiece(Knight, Black, f8);

            AddPiece(Queen, White, d1);
            AddPiece(Queen, Black, d8);

            AddPiece(King, White, e1);
            AddPiece(King, Black, e8);

            foreach (var piece in PiecesMap.Values)
            {
                AvailibleMovesMap[piece] = new List<PositionEnum>();
            }
        }

        private void AddPiece(PieceType type, PieceColor color, PositionEnum position)
        {
            var piece = new Piece(type, color, position, PiecesMap.Count);
            PiecesMap[piece.Position] = piece;
        }
        #endregion
    }
}
