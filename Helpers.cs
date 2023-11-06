using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSimulation2023
{
    internal class Helpers
    {
        public int Randomizer(int x, int y)
        {
            Random random = new Random();
            int z = random.Next(x, y);
            return z;
        }
    }
}
