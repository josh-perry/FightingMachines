namespace FightingMachines
{
    /// <summary>
    /// Defines a relationship between two persons.
    /// </summary>
    public class Relationship
    {
        /// <summary>
        /// What kind of relationship is it?
        /// </summary>
        public Relation RelationType;

        /// <summary>
        /// How much do they like each other?
        /// </summary>
        public int Friendliness = 0;

        /// <summary>
        /// The person in question.
        /// </summary>
        public Person Person;
    }

    /// <summary>
    /// Relationship types.
    /// </summary>
    public enum Relation
    {
        Sibling,
        Spouse,
        Father,
        Mother,
        Child,
        Friend
    }
}
