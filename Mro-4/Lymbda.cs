using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mro_4
{
    class Lymbda
    {
        public int[] mass;
        public int s;
        public Lymbda()
        {
        }
        public Lymbda(int k)
        {
            mass = new int[k];
            for(int i = 0; i < k; i++)
            {
                mass[i] = 1;
            }
        }
        public Lymbda(int[] ly)
        {
            mass = ly;
        }
    }
}
