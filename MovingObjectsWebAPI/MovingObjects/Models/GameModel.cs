using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovingObjects.Models
{
    public class GameModel
    {
        public string Name { get; set; }

        public DateTime SaveDate { get; set; }

        public int Id { get; set; }

        public GameStateModel GameState { get; set; }

        public ICollection<GameObjectModel> GameObjects { get; set; }
    }
}