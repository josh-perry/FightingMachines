using System;
using System.Collections.Generic;
using System.Threading;

namespace FightingMachines
{
    /// <summary>
    /// A self-contained gene pool that simulates people and genetics.
    /// </summary>
    class GenePool
    {
        /// <summary>
        /// A list of all live Persons currently inside the pool.
        /// </summary>
        public List<Person> People;
        
        /// <summary>
        /// The age at which Persons are allowed to have a spouse and children.
        /// </summary>
        public int AgeOfConsent = 16;

        /// <summary>
        /// Money "on the ground" from death etc.
        /// Can be randomly taken by people.
        /// </summary>
        public int MoneyInFlux { get; set; }

        /// <summary>
        /// Populates the people list with the specified number of randomly
        /// generated people, calls DateNight() and starts the main loop.
        /// </summary>
        /// <param name="size">The number of people to generate</param>
        public GenePool(int size)
        {
            // Create a bunch of people
            People = new List<Person>();

            for (var i = 0; i < size; i++)
            {
                People.Add(new Person(null));
            }

            // Put them in fancy clothes and make them mingle
            DateNight();
            FarmFood();

            // Start the simulation!
            new Thread(Run).Start();
        }

        /// <summary>
        /// The main simulation loop.
        /// </summary>
        private void Run()
        {
            for (var y = 0; y < int.MaxValue; y++)
            {
                // Current year
                Console.WriteLine("Year {0}", y);
                
                Thread.Sleep(1000);

                // Increase age, kill some people
                AdvanceAges();

                // Births
                Births();

                // New spouses
                DateNight();

                // Random transactions
                MoneyTransactions();

                // Eat food
                EatFood();

                // Distribute money in flux
                UpdateMoneyInFlux();

                // Foraging
                FarmFood();

                OutputStats();

                TimeManager.AdvanceYear();

                if (People.Count == 0)
                {
                    Console.WriteLine($"All life is extinguished in the year {y}!");
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// Give food randomly.
        /// </summary>
        private void FarmFood()
        {
            for (var i = 0; i < 50; i++)
            {
                var person = People[Rng.Instance.RandInt(0, People.Count)];
                var moneyToTake = Rng.Instance.RandInt(0, MoneyInFlux);

                person.Food += Rng.Instance.RandInt(0, 200);
            }
        }

        /// <summary>
        /// Take money randomly from the pool.
        /// </summary>
        private void UpdateMoneyInFlux()
        {
            for(var i = 0; i < 10; i++)
            {
                var person = People[Rng.Instance.RandInt(0, People.Count)];
                var moneyToTake = Rng.Instance.RandInt(0, MoneyInFlux);

                person.Money += moneyToTake;
                MoneyInFlux -= moneyToTake;
            }
        }

        /// <summary>
        /// Make random transactions between people in exchange for food and water.
        /// </summary>
        private void MoneyTransactions()
        {
            foreach (var person in People)
            {
                var peopleWithMoreFood = People.FindAll(x => x.Food > person.Food && x.Food > x.Hunger);
                var maximum = peopleWithMoreFood.Count;

                // Nobody has more food, don't buy anything
                if (maximum == 0)
                    break;

                var personToBuyFoodFrom = peopleWithMoreFood[Rng.Instance.RandInt(0, maximum)];

                var foodToBuy = Math.Min(person.Hunger, person.Money);
                var payment = foodToBuy;

                personToBuyFoodFrom.Money += payment;
                person.Money -= payment;
            }
        }

        /// <summary>
        /// Eat food.
        /// </summary>
        private void EatFood()
        {
            foreach(var person in People)
            {
                if (person.Dead) continue;

                if (person.Food < person.Hunger)
                {
                    Console.WriteLine("{0} dies of starvation at age {1}", person.Name, person.Age);
                    person.Die();
                    continue;
                }

                person.Food -= person.Hunger;
            }
        }

        /// <summary>
        /// Date night! Try to find everyone an eligible partner.
        /// </summary>
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

                    var otherPerson = eligibleFemales[Rng.Instance.RandInt(0, maximum)];

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

        /// <summary>
        /// Age everyone, check to see if they should have died and clean up
        /// the bodies from the people list.
        /// </summary>
        private void AdvanceAges()
        {
            var deadPeople = new List<Person>();
            foreach (var person in People)
            {
                person.AdvanceAge(AgeOfConsent);
                person.UpdateOrphanStatus();
                person.UpdateMarriages();

                if (person.Dead)
                {
                    deadPeople.Add(person);
                }
            }

            // Clean up corpses
            //foreach (var person in deadPeople)
            //{
            //    People.Remove(person);
            //}
        }

        /// <summary>
        /// Check to see if babies should be made and make them.
        /// </summary>
        private void Births()
        {
            foreach (var person in People.FindAll(x => x.Gender == Gender.Female && x.Spouse != null))
            {
                // If true, make babies
                if (!person.UpdatePregnancy()) continue;

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

        /// <summary>
        /// Debug output.
        /// </summary>
        private void OutputStats()
        {
            var males = People.FindAll(x => x.Gender == Gender.Male && !x.Dead);
            var females = People.FindAll(x => x.Gender == Gender.Female && !x.Dead);
            var eligibleMales = males.FindAll(x => x.Spouse == null);
            var eligibleFemales = females.FindAll(x => x.Spouse == null);

            Console.WriteLine("All");
            Console.WriteLine("\tTotal:        " + males.Count + females.Count);
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