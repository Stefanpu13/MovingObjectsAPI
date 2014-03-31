using MovingObjects.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovingObjects.Repository
{
    public interface IPlayerRepository<Player> : IRepository<Player>
    {
        Player Get(string username);
    }
}
