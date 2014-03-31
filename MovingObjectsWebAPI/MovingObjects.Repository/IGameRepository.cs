using MovingObjects.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovingObjects.Repository
{
    public interface IGameRepository<Game>:IRepository<Game>
    {
        Player GetPlayer(int id);
    }
}
