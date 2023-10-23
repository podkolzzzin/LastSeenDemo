import unittest
from datetime import datetime, timedelta

from format_time import format_time

class TestFormatTime(unittest.TestCase):
    def test_When_NowEqualsLastSeen_Expect_Online(self):
        now = datetime.now()
        last_seen = now
        result = format_time(now, last_seen)
        self.assertEqual(result, "Online")

    def test_When_SpanLessThan30Seconds_Expect_JustNow(self):
        now = datetime.now()
        last_seen = now - timedelta(seconds=15)
        result = format_time(now, last_seen)
        self.assertEqual(result, "Just Now")

    def test_When_SpanLessThan1Minute_Expect_LessThanAMinuteAgo(self):
        now = datetime.now()
        last_seen = now - timedelta(seconds=45)
        result = format_time(now, last_seen)
        self.assertEqual(result, "Less than a minute ago")

    def test_When_SpanLessThan1Hour_Expect_CoupleOfMinutesAgo(self):
        now = datetime.now()
        last_seen = now - timedelta(minutes=45)
        result = format_time(now, last_seen)
        self.assertEqual(result, "Couple of minutes ago")

    def test_When_SpanLessThan2Hours_Expect_AnHourAgo(self):
        now = datetime.now()
        last_seen = now - timedelta(minutes=90)
        result = format_time(now, last_seen)
        self.assertEqual(result, "an hour ago")

    def test_When_SameDay_Expect_Today(self):
        now = datetime.now()
        last_seen = datetime(now.year, now.month, now.day, 10, 0)
        result = format_time(now, last_seen)
        self.assertEqual(result, "today")

    def test_When_Yesterday_Expect_Yesterday(self):
        now = datetime.now()
        last_seen = now - timedelta(days=1)
        result = format_time(now, last_seen)
        self.assertEqual(result, "yesterday")

    def test_When_SpanLessThan1Week_Expect_ThisWeek(self):
        now = datetime.now()
        last_seen = now - timedelta(days=3)
        result = format_time(now, last_seen)
        self.assertEqual(result, "this week")

    def test_When_SpanMoreThan1Week_Expect_LongTimeAgo(self):
        now = datetime.now()
        last_seen = now - timedelta(days=10)
        result = format_time(now, last_seen)
        self.assertEqual(result, "long time ago")

if __name__ == '__main__':
    unittest.main()
