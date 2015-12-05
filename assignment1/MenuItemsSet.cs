/* 
    Author: Zachary Dwyer
    Class: CIS 237
*/

using System;
using System.Collections.Generic;

namespace assignment1
{

    /// <summary>
    /// Represents a collection of menu selections.
    /// </summary>
    class MenuItemsSet
    {
        // I am using this class to hold a list of menu items. This way, it will be easier to re-use menu drawing methods so I don't have to write it out every time. There are
        // numerous parts of the code that require the user to select from a set of options.

        // This class is used solely with the menu drawing method in UserInterface.

        // Private list of menu selections. Can only be retrieved.
        public List<String> MenuItems
        {
            get;
        }

        // Constructor
        public MenuItemsSet()
        {
            this.MenuItems = new List<string>();
        }

        // Creates a menu items set using a variable amount of strings
        public MenuItemsSet(params string[] menuItems)
        {
            this.MenuItems = new List<string>();
            this.AddMenuItems(menuItems);
        }

        /// <summary>
        /// Add a menu selection.
        /// </summary>
        /// <param name="menuItem">The menu item to be added</param>
        public void AddMenuItem(string menuItem)
        {
            this.MenuItems.Add(menuItem);
        }
        
        /// <summary>
        /// Add an array of strings to add to the MenuItems collection.
        /// </summary>
        /// <param name="menuItems">String array of menu item strings.</param>
        public void AddMenuItems(string[] menuItems)
        {
            // For every string in the array given...
            foreach(string menuItem in menuItems)
            {
                // ...add it to the collection
                this.MenuItems.Add(menuItem);
            }
        }
    }
}
