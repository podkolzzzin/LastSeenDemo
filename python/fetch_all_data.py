import requests

def fetch_all_data(base_url):
    offset = 0
    all_data = []

    while True:
        url = f"{base_url}?offset={offset}"
        response = requests.get(url)

        if response.status_code == 200:
            data = response.json()
            if data["data"] and len(data['data']) > 0:
                all_data.extend(data["data"])
                offset += len(data["data"])
            else:
                break
        else:
            print(f"Failed to fetch data from {url}")
            break

    return all_data