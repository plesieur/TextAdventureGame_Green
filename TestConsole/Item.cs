using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TextAdventureGameInputParser;

namespace TestConsole
{
    /*
    There are two different types of items, 1 can be picked up and used while 2 has 
    other items stored in it.
    */
    /*public class Item
    {
        private string _name;
        private int _type;

        public string Name { get {return _name;} set {_name = value;} }
        public int Type { get {return _type;} set {_type = value;} }

        // Type 1 is a tool, type 2 is searchable
        
        //If item is type 1, you can store it in your inventory and use it at anytime.
        //If item is type 2, you can search it for any stored items inside.

        //I'm still trying to use Google and AI to figure out what I can do.
        public string Tool(string Name)
        {
            this.Type = 1;

            static void Add(Player P, Item I)
            {
                P.Inventory.Add(Convert.ToString(I));
            }

            static void Remove(Player P, Item I)
            {
                P.Inventory.Remove(Convert.ToString(I));
            }

            if(Console.ReadLine() == "use" + this)
            {
                if ()
                {

                }
            }

            return "placeholder";
        }

        public string Searchable()
        {
            this.Type = 2;

            List<Item> StoredItems = new List<Item>();

            return "placeholder";
        }

        //The list of all the items
        string[] Itemlist =
        {

        };

        //Should I start from scratch? Nothing I've done here is working.
    }*/
}
