using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class StatisticsTask
{
	public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
	{
		return visits.GroupBy(visit => visit.UserId)
			.SelectMany(visit => visit.OrderBy(visit => visit.DateTime).Bigrams())
			.Where(visit => visit.First.SlideType == slideType
			&& visit.Second.DateTime.Subtract(visit.First.DateTime).TotalMinutes >= 1
			&& visit.Second.DateTime.Subtract(visit.First.DateTime).TotalMinutes <= 120)
			.Select(visit => visit.Second.DateTime.Subtract(visit.First.DateTime).TotalMinutes)
			.DefaultIfEmpty()
			.Median();
	}
}