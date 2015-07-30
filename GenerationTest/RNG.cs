using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GenerationTest
{
    public class RNG
    {
        private static readonly RNG instance = new RNG();
        private readonly Random random;

        private string[] male_names;
        private string[] female_names;
        private List<string> hairs = new List<string>();

        private Assembly assembly;

        static RNG()
        {

        }

        RNG()
        {
            random = new Random();
            LoadNames();

            assembly = typeof (RNG).Assembly;

            var e =
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes(), (a, t) => new { a, t })
                    .Where(@t1 => @t1.t.IsDefined(typeof(RandomableHair), false))
                    .Select(@t1 => @t1.t);

            foreach (var h in e.ToList())
            {
                hairs.Add(h.FullName);
            }
        }

        private void LoadNames()
        {
            male_names = File.ReadAllLines("data/male.txt");
            female_names = File.ReadAllLines("data/female.txt");
        }

        public int RandInt(int min, int max)
        {
            return random.Next(min, max);
        }

        public double RandDouble()
        {
            return random.NextDouble();
        }

        public string RandName(Gender gender)
        {
            if (gender == Gender.Male)
            {
                return male_names[RandInt(0, male_names.Length - 1)];
            }

            return female_names[RandInt(0, female_names.Length - 1)];
        }

        public Hair RandHair()
        {
            var className = RandInt(0, hairs.Count);
            var hair = hairs[className];

            return (Hair)Activator.CreateInstance(assembly.GetType(hair));
        }

        public static RNG Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
