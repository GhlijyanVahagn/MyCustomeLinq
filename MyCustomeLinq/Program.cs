using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyCustomeLinq
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> list = new List<string>() { "Mercedes", "Bmw", "Toyota", "JEEP", "Toyota" };
            List<Car> ListofCars = new List<Car>()
            {
                new Car{Power=150,Speed=300},
                new Car{Power=160,Speed=310},
                new Car{Power=170,Speed=320},
                new Car{Power=150,Speed=300}
            };

            List<int> ints =new List<int>{ 1, 2, 89,3,-9, 7 };

            var result=list.FirstOrDefault(x => x.Equals("Bmw"));
            var count = list.Count();
            var distinctElements = list.Distinct().ToList();
            var distinctCars = ListofCars.Distinct(new CarComparer());

            int sum=ints.Sum();

            int sumF = ints.Sum(function);

            Console.WriteLine($"Max element is {ints.Max()}");
            Console.WriteLine($"Max element is {ints.Max()}");


            Console.WriteLine($"Min element is {ints.Max(x=>x*9)}");

            var filter=ints.Where(x => x > 20);
            var tt=ints.Take(2);

            var listAll=ints.All(x => x > -10);


            var anyList = ints.Any(x => x > 8);
            Console.Read();

        }

        static int function(int z)
        {
            return z * 2;
        }


        public class CarComparer : IEqualityComparer<Car>
        {
            public bool Equals(Car x, Car y)
            {
                return x.Power == y.Power && y.Speed == x.Speed;
            }
            public int GetHashCode(Car obj)
            {
                return obj.Power + obj.Speed;
            }
        }
        public class Car
        {
            public int Speed { get; set; }
            public int Power { get; set; }
        }
    }

    public static class MyLinqExtension
    {
        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate) where TSource : class
        {
            if (source == null)
                throw new ArgumentNullException();
            var sourceList = source as IList<TSource>;

            if (sourceList != null)
            {
                if (sourceList.Count > 0)
                {
                    foreach (TSource item in sourceList)
                    {
                        if (predicate.Invoke(item))
                            return item;
                    }
                }
            }
            else
            {
                using (IEnumerator<TSource> enummerator = source.GetEnumerator())
                {
                    if (enummerator.MoveNext())
                        return enummerator.Current;
                }
            }
            return default(TSource);
        }
        public static TSource First<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException();
            var sourceList = source as IList<TSource>;

            if (sourceList != null)
            {
                if (sourceList.Count > 0)
                {
                    using (IEnumerator<TSource> enummerator = source.GetEnumerator())
                    {
                        if (enummerator.MoveNext())
                            return enummerator.Current;
                    }
                }
            }
            return default(TSource);
        }
        public static int Count<TSource>(this IEnumerable<TSource> source) where TSource : class
        {

            if (source == null)
                throw new ArgumentNullException("source");
            var sourceList = source as IList<TSource>;
            return sourceList.Count;


        }
        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source) where TSource : class
        {
            if (source == null)
                throw new ArgumentNullException("sourse is null");
            IList<TSource> sourceList = source as IList<TSource>;

            IList<TSource> resultList = new List<TSource>();


            foreach (TSource item in sourceList)
            {
                if (!resultList.Contains(item))
                    resultList.Add(item);
            }
            return resultList;
        }
        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> equalityComparer) where TSource : class
        {
            if (source == null)
                throw new ArgumentNullException("sourse is null");
            IList<TSource> sourceList = source as IList<TSource>;

            var result = new HashSet<TSource>(sourceList, equalityComparer);

            foreach (TSource item in result)
            {
                yield return item;
            }
        }
        public static int Sum(this IEnumerable<int> source)
        {
            if (source == null)
                throw new ArgumentNullException();

            int sum = 0;
            foreach (var item in source)
                sum += item;
            return sum;
        }
        public static int Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            if (source == null)
                throw new ArgumentNullException();
            int sum = 0;
            foreach (var item in source)
            {
                sum += selector.Invoke(item);
            }
            return sum;
        }
        public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source) where TSource : class
        {
            if (source == null)
                throw new ArgumentNullException();

            List<TSource> result = new List<TSource>();
            foreach (TSource item in source)
            {
                result.Add(item);
            }
            return result;
        }
        public static int Max(this IEnumerable<int> source)
        {
            if (source == null)
                throw new ArgumentException();

            IList<int> elements = source as IList<int>;
            if (elements.Count == 0)
                return 0;
            int max = elements[0];
            foreach (var item in elements)
                if (item > max)
                    max = item;
            return max;
        }
        public static int Max(this IEnumerable<int> source, Func<int, int> selector)
        {
            if (source == null)
                throw new ArgumentException();
            int max = (source as IList<int>)[0];
            foreach (int item in source)
            {
                var next = selector.Invoke(item);

                if (next > max)
                    max = next;

            }

            return max;
        }
        public static int Min(this IEnumerable<int> source)
        {
            if (source == null)
                throw new ArgumentException();

            IList<int> elements = source as IList<int>;
            if (elements.Count == 0)
                return 0;

            int min = elements[0];
            foreach (var item in elements)
                if (item < min)
                    min = item;

            return min;
        }
        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> sources, Predicate<TSource> predicate)
        {
            if (sources == null)
                throw new ArgumentException();
            foreach (TSource item in sources)
            {
                if (predicate.Invoke(item))
                    yield return item;
            }
        }
        public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> sources, int count)
        {
            if (sources == null)
                throw new ArgumentException();

            IList<TSource> sourceList = sources as IList<TSource>;
            if (sourceList.Count < count)
                throw new IndexOutOfRangeException();
            for (int i=0;i<count;i++)
            {
               yield return sourceList[i];
            }
          

        }
        public static IEnumerable<TSource> All<TSource>(this IEnumerable<TSource> sources,Predicate<TSource> predicate)
        {
            if (sources == null)
                throw new ArgumentException();
            IList<TSource> sourceList = sources as IList<TSource>;
            bool allIsOk = true;
            for (int i=0;i< sourceList.Count;i++)
            {
                if (!predicate.Invoke(sourceList[i]))
                {
                    allIsOk = false;
                    break;
                }
            }
            if(allIsOk)
            {
                return sourceList;
            }
            return null;
            
        }
        public static bool Any<TSource>(this IEnumerable<TSource> sources)
        {
            if (sources == null)
                throw new ArgumentException();

            using (IEnumerator<TSource> enumerator = sources.GetEnumerator())
            {
               return  enumerator.MoveNext();
            }
        }
        public static bool Any<TSource>(this IEnumerable<TSource> sources,Predicate<TSource> predicate)
        {
            if (sources == null)
                throw new ArgumentException();
            
            using (IEnumerator<TSource> enumerator = sources.GetEnumerator())
            {
                while(enumerator.MoveNext())
                {
                    if (predicate.Invoke(enumerator.Current))
                    {
                        return true;
                    }
                }
                
            }
            return false;
        }

    }

}
