namespace FightingMachines.LifeEvents
{
    class Death : LifeEvent
    {
        public Death(Person mainPerson = null)
        {
            Title = "Death";

            MainPerson = mainPerson;
            Year = TimeManager.Year;
        }

        public new string Description
        {
            get { return $"{MainPerson.Name} died of natural causes in the year {TimeManager.Year} at age {MainPerson.Age}"; }
            set { }
        }
    }
}
