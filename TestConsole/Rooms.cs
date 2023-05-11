using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    public class Room
    {
        private static Room _currentRoom;
        private static int _roomIndex;

        private string _name;
        private string _description;
        private List<string> _items = new List<string>();
        private int[] _direction;
        private List<string> _itemDesc = new List<string>();


        public string Name { get { return _name; } set { _name = value; } }
        public string Description { get { return _description; } set { _description = value; } }
        public List<string> Items
        {
            get { return _items; }
            set   
            {
                foreach (string item in value)
                {
                    _items.Add(item.ToLower());
                }
            }
        }

        public int[] Dir {
            get { return _direction; }
            set {
                _direction = value;
            }
        }

        public List<string> ItemDesc
        {
            get { return _itemDesc; }
            set
            {
                foreach (string itemDesc in value)
                {
                    _itemDesc.Add(itemDesc.ToLower());
                }
            }
        }

        public void movement(string direction, Player player1)
        {
            List<string> directions = new List<string>() { "NORTH", "EAST", "SOUTH", "WEST", "UP", "DOWN" };
            int location = Player.RoomIndex;

            int dir = directions.IndexOf(direction);
            if (this.Dir[dir] != -1)
            {
                location = this.Dir[dir];
                Player.RoomIndex = location;
                Player.CurrentRoom = Environment.Scene[location];
            } else
            {
                Console.WriteLine("I don't know how to go that way!\n");
            }
        }

        public void take(string item, Player player1)
        {

            if (this.Items.Contains(item))
            {
                player1.Inventory.Add(item);
                this.Items.Remove(item);
                Console.WriteLine("You have taken {0}", item);

            } else
            {
                Console.WriteLine("I don't see the {0}", item);
            }

        }

        public void drop(string item, Player player1)
        {

            if (player1.Inventory.Contains(item))
            {
                player1.Inventory.Remove(item);
                this.Items.Add(item);
                Console.WriteLine("You have dropped the {0}", item);
            }
            else
            {
                Console.WriteLine("You don't have the {0}", item);
            }

        }

        public void lookCmd()
        {
            Console.WriteLine("You are in the {0}", _name);
            Console.WriteLine(_description);
            
        }


    }

    public class Environment {

        private static List<Room> _scene = new List<Room>();

        public static List<Room> Scene { get { return _scene; } } 

        public Environment() {
            _scene = CreateRooms();
        }

        public Room CurrentRoom()
        {
            return _scene [Player.RoomIndex];
        }
        private static List<Room> CreateRooms()
        {
            List<Room> rooms = new List<Room>();
            /*Room order/number order:
                Central Hall: 0
                Living Room: 1
                Bathroom: 2
                Library: 3
                Dining Room: 4
                Kitchen: 5
                Upstairs Hallway: 6
                Greed's Room: 7
                Noble's Room: 8
             */

            //Dir hint: -1 means you can't go there. Rooms you can go to are 0-8, just check map to make sure it works.
            rooms.Add(new Room()
            {
                Name = "Central Hall",
                Description = "Main room of the house. There are 7 suspects standing to your left, a chandelier above, 2 chairs to the side and a large rug.\nThere are exits to the North, East, and West.",
                Items = new List<string> { },
                Dir = new int[6] { 1, 2, -1, 3, -1, -1 }
                //North to Living Room
                //East to Bathroom
                //West to Library
            });

            rooms.Add(new Room()
            {
                Name = "Living Room",
                Description = "Theres a couch, a coffee table, a small rug, and a fireplace with a TV hanging over. There are also huge windows and a plant in the corner.\nThere are exits to the South, East, and West.",
                Items = new List<string> { "Small Key", "Couch", "Empty Alcohol Bottles", "Coffee Table", "Rug", "Plant", "Computer", "Sword", "Duel Note", "Radio", "Dirty Bowl" },
                Dir = new int[6] { -1, 5, 0, 4, -1, -1 },
                ItemDesc = new List<string> { "Opens chest in bedroom.", "Just a couch, though there seems to be some bottles next to it and a sword under?", "Empty bottles of alcohol, they may have fingerprints.", "Theres a computer, a radio, and a bowl on here.", "Plain ol' rug, but it looks like theres a note under it.", "You move some pebbles around and find a small key.", "A tab was left open. These look like business documents showing Wrath was trying to sabotage the Noble's business.", "This is a duel sword.", "A letter from Pride, it says 'I challenge you Noble, to a duel.'", "A radio, this should be in the Kitchen instead.", "Dirty Bowl with cereal residue? The Noble never ate cereal, only the cooks do." }   
                //East to Kitchen
                //South to Main Room
                //West to Dining Room
            });

            rooms.Add(new Room()
            {
                Name = "Bathroom",
                Description = "There is a bathtub, a shower, a laundry basket, and a sink with some cabinets. There seems to be drops of blood on the floor near the sink.\nThere are exits to the North and West.",
                Items = new List<string> { "Bathtub", "Laundry Basket", "Towels", "Dirty Clothes", "Blood", "Sink", "Bandages", "Cleaning Supplies", "Pouch", "Silver Key" },
                Dir = new int[6] { 5, -1, -1, 0, -1, -1 },
                ItemDesc = new List<string> { "Very fancy Bathtub.", "Contains towels and dirty clothes.", "One seems to have blood stains.", "Someone didnt do the laundry", "Likely the Nobles.", "You look through the sink cabinets. You find bandages, cleaning supplies, and a pouch.", "Probably the ones the Noble used.", "For cleaning.", "Theres a Silver Key in here.", "Looks like it opens a cabinet." }
                //North to Kitchen
                //West to Main Room
            });


            rooms.Add(new Room()
            {
                Name = "Library",
                Description = "Books line the walls. There is a spiral staircase in the middle of the room that leads upstairs. There is a plant next to the staircase, a fallen book, and a bag.\nThere are exits to the North and East.",
                Items = new List<string> { "Staircase",  "Fallen Book", "Wrath's Bag", "Incriminating Books", "Fake Letters" },
                Dir = new int[6] { 4, 0, -1, -1, 6, -1 },
                ItemDesc = new List<string> { "Leads upstairs.", "This is about fantasy mixtures / chemicals.", "This is Wrath's Bag. It has letters and books.", "They talk about how to steal someones business and how to frame someone.", "These are fake letters that were clearly written to look like the Noble wrote them." }
                //North to Dining Room
                //East to Main Room
                //up to Greed's Room
            });

            rooms.Add(new Room()
            {
                Name = "Dining Room",
                Description = "There is a dining table with candles, chairs, and a mirror on the wall.\nThere are exits to the South and East.",
                Items = new List<string> { "Dining Table","Mirror","Piece of Paper","Coffee Cup" },
                Dir = new int[6] { -1, 1, 3, -1, -1, -1 },
                ItemDesc = new List<string> { "Made out of a nice wood. Theres also a coffee cup on here.", "There seems to be a piece of paper behind it.", "It says 'Im going out into the town, if anyone asks say I was out in the fields. - Envy'", "Almost empty." }
                //East to Living Room 
                //South to Library
            });

            rooms.Add(new Room()
            {
                Name = "Kitchen",
                Description = "Cabinets and cutting boards line the walls. Theres also a kitchen island in the middle of the room, a trash bag next to the backdoor, and a servant calendar on the wall.\nThere is an exit to the South and West.",
                Items = new List<string> { "Trash Bag","Kitchen Knives","Cabinets","Kitchen Island", "Duel Clothes", "Servant Calendar", "Diary" },
                Dir = new int[6] { -1, -1, 2, 1, -1, -1 },
                ItemDesc = new List<string> { "Contains bloody bedsheets, rags, and ripped duel clothes.", "Sharp.", "Nothing but kitchen supplies. Oh, wait. Is this a Diary ?", "Theres some knives and fruit on here.", "Bloody, Holes are poked into the lower part of the shirt. It looks like Duel Clothes.", "Envy is marked off for the day the Noble was murdered.", "This belongs to Envy. It says 'I hate working for the Noble. They dont deserve anything that they have, after all ive worked way harder. One day I will have all of this for myself.'" }
                //South to bathroom
                //West to Living Room
            });

            // Upstairs

            rooms.Add(new Room()
            {
                Name = "Upstairs Hallway",
                Description = "You can go West, East, or Down.",
                Dir = new int[6] {-1, 8, -1, 7, -1, 3},
                //East to the Noble's Room
                //West to Greed's Room
                //Downstairs
            });

            rooms.Add(new Room()
            {
                Name = "Greed's Room",
                Description = "Dark. Big and Lavish room with a bed and some dressers.\nThere is an exit to the East.",
                Items = new List<string> { "Check Book", "Bed", "Dressers", "Bank Papers", "Debt Papers" },
                Dir = new int[6] { -1, 6, -1, -1, -1, -1 },
                ItemDesc = new List<string> { "This belonged to the Noble.", "The bed is made. There seems to be a check book on here.", "A lot of fancy folded clothes, and important bank + debt papers.", "Bank Statements under Greeds name? Was he taking money from his father?", "Looks like Greed's in debt from how much money he spends." }
                //East to the Hallway

            });

            rooms.Add(new Room()
            {
                Name = "Noble's Room",
                Description = "The victims room. There is some broken glass next to the bed, a plate, and a dresser with a small chest on top.\nThere is an exit to the West.",
                Items = new List<string> { "Broken Glass", "Love Letters", "Lipstick", "Plate", "Chest", "Poison" },
                Dir = new int[6] { -1, -1, -1, 6, -1, -1 },
                ItemDesc = new List<string> { "Seems to be a broken cup.", "Love letters in Lust's handwriting. These are written to another man, and date back months. Some talk about getting rid of the Noble eventually.", "Red Lipstick.", "A plate with food crumbs.", "Needs a Key."/* or, " There seems to be Letters and lipstick in here. This chest must belong to Lust. Theres also a small bottle." */, "A small bottle of poison."}
                //West to the Hallway
            });

            return rooms;
        }

    }

}
