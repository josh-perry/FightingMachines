using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GenerationTest
{
    class Person
    {
        public Gender Gender;
        public int Age;
        public string Name;

        public Eyes Eyes;
        public Hair Hair;

        public bool Dead { get; set; }
        public bool Pregnant { get; set; }

        public Relationship Mother;
        public Relationship Father;
        public Relationship Spouse;
        public List<Relationship> Children = new List<Relationship>();
        public List<Relationship> Siblings = new List<Relationship>();

        public Person(Person mother, Person father)
        {
            DetermineGender();
            
            if (mother != null && father != null)
            {
                // Calculate genes based on parents
                Hair = (Hair) FightGenes(mother.Hair, father.Hair);
                Eyes = (Eyes) FightGenes(mother.Eyes, father.Eyes);
                Age = 0;
            }
            else // No parents: generate randomly
            {
                Hair = RandomHair();
                Eyes = RandomEyes();
                Age = RNG.Instance.RandInt(16, 60);
            }

            Name = RNG.Instance.RandName(Gender);
        }
        
        public void DetermineGender()
        {
            Gender = Gender.Female;
            if (RNG.Instance.RandInt(0, 2) == 1)
                Gender = Gender.Male;  
        }

        public bool GiveSignificantOther(Person other)
        {
            if (other.Spouse != null && other.Spouse.OtherPerson.Equals(this))
                return false;

            // Half your age plus 7 rules
            if ((Age/2) + 7 > other.Age)
            {
                //Console.WriteLine("{0} is too young for {1} ({2} vs {3})", other.Name, Name, other.Age, Age);
                return false;
            }
            if ((other.Age / 2) + 7 > Age)
            {
                //Console.WriteLine("{0} is too old for {1} ({2} vs {3})", other.Name, Name, other.Age, Age);
                return false;
            }

            Spouse = new Relationship { OtherPerson = other };
            Spouse.OtherPerson.Spouse = new Relationship { OtherPerson = this };
            
            return true;
        }

        public Person MakeBaby(Person other)
        {
            if (other == null && Spouse.OtherPerson != null)
                other = Spouse.OtherPerson;
            else if (Spouse.OtherPerson == null)
            {
                throw new Exception("No-one to make baby with!"); // teehee
            }

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

            Children.Add(new Relationship { OtherPerson = baby, RelationType = Relation.Child });
            baby.Mother = new Relationship
            {
                OtherPerson = mother,
                RelationType = Relation.Mother
            };

            baby.Father = new Relationship
            {
                OtherPerson = father,
                RelationType = Relation.Father
            };

            return baby;
        }

        // I was never very good at biology. I'm p sure this is how genetics work.
        public Gene FightGenes(Gene gene1, Gene gene2)
        {
            var rng = new Random();
            float x = rng.Next(0, 120);

            // Random mutation
            if (x >= 100)
            {
                if(gene1 is Eyes)
                    return RandomEyes();
                if(gene1 is Hair)
                    return RandomHair();
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

        public Hair RandomHair()
        {
            return RNG.Instance.RandHair();
        }

        public Eyes RandomEyes()
        {
            return RNG.Instance.RandEyes();
        }

        public void AdvanceAge()
        {
            Age++;

            var oneInX = CalculateDeathOdds();
            if (RNG.Instance.RandInt(1, oneInX) == 1)
            {
                Die();
            }
        }

        private int CalculateDeathOdds()
        {
            // Do something with this maybe: http://www.medicine.ox.ac.uk/bandolier/booth/Risk/dyingage.html
            // For now:
            return 200;
        }

        private void Die()
        {
            // Widowmaker
            if(Spouse != null)
                Spouse.OtherPerson.Spouse = null;

            Dead = true;
        }

        public bool PregnancyCheck()
        {
            // If not a girl or is single, assume not pregnant
            if (Gender != Gender.Female || Spouse.OtherPerson == null)
                return false;

            if (Pregnant)
                return true;

            if (RNG.Instance.RandInt(1, 20) == 1)
            {
                Console.WriteLine("{0} has fallen pregnant with {1}.", Name, Spouse.OtherPerson.Name);
                Pregnant = true;    
            }

            return false;
        }
    }
}
