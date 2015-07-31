namespace FightingMachines
{
    static class TimeManager
    {
        public static int Year;
        public static int Month;
        public static int Day;

        public static void AdvanceYear()
        {
            Year++;
        }

        public static void AdvanceMonth()
        {
            Month++;

            if (Month > 12)
            {
                Month = 1;
                AdvanceYear();
            }
        }

        public static void AdvanceDay()
        {
            Day++;

            if (Day >= 30)
            {
                Day = 1;
                AdvanceMonth();
            }
        }
    }
}
