using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrangeDataStructures
{
    /// <summary>
    /// A two key dictionary class useful for classifying a set A that has two disjoint sets B and C 
    /// that have a 1:1 mapping to it. For example an employee number or an employee name may both
    /// can both uniquely identify an employee profile. 
    /// </summary>
    /// <typeparam name="S"> Type of the first set of keys </typeparam>
    /// <typeparam name="T"> Type of the second set of keys </typeparam>
    /// <typeparam name="R"> Type of the value to be retrieved</typeparam>
    public class TwoKeyDictionary<S,T,R>
    {
        #region Properties
        // The dictionary is actually an implementation of two other dictionaries 
        Dictionary<S, TwoKeyContainer> Dictionary1 { get; set; }
        Dictionary<T, TwoKeyContainer> Dictionary2 { get; set; }
        #endregion Properties 

        #region Constructors
        /// <summary>
        /// Initializes the TwoKeyDictionary 
        /// </summary>
        public TwoKeyDictionary()
        {
            Dictionary1 = new Dictionary<S, TwoKeyContainer>();
            Dictionary2 = new Dictionary<T, TwoKeyContainer>();
        }
        #endregion Constructors

        #region Methods 
        /// <summary>
        /// Adds val to the TwoKeyDictionary with key1 and key2 pointing to it. 
        /// If key1 and key2 two exist already and point to the same value then the value is overridden. 
        /// If only key1 or key2 exist or both exist but point to separate values an 
        /// ArgumentException is thrown
        /// </summary>
        /// <param name="key1"> The first key pointing to val</param>
        /// <param name="key2"> The second key pointing to val</param>
        /// <param name="val"> The value for the keys</param>
        public void Add(S key1, T key2, R val)
        {
            // ensure the keys aren't already in use for unequal values 
            if(!AreKeysValid(key1, key2)) 
                throw new ArgumentException("One or more keys already exist and point to separate values");
            
            // initialize the container and load the dictionaries 
            TwoKeyContainer container = new TwoKeyContainer(key1, key2, val);
            Dictionary1[key1] = container; 
            Dictionary2[key2] = container;
        }

        // key1 and key2 both have to exist in Dictionry1 and Dictionary2 together or 
        // neither have to exist for the keys to be valid 
        private bool AreKeysValid(S key1, T key2)
        {
            // logical biconditional of containment has to be true for the keys to be valid along with 
            // the keys pointing to the same objects 
            return (Dictionary1.ContainsKey(key1) && Dictionary2.ContainsKey(key2) && Dictionary1[key1] == Dictionary2[key2])
                || (!Dictionary1.ContainsKey(key1) && !Dictionary2.ContainsKey(key2));
        }

        /// <summary>
        /// Returns a value if it exists for the given key. Returns the default value for 
        /// the type otherwise. 
        /// </summary>
        /// <param name="key"> The key that maps to the value</param>
        /// <returns> The value if it exists. Default of its type if it does not.</returns>
        public R Get1(S key)
        {
            if (Dictionary1.ContainsKey(key)) 
                return Dictionary1[key].Value;
            return default(R);
        }

        /// <summary>
        /// Returns a value if it exists for the given key. Returns the default value for 
        /// the type otherwise. 
        /// </summary>
        /// <param name="key"> The key that maps to the value</param>
        /// <returns> The value if it exists. Default of its type if it does not.</returns>
        public R Get2(T key)
        {
            if (Dictionary2.ContainsKey(key))
                return Dictionary2[key].Value;
            return default(R);
        }

        /// <summary>
        /// Removes both keys and the value associated with this key 
        /// </summary>
        /// <param name="key">Key mapping to the value to be removed</param>
        /// <returns>true if successfully removed. false if the key did not exist</returns>
        public bool Remove1(S key)
        {
            if (Dictionary1.ContainsKey(key))
            {
                TwoKeyContainer container = Dictionary1[key];
                return Dictionary1.Remove(key) && Dictionary2.Remove(container.Key2);
            }
            return false;
        }

        /// <summary>
        /// Removes both keys and the value associated with this key
        /// </summary>
        /// <param name="key">Key mapping to the value to be removed</param>
        /// <returns>true if successfully removed. false if the key did not exist</returns>
        public bool Remove2(T key)
        {
            if (Dictionary2.ContainsKey(key))
            {
                TwoKeyContainer container = Dictionary2[key];
                return Dictionary1.Remove(container.Key1) && Dictionary2.Remove(key);
            }
            return false;
        }

        #endregion Methods

        #region Classes
        // Facilitates the removal with only one key 
        class TwoKeyContainer
        {
            // the keys and values in  
            public S Key1 { get; private set; }
            public T Key2 { get; private set; }
            public R Value { get; private set; }

            // initialize an immutable container  
            public TwoKeyContainer(S key1, T key2, R val)
            {
                Key1 = key1;
                Key2 = key2;
                Value = val;
            }

            // structural equality 
            public override bool Equals(object obj)
            {
 	             if(obj  is TwoKeyContainer)
                 {
                     TwoKeyContainer twoKC = (TwoKeyContainer) obj;
                     return Key1.Equals(twoKC.Key1) && Key2.Equals(twoKC.Key2) && Value.Equals(twoKC.Value);
                 }
                return false; 
            }

            // hash codes are structurally created 
            public override int GetHashCode()
            {
                int hash = 13; 
                hash = hash*7 + Key1.GetHashCode(); 
                hash = hash*7 + Key2.GetHashCode();
                hash = hash*7 + Value.GetHashCode();
                return hash;
            }
        }
        #endregion Classes
    }
}
