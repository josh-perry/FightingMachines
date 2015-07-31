namespace FightingMachines
{
    public class Relationship
    {
        public Relation RelationType;
        public int Friendliness = 0;

        public Person Person;
    }

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
