namespace C8S.Common.Interfaces
{
	/// <summary>
	/// Simple interface for abstracting the randomizer so that we can
	/// test it more easily
	/// </summary>
	public interface IRandomizer
	{
		double GetDouble();
		int GetIntLessThan(int maximum);
		int GetIntBetween(int minimum, int maximum);

	  string GetRandomAlphanumeric(int length);
	  string GetRandomAlphanumeric(int minimum, int maximum);

	  TEnum GetRandomEnum<TEnum>()
			where TEnum : Enum;

		List<T> CreateRandomlySortedList<T>(IList<T> input);
	}
}