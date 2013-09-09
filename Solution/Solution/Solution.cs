using System;
using System.Collections.Generic;

/// Author name: Ganesh Chandrasekaran
/// E-mail: gxc4795@rit.edu
/// 
/// Program to write a generic class that implements chained hashtable.


namespace Solution
{
    /// <summary>
    /// An exception used to indicate a problem with how
    /// a HashTable instance is being accessed
    /// </summary>
    public class NonExistentKey<Key> : Exception
    {
        /// <summary>
        /// The key that caused this exception to be raised
        /// </summary>
        public Key BadKey { get; private set; }

        /// <summary>
        /// Create a new instance and save the key that
        /// caused the problem.
        /// </summary>
        /// <param name="k">
        /// The key that was not found in the hash table
        /// </param>
        public NonExistentKey(Key k) :
            base("Non existent key in HashTable: " + k)
        {
            BadKey = k;
        }
    }

    /// <summary>
    /// An associative (key-value) data structure.
    /// A given key may not appear more than once in the table,
    /// but multiple keys may have the same value associated with them.
    /// Tables are assumed to be of limited size are expected to automatically
    /// expand if too many entries are put in them.
    /// </summary>
    /// <param name="Key">the types of the table's keys (uses Equals())</param>
    /// <param name="Value">the types of the table's values</param>
    public interface Table<Key, Value> : IEnumerable<Key>
    {
        /// <summary>
        /// Add a new entry in the hash table. If an entry with the
        /// given key already exists, it is replaced without error.
        /// put() always succeeds.
        /// (Details left to implementing classes.)
        /// </summary>
        /// <param name="k">the key for the new or existing entry</param>
        /// <param name="v">the (new) value for the key</param>
        void Put(Key k, Value v);

        /// <summary>
        /// Does an entry with the given key exist?
        /// </summary>
        /// <param name="k">the key being sought</param>
        /// <returns>true iff the key exists in the table</returns>
        bool Contains(Key k);

        /// <summary>
        /// Fetch the value associated with the given key.
        /// </summary>
        /// <param name="k">The key to be looked up in the table</param>
        /// <returns>the value associated with the given key</returns>
        /// <exception cref="NonExistentKey">if Contains(key) is false</exception>
        Value Get(Key k);
    }

    /// <summary>
    /// A class representing an element consisting of the Key and Value. 
    /// </summary>
    /// <typeparam name="Key">The types of the table's keys (uses Equals())</typeparam>
    /// <typeparam name="Value">The types of the table's values</typeparam>
    class Node<Key, Value>
    {
        public Key key;
        public Value value;

        public Node(Key k, Value v)
        {
            key = k;
            value = v;
        }
    }

    /// <summary>
    /// An associative (key-value) data structure that implements the 
    /// given Table interface.
    /// A given key may not appear more than once in the table,
    /// but multiple keys may have the same value associated with them.
    /// Tables are assumed to be of limited size are expected to automatically
    /// expand if too many entries are put in them.
    /// </summary>
    /// <param name="Key">the types of the table's keys (uses Equals())</param>
    /// <param name="Value">the types of the table's values</param>
    class LinkedHashTable<Key, Value> : Table<Key, Value>
    {
        int sizeOfArray;
        double definedThreshold;

        int countOfElements = 0;

        public static List<Node<Key, Value>>[] arrayList;

        /// <summary>
        /// A parametrized constructor which when invoked 
        /// initializes the associative data structure.
        /// </summary>
        /// <param name="size">The initial maximum size of the table</param>
        /// <param name="threshold">The fraction of the table's capacity that when
        /// reached will cause a rebuild of the table to a 50% larger size</param>
        public LinkedHashTable(Int32 size, Double threshold)
        {
            this.definedThreshold = threshold;
            this.sizeOfArray = size;

            arrayList = new List<Node<Key, Value>>[sizeOfArray];
        }

