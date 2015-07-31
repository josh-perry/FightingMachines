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
        public List<Person> People;
        public int AgeOfConsent = 16;

        public GenePool(int size)
        {
            People = new List<Person>();

            for (var i = 0; i < size; i++)
            {
                People.Add(new Person(null, null));
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
            var eligiblePeople = People.FindAll(x => x.Spouse == null);
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

                eligibleFemales.Remove(person.Spouse.Person);
            }
        }

        private void AdvanceAges()
        {
            var deadPeople = new List<Person>();
            foreach (var person in People)
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
                People.Remove(person);
            }
        }

        private void Births()
        {
            foreach (var person in People.FindAll(x => x.Gender == Gender.Female && x.Spouse != null))
            {
                // If true, make babies
                if (!person.PregnancyCheck()) continue;

                try
                {
                    var baby = person.MakeBaby(person.Spouse.Person);
                    People.Add(baby);
                }
                catch (Exception e)
                {
                    // Something went awry, mister and miss probably aren't a good couple
                    person.Spouse.Person.Spouse = null;
                    person.Spouse.Person = null;

                    Console.WriteLine(e);
                }
            }
        }

        private void OutputStats()
        {
            var males = People.FindAll(x => x.Gender == Gender.Male);
            var females = People.FindAll(x => x.Gender == Gender.Female);
            var eligibleMales = males.FindAll(x => x.Spouse == null);
            var eligibleFemales = females.FindAll(x => x.Spouse == null);

            Console.WriteLine("All");
            Console.WriteLine("\tTotal:        " + People.Count);
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
