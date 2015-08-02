using System;
using System.Collections.Generic;

namespace FightingMachines.LifeEvents
{
    public class Birth : LifeEvent
    {
        public Birth(Person mainPerson = null, List<Person> people = null)
        {
            Title = "Birth";

            MainPerson = mainPerson;
            RelatedPeople = people;
        }

        public override string Description
        {
            get
            {
                try
                {
                    return $"{MainPerson.Name} was born to parents {MainPerson.Mother.Person.Name} and {MainPerson.Father.Person.Name} in the year {Year}.";
                }
                catch (NullReferenceException)
                {
                    return $"{MainPerson.Name} was born in the year {Year}.";
                }
            }
            set { }
        }
    }
}
