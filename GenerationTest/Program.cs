using System;
using System.Collections.Generic;
using System.Linq;

namespace GenerationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var sim = new GenePool(10);
        }
    }

    class GenePool
    {
        public List<Person> people;
        public int AgeOfConsent = 16;

        public GenePool(int size)
        {
            people = new List<Person>();
            var eligiblePeople = new List<Person>();

            for (var i = 0; i < size; i++)
            {
                var p = new Person();
                people.Add(p);
                eligiblePeople.Add(p);
            }

            var eligibleMales = eligiblePeople.FindAll(x => x.Gender == Gender.Male && x.Age >= AgeOfConsent);
            var eligibleFemales = eligiblePeople.FindAll(x => x.Gender == Gender.Female && x.Age >= AgeOfConsent);

            foreach (var person in eligibleMales)
            {
                // Always assume failure
                var fail = true;

                // 10 hot dates, 10 chances
                for (var i = 0; i < 10; i++)
                {
                    var otherPerson = eligibleFemales[RNG.Instance.RandInt(0, eligibleFemales.Count)];

                    // If already taken, remain taken
                    if (person.SignificantOther != null)
                        continue;

                    // Adam and Eve, not Adam and Steve: remain single
                    if (person.Gender == otherPerson.Gender)
                        continue;

                    // If the other person is him/herself or the first date didn't go well, remain single
                    if (otherPerson.Equals(person) || !person.GiveSignificantOther(otherPerson))
                        fail = false;
                }

                if(fail) continue;

                eligibleFemales.Remove(person.SignificantOther);
            }

            OutputStats();

            var year = 0;
            while (true)
            {
                Console.WriteLine("Year {0}", year);
                Console.ReadKey();
                AdvanceYear();
                year++;
            }
        }

        private void AdvanceYear()
        {
            // Increase age, kill some people
            AdvanceAges();

            // Births
            Births();

            // New spouses
        }

        private void AdvanceAges()
        {
            var deadPeople = new List<Person>();
            foreach (var person in people)
            {
                person.AdvanceAge();

                if (person.Dead)
                {
                    Console.WriteLine("{0} dies of natural causes at age {1}", person.Name, person.Age);
                    deadPeople.Add(person);
                }
            }

            // Clean up corpses
            foreach (var person in deadPeople)
            {
                people.Remove(person);
            }
        }

        private void Births()
        {
            foreach (var person in people.FindAll(x => x.Gender == Gender.Female && x.SignificantOther != null))
            {
                // If true, make babies
                if (!person.PregnancyCheck()) continue;

                var baby = person.MakeBaby(person.SignificantOther);
                people.Add(baby);
            }
        }

        private void OutputStats()
        {
            var males = people.FindAll(x => x.Gender == Gender.Male);
            var females = people.FindAll(x => x.Gender == Gender.Female);
            var eligibleMales = males.FindAll(x => x.SignificantOther == null);
            var eligibleFemales = females.FindAll(x => x.SignificantOther == null);

            Console.WriteLine("All");
            Console.WriteLine("\tTotal:        " + people.Count);
            Console.WriteLine("");
            Console.WriteLine("Males");
            Console.WriteLine("\tTotal:        " + males.Count);
            Console.WriteLine("\tSingle:       " + eligibleMales.Count);
            Console.WriteLine("\t< Consent:    " + eligibleMales.FindAll(x => x.Age < AgeOfConsent).Count);
            Console.WriteLine("\t> Consent:    " + eligibleMales.FindAll(x => x.Age >= AgeOfConsent).Count);
            Console.WriteLine("");
            Console.WriteLine("Females");
            Console.WriteLine("\tTotal:        " + females.Count);
            Console.WriteLine("\tSingle:       " + eligibleFemales.Count);
            Console.WriteLine("\t< Consent:    " + eligibleFemales.FindAll(x => x.Age < AgeOfConsent).Count);
            Console.WriteLine("\t> Consent:    " + eligibleFemales.FindAll(x => x.Age >= AgeOfConsent).Count);
        }
    }
}
