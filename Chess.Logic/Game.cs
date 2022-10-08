using Chess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Logic
{
    public class Game
    {
        private Board board;
        private PieceColor currentPlayer;

        public bool IsStarted => board != null;

        public void StartGame()
        {
            board = new Board();
            currentPlayer = PieceColor.White;
        }

        public List<PieceDto> GetPieces()
        {
            var pieces = board.PiecesMap.Values.Select(x => x.ToDto(currentPlayer)).ToList();

            return pieces;
        }

        public void MakeMove(MoveDto moveDto)
        {
            var piece = board.PiecesMap.Values.First(x => x.Id == moveDto.PieceId);
            var move = piece.PossibleMoves.First(x => x.TargetPos.X == moveDto.TargetPos.X
                                                    && x.TargetPos.Y == moveDto.TargetPos.Y);
            move.MakeMove();
            currentPlayer = (PieceColor)((int)(currentPlayer + 1) % 2);
        }
    }
}
