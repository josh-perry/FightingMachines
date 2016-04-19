using System.Collections.Generic;

namespace FightingMachines.LifeEvents
{
    public class ComingOfAge : LifeEvent
    {
        public ComingOfAge(Person mainPerson = null, List<Person> people = null)
        {
            Title = "Coming of Age";

            MainPerson = mainPerson;
            RelatedPeople = people;
        }

        public new string Description
        {
            get
            {
                return $"{MainPerson.Name} came of age in the year {Year}.";
            }
            set { }
        }
    }
}
