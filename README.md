# Assignment 5 - Update the Assignment 1 with Wines to use a database instead of a CSV

## Due 11-24-2015

## Author
Zachary Dwyer

## Description
A command line program that accesses a database filled with Beverage objects. 

# Features
This program allows you to manipulate its connected database by searching, listing, adding, altering and removing beverages.

### Notes
* This program will, by default (unless the dontAddBeveragesWithTheSameID variable is set to true), will not add beverages to the database with duplicate IDs. I figured that all ID's needed to be unique.

## Outside Resources Used
* Using "params" to dynamically add multiple values [http://blogs.msdn.com/b/csharpfaq/archive/2004/05/13/how-do-i-write-a-method-that-accepts-a-variable-number-of-parameters.aspx]
* Working with the "out" parameter [http://stackoverflow.com/questions/4740193/is-there-a-way-to-omit-out-parameter]
* How the && operator doesn't evaluate subsequent expressions if the first one evaluates false [https://msdn.microsoft.com/en-us/library/2a723cdk.aspx]
* General information on lambda expressions [https://msdn.microsoft.com/en-us/library/bb397687.aspx]

## Known Problems, Issues, And/Or Errors in the Program
* None known
