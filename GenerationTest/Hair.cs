using System;

namespace GenerationTest
{
    public abstract class Hair : Gene
    {
        public string Colour;
        public int Length; // Centimeters
    }

    public class RandomableHair : Attribute
    {
        
    }
}
