using System;

namespace FightingMachines
{
    /// <summary>
    /// A person's eyes.
    /// </summary>
    public abstract class Eyes : Gene
    {
        /// <summary>
        /// Eye colour.
        /// </summary>
        public string Colour;
    }

    /// <summary>
    /// Attribute to mark the eye as selectable randomly.
    /// </summary>
    public class RandomableEyes : Attribute
    {

    }
}
