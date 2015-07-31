using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GenerationTest
{
    class Person
    {
        public Gender Gender;
        public int Age;
        public Person SignificantOther;

        public string Name;

        public Eyes Eyes;
        public Hair Hair;

        public bool Dead { get; set; }
        public bool Pregnant { get; set; }

        public Person(Person mother, Person father)
        {
            DetermineGender();

            // Calculate genes
            Hair = (Hair) FightGenes(mother.Hair, father.Hair);
            Eyes = (Eyes) FightGenes(mother.Eyes, father.Eyes);
            Name = RNG.Instance.RandName(Gender);

            Age = 0;
        }

        public Person()
        {
            DetermineGender();

            Hair = RandomHair();
            Eyes = RandomEyes();
            Name = RNG.Instance.RandName(Gender);

            Age = RNG.Instance.RandInt(5, 60);
        }

        public void DetermineGender()
        {
            Gender = Gender.Female;
            if (RNG.Instance.RandInt(0, 2) == 1)
                Gender = Gender.Male;  
        }

        public bool GiveSignificantOther(Person other)
        {
            if (other.SignificantOther != null && other.SignificantOther.Equals(this))
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

            SignificantOther = other;
            other.SignificantOther = this;
            return true;
        }

        public Person MakeBaby(Person other)
        {
            if (other == null && SignificantOther != null)
                other = SignificantOther;
            else if (SignificantOther == null)
            {
                throw new Exception("No-one to make baby with!"); // teehee
            }

            Person baby;
            if (Gender == Gender.Female)
                baby = new Person(mother: this, father: other);
            else
                baby = new Person(mother: other, father: this);

            
            Console.WriteLine("{0} and {1} are the proud new parents of baby {2}!", Name, other.Name, baby.Name);
            
            Pregnant = false;
            other.Pregnant = false;

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
            if(SignificantOther != null)
                SignificantOther.SignificantOther = null;

            Dead = true;
        }

        public bool PregnancyCheck()
        {
            // If not a girl or is single, assume not pregnant
            if (Gender != Gender.Female || SignificantOther == null)
                return false;

            if (Pregnant)
                return true;

            if (RNG.Instance.RandInt(1, 20) == 1)
            {
                Console.WriteLine("{0} has fallen pregnant with {1}.", Name, SignificantOther.Name);
                Pregnant = true;    
            }

            return false;
        }
    }
}
