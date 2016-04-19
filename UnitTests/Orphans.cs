using FightingMachines;

using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class Orphans
    {
        [Test]
        public void Test_CheckOrphanStatus_BothParentsAlive_NotOrphaned()
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

            var child = mother.MakeBaby(father);
            
            // Act
            child.CheckOrphanStatus();

            // Assert
            Assert.AreEqual(mother.Dead, false);
            Assert.AreEqual(father.Dead, false);
            Assert.AreEqual(child.Orphaned, false);
        }

        [Test]
        public void Test_CheckOrphanStatus_OneParentAlive_NotOrphaned()
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

            var child = mother.MakeBaby(father);

            mother.Dead = true;

            // Act
            child.CheckOrphanStatus();

            // Assert
            Assert.AreEqual(mother.Dead, true);
            Assert.AreEqual(father.Dead, false);
            Assert.AreEqual(child.Orphaned, false);
        }

        [Test]
        public void Test_CheckOrphanStatus_BothParentsDead_Orphaned()
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

            var child = mother.MakeBaby(father);

            mother.Dead = true;
            father.Dead = true;

            // Act
            child.CheckOrphanStatus();

            // Assert
            Assert.AreEqual(mother.Dead, true);
            Assert.AreEqual(father.Dead, true);
            Assert.AreEqual(child.Orphaned, true);
        }

        [Test]
        public void Test_CheckOrphanStatus_NullParents_Orphaned()
        {
            // Arrange
            var child = new Person
            {
                Hair = new BrownHair(),
                Eyes = new BlueEyes(),
                Name = "Baby"
            };

            // Act
            child.CheckOrphanStatus();

            // Assert
            Assert.AreEqual(child.Orphaned, true);
        }
    }
}