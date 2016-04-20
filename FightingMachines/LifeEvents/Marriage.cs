using System;
using System.Collections.Generic;

namespace FightingMachines.LifeEvents
{
    public class Marriage : LifeEvent
    {
        public Marriage(Person mainPerson = null, List<Person> people = null)
        {
            Title = "Marriage";

            MainPerson = mainPerson;
            RelatedPeople = people;
        }

        public new string Description
        {
            get
            {
                try
                {
                    return $"{MainPerson.Name} married {MainPerson.Spouse.Person.Name} in the year {Year}.";
                }
                catch (NullReferenceException)
                {
                    return $"{MainPerson.Name} married someone in the year {Year}.";
                }
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
            }
        }
    }
}
