# -*- coding: utf-8 -*-

import json
import time

from selenium import webdriver
from selenium.common.exceptions import TimeoutException
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By
from selenium.common.exceptions import NoSuchElementException
from selenium.webdriver.common.desired_capabilities import DesiredCapabilities


class Scraper:
    def __init__(self):
        self.pause = 0.2
        self.timeout = 30
        print("opening automated browser ...")
        with open("appsettings.json") as f:
            appsettings = json.load(f)

        capabilities = (
            webdriver.common.desired_capabilities.DesiredCapabilities.CHROME.copy()
        )
        capabilities["javascriptEnabled"] = True

        options = webdriver.ChromeOptions()
        options.add_argument("--user-agent=" + appsettings["UserAgent"])

        self.browser = webdriver.Remote(
            command_executor="http://chrome:4444/wd/hub",
            desired_capabilities=capabilities,
            options=options,
        )

    def load_url(self, url):
        self.browser.set_page_load_timeout(60)
        load_count = 0
        while load_count < 5:
            try:
                self.browser.get(url)
            except TimeoutException:
                load_count += 1
                continue
            else:
                break

        if load_count < 5:
            return True

        return False

    def page_has_loaded(self):
        page_state = self.driver.execute_script("return document.readyState;")
        return page_state == "complete"

    def wait_page_to_load(self, xpath):
        try:
            element_present = EC.presence_of_element_located((By.XPATH, xpath))
            WebDriverWait(self.browser, self.timeout).until(element_present)
        except TimeoutException:
            print("Timed out waiting for page to load")

        return True

    def wait_page_to_load_finite(self, xpath, n_times):
        load_count = 0

        while load_count < n_times:
            try:
                element_present = EC.presence_of_element_located((By.XPATH, xpath))
                WebDriverWait(self.browser, self.timeout).until(element_present)
            except TimeoutException:
                load_count += 1
                continue
            else:
                break

        if load_count < n_times:
            return True

        return False

    def wait_page_to_load_infinite(self, xpath):
        while True:
            try:
                element_present = EC.presence_of_element_located((By.XPATH, xpath))
                WebDriverWait(self.browser, self.timeout).until(element_present)
            except TimeoutException:
                continue
            else:
                break

        return False

    def get_elements_by_xpath(self, xpath):
        try:
            elements = self.browser.find_elements_by_xpath(xpath)
        except NoSuchElementException:
            return False
        return elements

    def get_element_by_xpath(self, xpath):
        try:
            element = self.browser.find_element_by_xpath(xpath)
        except NoSuchElementException:
            return False
        return element

    def get_elements_by_class_name(self, class_name):
        try:
            elements = self.browser.find_elements_by_class_name(class_name)
        except NoSuchElementException:
            return False
        return elements

    def get_element_by_class_name(self, class_name):
        try:
            element = self.browser.find_elements_by_class_name(class_name)
        except NoSuchElementException:
            return False
        return element

    def fill_element_by_xpath(self, xpath, text):
        element = self.get_element_by_xpath(xpath)
        element.send_keys(text)
        time.sleep(self.pause)

    def click_element_by_xpath(self, xpath):
        element = self.get_element_by_xpath(xpath)
        element.click()
        time.sleep(self.pause)

    def check_element_by_xpath(self, xpath):
        element = self.get_element_by_xpath(xpath)
        if not element.get_attribute("checked"):
            element.click()
        time.sleep(self.pause)

    def __del__(self):
        print("closing automated browser ...")
        self.browser.quit
