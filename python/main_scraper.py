# -*- coding: utf-8 -*-

import redis
import json
import time
from scraper import Scraper

scraper = Scraper()
cache = redis.StrictRedis(host="redis", port=int(6379),
                decode_responses=True)

while True:
    scraper.browser.get("https://www.saucedemo.com/")

    # Put here a xpath element that has to be present in order to continue
    scraper.wait_page_to_load_infinite("/html/body/div/div/div[2]/div[1]/div[1]/div/form")

    # Login
    scraper.fill_element_by_xpath('//*[@id="user-name"]', "standard_user")
    scraper.fill_element_by_xpath('//*[@id="password"]', "secret_sauce")
    scraper.click_element_by_xpath('//*[@id="login-button"]')

    scraper.wait_page_to_load_infinite('//*[@id="add-to-cart-sauce-labs-backpack"]')

    items_data = {"data": []}
    elements = scraper.get_elements_by_class_name("inventory_item_description")
    for element in elements:
        title = element.find_element_by_class_name("inventory_item_name").text
        description = element.find_element_by_class_name("inventory_item_desc").text
        price = element.find_element_by_class_name("inventory_item_price").text
        items_data["data"].append(
            {"title": title, "description": description, "price": price}
        )

    items_data_string = json.dumps(items_data)
    print(items_data_string)
    cache.set("items", items_data_string)
    time.sleep(60)