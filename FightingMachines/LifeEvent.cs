using System.Collections.Generic;

namespace FightingMachines
{
    public class LifeEvent
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public virtual string Description { get; set; }

        public Person MainPerson { get; set; }
        public List<Person> RelatedPeople { get; set; }
    }
}