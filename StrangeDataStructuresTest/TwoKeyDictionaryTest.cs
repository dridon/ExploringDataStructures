using System;
using StrangeDataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StrangeDataStructuresTest
{
    /// <summary>
    /// Tests the majority of the methods in the TwoKeyDictionary 
    /// </summary>
    [TestClass]
    public class TwoKeyDictionaryTest
    {
        #region Test Methods
        /// <summary>
        /// Tests that elements are successfully added and
        /// retrieved from the TwoKeyDictionary
        /// </summary>
        [TestMethod]
        public void TestSuccessfulAddAndRetrieve()
        {
            Employee e1 = new Employee("Emma", "emma.powers@mindset.com", "1105010");
            Employee e2 = new Employee("Han", "han.yun@mindset.com", "5530223");

            TwoKeyDictionary<String, String, Employee> twoKC = new TwoKeyDictionary<String, String, Employee>();
            twoKC.Add(e1.ID, e1.Email, e1);
            twoKC.Add(e2.ID, e2.Email, e2);

            // check for employee 1 
            Assert.AreEqual(twoKC.Get1(e1.ID), e1);
            Assert.AreEqual(twoKC.Get2(e1.Email), e1);

            // check for employee 2 
            Assert.AreEqual(twoKC.Get1(e2.ID), e2);
            Assert.AreEqual(twoKC.Get2(e2.Email), e2);
        }

        /// <summary>
        /// Tests to ensure that the cases that should not retrieve anything
        /// indeed do not in the TwoKeyDictionary
        /// </summary>
        [TestMethod]
        public void TestFailedAddAndRetrieve()
        {
            
            Employee e1 = new Employee("Priya", "priya.naria@footloose.com", "8868");
            Employee e2 = new Employee("Bruno", "burno.venus@footloose.com", "1324");

            TwoKeyDictionary<String, String, Employee> twoKC = new TwoKeyDictionary<String, String, Employee>();
            twoKC.Add(e1.ID, e1.Email, e1);

            // cann not retrieve from the wrong key
            Assert.IsNull(twoKC.Get2(e1.ID));       
           
            // ensure a non-existing object has a  default return 
            Assert.IsNull(twoKC.Get1(e2.ID));            
        }

        /// <summary>
        /// Tests to see if the remove methods work correctly in the TwoKeyDictionary
        /// </summary>
        [TestMethod]
        public void TestRemove()
        {
            Employee e1 = new Employee("Priya", "priya.naria@footloose.com", "8868");
            Employee e2 = new Employee("Bruno", "burno.venus@footloose.com", "1324");

            TwoKeyDictionary<String, String, Employee> twoKC = new TwoKeyDictionary<String, String, Employee>();
            twoKC.Add(e1.ID, e1.Email, e1);
            twoKC.Add(e2.ID, e2.Email, e2);

            // test Remove1 and ensure Remove2 doesn't find an object to remove 
            Assert.IsTrue(twoKC.Remove1(e1.ID));
            Assert.IsFalse(twoKC.Remove2(e1.Email));

            // test Remove2 and ensure Remove1 doesn't find an object to remove 
            Assert.IsTrue(twoKC.Remove2(e2.Email));
            Assert.IsFalse(twoKC.Remove1(e2.ID));            
        }

        /// <summary>
        /// Ensures that the ArgumentException in the TwoKeyDictionary is behaving 
        /// properly 
        /// </summary>
        [TestMethod]
        public void TestException()
        {
            Employee e1 = new Employee("Priya", "priya.naria@footloose.com", "8868");
            Employee e2 = new Employee("Priya2", "priya.naria@footloose.com", "8868");
            Employee e3 = new Employee("Priya3", "priya.naria@footloose.com", "8869");

            TwoKeyDictionary<String, String, Employee> twoKC = new TwoKeyDictionary<String, String, Employee>();
            twoKC.Add(e1.ID, e1.Email, e1);
            try
            {
                // this should pass since its an override 
                twoKC.Add(e2.ID, e2.Email, e2);
            }
            catch (ArgumentException e)
            {

                // getting here suggests overriding failed
                Assert.Fail();
            }
            try
            {
                // this should raise an exception since one of the keys already points to someone else
                twoKC.Add(e3.ID, e3.Email, e3);                
                Assert.Fail();
            }
            catch (ArgumentException ae){ /*Test passes here*/}
        }
        #endregion Test Methods

        #region Classes
        // Test class for two key dictionary
        class Employee
        {
            public String Name { get; private set; }
            public String Email { get; private set; }
            public String ID { get; private set; }

            // initialize as immutable object 
            public Employee(String name, String email, String id)
            {
                Name = name; 
                Email = email; 
                ID = id;
            }

            // structural equality 
            public override bool Equals(object obj)
            {
                if(obj is Employee)
                {
                    Employee e = (Employee) obj;
                    return Name == e.Name && Email == e.Email && ID == e.ID;
                }
                return false;
            }

            // hash codes are structurally created 
            public override int GetHashCode()
            {
                int hash = 13;
                hash = hash * 7 + Name.GetHashCode();
                hash = hash * 7 + Email.GetHashCode();
                hash = hash * 7 + ID.GetHashCode();
                return hash; 
            }
        }
        #endregion Classes
    }
}
