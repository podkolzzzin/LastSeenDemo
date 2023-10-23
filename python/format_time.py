from datetime import datetime, timedelta

def format_time(now, last_seen):
    span = now - last_seen
    if span == timedelta(0):
        return "Online"
    elif span < timedelta(seconds=30):
        return "Just Now"
    elif span < timedelta(minutes=1):
        return "Less than a minute ago"
    elif span < timedelta(minutes=60):
        return "Couple of minutes ago"
    elif span < timedelta(minutes=120):
        return "an hour ago"
    elif now.date() == last_seen.date():
        return "today"
    elif (now.date() - last_seen.date()) == timedelta(days=1):
        return "yesterday"
    elif span < timedelta(days=7):
        return "this week"
    else:
        return "long time ago"