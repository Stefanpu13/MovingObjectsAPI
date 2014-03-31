using MovingObjects.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovingObjects.Repository
{
    public class PlayerRepository:IPlayerRepository<Player>
    {
        private MovingObjectsEntities context;

        public PlayerRepository()
        {
            this.context = new MovingObjectsEntities();
        }

        public IEnumerable<Player> GetAll() 
        {
            return context.Players;
        }
        public Player Get(int id)
        {
            return context.Players.FirstOrDefault(p => p.Id == id);
        }
        public Player Get(string username) 
        {
            return context.Players.FirstOrDefault(p => p.UserName.ToLower() == username.ToLower());
        }

        public int Add(Player player) 
        {
            int id = 0;
            if (player != null)
            {
                context.Players.Add(player);
                context.SaveChanges();
                id = player.Id;                
            }
            
            return id;
        }
        public Player Delete(int id) 
        {
            throw new NotImplementedException();
        }
        public void Update(Player item) 
        {
            throw new NotImplementedException();
        }
    }
}
