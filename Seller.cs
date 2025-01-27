using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7
{
    internal class Seller
    {
        public string Name { get; set; }
        public int StartWorkingTime { get; set; }
        public int EndWorkingTime { get; set; }

        protected Shop _shop { get; set; }

        public Seller(Shop shop, string name, int startWorkingTime,
            int endWorkingTime) 
        {
            _shop = shop;
            Name = name;
            StartWorkingTime = startWorkingTime;
            EndWorkingTime = endWorkingTime;
        }

        public bool должен_ли_работать
        {
            get
            {
                int currentHour = _shop.CurrentTime.Hour;
                int currentMinute = _shop.CurrentTime.Minute;

                if (currentHour > StartWorkingTime && currentHour < EndWorkingTime)
                {
                    return true;
                }

                if (currentHour == StartWorkingTime && currentMinute >= 0)
                {
                    return true;
                }

                if (currentHour == EndWorkingTime && currentMinute == 0)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
