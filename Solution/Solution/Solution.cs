using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Solution
{
class Node<Key,Value>
        {
           public Key key;
          public  Value value;
            public Node<Key, Value> next;
            

            public Node(Key k, Value v)
            {
                key = k;
                value = v;
                next = null;
            }
        }
    
    class LinkedHashTable<Key, Value>
    {
        static Node<Key, Value>[] arrayList = new Node<Key, Value>[100];

     
        private IEnumerator<Key> enumerator;
        public int position;

              public void Put(Key k, Value v)
            {
                Node<Key, Value> newNode = new Node<Key,Value>(k,v);
                
                    int location = HashFunction(k);
                    if (arrayList[location] == null)
                    {

                        arrayList[location] = newNode;
                    }
                    else
                    {
                        Node<Key,Value> runner = arrayList[location];
                        while (runner.next != null)
                        {
                            runner = runner.next;
                        }
                        runner.next = newNode;
                    }
                   
                
            }

            public int HashFunction(Key k)
            {
                return ((k.GetHashCode())%100);
            }

            public bool Contains(Key k)
            {
                int location = HashFunction(k);

                Node<Key, Value> runner = arrayList[location];


                if ((runner.key).Equals(k))
                {

                    return true;
                }
                else
                {

                    while (runner.next != null)
                    {
                        runner = runner.next;

                        if ((runner.key).Equals(k))
                        {

                            return true;
                        }
                    }

                }
                return false;
            }
            public Value Get(Key k) 
            {
                int location = HashFunction(k);

                Node<Key, Value> runner = arrayList[location];

                while (runner.next != null)
                {
                    if ((runner.key).Equals(k))
                    {

                        return runner.value;
                    }
                    else
                    {

                        while (runner.next != null)
                        {
                            runner = runner.next;

                            if ((runner.key).Equals(k))
                            {

                                return runner.value;
                            }
                        }

                    }
                }
                    return runner.value;
                
            }


           

        }

    public class Test
    {
         static void Main()
            {
                LinkedHashTable<Int32, String> myTable = new LinkedHashTable<int, string>();

                myTable.Put(1, "Hi");
                myTable.Put(1, "Hello");
               Console.WriteLine( myTable.Contains(1));
               Console.WriteLine(myTable.Get(1));
            }
    }

}


          

           
            

        
    

    //class TestTable
    //{
    //    public static void test()
    //    {
    //    }
    //}





    ///// <summary>
    ///// An exception used to indicate a problem with how
    ///// a HashTable instance is being accessed
    ///// </summary>
    //public class NonExistentKey<Key> : Exception
    //{
    //    /// <summary>
    //    /// The key that caused this exception to be raised
    //    /// </summary>
    //    public Key BadKey { get; private set; }

    //    /// <summary>
    //    /// Create a new instance and save the key that
    //    /// caused the problem.
    //    /// </summary>
    //    /// <param name="k">
    //    /// The key that was not found in the hash table
    //    /// </param>
    //    public NonExistentKey(Key k) :
    //        base("Non existent key in HashTable: " + k)
    //    {
    //        BadKey = k;
    //    }

    //}

    ///// <summary>
    ///// An associative (key-value) data structure.
    ///// A given key may not appear more than once in the table,
    ///// but multiple keys may have the same value associated with them.
    ///// Tables are assumed to be of limited size are expected to automatically
    ///// expand if too many entries are put in them.
    ///// </summary>
    ///// <param name="Key">the types of the table's keys (uses Equals())</param>
    ///// <param name="Value">the types of the table's values</param>
    //public interface Table<Key, Value> : IEnumerable<Key>
    //{
    //    /// <summary>
    //    /// Add a new entry in the hash table. If an entry with the
    //    /// given key already exists, it is replaced without error.
    //    /// put() always succeeds.
    //    /// (Details left to implementing classes.)
    //    /// </summary>
    //    /// <param name="k">the key for the new or existing entry</param>
    //    /// <param name="v">the (new) value for the key</param>
    //    void Put(Key k, Value v);

    //    /// <summary>
    //    /// Does an entry with the given key exist?
    //    /// </summary>
    //    /// <param name="k">the key being sought</param>
    //    /// <returns>true iff the key exists in the table</returns>
    //    bool Contains(Key k);

    //    /// <summary>
    //    /// Fetch the value associated with the given key.
    //    /// </summary>
    //    /// <param name="k">The key to be looked up in the table</param>
    //    /// <returns>the value associated with the given key</returns>
    //    /// <exception cref="NonExistentKey">if Contains(key) is false</exception>
    //    Value Get(Key k);
    //}

    //class TableFactory
    //{
    //    /// <summary>
    //    /// Create a Table.
    //    /// (The student is to put a line of code in this method corresponding to
    //    /// the name of the Table implementor s/he has designed.)
    //    /// </summary>
    //    /// <param name="K">the key type</param>
    //    /// <param name="V">the value type</param>
    //    /// <param name="capacity">The initial maximum size of the table</param>
    //    /// <param name="loadThreshold">
    //    /// The fraction of the table's capacity that when
    //    /// reached will cause a rebuild of the table to a 50% larger size
    //    /// </param>
    //    /// <returns>A new instance of Table</returns>
    //    public static Node<K, V> Make<K, V>(int capacity = 100, double loadThreshold = 0.75)
    //    {
            

    //        return null;
    //    }
    //}

    ////class MainClass
    ////{
    ////    public static void Main(string[] args)
    ////    {
    ////        Table<String, String> ht = TableFactory.Make<String, String>(4, 0.5);
    ////        ht.Put("Joe", "Doe");
    ////        ht.Put("Jane", "Brain");
    ////        ht.Put("Chris", "Swiss");
    ////        try
    ////        {
    ////            foreach (String first in ht)
    ////            {
    ////                Console.WriteLine(first + " -> " + ht.Get(first));
    ////            }
    ////            Console.WriteLine("=========================");

    ////            ht.Put("Wavy", "Gravy");
    ////            ht.Put("Chris", "Bliss");
    ////            foreach (String first in ht)
    ////            {
    ////                Console.WriteLine(first + " -> " + ht.Get(first));
    ////            }
    ////            Console.WriteLine("=========================");

    ////            Console.Write("Jane -> ");
    ////            Console.WriteLine(ht.Get("Jane"));
    ////            Console.Write("John -> ");
    ////            Console.WriteLine(ht.Get("John"));
    ////        }
    ////        catch (NonExistentKey<String> nek)
    ////        {
    ////            Console.WriteLine(nek.Message);
    ////            Console.WriteLine(nek.StackTrace);
    ////        }

    ////        Console.ReadLine();
    ////    }
    ////}



