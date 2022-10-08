using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public class PieceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public PositionDto Position { get; set; }
        public List<PositionDto> Moves { get; set; }
    }
}