        /// <summary>
        /// Add a new entry in the hash table. If an entry with the
        /// given key already exists, it is replaced without error.
        /// Put() always succeeds.
        /// </summary>
        /// <param name="k">The key for the new or existing entry</param>
        /// <param name="v">The (new) value for the key</param>
        public void Put(Key k, Value v)
        {
            int location = HashFunction(k);

            // If a key does not exist in the data structure
            if (!Contains(k))
            {
                // Creates a new node to be added in the data structure.
                Node<Key, Value> newNode = new Node<Key, Value>(k, v);

                if (arrayList[location] == null)
                {
                    List<Node<Key, Value>> fillerList = new List<Node<Key, Value>>();
                    arrayList[location] = fillerList;
                    arrayList[location].Add(newNode);
                    countOfElements++;
                    if (exceedsThreshold())
                    {
                        resizeTable();
                    }
                }
                else
                {
                    arrayList[location].Add(newNode);
                    countOfElements++;
                    if (exceedsThreshold())
                    {
                        resizeTable();
                    }
                }
            }
            // If a key already exists, then its value is updated with
            // the new value.
            else
            {
                List<Node<Key, Value>> myList = arrayList[location];

                foreach (Node<Key, Value> node in myList)
                {
                    if (node.key.Equals(k))
                    {
                        node.value = v;
                    }
                }
            }
        }

        /// <summary>
        /// Takes the key as parameter and returns the loctaion
        /// of array at which the corressponding key-value pair needs to be added.
        /// </summary>
        /// <param name="k">The key on which hashcode needs to be done</param>
        /// <returns>Location of array at which key-value pair will be added</returns>
        int HashFunction(Key k)
        {
            return (Math.Abs(k.GetHashCode()) % sizeOfArray);
        }

        /// <summary>
        /// Checks if an entry with the given key exists.
        /// </summary>
        /// <param name="k">The key being sought</param>
        /// <returns>true if and only if the key exists in the table</returns>
        public bool Contains(Key k)
        {
            int location = HashFunction(k);

            List<Node<Key, Value>> myList = arrayList[location];

            if (myList == null)
                return false;

            foreach (Node<Key, Value> node in myList)
            {
                if (node.key.Equals(k))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Fetch the value associated with the given key.
        /// </summary>
        /// <param name="k">The key to be looked up in the table</param>
        /// <returns>the value associated with the given key</returns>
        /// <exception cref="NonExistentKey">if Contains(key) is false</exception>
        public Value Get(Key k)
        {
            if (!Contains(k))
            {
                throw new NonExistentKey<Key>(k);
            }

            int location = HashFunction(k);

            Value nodeValue = default(Value);

            List<Node<Key, Value>> myList = arrayList[location];

            // If the given location of array is null
            // exception is thrown as the given key does 
            // not exists in the table.
            if (myList == null)
            {
                throw new NonExistentKey<Key>(k);
            }

            // Iterates each node in the list at given
            // location of array.
            foreach (Node<Key, Value> node in myList)
            {
                if (node.key.Equals(k))
                {
                    nodeValue = node.value;
                }
            }
            return nodeValue;
        }

        /// <summary>
        /// This method checks if the current threshold exceeds defined
        /// threshold.
        /// </summary>
        /// <returns>true if and only if current threshold
        /// exceeds defined threshold.</returns>
        bool exceedsThreshold()
        {
            double getCount = countOfElements;
            double getSize = sizeOfArray;
            double currentThreshold = getCount / getSize;
            if (currentThreshold >= definedThreshold)
                return true;
            else
                return false;
        }

        public static List<Node<Key, Value>>[] newArrayList;

        /// <summary>
        /// Creates a new array 50% larger than the size of current array.
        /// Calls a method to rehash exiting elements of the list in array.
        /// </summary>
        void resizeTable()
        {
            double computeNewSize = (sizeOfArray + (sizeOfArray / 2));
            int newSize = (int)(computeNewSize);

            this.sizeOfArray = newSize;

            newArrayList = new List<Node<Key, Value>>[sizeOfArray];

            for (int i = 0; i < arrayList.Length; i++)
            {
                rehashTable(i);
            }

            // updates existing arrayList to the newly created arraylist
            arrayList = newArrayList;
            // Sets the temporary array list to null to make it 
            // eligible for garbage collection
            newArrayList = null;
        }

        /// <summary>
        /// Rehashes all the elements from old array to new array.
        /// </summary>
        /// <param name="i"></param>
        void rehashTable(int i)
        {
            List<Node<Key, Value>> myList = arrayList[i];

            if (myList == null)
                return;

            foreach (Node<Key, Value> node in myList)
            {
                Key k;
                Value v;

                if (node == null)
                {
                    Console.Write("");
                }
                else
                {
                    k = node.key;
                    v = node.value;
                    rePut(k, v);
                }
            }
        }

        /// <summary>
        /// Add the existing entry in the new hash table.
        /// rePut() always succeeds.
        /// </summary>
        /// <param name="k">The key for the new or existing entry</param>
        /// <param name="v">The (new) value for the key</param>
        void rePut(Key k, Value v)
        {
            int location = HashFunction(k);

            // Creates a new node to be added in the data structure.
            Node<Key, Value> newNode = new Node<Key, Value>(k, v);

            if (newArrayList[location] == null)
            {
                List<Node<Key, Value>> fillerList = new List<Node<Key, Value>>();
                newArrayList[location] = fillerList;
                newArrayList[location].Add(newNode);
            }
            else
            {
                newArrayList[location].Add(newNode);
            }
        }

        /// <summary>
        /// Iterates the Table data-structure
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Key> GetEnumerator()
        {
            // Traverse the entire table array
            for (int i = 0; i < arrayList.Length; i++)
            {
                List<Node<Key, Value>> myList = arrayList[i];

                if (myList == null)
                {
                    Console.Write("");
                }

                else
                {
                    // Traverses each node in the list
                    foreach (Node<Key, Value> node in myList)
                    {
                        Key k = node.key;
                        yield return k;
                    }
                }
            }
        }

        /// <summary>
        /// Implements the IEnumerable interface method
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            // Calls the generic version of enumerator here
            return GetEnumerator();
        }
    }

