using System.Collections.Generic;

namespace FightingMachines
{
    /// <summary>
    /// Significant events that occur in a person's life.
    /// </summary>
    public class LifeEvent
    {
        /// <summary>
        /// Name of the event.
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// The year the event occured in.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// A more detailed description of the event.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The person the life event is relevant to.
        /// </summary>
        public Person MainPerson { get; set; }

        /// <summary>
        /// Other people related to the event that aren't the main person in question.
        /// </summary>
        public List<Person> RelatedPeople { get; set; }
    }
}