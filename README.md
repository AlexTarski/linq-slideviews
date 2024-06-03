# linqslide

This project collects statistics on slide visits and the median time spent by users on each type of slides.

Input files: slide.txt and visits.txt.

Slide.txt consists of info about slide in next format:
SlideID;SlideType;UnitTitle

Visits.txt consists of info about slide visit in next format:
UserID;SlideID;Date;Time

Working with those files is implemented through 2 parsers: ParseSlideRecords() and ParseVisitRecords().

Main method is GetMedianTimeSlide().
Input - data, prepared by parsers.
Output - <double> median visit time per slide in minutes.
Visit time less than 1 minute and more than 2 hours is ignored.


Median() and Bigrams<T>() are LINQ extension methods.

Median() computes the median of input <double> collection;
Bigrams<T>() makes bigrams (pairs) of input <T> elements.
These methods are necessary for the implementation of the main GetMedianTimePerSlide() method.
