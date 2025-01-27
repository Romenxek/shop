using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7
{
    internal class CalorieContent
    {
        public int Protein { get; set; }
        public int Carbs { get; set; }
        public int Fats { get; set; }

        public CalorieContent(int protein, int fats, int carbs)
        {
            Protein = protein;
            Carbs = carbs;
            Fats = fats;
        }

        public override string ToString()
        {
            return $"proteins: {Protein}, fats: {Fats}, carbs:{Carbs}";
        }
    }
}
