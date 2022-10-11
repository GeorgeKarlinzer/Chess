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
        private readonly Board board;
        private readonly PlayerSwitch playerSwitch;

        public Game(int timeSec, int bonusSec)
        {
            playerSwitch = new PlayerSwitch();
            board = new Board(timeSec, bonusSec, playerSwitch);
        }

        public (List<PieceDto> pieces, bool isCheck, bool isEnd, int blackTime, int whiteTime, string currentPlayer) GetGameState()
        {
            var pieces = board.PiecesMap.Values.Select(x => x.ToDto(board.CurrentPlayer)).ToList();

            var isCheck = board.IsCheck;
            var isEnd = board.IsEnd;
            var whiteTime = board.Clock.GetTime(PlayerColor.White);
            var blackTime = board.Clock.GetTime(PlayerColor.Black);

            return (pieces, isCheck, isEnd, blackTime, whiteTime, board.CurrentPlayer.ToString());
        }

        /// <summary>
        /// Move code in format: {sourcePos}{targetPos}(optional){new piece code} Examples: 1) a3a4 2) c7c8Q
        /// </summary>
        public void MakeMove(string moveCode)
        {
            if (moveCode.Length < 4 || board.IsEnd)
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
