using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FightingMachines
{
    static class DeathOdds
    {
        private static Dictionary<int, int> MaleOdds;
        private static Dictionary<int, int> FemaleOdds;

        static DeathOdds()
        {
            // http://www.medicine.ox.ac.uk/bandolier/booth/Risk/dyingage.html
            MaleOdds = new Dictionary<int, int>
            {
                { 1, 177 },
                { 4, 4386 },
                { 14, 8333 },
                { 24, 1908 },
                { 34, 1215 },
                { 44, 663 },
                { 54, 279 },
                { 64, 112 },
                { 74, 42 },
                { 84, 15 },
                { 85, 6 }
            };

            FemaleOdds = new Dictionary<int, int>
            {
                { 1, 227 },
                { 4, 5376 },
                { 14, 10417 },
                { 24, 4132 },
                { 34, 2488 },
                { 44, 1106 },
                { 54, 421 },
                { 64, 178 },
                { 74, 65 },
                { 84, 21 },
                { 85, 7 }
            };
        }

        public static int GetDeathChance(int age, Gender gender)
        {
            if (gender == Gender.Male)
                return GetDeathOdds(age, MaleOdds);

            else if (gender == Gender.Female)
                return GetDeathOdds(age, FemaleOdds);

            return 0;
        }

        private static int GetDeathOdds(int age, Dictionary<int, int> oddsTable)
        {
            foreach(var i in oddsTable)
            {
                if (age <= i.Key)
                    return i.Value;
            }

            return oddsTable.Last().Value;
        }
    }
}
