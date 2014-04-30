using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnumTest
{

    public class enumClassEnumerator : IEnumerator<int>
    {
        public int Current
        {
            get { return currIt.item; }
        }

        public void Dispose()
        {
            return;
        }

        object System.Collections.IEnumerator.Current
        {
            get { return this; }
        }

        public bool MoveNext()
        {
            if (currIt.next == null)
            {
                return false;
            }
            else
            {
                currIt = currIt.next;
                return true;
            }
        }

        public void Reset()
        {
            currIt = head;
        }

        public void Add(int x)
        {
            if (head == null)
            {
                head = new node();
                currIt = head;
            }
            node it = head;
            while (it.next != null)
            {
                it = it.next;
            }
            node newNode = new node();
            newNode.item = x;
            it.next = newNode;
        }

        private node head = null;
        private node currIt = null;

        private class node
        {
            public int item { get; set; }
            public node next { get; set; }
        }

    }

    public class enumClass : IEnumerable<int>, IOrderedEnumerable<int>
    {
        private enumClassEnumerator _myEnumerator { get; set; }

        public enumClass()
        {
            _myEnumerator = new enumClassEnumerator();
        }

        public void Add(int x)
        {
            _myEnumerator.Add(x);
        }

        public IEnumerator<int> GetEnumerator()
        {
            _myEnumerator.Reset();
            return _myEnumerator;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            _myEnumerator.Reset();
            return _myEnumerator;
        }

        public IOrderedEnumerable<int> CreateOrderedEnumerable<TKey>(Func<int, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            return CreateOrderedEnumerable<int>(x => _myEnumerator.Current, Comparer<int>.Default, false);
        }
    }

    class Program
    {
        public static void printmyEnum(enumClass eclass)
        {
            int i = 0;
            foreach (var item in eclass)
            {
                Console.WriteLine(++i + ", item is " + item);
            }
        }
         
        public static void printEnum(IEnumerable<int> list)
        {
            int i = 0;
            foreach (var item in list)
            {
                Console.WriteLine(++i + ", item is " + item);
            }
        }

        static void Main(string[] args)
        {
            enumClass _myEnum = new enumClass();
            _myEnum.Add(3);
            _myEnum.Add(6);
            _myEnum.Add(9);
            _myEnum.Add(2);
            _myEnum.Add(-4);
            _myEnum.Add(-2);

            printmyEnum(_myEnum);

            Console.WriteLine("Now sorting...");
            var single = _myEnum.Where(x => x == -2).Single();
            Console.WriteLine("Got " + single);
            var sorted = _myEnum.OrderBy(x => x);
            printEnum(sorted);
        }
    }
}
