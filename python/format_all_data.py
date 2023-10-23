from datetime import datetime

from format_time import format_time
import dateutil.parser

def format_all_data(data):
    formatted_results = []

    for item in data:
        last_seen_date_str = item.get("lastSeenDate")
        if last_seen_date_str:
            last_seen_date = dateutil.parser.isoparse(last_seen_date_str)  # Parse ISO 8601 datetime string
            now = datetime.now(last_seen_date.tzinfo)  # Use the same timezone as last_seen_date
            formatted_result = f'User {item.get("nickname")} {format_time(now, last_seen_date)} {last_seen_date.strftime("%Y-%m-%d %H:%M:%S")}'
        else:
            formatted_result = f'User is online'
        formatted_results.append(formatted_result)

    return formatted_results