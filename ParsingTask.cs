using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace linq_slideviews;

public class ParsingTask
{
	/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
	/// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
	/// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
	public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
	{
		IDictionary<int, SlideRecord> result = lines.Select(line => line.Split(';'))
			.Where(line => line.Length == 3 
						&& Enum.TryParse(line[1], true, out SlideType slideType)
						&& int.TryParse(line[0], out int slideID))
			.ToDictionary(line => int.Parse(line[0]), line => new SlideRecord(int.Parse(line[0]), Enum.Parse<SlideType>(line[1], true), line[2]));
		return result;
	}

	/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
	/// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
	/// Такой словарь можно получить методом ParseSlideRecords</param>
	/// <returns>Список информации о посещениях</returns>
	/// <exception cref="FormatException">Если среди строк есть некорректные</exception>
	public static IEnumerable<VisitRecord> ParseVisitRecords(
		IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
	{
		return lines.Select(line => { string[] splitedLine = line.Split(';'); if (splitedLine.Length != 4) { throw new FormatException($"Wrong line [{line}]"); } return splitedLine; })
			.Skip(1)
			//.Where(line => line.Length > 0 && DateTime.TryParseExact($"{line[2]} {line[3]}", "YYYY-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
			.Where(line => line.Length > 0)
			.Select(line =>
			{
				if (DateTime.TryParseExact($"{line[2]} {line[3]}", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime)
					&& int.TryParse(line[0], out int userID) && int.TryParse(line[1], out int slideID) && slides.ContainsKey(slideID))
				{
					return new VisitRecord(userID, slideID, dateTime, slides[slideID].SlideType);
				}
				else
				{
					throw new FormatException($"Wrong line [{line[0]};{line[1]};{line[2]};{line[3]}]");
				}
			});
	}
}