    //class TestTable
    //{
    //    public static void test()
    //    {
    //    }
    //}

    class TableFactory
    {
        /// <summary>
        /// Create a Table.
        /// (The student is to put a line of code in this method corresponding to
        /// the name of the Table implementor s/he has designed.)
        /// </summary>
        /// <param name="K">the key type</param>
        /// <param name="V">the value type</param>
        /// <param name="capacity">The initial maximum size of the table</param>
        /// <param name="loadThreshold">
        /// The fraction of the table's capacity that when
        /// reached will cause a rebuild of the table to a 50% larger size
        /// </param>
        /// <returns>A new instance of Table</returns>
        public static Table<K, V> Make<K, V>(int capacity, double loadThreshold)
        {
            return (new LinkedHashTable<K, V>(capacity, loadThreshold));
        }
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            Table<String, String> ht = TableFactory.Make<String, String>(4, 0.5);
            ht.Put("Joe", "Doe");
            ht.Put("Jane", "Brain");
            ht.Put("Chris", "Swiss");

            try
            {
                foreach (String first in ht)
                {
                    Console.WriteLine(first + " -> " + ht.Get(first));
                }
                Console.WriteLine("=========================");

                ht.Put("Wavy", "Gravy");
                Console.WriteLine(ht.Get("Wavy"));
                ht.Put("Chris", "Bliss");
                Console.WriteLine(ht.Get("Chris"));
                foreach (String first in ht)
                {
                    Console.WriteLine(first + " -> " + ht.Get(first));
                }
                Console.WriteLine("=========================");

                Console.Write("Jane -> ");
                Console.WriteLine(ht.Get("Jane"));
                Console.Write("John -> ");
                Console.WriteLine(ht.Get("John"));
            }
            catch (NonExistentKey<String> nek)
            {
                Console.WriteLine(nek.Message);
                Console.WriteLine(nek.StackTrace);
            }
            Console.ReadLine();
        }
    }
}
