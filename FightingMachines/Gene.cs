namespace FightingMachines
{
    /// <summary>
    /// Abstract base class for things like hair and eyes.
    /// </summary>
    public abstract class Gene
    {
        /// <summary>
        /// Should the gene be more likely to be passed down genereations?
        /// </summary>
        public bool DominantGene;
    }
}
