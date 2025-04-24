using System.Diagnostics.CodeAnalysis;
using SC.Common.Interfaces;

namespace SC.Common.Helpers.PassThrus
{
    /// <summary>
    /// Passes the IRandomizer calls directly through to the static random class
    /// Does not require unit testing
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PassthruRandomizer : IRandomizer
    {
        private readonly Random _random = new Random();

        public double GetDouble()
        {
            return _random.NextDouble();
        }
        public int GetIntLessThan(int maximum)
        {
            return _random.Next(maximum);
        }

        public int GetIntBetween(int minimum, int maximum)
        {
            return _random.Next(minimum, maximum);
        }

        private const string Alphanumerics = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public string GetRandomAlphanumeric(int minimum, int maximum)
        {
            var length = GetIntBetween(minimum, maximum);
            return GetRandomAlphanumeric(length);
        }

        public string GetRandomAlphanumeric(int length)
        {
            var chars = new char[length];
            for (int index = 0; index < length; index++)
                chars[index] = (char)Alphanumerics[GetIntLessThan(Alphanumerics.Length)];

            return new String(chars);
        }

        public TEnum GetRandomEnum<TEnum>()
            where TEnum : Enum
        {
            var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
            return values[this.GetIntLessThan(values.Count)];
        }

        public List<T> CreateRandomlySortedList<T>(IList<T> input)
        {
            var tupleList = new List<Tuple<double, T>>(input.Select(i => new Tuple<double, T>(GetDouble(), i)));
            return tupleList.OrderBy(tl => tl.Item1).Select(tl => tl.Item2).ToList();
        }
    }
}
