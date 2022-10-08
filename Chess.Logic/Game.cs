using Chess.Logic.Moves;
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
            var piece = board.PiecesMap.Values.FirstOrDefault(x => x.Id == moveDto.PieceId && x.Color == currentPlayer);
            var move = piece?.PossibleMoves.FirstOrDefault(x => x.IsThisMove(moveDto.TargetPos.ToVector(), moveDto.Parameter));

            if (move is null)
                return;

            move.MakeMove();
            currentPlayer = (PieceColor)((int)(currentPlayer + 1) % 2);
        }
    }
}
