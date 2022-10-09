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

        public Game()
        {
            board = new Board();
        }

        public (List<PieceDto> pieces, bool isCheck, bool isEnd) GetGameState()
        {
            var pieces = board.PiecesMap.Values.Select(x => x.ToDto(board.CurrentPlayer)).ToList();

            return (pieces, board.IsCheck, board.IsEnd);
        }

        public void MakeMove(MoveDto moveDto)
        {
            var piece = board.PiecesMap.Values.FirstOrDefault(x => x.Id == moveDto.PieceId && x.Color == board.CurrentPlayer);
            var move = piece?.PossibleMoves.FirstOrDefault(x => x.IsThisMove(moveDto.TargetPos.ToVector(), moveDto.Parameter));

            if (move is null)
                return;

            board.MakeMove(move);
        }
    }
}
