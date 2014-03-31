using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovingObjects.Repository
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        int Add(T item);
        T Delete(int id);
        void Update(T item);
    }
}
