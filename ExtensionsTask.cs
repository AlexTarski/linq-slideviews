using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public static class ExtensionsTask
{
	/// <summary>
	/// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
	/// Медиана списка из четного количества элементов — это среднее арифметическое 
    /// двух серединных элементов списка после сортировки.
	/// </summary>
	/// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
	public static double Median(this IEnumerable<double> items)
	{
		List<double> result = items.OrderBy(x => x).ToList();
		if (result.Count == 0)
		{
			throw new InvalidOperationException("Collection is empty");
		}
		return result.Count % 2 == 0 ? (result[result.Count / 2] + result[(result.Count / 2) - 1]) / 2 : result[result.Count / 2];
	}

	/// <returns>
	/// Возвращает последовательность, состоящую из пар соседних элементов.
	/// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
	/// </returns>
	public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items)
	{
		Queue<T> queue = new Queue<T>();
		foreach (var item in items)
		{
			queue.Enqueue(item);
			if (queue.Count > 1)
			{
				yield return new ValueTuple<T, T>(queue.Dequeue(), item);
            }
		}
	}
}