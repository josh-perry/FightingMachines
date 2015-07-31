using System;
using System.Collections.Generic;

namespace GenerationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var sim = new GenePool(100);
            Console.ReadKey();
        }
    }

    class GenePool
    {
        public List<Person> people;
        public int AgeOfConsent = 16;

        public GenePool(int size)
        {
            people = new List<Person>();

            for (var i = 0; i < size; i++)
            {
                var p = new Person();
                people.Add(p);
            }

            DateNight();

            OutputStats();

            //while (true)
            for(var y = 0; y < 1000; y++)
            {
                Console.WriteLine("Year {0}", y);
                Console.ReadKey();
                AdvanceYear();
            }
        }

        private void AdvanceYear()
        {
            // Increase age, kill some people
            AdvanceAges();

            // Births
            Births();

            // New spouses
            DateNight();
        }

        private void DateNight()
        {
            var eligiblePeople = people.FindAll(x => x.Spouse == null);
            var eligibleMales = eligiblePeople.FindAll(x => x.Gender == Gender.Male && x.Age >= AgeOfConsent);
            var eligibleFemales = eligiblePeople.FindAll(x => x.Gender == Gender.Female && x.Age >= AgeOfConsent);

            foreach (var person in eligibleMales)
            {
                // Always assume failure
                var fail = true;

                // 10 hot dates, 10 chances
                for (var i = 0; i < 10; i++)
                {
                    var maximum = eligibleFemales.Count;

                    if (maximum == 0)
                        break;

                    var otherPerson = eligibleFemales[RNG.Instance.RandInt(0, maximum)];

                    // If already taken, remain taken
                    if (person.Spouse != null)
                        continue;

                    // Adam and Eve, not Adam and Steve: remain single
                    if (person.Gender == otherPerson.Gender)
                        continue;

                    // If the other person is him/herself or the first date didn't go well, remain single
                    if (otherPerson.Equals(person))
                        continue;

                    if (person.GiveSignificantOther(otherPerson))
                    {
                        fail = false;
                        break;
                    }
                }

                if (fail) continue;

                eligibleFemales.Remove(person.Spouse.OtherPerson);
            }
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
            foreach (var person in people.FindAll(x => x.Gender == Gender.Female && x.Spouse != null))
            {
                // If true, make babies
                if (!person.PregnancyCheck()) continue;

                try
                {
                    var baby = person.MakeBaby(person.Spouse.OtherPerson);
                    people.Add(baby);
                }
                catch (Exception e)
                {
                    // Something went awry, mister and miss probably aren't a good couple
                    person.Spouse.OtherPerson.Spouse = null;
                    person.Spouse.OtherPerson = null;

                    Console.WriteLine(e);
                }
            }
        }

        private void OutputStats()
        {
            var males = people.FindAll(x => x.Gender == Gender.Male);
            var females = people.FindAll(x => x.Gender == Gender.Female);
            var eligibleMales = males.FindAll(x => x.Spouse == null);
            var eligibleFemales = females.FindAll(x => x.Spouse == null);

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
