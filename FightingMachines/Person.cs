using System;
using System.Collections.Generic;
using FightingMachines.LifeEvents;

namespace FightingMachines
{
    public class Person
    {
        /// <summary>
        /// The person's gender, determines who they can date, death rate,
        /// whether they can get pregnant etc. etc.
        /// </summary>
        public Gender Gender;

        /// <summary>
        /// How old the person is in years.
        /// </summary>
        public int Age;

        /// <summary>
        /// Their first name.
        /// </summary>
        public string Name;

        /// <summary>
        /// Gene that determines their eyes.
        /// </summary>
        public Eyes Eyes;

        /// <summary>
        /// Gene that determines their hair.
        /// </summary>
        public Hair Hair;

        /// <summary>
        /// Do they breathe?
        /// </summary>
        public bool Dead { get; set; }

        /// <summary>
        /// Do they have a tiny adorable parasite?
        /// </summary>
        public bool Pregnant { get; set; }

        /// <summary>
        /// Reference to the person's mother.
        /// </summary>
        public Relationship Mother;

        /// <summary>
        /// Reference to the person's father.
        /// </summary>
        public Relationship Father;

        /// <summary>
        /// Reference to the person's significant other.
        /// </summary>
        public Relationship Spouse;

        /// <summary>
        /// List of offspring people.
        /// </summary>
        public List<Relationship> Children = new List<Relationship>();

        /// <summary>
        /// List of brothers and sisters.
        /// </summary>
        public List<Relationship> Siblings = new List<Relationship>();

        /// <summary>
        /// A list of all significant events across the person's lifespan.
        /// </summary>
        public List<LifeEvent> LifeEvents = new List<LifeEvent>();

        /// <summary>
        /// Does this person have no surviving parents?
        /// </summary>
        public bool Orphaned { get; set; }

        /// <summary>
        /// Intelligence.
        /// </summary>
        public int Iq { get; set; }

        /// <summary>
        /// Create a new person from mother + father's genes or entirely
        /// randomly if there is no mother/father.
        /// </summary>
        /// <param name="mother">The mother</param>
        /// <param name="father">The father</param>
        public Person(Person mother = null, Person father = null)
        {
            // Impose role
            DetermineGender();
            
            // If there is a mother and father, create person using their genes as a base
            if (mother != null && father != null)
            {
                // Calculate genes based on parents
                Hair = (Hair) FightGenes(mother.Hair, father.Hair);
                Eyes = (Eyes) FightGenes(mother.Eyes, father.Eyes);
                Age = 0;
            }
            else // No parents: generate randomly
            {
                Hair = Rng.Instance.RandHair();
                Eyes = Rng.Instance.RandEyes();
                Age = Rng.Instance.RandInt(16, 60);
                Iq = Rng.Instance.RandInt(70, 130);

                AddLifeEvent(new Birth { Year = TimeManager.Year - Age });
            }

            Name = Rng.Instance.RandName(Gender);
        }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Person()
        {
            
        }
        
        /// <summary>
        /// Set this person to be orphaned if there are no surviving parents.
        /// </summary>
        public void UpdateOrphanStatus()
        {
            // If they are already orphaned, don't bother checking again!
            if (Orphaned)
                return;
            
            // If they have no existing mother and father, then orphan them
            if (Mother == null && Father == null)
            {
                Orphaned = true;
            }

            if (Mother == null || Mother.Person.Dead)
            {
                if (Father == null || Father.Person.Dead)
                {
                    Orphaned = true;
                }
            }
        }

        /// <summary>
        /// Assign them a gender randomly.
        /// </summary>
        public void DetermineGender()
        {
            Gender = Gender.Female;

            if (Rng.Instance.RandInt(0, 2) == 1)
                Gender = Gender.Male;
        }

        /// <summary>
        /// Give them a spouse if suitable.
        /// </summary>
        /// <param name="other">The spouse in question</param>
        /// <returns>
        /// True if the person is suitable, false if they are not in
        /// the right age range, is a relative or otherwise unsuitable.
        /// </returns>
        public bool GiveSignificantOther(Person other)
        {
            if (other.Spouse != null && other.Spouse.Person.Equals(this))
                return false;

            // Half your age plus 7 rules
            if ((Age/2) + 7 > other.Age)
            {
                return false;
            }
            if ((other.Age / 2) + 7 > Age)
            {
                return false;
            }

            Spouse = new Relationship { Person = other };
            Spouse.Person.Spouse = new Relationship { Person = this };
            
            return true;
        }

