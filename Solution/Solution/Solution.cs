using System;
using System.Collections.Generic;

namespace Solution
{
    class Node<Key, Value>
    {
        public Key key;
        public Value value;
        public Node<Key, Value> next;

        public Node(Key k, Value v)
        {
            key = k;
            value = v;
            next = null;
        }
    }

    class LinkedHashTable<Key, Value> : Table<Key, Value>
    {
        int sizeOfArray;
        double threshold;

        public static List<Node<Key, Value>>[] arrayList;



        public LinkedHashTable(Int32 size, Double threshold)
        {
            this.threshold = threshold;
            this.sizeOfArray = size;

            arrayList = new List<Node<Key, Value>>[sizeOfArray];
            

        }
        public void Put(Key k, Value v)
        {
            Node<Key, Value> newNode = new Node<Key, Value>(k, v);
            List<Node<Key, Value>> fillerList;
            int location = HashFunction(k);
            if (!Contains(k))
            {
                if (arrayList[location] == null)
                {
                    fillerList = new List<Node<Key, Value>>();
                    arrayList[location] = fillerList;
                    arrayList[location].Add(newNode);
                }
                else
                {
                    arrayList[location].Add(newNode);

                }
            }
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

        public int HashFunction(Key k)
        {
            return (Math.Abs(k.GetHashCode()) % sizeOfArray);
        }

        public bool Contains(Key k)
        {
            int location = HashFunction(k);

           List<Node<Key,Value>> myList = arrayList[location];

           if (myList == null)
               return false;

           try
           {

               foreach (Node<Key, Value> node in myList)
               {
                   if (node.key.Equals(k))
                   {
                       return true;
                   }
               }
           }
           catch (NonExistentKey<Key> nef)
           {
           }

           return false;
        }
        public Value Get(Key k)
        {
            int location = HashFunction(k);

            List<Node<Key, Value>> myList = arrayList[location];
            Value nodeValue=default(Value);
            foreach (Node<Key, Value> node in myList)
            {
                if (node.key.Equals(k))
                {
                    nodeValue = node.value;
                }
            }
            return nodeValue;
        }
    }
    //public IEnumerable<Key> GetEnumerator()
    //{

    //    foreach (Node<Key,Value> node in arrayList) yield return node.key;
    //}


    //class TestTable
    //{
    //    public static void test()
    //    {
    //    }
    //}

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
    public interface Table<Key, Value>
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
        public static Table<K, V> Make<K, V>(int capacity = 100, double loadThreshold = 0.75)
        {
           return (new LinkedHashTable<K, V>(capacity, loadThreshold));
        }

    }
        class MainClass
        {
            public static void Main(String[] args)
            {
                Table<Int32, String> ht = TableFactory.Make<Int32, String>(100, 0.5);
                ht.Put(1, "Hi");
                ht.Put(1, "Hello");
                ht.Put(2, "Height");
                ht.Put(3, "Lolwa");
                ht.Put(2, "HelloKitty");
                Console.WriteLine(ht.Get(1));
                Console.WriteLine(ht.Get(2));
            }
        }

    }


    //class MainClass
    //{
    //    public static void Main(string[] args)
    //    {
    //        Table<String, String> ht = TableFactory.Make<String, String>(4, 0.5);
    //        ht.Put("Joe", "Doe");
    //        ht.Put("Jane", "Brain");
    //        ht.Put("Chris", "Swiss");
    //        try
    //        {
    //            foreach (String first in ht)
    //            {
    //                Console.WriteLine(first + " -> " + ht.Get(first));
    //            }
    //            Console.WriteLine("=========================");

    //            ht.Put("Wavy", "Gravy");
    //            ht.Put("Chris", "Bliss");
    //            foreach (String first in ht)
    //            {
    //                Console.WriteLine(first + " -> " + ht.Get(first));
    //            }
    //            Console.WriteLine("=========================");

    //            Console.Write("Jane -> ");
    //            Console.WriteLine(ht.Get("Jane"));
    //            Console.Write("John -> ");
    //            Console.WriteLine(ht.Get("John"));
    //        }
    //        catch (NonExistentKey<String> nek)
    //        {
    //            Console.WriteLine(nek.Message);
    //            Console.WriteLine(nek.StackTrace);
    //        }
    //        Console.ReadLine();
    //    }
    //}
