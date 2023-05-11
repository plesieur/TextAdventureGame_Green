using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading;
using TextAdventureGameInputParser;
using TextAdventureGameInputParser.WordClass;

namespace TestConsole
{
    internal class Program
    {

        const bool DEBUG = true;
        const int MAX_INVENTORY = 5;
        const bool CHANGE = false;
        private static void Main()
        {

            var parser = CreateParser();
            Environment scene = new Environment();
            Sentence results;
            Player player1 = new Player(0);
            Npcs NPC = new Npcs(0);
            Journal yourJournal = new Journal();

            Console.WriteLine("Type 'help' or '?' for a list of commands\n");
            Console.WriteLine("After a surprise call from the detective agency you quickly board the nearest carriage to the town of Sinvile.Apparently an important nobleman lies dead and seven people are suspects in the murder. As you arrive to the fanciful gates of the house you brace yourself for the investigation and step inside. You are immediately greeted with the sight of the body of the noble lying face down on the ground with seven figures gathered around him. You see a tall women in a slender red dress, an old balding man in a tailcoat suit hunching over and looking as if he could nod off at any minute, next to him you see a well kept man with a monocle and a mustache accenting his red brestcoat ornately embroidered in gold. At his side is an empty scabbard. Next to him you see a middle aged gentleman in a midnight blue suit, his fingers lined with thick gold rings. He has an angered look on his face and appears to be pacing the room. You note a very large man in a stained apron and a large white chef's hat. He holds a ladle in one hand and a large pot of soup in the other, you also notice a trail of crumbs across his face as he seems to be chewing on...something. Standing in the corner is a very disinterested looking teenager, more so preoccupied with his DS than the body in front of him. Lastly you see who appears to be a peasant with dirt covering their face and only simple cloth coverings and a leather flat-cap.");
            Console.WriteLine();
            scene.CurrentRoom().lookCmd();
            do
            {
                Console.Write("\nYou > ");
                var input = Console.ReadLine() ?? "";
                Console.WriteLine();
                if (string.IsNullOrWhiteSpace(input))
                    Console.WriteLine("hello?");
                results = parser.Parse(input);
                executeCommand(results, parser, scene, player1, NPC, yourJournal);
            } while (true);
        }

