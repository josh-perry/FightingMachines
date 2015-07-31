using System.Collections.Generic;

namespace GenerationTest
{
    public class LifeEvent
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }

        public Person MainPerson { get; set; }
        public List<Person> RelatedPeople { get; set; }
    }
}