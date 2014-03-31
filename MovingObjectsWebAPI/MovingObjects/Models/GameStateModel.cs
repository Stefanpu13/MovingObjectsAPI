using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovingObjects.Models
{
    public class GameStateModel
    {
        public int Score { get; set; }
        public string GameLevel { get; set; }
        public string GameDifficulty { get; set; }
        public string GameSpeed { get; set; }
        public int Bonuses { get; set; }
        public int Restarts { get; set; }
    }
}