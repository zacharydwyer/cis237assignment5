/* 
    Author: Zachary Dwyer
    Class: CIS 237
*/
using System;
using System.Collections.Generic;
using System.Linq;

namespace assignment1
{
    class Program
    {
        // Private, class-level variables. Initialized in "initializeData" method.
        private static UserInterface userInterface;                     // User interface we will be working with
        private static BeverageZDwyerEntities beverageDatabaseHook;     // Hook into the beverage database
        private static string greeting;                                 // Greeting to display 
        
        private static MenuItemsSet mainMenuItemsSet;                   
        private static MenuItemsSet findItemByWhatMenuItemsSet;         
        private static MenuItemsSet priceMenuItemsSet;
        private static MenuItemsSet findItemByActiveStatus;

        private static bool dontAddBeveragesWithTheSameID;             

        /// <summary>
        /// Initializes the data that the program will be working with.
        /// </summary>
        private static void initializeData()
        {
            // TODO: This data technically has to do with the user interface, but it's specific to this program only - hence why I put it in Program.cs.
            //       The User Interface class is meant to be extremely generic so that it can be utilized more dynamically, so all specific data that
            //       only applies to this single program was written in Program.cs.

            // Object initialization
            userInterface = new UserInterface();
            beverageDatabaseHook = new BeverageZDwyerEntities();

            // Data initialization (don't have to do it here, technically, but figured might as well)
            greeting = "Welcome to the beverage database management system.";
            dontAddBeveragesWithTheSameID = true;                                   // Should we add beverages to the database that have a duplicate id?

            /* MENU ITEM SET INITIALIZATION */
            /* (MenuItemSet objects are used by the DisplayMenuAndGetResponse method in UserInterface to draw a menu) */
            /* (The MenuItemSet constructor can take a variable amount of strings with the params argument specifier I just found out about a few minutes ago lol) */

            // Main menu
            mainMenuItemsSet = new MenuItemsSet
            (
                "Print all beverages",
                "Retreive beverage by property or ID",
                "Add a new beverage",
                "Update an existing beverage",
                "Delete a beverage",
                "Exit program"
            );

            // Find item by property menu
            findItemByWhatMenuItemsSet = new MenuItemsSet
            (
                "ID",
                "Name",
                "Pack",
                "Price",
                "Active status",
                "(Back to main menu)"
            );

            // Find beverages with a price that is... menu
            priceMenuItemsSet = new MenuItemsSet
            (
                "...less than or equal to a given value",
                "...equal to a given value",
                "...more than or equal to a given value"
            );

            // Find by active status menu
            findItemByActiveStatus = new MenuItemsSet
            (
                "Is active",
                "Is not active"
            );
        }
        
        // Print main menu
        static void Main(string[] args)
        {
            // Initialize data
            initializeData();

            // Display greeting to user.
            userInterface.DisplayMessage(greeting);

            // Get the choice from the user
            int choice = userInterface.DisplayMenuAndGetResponse(mainMenuItemsSet, "Main Menu");

            // While the choice was not the last choice (exit)
            while (choice != 6)
            {
                // Observe the choice
                switch (choice)
                {
                    // Print the entire database of items
                    case 1:
                        mainMenu_printEntireDatabase();
                        break;

                    // Retrieve beverage by property or ID
                    case 2:
                        mainMenu_retrieveBeverageByPropertyOrID();
                        break;
                    
                    // Add a new beverage to the database
                    case 3:
                        mainMenu_addNewBeverage();
                        break;

                    // Update an existing beverage
                    case 4:
                        mainMenu_updateExistingBeverage();
                        break;

                    // Delete a beverage
                    case 5:
                        mainMenu_deleteABeverage();
                        break;
                }

                // Get a new menu selection from the user.
                choice = userInterface.DisplayMenuAndGetResponse(mainMenuItemsSet, "Main Menu");
            }

        }

        #region Main Menu methods
        /// <summary>
        /// Print the entire database worth of beverages.
        /// </summary>
        private static void mainMenu_printEntireDatabase()
        {
            userInterface.PrintListOfBeverages(beverageDatabaseHook.Beverages.ToList<Beverage>());
        }

