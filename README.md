![repository-open-graph-template](https://user-images.githubusercontent.com/11726956/166069932-69108c24-06d5-45ff-b9eb-2b4e08956c28.png)

# DarkHorse
 A modern front-end for the Land Information System.
 
# Update 6/30/2020
Due to other commitments development stopped on this project in late May. I am picking back up on it now. It seems unlikely that any of this will ever be used so I am trying to develop alternative uses for this code base. [ABStreet](https://github.com/dabreegster/abstreet) is a pretty neat project so perhaps I can find a way to transition this app in that direction.
 
# Update 5/17/2020
This week we finished building out the Cadastral page which is both a look up for existing cadastral actions and a secondary page for creating new actions and walking through all of the steps required to complete a cadastral action.

Then we built out the Non-profit page. Interestingly the page doesn't appear to get much use as the most recent entry in the database for this page is from 2004.

One of the key peice of feedback we recieved at the inital Darkhorse demo was that we hadn't developed a strategy to handle the reporting aspect of LIS. I would say the reports are roughly half the code base of this application so that was a pretty big hole in the concept of Darkhorse.

To remedy this problem we wrote a client for the existing Oracle Reporting system that allows us to run and capture any of the reports designed for the existing system inside of Darkhorse.

Thanks to this new capability we were able to start building out the Real Property Reports page and hooking up the reports to these inputs.

The reporting system is quite crufty and difficult to maintain. After Darkhorse goes live we'll have to come back and rebuild these reports in SQL Server Reporting Service.

# Update 5/10/2020
This week we built out the Contacts and Real Property Contacts pages.

LIS has the concept of a contact that is common to all account types. It then relates that contact to a specific account type, like a real property account.

This means we can do cool things like find all of the contacts related to a specific account or find all of the different accounts related to a specific contacts.

We also spent some time revising the Real Accounts page and the Search page to improve the look. We added the zoning codes and tax service information which was missing from older version of the Accounts page.

In the account "Details" menu we built out the "Account History by Owner" page. This page is list of contacts that have been associated with a Real Property Account. It links out to the page for that specific contact. It also maintains a list of Account Groups that the owner's account was a part of.

To enable this linking we built out the Account Groups page. It contains basic contact information about the account group.

Then we built out the Section, Township, and Range page. This page allows you to lookup a specific entry and then see all of the plats associated with it. We also enabled searching by partial section, township, and range values. 

It seemed odd that the lookup stopped at the plat; so to make this page more useful we've added links for each plat to take you to a list of accounts related to that plat in the stand-alone Plats Search application.

Finally we started building out the Cadastral Actions page which is very similar in concept to the Section, Township, and Range page.
 
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

# Why Darkhorse?

Darkhorse is a clone of Kitsap County’s Land Information System (LIS).

Fundamentally, the administration and collection of property tax is a complex business. Each county has a different set of rules and institutional business processes to consider. This means that every county has a slightly different tax administration system. These customizations may not seem that important as taxes have been collected consistently for most of human history; But they have a big impact on operating costs, employee happiness, and the County’s ability to provide a high-quality service to the public.

If you’re familiar with software development you’ll enjoy this read on internal politics on the Microsoft Excel team in the mid-1990’s: https://www.joelonsoftware.com/2001/10/14/in-defense-of-not-invented-here-syndrome/

For everyone else the key point is this:

**If it’s a core business function — do it yourself, no matter what.**

Pick your core business competencies and goals, and do those in house. If you’re a software company, writing excellent code is how you’re going to succeed. Go ahead and outsource the company cafeteria and the CD-ROM duplication. If you’re a pharmaceutical company, write software for drug research, but don’t write your own accounting package. If you’re a web accounting service, write your own accounting package, but don’t try to create your own magazine ads. If you have customers, never outsource customer service.

The core business of the County is to assess and collect property tax. Those tax dollars enable all the other services we provide and business we engage in.

Currently Kitsap County uses LIS to enable the assessment of taxes and the collection of taxes. This system was developed by a private contractor specifically for use by Kitsap County and its partially owned by the County. We have copies of its source code and its database is hosted on premises.

Unfortunately, the technology it’s built on is now wildly outdated and highly insecure. Additionally, there are more than a decade’s worth of integrations that rely on consistent access to this system.

A few years ago, the County decided that it was time to replace LIS. They brought in a private contracting group that over the course of the past 3 years worked on developing a replacement. In parallel Kitsap County began devoting significant resources towards building custom software integrations against this contractor’s product and providing feedback to them.
This process has been difficult. There are many reasons, but essentially problem is that the contractor took to many technical risks and that led to extreme schedule creep and mis-aligned expectations.

To directly address this problem Darkhorse was created as a minimum risk pathway towards a modern tax administration system. It is a direct clone of LIS, but it is built using the latest tooling and technologies from Microsoft. People new to Darkhorse will find that all the buttons and screens they use every day in LIS have been painstakingly recreated in Darkhorse. Initially it will reuse the database and infrastructure of the existing system, LIS. Once we can prove to ourselves that it works as intended, we can transition from the existing LIS desktop application at our own pace.

Because we are retaining the same database, the software customizations and integrations that have been built over the lifetime of LIS will continue to work without interruption when Darkhorse goes live. There won’t be a migration to perform and if necessary, the existing LIS desktop application will continue working as a fall back.

We are developing Darkhorse from the start out in the open. If you want to see what work is being done or check on the status of our efforts, you can review the code, bugs, and Kanban board for yourself here on Github.
