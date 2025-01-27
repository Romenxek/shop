using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7
{
    internal class Shop
    {
        public Showcase<BakeryProduct> CurrentShowcase = new Showcase<BakeryProduct>(2, 3); //витрина магазина
        public Seller Seller { get; set; }
        public int AdditingTime = 0;
        public DateTime CurrentTime
        {
            get
            {
                return DateTime.Now.AddHours(AdditingTime);
            }
        }
        public Shop(){}
        public void LowerTheTime(int time)
        {
            AdditingTime -= time;
            foreach (BakeryProduct product in CurrentShowcase)
            {
                product.UpdateExpirationStatus(this);
            }
        }
        public void UpperTheTime(int time)
        {
            AdditingTime += time;
            foreach (BakeryProduct product in CurrentShowcase)
            {
                product.UpdateExpirationStatus(this);
            }
        }

        public bool IsExpired(BakeryProduct product)
        {
            return CurrentTime > product.ExpirationDate;
        }
    }
}
