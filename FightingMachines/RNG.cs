using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FightingMachines
{
    public class Rng
    {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Singleton shenanigans.
        /// </summary>
        private static readonly Rng instance = new Rng();

        /// <summary>
        /// Random object to generate numbers from.
        /// </summary>
        private readonly Random _random;

        /// <summary>
        /// List of all the possible male names.
        /// </summary>
        private string[] _maleNames;

        /// <summary>
        /// List of all the possible female names.
        /// </summary>
        private string[] _femaleNames;

        /// <summary>
        /// List of all the hair types.
        /// </summary>
        private readonly List<string> _hairs = new List<string>();

        /// <summary>
        /// List of all the eyes.
        /// </summary>
        private readonly List<string> _eyes = new List<string>(); 

        /// <summary>
        /// The current assembly. Used for reflection.
        /// </summary>
        private readonly Assembly _assembly;

        /// <summary>
        /// Constructor sets up the RNG and initializes the lists.
        /// </summary>
        private Rng()
        {
            _random = new Random();
            LoadNames();

            _assembly = typeof (Rng).Assembly;

            GetHair();
            GetEyes();
        }

        /// <summary>
        /// Get the full list of hair types and add them to the list.
        /// </summary>
        private void GetHair()
        {
            var e =
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes(), (a, t) => new { a, t })
                    .Where(@t1 => @t1.t.IsDefined(typeof(RandomableHair), false))
                    .Select(@t1 => @t1.t);

            foreach (var h in e.ToList())
            {
                _hairs.Add(h.FullName);
            }
        }

        /// <summary>
        /// Get the full list of eye types and add them to the list.
        /// </summary>
        private void GetEyes()
        {
            var e =
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes(), (a, t) => new { a, t })
                    .Where(@t1 => @t1.t.IsDefined(typeof(RandomableEyes), false))
                    .Select(@t1 => @t1.t);

            foreach (var h in e.ToList())
            {
                _eyes.Add(h.FullName);
            }
        }

        /// <summary>
        /// Load the names from disk into the right lists.
        /// </summary>
        private void LoadNames()
        {
            _maleNames = File.ReadAllLines("data/male.txt");
            _femaleNames = File.ReadAllLines("data/female.txt");
        }

        /// <summary>
        /// Return a random integer.
        /// </summary>
        /// <param name="min">Minimum number</param>
        /// <param name="max">Maximum number</param>
        /// <returns>Generated number</returns>
        public int RandInt(int min, int max)
        {
            return _random.Next(min, max);
        }

        /// <summary>
        /// Return a random double.
        /// </summary>
        /// <returns>Generated double</returns>
        public double RandDouble()
        {
            return _random.NextDouble();
        }

        /// <summary>
        /// Get a random name from the list of specified gender.
        /// </summary>
        /// <param name="gender">A name of which gender?</param>
        /// <returns>The name</returns>
        public string RandName(Gender gender)
        {
            if (gender == Gender.Male)
            {
                return _maleNames[RandInt(0, _maleNames.Length - 1)];
            }

            return _femaleNames[RandInt(0, _femaleNames.Length - 1)];
        }

        /// <summary>
        /// Choose a random hair
        /// </summary>
        /// <returns>The hair</returns>
        public Hair RandHair()
        {
            var className = RandInt(0, _hairs.Count);
            var hair = _hairs[className];

            return (Hair)Activator.CreateInstance(_assembly.GetType(hair));
        }

        /// <summary>
        /// Choose a random eye type
        /// </summary>
        /// <returns>The eyes</returns>
        public Eyes RandEyes()
        {
            var className = RandInt(0, _eyes.Count);
            var eye = _eyes[className];

            return (Eyes)Activator.CreateInstance(_assembly.GetType(eye));
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static Rng Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
