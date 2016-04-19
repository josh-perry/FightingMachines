using System;
using System.Collections.Generic;
using System.Linq;

namespace FightingMachines
{
    /// <summary>
    /// Static class to return the chances of a person dying.
    /// </summary>
    public static class DeathOdds
    {
        /// <summary>
        /// A dictionary holding the chances of a male dying at a particular
        /// age where the key is the max age boundary and the value is the
        /// chance.
        /// </summary>
        private static Dictionary<int, int> MaleOdds;

        /// <summary>
        /// A dictionary holding the chances of a female dying at a particular
        /// age where the key is the max age boundary and the value is the
        /// chance.
        /// </summary>
        private static Dictionary<int, int> FemaleOdds;

        /// <summary>
        /// Constructor sets up the male and female death odd dictionaries.
        /// </summary>
        static DeathOdds()
        {
            // Source: http://www.medicine.ox.ac.uk/bandolier/booth/Risk/dyingage.html
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

        /// <summary>
        /// Calls GetDeathOdds with the provided age and gender and returns the
        /// value.
        /// </summary>
        /// <param name="age">How old is this person?</param>
        /// <param name="gender">What is their gender?</param>
        /// <returns>The chance of a person dying (as 1 in X).</returns>
        public static int GetDeathChance(int age, Gender gender)
        {
            switch (gender)
            {
                case Gender.Male:
                    return GetDeathOdds(age, MaleOdds);
                case Gender.Female:
                    return GetDeathOdds(age, FemaleOdds);
                default:
                    throw new ArgumentOutOfRangeException(nameof(gender), gender, null);
            }
        }

        /// <summary>
        /// Return the value from the odds tables for the provided age and
        /// gender.
        /// </summary>
        /// <param name="age">How old is this person?</param>
        /// <param name="oddsTable">Which table should we be looking at?</param>
        /// <returns></returns>
        private static int GetDeathOdds(int age, Dictionary<int, int> oddsTable)
        {
            // Loop over the table
            foreach (var i in oddsTable)
            {
                // If the person is younger than the current maximum boundary then
                // return that chance.
                if (age <= i.Key)
                    return i.Value;
            }

            // If the person is older than the oldest max boundary then return the
            // last item in the table.
            return oddsTable.Last().Value;
        }
    }
}
