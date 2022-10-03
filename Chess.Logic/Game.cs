using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.Logic.PieceType;
using static Chess.Logic.PieceColor;
using static Chess.Logic.Positions;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Chess.Logic
{
    public class Game
    {
        private readonly List<Move> moves;

        public Dictionary<Piece, List<Vector2>> AvailibleMovesMap { get; }
        public Dictionary<Vector2, Piece> PiecesMap { get; }

        public Game()
        {
            PiecesMap = new();
            AvailibleMovesMap = new();

            moves = new List<Move>();

            GeneratePieces();

            foreach (var piece in PiecesMap.Values)
            {
                AvailibleMovesMap[piece] = new List<Vector2>();
            }

            CalculateAvailibleMoves(PieceColor.White);
        }

        public void MakeMove(Piece piece, Vector2 targetPos, object parameter = null)
        {
            var move = new Move(moves.Count, piece, piece.Position, targetPos);
            moves.Add(move);

            PiecesMap.Remove(targetPos);
            PiecesMap.Remove(move.SourcePos);

            PiecesMap[targetPos] = piece;
            piece.Position = move.TargetPos;

            CalculateAvailibleMoves(piece.Color == White ? Black : White);
        }

        #region Moves calculating

        private void CalculateAvailibleMoves(PieceColor color)
        {
            foreach (var piece in AvailibleMovesMap.Keys)
            {
                AvailibleMovesMap[piece].Clear();
                if (piece.Color != color)
                    continue;

                switch (piece.Type)
                {
                    case Pawn: AddPawnMoves(piece); break;
                    case Bishop: AddBishopMoves(piece); break;
                    case Knight: AddKnightMoves(piece); break;
                    case Rook: AddRookMoves(piece); break;
                    case Queen: AddQueenMoves(piece); break;
                    case King: AddKingMoves(piece); break;
                }
            }

            var king = PiecesMap.Values
                .First(x => x.Type == King && x.Color == color);

            var kingPos = king.Position;

            foreach (var enemyPiece in PiecesMap.Values.Where(x => x.Color != color))
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
                        .Where(x => x.Key.Color == color && x.Key.Type != King))
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
            var oneStepUp = piece.Color == White ? new Vector2(0, 1) : new Vector2(0, -1);
            var twoStepsUp = oneStepUp * 2;
            var rightAttack = piece.Color == White ? new Vector2(1, 1) : new Vector2(1, -1);
            var leftAttack = piece.Color == White ? new Vector2(-1, 1) : new Vector2(-1, -1);

            var moves = AvailibleMovesMap[piece];
            var pos = piece.Position;

            if (CanMove(pos + oneStepUp))
                moves.Add(pos + oneStepUp);

            if (pos.Y == startPosY && CanMove(pos + twoStepsUp))
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

                    if (pos.Y == 4
                        && lastMove.Piece.Type == Pawn
                        && (Math.Abs(lastMove.TargetPos.X - piece.Position.X) == 1)
                        && lastMove.SourcePos.Y - lastMove.TargetPos.Y == 2)
                    {
                        moves.Add(pos + delta);
                    }
                }
            }
        }

        private void AddBishopMoves(Piece piece)
        {
            AddContinuousMove(piece, new(1, 1));
            AddContinuousMove(piece, new(-1, -1));
            AddContinuousMove(piece, new(1, -1));
            AddContinuousMove(piece, new(-1, 1));
        }

        private void AddKnightMoves(Piece piece)
        {
            Vector2[] deltaPositions = new Vector2[]
            {
                new(1, 2), new(-1, 2), new(2, 1), new(-2, 1),
                new(1, -2), new(-1, -2), new(2, -1), new(-2, -1)
            };

            foreach (var deltaPos in deltaPositions)
            {
                var pos = piece.Position + deltaPos;

                if (CanMove(pos) || CanBeat(pos, piece.Color))
                    AvailibleMovesMap[piece].Add(pos);
            }
        }

        private void AddRookMoves(Piece piece)
        {
            AddContinuousMove(piece, new(1, 0));
            AddContinuousMove(piece, new(-1, 0));
            AddContinuousMove(piece, new(0, 1));
            AddContinuousMove(piece, new(0, -1));
        }

        private void AddQueenMoves(Piece piece)
        {
            AddBishopMoves(piece);
            AddRookMoves(piece);
        }

        private void AddKingMoves(Piece piece)
        {
            var pos = piece.Position;
            Vector2[] deltaPositions = new Vector2[]
            {
                new(1, 0), new(0, 1), new(1, -1), new(1, 1),
                new(-1, 0), new(-1, 0), new(-1, 1), new(-1, -1)
            };
            foreach (var delta in deltaPositions)
            {
                if (CanMove(pos + delta) || CanBeat(pos + delta, piece.Color))
                    AvailibleMovesMap[piece].Add(pos + delta);
            }
        }

        private void AddContinuousMove(Piece piece, Vector2 deltaPos)
        {
            var targetPos = piece.Position;

            while (true)
            {
                targetPos += deltaPos;
                if (CanMove(targetPos))
                {
                    AvailibleMovesMap[piece].Add(targetPos);
                }
                else
                {
                    if (CanBeat(targetPos, piece.Color))
                        AvailibleMovesMap[piece].Add(targetPos);

                    return;
                }
            }
        }

        private bool CanMove(Vector2 targetPos)
        {
            return targetPos.IsValidChessPos() && !PiecesMap.ContainsKey(targetPos);
        }

        private bool CanBeat(Vector2 targetPos, PieceColor color)
        {
            return PiecesMap.TryGetValue(targetPos, out var piece) && piece.Color != color;
        }
        #endregion

        #region Generating
        private void GeneratePieces()
        {
            var pawnPos = new[] { A2, B2, C2, D2, E2, F2, G2, H2 };

            foreach (var pos in pawnPos)
            {
                AddPiece(Pawn, White, pos);
                AddPiece(Pawn, Black, new(pos.X, pos.Y + 5));
            }

            AddPiece(Rook, White, A1);
            AddPiece(Rook, White, H1);
            AddPiece(Rook, Black, A8);
            AddPiece(Rook, Black, H8);

            AddPiece(Knight, White, B1);
            AddPiece(Knight, White, G1);
            AddPiece(Knight, Black, B8);
            AddPiece(Knight, Black, G8);

            AddPiece(Bishop, White, C1);
            AddPiece(Bishop, White, F1);
            AddPiece(Bishop, Black, C8);
            AddPiece(Bishop, Black, F8);

            AddPiece(Queen, White, D1);
            AddPiece(Queen, Black, D8);

            AddPiece(King, White, E1);
            AddPiece(King, Black, E8);
        }

        private void AddPiece(PieceType type, PieceColor color, Vector2 position)
        {
            var piece = new Piece(type, color, position, PiecesMap.Count);
            PiecesMap[piece.Position] = piece;
        }
        #endregion
    }
}
