# DarkHorse
 A modern front-end for the Land Information System.
 
# Update 5/3/2020
This week we rebuilt the Real Property Prepayment Calculator. This is the first page that directly calls to a stored procedure. 

Stored procedures are complex chunks of code written in SQL. It would take significant effort to rewrite these stored procedures in C#.

Initially we had planning on not rewritten them, and this week were sucessfully able to prove that we won't have to rewrite them. This should shave about a month off of the schedule for this project.

Although it's purposely disabled at the moement, we built out the pages for prorating a tax year.

We also built out the payments page and the receiepts and refunds pages. The goal here was to make all of the buttons on the tax years page work. These can all be found on the "Balance" tab of the tax years page.

To that end we also completed the SSWM, FFP, Nox Weed, and Other Assessments type lookup pages which can be found on the "Assessments" tab on the tax years page.

There are still some buttons that don't do anything on the tax years page. This is because they are editing operations, which is a feature we are going to ignore for now because we don't want to change any values in the database yet.

We also completed significant work on the Real Account Filtering page. Searching by contact name, account group, address, and section, township, and range, and tag codes are now enabled.

# Update 4/26/2020
This week the Real Account Tax Year page was largely completed.

You can review it by Searching for Real Account, selecting it, and then using the "Details" dropdown to select the "Tax Years" page.

This page is quite a complex creation because it's content is so core to core to correctly administering property taxes.

This is yet another page with many tabs. Each tab typically takes an equal amount of work to implement as the main panel of the page.

Additionally there are many button in the tabs that link out to other pages in the application. For right now I am not implementing any of the Override buttons but I am creating the other pages that don't lead to places where you can edit values.

On the backend we've started revising the data models to better match with the data types present in database. Specifically we are not marking some fields as nullable as they are in the database.