        /// <summary>
        /// Create a new person from the genes of this person and another.
        /// </summary>
        /// <param name="other">The other parent</param>
        /// <returns>The new baby!</returns>
        public Person MakeBaby(Person other)
        {
            if (other == null && Spouse.Person != null)
                other = Spouse.Person;

            if (other == null)
                throw new Exception("No-one to make baby with!");

            Person mother;
            Person father;
            if (Gender == Gender.Female)
            {
                father = other;
                mother = this;
            }
            else
            {
                father = this;
                mother = other;
            }

            var baby = new Person(mother, father);
            
            Console.WriteLine("{0} and {1} are the proud new parents of baby {2}!", mother.Name, father.Name, baby.Name);
            
            Pregnant = false;
            other.Pregnant = false;

            Children.Add(new Relationship { Person = baby, RelationType = Relation.Child });
            baby.Mother = new Relationship
            {
                Person = mother,
                RelationType = Relation.Mother
            };

            baby.Father = new Relationship
            {
                Person = father,
                RelationType = Relation.Father
            };

            var iqMin = Math.Min(mother.Iq, father.Iq) - 20;
            var iqMax = Math.Max(mother.Iq, father.Iq) + 20;
            baby.Iq = Rng.Instance.RandInt(Math.Min(40, iqMin), Math.Max(160, iqMax));

            baby.AddLifeEvent(new Birth { Year = TimeManager.Year });
            return baby;
        }

        /// <summary>
        /// Add the specified life event to the list.
        /// </summary>
        /// <param name="le"></param>
        public void AddLifeEvent(LifeEvent le)
        {
            le.MainPerson = this;
            LifeEvents.Add(le);
        }

        /// <summary>
        /// Decide which gene should exist out of the two given, based on
        /// recessiveness/dominance.
        /// </summary>
        /// <param name="gene1">Gene 1</param>
        /// <param name="gene2">Gene 2</param>
        /// <returns>The winning gene.</returns>
        public Gene FightGenes(Gene gene1, Gene gene2)
        {
            // I was never very good at biology. I'm p sure this is how genetics work.
            var rng = new Random();
            float x = rng.Next(0, 120);

            // Random mutation
            if (x >= 100)
            {
                if(gene1 is Eyes)
                    return Rng.Instance.RandEyes();
                if(gene1 is Hair)
                    return Rng.Instance.RandHair();
            }

            var gene1Odds = 50;

            if (gene1.DominantGene && gene2.DominantGene)
            {
                gene1Odds = 50;
            }
            else if (gene1.DominantGene && !gene2.DominantGene)
            {
                gene1Odds = 75;
            }
            else if (gene2.DominantGene && !gene1.DominantGene)
            {
                gene1Odds = 25;
            }

            if (x <= gene1Odds)
                return gene1;

            return gene2;
        }

        /// <summary>
        /// Age the person by a year, kill them of natural causes if they won
        /// the lottery.
        /// Also give them the AgeOfConsent life event if they are now old
        /// enough.
        /// </summary>
        /// <param name="ageOfConsent">The current age of consent.</param>
        public void AdvanceAge(int ageOfConsent)
        {
            Age++;

            if(Age == ageOfConsent)
            {
                // Reached age of consent!
                Console.WriteLine($"{Name} came of age.");
                AddLifeEvent(new ComingOfAge());
            }

            var oneInX = DeathOdds.GetDeathChance(Age, Gender);
            if (Rng.Instance.RandInt(1, oneInX) == 1)
            {
                Die();
            }
        }

        /// <summary>
        /// Rest in peace. 
        /// Make a widow if needed.
        /// </summary>
        private void Die()
        {
            // Widowmaker
            if (Spouse != null)
                Spouse.Person.Spouse = null;

            Dead = true;
            AddLifeEvent(new Death());
        }
        
        /// <summary>
        /// Checks to see if the person should be pregnant or not.
        /// </summary>
        /// <returns>True if they have fallen pregnant, false otherwise.</returns>
        public bool UpdatePregnancy()
        {
            // If not a girl or is single, assume not pregnant
            if (Gender != Gender.Female || Spouse.Person == null)
                return false;

            if (Children.Count >= 3)
                return false;

            if (Pregnant)
                return true;

            if (Rng.Instance.RandInt(1, 20) == 1)
            {
                Console.WriteLine("{0} has fallen pregnant with {1}.", Name, Spouse.Person.Name);
                Pregnant = true;    
            }

            return false;
        }
    }
}
