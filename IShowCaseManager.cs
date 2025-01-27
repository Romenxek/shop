using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7
{
    public interface IShowсaseManeger<in T, out K>
    {

        void Delete(T item);
        K Get(string name);
        IEnumerable<K> GetAll();
    }
}
