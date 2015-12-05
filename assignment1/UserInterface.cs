/* 
    Author: Zachary Dwyer
    Class: CIS 237
*/

using System;
using System.Collections.Generic;

namespace assignment1
{

    class UserInterface
    {

        /// <summary>
        /// Writes a single message to the console.
        /// </summary>
        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Displays a menu and get a valid numeric response from the user.
        /// </summary>
        /// <returns>An integer representing the menu choice the user chose.</returns>
        public int DisplayMenuAndGetResponse(MenuItemsSet menuItemsSet, string menuTitle)
        {
            // Print the menu's title
            Console.WriteLine();
            Console.WriteLine(menuTitle);
            Console.WriteLine();

            // (Used to write the menu number. Will be incremented in each pass through the upcoming loop.)
            int currentMenuNumber = 1;

            // Print off each menu item in the menu items set. Write the number before each one and increment for every time you print a menu item.
            foreach (string menuItem in menuItemsSet.MenuItems)
            {
                Console.WriteLine(currentMenuNumber + ". " + menuItem);
                currentMenuNumber++;
            }

            // (This while loop will exit on a return statement)
            while (true)
            {
                // (Used to store the converted numeric user input)
                int numericValidUserInput;

                // Prompt for a menu selection
                string rawUserInput = GetStringResponse("Enter a menu option");

                // If the selection is valid [is it a number? Is that number between 1 and the last item in the list of menu items?]
                if (int.TryParse(rawUserInput, out numericValidUserInput) && numericValidUserInput >= 1 && numericValidUserInput <= menuItemsSet.MenuItems.Count)
                {
                    // We have a valid answer; return the data.
                    return numericValidUserInput;
                }
                else
                {
                    // We don't have a valid answer - display an error
                    DisplayMessage("That is not a valid option. Please make a valid choice.");
                }
            }
        }

        #region Get string/int/decimal/boolean response from the user
        /// <summary>
        /// Query the user for a response
        /// </summary>
        /// <returns>The search query.</returns>
        public string GetStringResponse(string message)
        {
            Console.WriteLine();
            Console.WriteLine(message);
            Console.Write("> ");
            return Console.ReadLine();
        }

        public int GetIntResponse(string message)
        {
            string uncheckedRawString = GetStringResponse(message);         // Get response from user
            int numericUserAnswer = 0;                                      // Numeric response to return

            // While the raw string data does not convert to a number...
            while(! int.TryParse(uncheckedRawString, out numericUserAnswer))
            {
                DisplayMessage("That is not a number. Please input a whole number.");       // Print error
                uncheckedRawString = GetStringResponse(message);                            // Get response from user
            }

            // Return the valid numeric response
            return numericUserAnswer;
        }

        public int GetIntResponse(string message, int lowestNumberAllowed, int highestNumberAllowed)
        {
            string uncheckedRawString = GetStringResponse(message);         // Get response from user
            int numericUserAnswer = 0;                                      // Numeric response to return

            // While the raw string data does not convert to a number, or that number is not between the lowest and highest number allowed...
            while ( ! (int.TryParse(uncheckedRawString, out numericUserAnswer)) || 
                ! (numericUserAnswer >= lowestNumberAllowed) || ! (numericUserAnswer <= highestNumberAllowed))
            {
                // Show error
                DisplayMessage("That is not a number between " + lowestNumberAllowed + " and " + highestNumberAllowed + ". Please try again.");
                uncheckedRawString = GetStringResponse(message);                            // Get response from user
            }

            // Return the valid numeric response
            return numericUserAnswer;
        }

        public decimal GetDecimalResponse(string message)
        {
            string uncheckedRawString = GetStringResponse(message);         // Get response from user
            decimal numericUserAnswer = 0;                                  // Numeric response to return

            // While the raw string data does not convert to a number...
            while (!decimal.TryParse(uncheckedRawString, out numericUserAnswer))
            {
                DisplayMessage("That is not a number. Please input a number.");       // Print error
                uncheckedRawString = GetStringResponse(message);                      // Get response from user
            }

            // Return the valid numeric response
            return numericUserAnswer;
        }

        public decimal GetDecimalResponse(string message, decimal lowestNumberAllowed, decimal highestNumberAllowed)
        {
            string uncheckedRawString = GetStringResponse(message);         // Get response from user
            decimal numericUserAnswer = 0;                                  // Numeric response to return

            // While the raw string data does not convert to a number...
            while (!(decimal.TryParse(uncheckedRawString, out numericUserAnswer)) || !(numericUserAnswer >= lowestNumberAllowed) || !(numericUserAnswer <= highestNumberAllowed))
            {
                DisplayMessage("That is not a number. Please input a number.");       // Print error
                uncheckedRawString = GetStringResponse(message);                      // Get response from user
            }

            // Return the valid numeric response
            return numericUserAnswer;
        }

        /// <summary>
        /// Remember you don't need to type "y/n", this does that for you automatically.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool GetBooleanResponse(string message)
        {
            string uncheckedRawString = GetStringResponse(message + " (y/n)");

            // While the user answer is not "y" or "n"
            while(uncheckedRawString.ToLower().Trim() != "y" && uncheckedRawString.ToLower().Trim() != "n")
            {
                DisplayMessage("That is not a valid choice; please try again.");       // Print error
                uncheckedRawString = GetStringResponse(message + "(y/n)");                       // Get response from user
            }

            if (uncheckedRawString.ToLower().Trim() == "y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// Prints a List of Beverages
        /// </summary>
        public void PrintListOfBeverages(List<Beverage> listOfBeverages)
        {
            // (Used to keep track of the number of results found)
            int resultsFound = 0;

            Console.WriteLine();

            // For each beverage in the list
            foreach (Beverage singularBeverage in listOfBeverages)
            {
                // Print the beverage...
                PrintSingleBeverage(singularBeverage);

                // ...and add one to the beverage count we have found
                resultsFound++;
            }
            Console.WriteLine(Environment.NewLine + resultsFound + " result(s) found.");
            Console.WriteLine();
        }

        public void PrintSingleBeverage(Beverage singularBeverage)
        {
            Console.WriteLine("ID: " + singularBeverage.id);
            Console.WriteLine("Name: " + singularBeverage.name);
            Console.WriteLine("Pack: " + singularBeverage.pack);
            Console.WriteLine("Price: " + singularBeverage.price);
            Console.WriteLine("Active?: " + singularBeverage.active);
            Console.WriteLine();
        }
    }
}