        private static Parser CreateParser()
        {
            var parser = new Parser();

            //interactable
            parser.AddVerbs("GO", "OPEN", "CLOSE", "SHOW", "LOOK", "INVENTORY", "ITEMS",  "GET", "TAKE", "DROP", "EXAMINE", "HELP", "QUIT", "ACCUSE", "TALK", "SUSPECT", "SMALL KEY", "COUCH", "COFFEE TABLE", "RUG", "FIREPLACE", "PLANT", "BATHTUB", "SHOWER", "LAUNDRY BASKET", "BLOOD", "SINK", "BANDAGES", "ClEANING SUPPLIES", "POUCH", "STAIRCASE", "PLANT", "DINING TABLE", "MIRROR", "TRASH BAG", "KITCHEN KNIVES", "CABINETS", "KITCHEN ISLAND", "STAIRS", "BED", "BROKEN GLASS", "ALCOHOL BOTTLES", "COMPUTER", "BOWL", "TOWELS", "DIRTY CLOTHES", "SILVER KEY", "FALLEN BOOK", "SERVANTS NOTE", "COFFEE CUP", "CHECK BOOK", "LOVE LETTERS", "LIPSTICK", "PLATE", "CHEST", "DUEL CLOTHES", "SWORD", "DUEL NOTE", "RADIO", "BAG", "BOOKS", "LETTERS", "SERVANT CALENDAR", "DIARY", "DRESSERS", "BANK STATEMENT", "DEBT LETTER", "SMALL BOTTLE", "FANCY NOTE", "JOURNAL");
            parser.AddImportantFillers("TO", "ON", "IN", "WITH");
            parser.AddUnimportantFillers("THE", "A", "AN", "AT");
            parser.AddNouns(
                "NORTH",
                "EAST",
                "WEST",
                "SOUTH",
                "UP",
                "DOWN",
                //pick up items
                "SMALL KEY",
                "ALCOHOL BOTTLES",
                "COMPUTER",
                "BOWL",
                "TOWELS",
                "DIRTY CLOTHES",
                "SILVER KEY",
                "FALLEN BOOK",
                "SERVANTS NOTE",
                "COFFEE CUP",
                "CHECK BOOK",
                "LOVE LETTERS",
                "LIPSTICK",
                "PLATE",
                "CHEST",
                "DUEL CLOTHES",
                "SWORD",
                "DUEL NOTE",
                "FANCY NOTE",
                "BAG",
                "SMALL BOTTLE",
                //added suspects to Nouns
                "LUST",
                "SLOTH",
                "PRIDE",
                "GLUTTONY",
                "WRATH",
                "ENVY",
                "GREED"
            );
            parser.Aliases.Add("GO NORTH", "N", "NORTH", "MOVE NORTH");
            parser.Aliases.Add("GO EAST", "E", "EAST", "MOVE EAST");
            parser.Aliases.Add("GO SOUTH", "S", "SOUTH", "MOVE SOUTH");
            parser.Aliases.Add("GO WEST", "W", "WEST", "MOVE WEST");
            parser.Aliases.Add("GO UP", "U", "UP");
            parser.Aliases.Add("GO DOWN", "D", "DOWN");
            parser.Aliases.Add("INVENTORY", "I", "INV");
            parser.Aliases.Add("HELP", "H", "?");
            parser.Aliases.Add("QUIT", "Q", "EXIT", "ESCAPE", "ESC");
            parser.Aliases.Add("EXAMINE", "SEARCH", "STUDY" );
            parser.Aliases.Add("TAKE", "COLLECT", "PICK UP", "GRAB");
            parser.Aliases.Add("SHOW", "GIVE", "PRESENT");
            parser.Aliases.Add("ACCUSE", "GUESS");  
            parser.Aliases.Add("SMALL KEY", "KEY");
            parser.Aliases.Add("SUSPECT", "WHO");
            parser.Aliases.Add("ITEMS", "ITEMS OF");
            parser.Aliases.Add("GLUTTONY", "GLUT");
            parser.Aliases.Add("DUEL NOTE", "NOTE");
            parser.Aliases.Add("DUEL CLOTHES", "RIPPED CLOTHES");
            parser.Aliases.Add("JOURNAL", "J");
           // parser.Aliases.Add("SERVANTS NOTE", "SERVANT NOTE");

            return parser;
        }