        private static void mainMenu_retrieveBeverageByPropertyOrID()
        {
            // Find out if they want to use a property (and what property) or the ID to retrieve a beverage
            int choiceNum = userInterface.DisplayMenuAndGetResponse(findItemByWhatMenuItemsSet, "Search for an item using its...");

            // (Will store the beverages found)
            List<Beverage> beveragesFound = null;

            // While the choice isn't 6 ("Back to main menu")
            while(choiceNum != 6)
            {
                switch(choiceNum)
                {
                    // ID
                    case 1:
                        string response = userInterface.GetStringResponse("Enter the ID of the beverage you are trying to find");       // Get user response
                        beveragesFound = findBeveragesByID(response);                                                                   // Use user response to query db
                        break;

                    // Name
                    case 2:
                        response = userInterface.GetStringResponse("Enter the name of the beverage you are trying to find");
                        beveragesFound = findBeveragesByName(response);
                        break;

                    // Pack
                    case 3:
                        response = userInterface.GetStringResponse("Enter the pack of the beverage you are trying to find");
                        beveragesFound = findBeveragesByPack(response);
                        break;

                    // Price
                    case 4:
                        beveragesFound = retrieveBeverageByPropertyOrID_priceMenu();            // Price is a little more complicated, I put that in another method.
                        break;

                    // Active status
                    case 5:
                        beveragesFound = retrieveBeverageByActiveStatus_activeStatusMenu();     // Active status is also more complicated. It need to verify the user put in a true or false value.
                        break;

                    // This should never happen
                    default:
                        Console.WriteLine("A serious error has occured in mainMenu_retrieveBeverageBypropertyOrID");
                        break;
                }

                // Print out the found beverages
                userInterface.PrintListOfBeverages(beveragesFound);

                // Ask for the choice again
                choiceNum = userInterface.DisplayMenuAndGetResponse(findItemByWhatMenuItemsSet, "Search for an item using its...");
            }
        }

        private static void mainMenu_addNewBeverage()
        {
            // Get properties
            /* Make sure the ID isn't a duplicate in the system if "dontAddBeveragesWithSameID" is set to true */

            // Holds the ID.
            string id;

            // Ask for the ID for this new beverage
            id = userInterface.GetStringResponse("Enter the ID");

            // If we're supposed to not add beverages with the same ID (true by default), and the retrieved beverage with the given ID is at least 1 (meaining there's already a beverage with that ID)
            while (dontAddBeveragesWithTheSameID == true && findBeveragesByID(id).Count >= 1)
            {
                userInterface.DisplayMessage("There is already a beverage with that ID. Please use a new one.");        // Error
                id = userInterface.GetStringResponse("Enter the ID");                                                   // Ask again
            }
            
            // At this point we can just ask for these properties now, no need to confirm they're especially valid
            string name = userInterface.GetStringResponse("Enter the Name");
            string pack = userInterface.GetStringResponse("Enter the Pack");
            decimal price = userInterface.GetDecimalResponse("Enter the Price (must be more than $0)", 0.01M, decimal.MaxValue);
            bool activeStatus = userInterface.GetBooleanResponse("Enter whether the beverage is active or not");

            // Create beverage using inputted properties
            Beverage beverageToAdd = new Beverage();
            beverageToAdd.id = id;
            beverageToAdd.name = name;
            beverageToAdd.pack = pack;
            beverageToAdd.price = price;
            beverageToAdd.active = activeStatus;

            // Add beverage to database
            beverageDatabaseHook.Beverages.Add(beverageToAdd);

            // Save changes to database
            beverageDatabaseHook.SaveChanges();

            // Print the new beverage
            userInterface.PrintSingleBeverage(beverageDatabaseHook.Beverages.Where(beverage => beverage.id == id).First<Beverage>());

            // Display notification
            userInterface.DisplayMessage("Beverage added!");
        }

        private static void mainMenu_updateExistingBeverage()
        {
            string idQuery = userInterface.GetStringResponse("Enter the ID of the existing beverage, or type \"exit\" to return back to the main menu.");

            // As long as they're not trying to exit, keep checking 
            while(idQuery.ToLower().Trim() != "exit" && findBeveragesByID(idQuery).Count < 1)
            {
                userInterface.DisplayMessage("Could not find a beverage with that ID. Please try again.");                                                  // Error
                idQuery = userInterface.GetStringResponse("Enter the ID of the existing beverage, or type \"exit\" to return back to the main menu.");
            }

            // We've exited out of the loop, meaning either they typed "exit" or a beverage with the typed ID was found.
            if (idQuery.ToLower().Trim() != "exit")
            {
                // Get handle to beverage we are editing
                Beverage beverageToUpdate = beverageDatabaseHook.Beverages.Find(idQuery);

                // Start asking what they want to change
                if (userInterface.GetBooleanResponse("Change the name?"))
                {
                    beverageToUpdate.name = userInterface.GetStringResponse("Enter the Name");
                }

                if (userInterface.GetBooleanResponse("Change the pack?"))
                {
                    beverageToUpdate.pack = userInterface.GetStringResponse("Enter the Pack");
                }

                if (userInterface.GetBooleanResponse("Change the price?"))
                {
                    beverageToUpdate.price = userInterface.GetDecimalResponse("Enter the Price (must be more than $0.01)", 0.01M, decimal.MaxValue);
                }

                if (userInterface.GetBooleanResponse("Change the active status?"))
                {
                    beverageToUpdate.active = userInterface.GetBooleanResponse("Enter whether the beverage is active or not");
                }

                // Save the changes
                beverageDatabaseHook.SaveChanges();
            }
        }

