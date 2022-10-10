using Chess.Logic.Moves;
using Chess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Chess.Logic.Positions;

namespace Chess.Logic
{
    public class Game
    {
        private Board board;

        public Game(int time, int bonus)
        {
            board = new Board();
        }

        public (List<PieceDto> pieces, bool isCheck, bool isEnd) GetGameState()
        {
            var pieces = board.PiecesMap.Values.Select(x => x.ToDto(board.CurrentPlayer)).ToList();

            return (pieces, board.IsCheck, board.IsEnd);
        }

        /// <summary>
        /// Move code in format: {sourcePos}{targetPos}(optional){new piece code} Examples: 1) a3a4 2) c7c8Q
        /// </summary>
        public void MakeMove(string moveCode)
        {
            if (moveCode.Length < 4)
                return;

            var sourcePosCode = new string(new[] { moveCode[0], moveCode[1] });

            if (!sourcePosCode.IsValidChessPos())
                return;

            var sourcePos = VectorsMap[sourcePosCode];
            var shortCode = moveCode.Remove(0, 2);

            var piece = board.PiecesMap.Values.FirstOrDefault(x => x.Position == sourcePos && x.Color == board.CurrentPlayer);
            var move = piece?.PossibleMoves.FirstOrDefault(x => x.Code == shortCode);

            if (move is null)
                return;

            board.MakeMove(move);
        }
    }
}
