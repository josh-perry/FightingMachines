using System;

namespace FightingMachines
{
    /// <summary>
    /// A person's hair.
    /// </summary>
    public abstract class Hair : Gene
    {
        /// <summary>
        /// Hair colour.
        /// </summary>
        public string Colour;

        /// <summary>
        /// Length of the hair in centimeters.
        /// </summary>
        public int Length;
    }

    /// <summary>
    /// Attribute to mark the hair as selectable randomly.
    /// </summary>
    public class RandomableHair : Attribute
    {
        
    }
}