        private static void mainMenu_deleteABeverage()
        {
            string idQuery = userInterface.GetStringResponse("Enter the ID of the beverage you wish to delete, or type \"exit\" to return back to the main menu.");

            // As long as they're not trying to exit, keep checking 
            while (idQuery.ToLower().Trim() != "exit" && findBeveragesByID(idQuery).Count < 1)
            {
                userInterface.DisplayMessage("Could not find a beverage with that ID. Please try again.");                                                  // Error
                idQuery = userInterface.GetStringResponse("Enter the ID of the beverage you wish to delete, or type \"exit\" to return back to the main menu.");
            }

            // We've exited out of the loop, meaning either they typed "exit" or a beverage with the typed ID was found.
            if (idQuery.ToLower().Trim() != "exit")
            {
                // Get handle to beverage we are deleting
                Beverage beverageToDelete = beverageDatabaseHook.Beverages.Find(idQuery);

                // Remove the beverage
                beverageDatabaseHook.Beverages.Remove(beverageToDelete);

                // Save the changes
                beverageDatabaseHook.SaveChanges();

                // Give notification
                userInterface.DisplayMessage("Beverage successfully deleted.");
            }
        }

        #endregion

        #region "Retrieve beverages by property or id" menu methods

        // Get a list of beverages based on a price
        private static List<Beverage> retrieveBeverageByPropertyOrID_priceMenu()
        {
            // Print a menu item set giving options to retrieve a beverage that is less than, equal to, or more than a given price.
            int userChoice = userInterface.DisplayMenuAndGetResponse(priceMenuItemsSet, "Find beverages with a price that is...");

            // Query for the value they want to use to query
            decimal value = userInterface.GetDecimalResponse("Enter the value");

            // Get beverages based on the choices so far
            switch (userChoice)
            {
                // Less than/equal to a value
                case 1:
                    return findBeveragesWithPriceLessThan(value);
                case 2:
                    return findBeveragesWithPriceEqualTo(value);
                case 3:
                    return findBeveragesWithPriceMoreThan(value);
                default:
                    Console.WriteLine("A serious error has occured in mainMenu_retrieveBeverageBypropertyOrID_priceMenu()");
                    return null;
            }
        }

        // Get a list of beverages based on whether the beverage is "active" or not.
        private static List<Beverage> retrieveBeverageByActiveStatus_activeStatusMenu()
        {
            // Print menu and get response
            int userChoice = userInterface.DisplayMenuAndGetResponse(findItemByActiveStatus, "Find beverages with a status that is...");
            
            // Get beverages based on the choice
            switch (userChoice)
            {
                case 1:
                    return findBeverageWithActiveStatus(true);
                case 2:
                    return findBeverageWithActiveStatus(false);
                default:
                    Console.WriteLine("A serious error has occured in mainMenu_retrieveBeverageBypropertyOrID_activeStatusMenu()");
                    return null;
            }
        }
        #endregion

        #region Find Beverage methods

        /// <summary>
        /// Find beverages by a given ID. 
        /// </summary>
        private static List<Beverage> findBeveragesByID(string id_query)
        {
            return beverageDatabaseHook.Beverages.Where(beverage => beverage.id == id_query).ToList<Beverage>();
        }

        /// <summary>
        /// Find beverages by a given Name. 
        /// </summary>
        private static List<Beverage> findBeveragesByName(string name_query)
        {
            return beverageDatabaseHook.Beverages.Where(beverage => beverage.name == name_query).ToList<Beverage>();
        }

        /// <summary>
        /// Find beverages by a given Pack.
        /// </summary>
        private static List<Beverage> findBeveragesByPack(string pack_query)
        {
            return beverageDatabaseHook.Beverages.Where(beverage => beverage.pack == pack_query).ToList<Beverage>();
        }

        /// <summary>
        /// Find beverages that are less than or equal to a given Price
        /// </summary>
        private static List<Beverage> findBeveragesWithPriceLessThan(decimal price_query)
        {
            return beverageDatabaseHook.Beverages.Where(beverage => beverage.price <= price_query).ToList<Beverage>();
        }

        /// <summary>
        /// Find beverages equal to a given Price
        /// </summary>
        private static List<Beverage> findBeveragesWithPriceEqualTo(decimal price_query)
        {
            return beverageDatabaseHook.Beverages.Where(beverage => beverage.price == price_query).ToList<Beverage>();
        }

        /// <summary>
        /// Find beverages more than or equal to a given Price
        /// </summary>
        private static List<Beverage> findBeveragesWithPriceMoreThan(decimal price_query)
        {
            return beverageDatabaseHook.Beverages.Where(beverage => beverage.price >= price_query).ToList<Beverage>();
        }

        /// <summary>
        /// Find beverages that are active or (if false) not active
        /// </summary>
        private static List<Beverage> findBeverageWithActiveStatus(bool status_query)
        {
            return beverageDatabaseHook.Beverages.Where(beverage => beverage.active == status_query).ToList<Beverage>();
        }

        #endregion
    }
}
