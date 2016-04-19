using FightingMachines;

using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class DeathOddsUnitTests
    {
        [Test]
        public void Test_GetDeathChance_1yoMale_177Out()
        {
            // Arrange
            // Act
            var odds = DeathOdds.GetDeathChance(1, Gender.Male);

            // Assert
            Assert.AreEqual(odds, 177);
        }

        [Test]
        public void Test_GetDeathChance_1yoFemale_177Out()
        {
            // Arrange
            // Act
            var odds = DeathOdds.GetDeathChance(1, Gender.Female);

            // Assert
            Assert.AreEqual(odds, 227);
        }

        [Test]
        public void Test_GetDeathChance_40yoMale_663Out()
        {
            // Arrange
            // Act
            var odds = DeathOdds.GetDeathChance(40, Gender.Male);

            // Assert
            Assert.AreEqual(odds, 663);
        }

        [Test]
        public void Test_GetDeathChance_40yoFemale_1106Out()
        {
            // Arrange
            // Act
            var odds = DeathOdds.GetDeathChance(40, Gender.Female);

            // Assert
            Assert.AreEqual(odds, 1106);
        }
    }
}