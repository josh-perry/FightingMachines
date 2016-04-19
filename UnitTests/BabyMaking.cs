using FightingMachines;

using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class BabyMaking
    {
        [Test]
        public void Test_MakeBaby_CorrectInput_CorrectOutput()
        {
            // Arrange
            var father = new Person
            {
                Hair = new BrownHair(),
                Eyes = new BlueEyes(),
                Name = "Papa",
                Gender = Gender.Male
            };

            var mother = new Person
            {
                Hair = new BlondeHair(),
                Eyes = new HazelEyes(),
                Name = "Mama",
                Gender = Gender.Female
            };

            // Act
            var child = mother.MakeBaby(father);

            // Assert
            Assert.AreEqual(mother.Dead, false);
            Assert.AreEqual(father.Dead, false);
            Assert.AreEqual(child.Orphaned, false);

            Assert.AreEqual(child.Mother.Person.Name, "Mama");
            Assert.AreEqual(child.Mother.Person.Gender, Gender.Female);

            Assert.AreEqual(child.Father.Person.Name, "Papa");
            Assert.AreEqual(child.Father.Person.Gender, Gender.Male);
        }
    }
}