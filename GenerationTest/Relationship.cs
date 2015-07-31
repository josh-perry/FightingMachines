namespace GenerationTest
{
    class Relationship
    {
        public Relation RelationType;
        public int Friendliness = 0;

        public Person Person;
    }

    enum Relation
    {
        Sibling,
        Spouse,
        Father,
        Mother,
        Child,
        Friend
    }
}