        private static void executeCommand(Sentence results, Parser parser, Environment scene, Player player1, Npcs NPC, Journal journals)
        {

            if (DEBUG)
            {
                Console.WriteLine(results);   //print debug info about parsed sentence
            }
            if (!results.ParseSuccess)
            {
                Console.WriteLine("Excuse Me?");   //Did not recognize command
            }
            else if (results.Ambiguous)
            {
                Console.WriteLine("Be more specfic with {0}", results.Word4.Value.ToLower());
            } else {
                switch (results.Word1.Value)
                {
                    case "HELP":
                        Console.WriteLine("COMMANDS\n--------\n");
                       
                        parser.PrintVerbs(17);  
                     
                        break;
                    case "QUIT":
                        Console.WriteLine("See Ya\n");
                        System.Environment.Exit(0);
                        break;
                    case "GO":
                        Player.CurrentRoom.movement(results.Word4.Value, player1);
                        /*Typing 'GO' alone caused the game to crash with the following note.
                        System.NullReferenceException: 'Object reference not set to an instance of an object.'

                        TextAdventureGameInputParser.Sentence.Word4.get returned null.*/
                        scene.CurrentRoom().lookCmd();
                        break;
                    case "LOOK":
                        scene.CurrentRoom().lookCmd();
                        break;
                    case "INVENTORY":
                        if (player1.Inventory.Count == 0)
                        {
                            Console.WriteLine("You are not carrying anything");
                        } else
                        {
                            Console.WriteLine("You are carrying");
                            foreach (string item in player1.Inventory)
                            {
                                Console.WriteLine("  {0}", item);
                            }
                        }
                        break;
                    case "TAKE":
                        if (player1.Inventory.Count == MAX_INVENTORY)
                        {
                            Console.WriteLine("You cannot carry any more items");
                            Console.WriteLine("Drop an item first");
                        } else
                        {
                            string item = parseItem(results.Word4.Value.ToLower(),
                                results.Word4.PrecedingAdjective == null ? null : results.Word4.PrecedingAdjective.Value.ToLower());
                            /*Game chrashed when I typed 'SHOW' alone with the following message
                            System.NullReferenceException: 'Object reference not set to an instance of an object.'

                            TextAdventureGameInputParser.Sentence.Word4.get returned null.*/
                            Player.CurrentRoom.take(item, player1);
                        }
                        break;
                    case "ACCUSE":
                        string suspect = parseItem(results.Word4.Value.ToLower(),
                               results.Word4.PrecedingAdjective == null ? null : results.Word4.PrecedingAdjective.Value.ToLower());
                        /*Typing 'accuse' alone crashes the game gives the following message:
                        System.NullReferenceException: 'Object reference not set to an instance of an object.'

                        TextAdventureGameInputParser.Sentence.Word4.get returned null.*/
                        int returnVal = NPC.winLose(suspect);
                        if (returnVal == 1 || returnVal == 2)
                        {
                            while (true)
                            {
                                Console.WriteLine("Press Any key to exit");
                                Console.ReadLine();
                                System.Environment.Exit(0);
                            }
                        }
                        break;
                    case "TALK":
                        //used to call the general alibis given by the suspects. using the SHOW command will update the ability to get different alibis from them. 
                        string talkTo = parseItem(results.Word4.Value.ToLower(), results.Word4.PrecedingAdjective == null ? null : results.Word4.PrecedingAdjective.Value.ToLower());
                        /*Typign 'talk' causes a crash and gives the following message:
                         System.NullReferenceException: 'Object reference not set to an instance of an object.'

                         TextAdventureGameInputParser.Sentence.Word4.get returned null.*/
                        Console.WriteLine(NPC.TalkToSuspect(talkTo));
                        break;
                    /*case "PUSH":
                        string check = parseItem(results.Word4.Value.ToLower(),
                          results.Word4.PrecedingAdjective == null ? null : results.Word4.PrecedingAdjective.Value.ToLower());
                        Typing 'push' alone crashes the game and brings up the following message:
                         System.NullReferenceException: 'Object reference not set to an instance of an object.'

                         TextAdventureGameInputParser.Sentence.Word4.get returned null.

                        //checks if the player is in the right room
                        if (Player.CurrentRoom == Npcs.CurrentRoom)
                        {
                            Console.WriteLine(NPC.pushReaction(check));

                        }
                        else {
                            Console.WriteLine("There is no one in this room for you to interact with.");


                        }
                        break;*/
                    case "SUSPECT":
                        if (Player.CurrentRoom == Npcs.CurrentRoom)
                        {
                            NPC.suspectList();
                        }
                        else {
                            Console.WriteLine("There are no Suspects to list in this room.");
                        }
                       
                        break;
                    case "DROP":
                        if (player1.Inventory.Count == 0)
                        {
                            Console.WriteLine("You don't have any items to drop");
                        }
                        else
                        {
                            string item = parseItem(results.Word4.Value.ToLower(), 
                                results.Word4.PrecedingAdjective == null ? null : results.Word4.PrecedingAdjective.Value.ToLower());
                            Player.CurrentRoom.drop(item, player1);
                        }
                        break;
                    case "SHOW":
                        string person = parseItem(results.Word4.Value.ToLower(), results.Word4.PrecedingAdjective == null ? null : results.Word4.PrecedingAdjective.Value.ToLower());
                        /*Game chrashed when I typed 'SHOW' alone with the following message
                        System.NullReferenceException: 'Object reference not set to an instance of an object.'

                        TextAdventureGameInputParser.Sentence.Word4.get returned null.*/
                        //grab the item that is being shown to the person, but only if it is in their inventory. figure that out later ig.

                        string gift = parseItem(results.Word7.Value.ToLower(), results.Word7.PrecedingAdjective == null ? null : results.Word7.PrecedingAdjective.Value.ToLower());

                        if (!player1.Inventory.Contains(gift)) {
                            Console.WriteLine("You do not have that item on you!");
                        }
                        else {
                            //grabs the NPC & 'GIFT'
                            if (Player.CurrentRoom == Npcs.CurrentRoom)
                            {
                                Console.WriteLine(NPC.reactToItems(person, gift));
                            }
                            else {
                                Console.WriteLine("There is no one in this room for you to give an item to.");
                            }  
                        }
                        
                        break;
                    case "ITEMS":
                        string who = parseItem(results.Word4.Value.ToLower(), results.Word4.PrecedingAdjective == null ? null : results.Word4.PrecedingAdjective.Value.ToLower());
                        /*Game crashed when I typed 'Items' alone with the following message
                        System.NullReferenceException: 'Object reference not set to an instance of an object.'

                        TextAdventureGameInputParser.Sentence.Word4.get returned null.*/
                        Console.WriteLine(NPC.itemsLeft(who));
                        /*'itemsLeft' has the following error
                        CS1061: 'Npcs' does not contain a definition for 'itemsLeft' and no accessible extension method 
                        'name' accepting a first argument of type 'Npcs' could be found (are you missing a using 
                        directive or an assembly reference?).
                        I did comment out all of the code in Item.cs, so that's probably why this is happening.*/
                        break;
                    //items
                    case "SMALL KEY":
                        Console.WriteLine("Cute Key. Seems like it opens a box of some sort.");
                        
                        break;
                    case "COUCH":
                            Console.WriteLine("Just a couch, though there seems to be some bottles next to it and a sword under it?");
                        break;

                    case "ALCOHOL BOTTLES":
                        Console.WriteLine("Empty bottles of alcohol, they may have fingerprints.");
                        break;

                    case "COFFEE TABLE":
                            Console.WriteLine("Theres a computer, a radio, and a bowl on here.");
                        break;

                    case "RUG":
                        Console.WriteLine("Plain ol' rug, but it looks like theres a note under it.");
                        break;
                    case "FANCY NOTE":
                        Console.WriteLine("Its a note addressed to Sloth, telling him to not do the laundry until half-past noon. The handwriting is clearly the lady of the house's.");
                        break;

                    case "PLANT":
                        Console.WriteLine("You move some pebbles around and find a small key.");
                        break;

                    case "COMPUTER":
                        
                      Console.WriteLine("A tab was left open. These look like business documents showing Wrath was trying to sabotage the Noble's business.");
                        
                        break;

                    case "SWORD":
                        
                        Console.WriteLine("This is a duelling sword.");
                        
                        break;

                    case "DUEL NOTE":   
                        Console.WriteLine("A letter from Pride, it says 'I challenge you Noble, to a duel.'");
                        
                        break;
                    case "RADIO":
                        Console.WriteLine("A radio, this should be in the Kitchen instead.");
                        break;
                    case "BOWL":
                            Console.WriteLine("Dirty Bowl with cereal residue? The Noble never ate cereal, only the cooks do.");
                        break;
                    case "BATHTUB":
                        Console.WriteLine("Very fancy Bathtub.");
                        break;
                    case "LAUNDRY BASKET":
                        Console.WriteLine("Contains towels and dirty clothes.");
                        break;
                    case "TOWELS":
                        Console.WriteLine("One seems to have blood stains.");
                        break;
                    case "DIRTY CLOTHES":
                        Console.WriteLine("Someone didnt do the laundry");
                        break;
                    case "BLOOD":
                        Console.WriteLine("Likely the Nobles.");
                        break;
                    case "SINK":
                        Console.WriteLine("You look through the sink cabinets. You find bandages, cleaning supplies, and a pouch.");
                        break;
                    case "BANDAGES":
                        Console.WriteLine("Probably the ones the Noble used.");
                        break;
                    case "CLEANING SUPPLIES":
                        Console.WriteLine("For Cleaning.");
                        break;
                    case "POUCH":
                        Console.WriteLine("Theres a Silver Key in here.");
                        break;
                    case "SILVER KEY":
                        Console.WriteLine("Looks like it opens a cabinet.");
                        break;
                    case "STAIRCASE":
                        Console.WriteLine("Leads upstairs.");
                        break;
                    case "FALLEN BOOK":
                        Console.WriteLine("This is about fantasy mixtures / chemicals.");
                        break;
                    case "BAG":
                        Console.WriteLine("This is Wrath's Bag. It has letters and a few books.");
                        break;
                    case "BOOKS":
                        Console.WriteLine("Its a book with the title 'How to Frame Someone, For Dummies, VOL 1");
                        break;
                    case "LETTERS":
                        Console.WriteLine("These are fake letters that were clearly written to look like the Noble wrote them.");
                        break;
                    case "DINING TABLE":
                        Console.WriteLine("Made out of a nice wood. Theres also a coffee cup on here.");
                        break;
                    case "MIRROR":
                        Console.WriteLine("There seems to be a note from a servant behind it.");
                        break;
                    case "SERVANTS NOTE":
                        Console.WriteLine("It says 'Im going out into the town, if anyone asks say I was out in the fields. - Envy'");
                        break;
                    case "COFFEE CUP":
                        Console.WriteLine("Almost empty.");
                        break;
                    case "TRASH BAG":
                        Console.WriteLine("Contains bloody bedsheets, rags, and ripped clothes.");
                        break;
                    case "KITCHEN KNIVES":
                        Console.WriteLine("sharp.");
                        break;
                    case "CABINETS":
                        Console.WriteLine("Nothing but kitchen supplies. Oh, wait. Is this a Diary ?");
                        break;
                    case "KITCHEN ISLAND":
                        Console.WriteLine("Theres some knives and fruit on here.");
                        break;
                    case "DUEL CLOTHES":
                        Console.WriteLine("Bloody, Holes are poked into the lower part of the shirt. Someone clearly had a duel in this.");
                        break;
                    case "SERVANT CALENDAR":
                        Console.WriteLine("Envy is marked off for the day the Noble was murdered.");
                        break;
                    case "DIARY":
                        Console.WriteLine("This belongs to Envy. It says 'I hate working for the Noble. They dont deserve anything that they have, after all ive worked way harder. One day I will have all of this for myself.'");
                        break;
                    case "CHECK BOOK":
                        Console.WriteLine("This belonged to the Noble. Looks like someone was writing out checks to some organization.");
                        break;
                    case "BED":
                        Console.WriteLine("The bed is made. There seems to be a check book on here.");
                        break;
                    case "DRESSERS":
                        Console.WriteLine("A lot of fancy folded clothes, and important bank + debt papers.");
                        break;
                    case "BANK STATEMENT":
                        Console.WriteLine("Bank Statements under Greeds name? Was he taking money from his father?");
                        break;
                    case "DEBT LETTER":
                        Console.WriteLine("Looks like Greed's in debt from how much money he spends.");
                        break;
                    case "BROKEN GLASS":
                        Console.WriteLine("Seems to be a broken cup.");
                        break;
                    case "LOVE LETTERS":
                        Console.WriteLine("Love letters in Lust's handwriting. These are written to another man, and date back months. Some talk about getting rid of the Noble eventually.");
                        break;
                    case "LIPSTICK":
                            Console.WriteLine("Red Lipstick.");
                        
                        break;
                    case "PLATE":
                        Console.WriteLine("A plate with food crumbs.");
                        break;
                    case "CHEST":
                        //if small key available
                        if (player1.Inventory.Contains("small key")) {
                            Console.WriteLine("There seems to be Love Letters and lipstick in here. This chest must belong to Lust. Theres also a small bottle.");
                        }
                           
                        else
                        {
                            Console.WriteLine("Needs a Key.");
                        }
                        break;
                    case "SMALL BOTTLE":
                        Console.WriteLine("A small bottle of poison.");
                        
                        break;

                    default:
                        Console.WriteLine("That doesn't exist. Let that sink in.");
                        break;

                }
            }

        }

        static string parseItem (string noun, string adjective) {
            string item;

            if (adjective != null)
            {
                item = adjective + " " + noun;
            }
            else
            {
                item = noun;
            }
            return item;
        }



    }
}