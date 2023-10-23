from typing import Union
from fastapi import FastAPI
from fetch_all_data import fetch_all_data
from format_all_data import format_all_data

app = FastAPI() # https://www.datacamp.com/tutorial/introduction-fastapi-tutorial

base_url = "https://sef.podkolzin.consulting/api/users/lastSeen"

@app.get("/")
def read_root():
    return fetch_all_data(base_url)


@app.get("/formatted")
def read_item():
    return format_all_data(fetch_all_data(base_url))

