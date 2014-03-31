using MovingObjects.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovingObjects.Repository
{
    public class GameRepository:IGameRepository<Game>
    {
        private MovingObjectsEntities context;

        public GameRepository()
        {
            this.context = new MovingObjectsEntities();
        }

        public IEnumerable<Game> GetAll() 
        {
            return context.Games;
        }

        public Player GetPlayer(int id)
        {
            return this.context.Players.FirstOrDefault(p => p.Id == id);
        }

        public Game Get(int id) 
        {
            return this.context.Games.FirstOrDefault(g => g.Id == id);
        }

        public int Add(Game game) 
        {
            int id = 0;
            if (game != null)
            {
                // Adding 'game':
                /*
                 * 1. Add 'GameState'
                 * 2. Add 'GameObjects'
                 * 3. Add 'game'                 
                 */
                
                
                this.context.Games.Add(game);
                context.SaveChanges();
                id = game.Id;
            }

            return id;
        }

        public Game Delete(int id) 
        {
            Game game;
            using (context = new MovingObjectsEntities())
            {
                game = context.Games.Find(id);

                if (game != null)
                {
                    var gameState = context.GameStates.FirstOrDefault(gs => gs.GameId == id);
                    context.GameStates.Remove(gameState);

                    var gameObjects = context.GameObject.Where(go => go.GameId == id);
                    context.GameObject.RemoveRange(gameObjects);

                    context.Games.Remove(game);
                    context.SaveChanges();
                }
            }

            return game;
        }

        public void Update(Game game) 
        {
            throw new NotImplementedException();
        }
    }
}
