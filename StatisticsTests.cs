using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace linq_slideviews;

[TestFixture]
public class StatisticsTests
{
	[Test]
	public void ManyVisitsOfOneUser()
	{
		var time = DateTime.Now;
		var visits = new List<VisitRecord>
		{
			new VisitRecord(1, 10, time, SlideType.Exercise),
			new VisitRecord(1, 11, time + TimeSpan.FromMinutes(2), SlideType.Exercise),
			new VisitRecord(1, 12, time + TimeSpan.FromMinutes(4), SlideType.Exercise),
			new VisitRecord(1, 13, time + TimeSpan.FromMinutes(8), SlideType.Quiz),
			new VisitRecord(1, 14, time + TimeSpan.FromMinutes(100), SlideType.Exercise)
		};
		var median = StatisticsTask.GetMedianTimePerSlide(visits, SlideType.Exercise);
		Assert.AreEqual(2, median, 1e-5);
	}

	[Test]
	public void NoVisits()
	{
		var median = StatisticsTask.GetMedianTimePerSlide(new List<VisitRecord>(), SlideType.Exercise);
		Assert.AreEqual(0, median, 1e-5);
	}

	[Test]
	public void NoMatchingVisits()
	{
		var time = DateTime.Now;
		var visits = new List<VisitRecord>
		{
			new VisitRecord(1, 10, time, SlideType.Quiz),
			new VisitRecord(1, 11, time + TimeSpan.FromMinutes(2), SlideType.Quiz)
		};
		var median = StatisticsTask.GetMedianTimePerSlide(visits, SlideType.Exercise);
		Assert.AreEqual(0, median, 1e-5);
	}

	[Test]
	public void TwoUsers()
	{
		var time = DateTime.Now;
		var visits = new List<VisitRecord>
		{
			new VisitRecord(1, 10, time, SlideType.Exercise),
			new VisitRecord(1, 11, time + TimeSpan.FromMinutes(2), SlideType.Exercise),
			new VisitRecord(2, 13, time + TimeSpan.FromMinutes(100), SlideType.Exercise),
			new VisitRecord(2, 14, time + TimeSpan.FromMinutes(110), SlideType.Exercise),
			new VisitRecord(2, 15, time + TimeSpan.FromMinutes(120), SlideType.Exercise)
		};
		var median = StatisticsTask.GetMedianTimePerSlide(visits, SlideType.Exercise);
		Assert.AreEqual(10, median, 1e-5);
	}

	[Test]
	public void TwoVisits()
	{
		var time = DateTime.Now;
		var visits = new List<VisitRecord>
		{
			new VisitRecord(1, 10, time, SlideType.Exercise),
			new VisitRecord(1, 11, time + TimeSpan.FromMinutes(2), SlideType.Exercise)
		};
		var median = StatisticsTask.GetMedianTimePerSlide(visits, SlideType.Exercise);
		Assert.AreEqual(2, median, 1e-5);
	}

    [Test]
    public void TestOnRealFiles()
    {
        var separators = new char[] { '\r', '\n' };
        var visits = File.ReadAllText("visits.txt").Split(separators, StringSplitOptions.RemoveEmptyEntries);
        var slides = File.ReadAllText("slides.txt").Split(separators, StringSplitOptions.RemoveEmptyEntries);
        var parsedSlides = ParsingTask.ParseSlideRecords(slides);
        var parsedVisits = ParsingTask.ParseVisitRecords(visits, parsedSlides);
        var median = StatisticsTask.GetMedianTimePerSlide(parsedVisits.ToList(), SlideType.Exercise);
        Assert.AreEqual(8.3333333333333339d, median, 1e-5);
    }
}