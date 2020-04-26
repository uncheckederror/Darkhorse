# DarkHorse
 A modern front-end for the Land Information System.

# Update 4/26/2020
This week the Real Account Tax Year page was largely completed.

You can review it by Searching for Real Account, selecting it, and then using the "Details" dropdown to select the "Tax Years" page.

This page is quite a complex creation because it's content is so core to core to correctly administering property taxes.

This is yet another page with many tabs. Each tab typically takes an equal amount of work to implement as the main panel of the page.

Additionally there are many button in the tabs that link out to other pages in the application. For right now I am not implementing any of the Override buttons but I am creating the other pages that don't lead to places where you can edit values.

On the backend we've started revising the data models to better match with the data types present in database. Specifically we are not marking some fields as nullable as they are in the database.
