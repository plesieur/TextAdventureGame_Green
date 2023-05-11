using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

//npcs must be able to interact with
//must track alibi & say different lines
//react to different commands of the player (accuse, push, etc)
//Win / Lose condition as well.

namespace TestConsole
{
    public class Npcs
    {
        //private static Room _currentRoom;
        //private static int _roomIndex;
        Journal journal = new Journal();
        private static Room _currentRoom;
        private static int _roomIndex;
        int guesses = 2;
        //the array for counting: in order "Lust, Sloth, Pride, Gluttony, Wrath, Envy, Greed"
        int[] counters = new int[7];

        string[] suspects = { "Lust", "Sloth", "Pride", "Gluttony", "Wrath", "Envy", "Greed" };
        string[,] susAlibi = //currently holds placeholders of each suspects alibis
        {
            {"Lust: Oh~? Dear me, if I had known the detective trying to solve this mystery was someone like you, I would’ve dressed to impress! Where was I, you ask? Well, darling- first, I went shopping with an old friend of mine! Then, I came home, said hello to my husband, and went to prepare for bed! It was close to night at that time. I gave my darling husband his dinner for the night, and then I went to bed. When I woke up… well, I’m always an early riser so I didn’t realize anything was wrong… but, as it turned out he was dead! Somebody killed my beloved, I’m sure!", "Lust:  A-ah? Don’t- don’t worry about it! So what if I had poison on me? Any self-respecting noble lady would to keep herself safe! Everything can’t be taken at face value darling~. So what if I wrote to that other man while I was with my husband? Its not UNCOMMON for people to have a side-affair. But I loved my husband dearly- I really would never murder him! I made him food, and what- just because he died the morning after means I did it? You have no clue what I wouldn’t do for my beloved husband."},

            {"Sloth: (yawn) Mhm….OH! Hello, you must be the detective! yes, yes. I presume that you want me to recount my series of events? Yes I can certainly do that… (he yawns) I spent most of my day simply conducting my butler-ly duties, washing clothes and the like. When the good sir died I was on my break catching up on some much needed sleep so I wouldn’t know the least about what happened.", "Sloth: (big yawn) mhmm perhaps I didn't put the laundry in. That hardly makes me a criminal. I was simply napping. A man has got to get his rest in you know. Anyhow I was told not to wash several items by the lady of the house anyways. It is quite funny now that I think back on it, she asked to serve the noble his breakfast too. hmm, well I'll have to sleep on this." },

            {"Pride: Oho! The detective, yes? How wonderful, I’ve been dreadfully bored waiting for somebody to come and take my statement. Waiting is beneath me, as I’m sure you’re aware of by now. Wondering what I did? No need to worry any longer, I, Lord Pride, shall fill you in on his day! I woke up, first thing in the morning, sent a letter to a dear friend of mine-- the noble, as I’m sure you guessed- to let him know I was coming over, and then, annoyingly, my carriage broke down! So I spent the next couple hours waiting for my driver to fix it. When I FINALLY got to this manor, I was informed that the Lord of this house was dead! How could I have anything to do with it?", " Hmm? Well, it seems that you know a bit more than I thought. Yes I was here on the day of the murder and yes, I did duel the noble but I'm positive I didn't kill him. He challenged my honour and I responded how most would! Then we went our separate ways. The reason I didn't tell you this at the start was that I… I lost the duel. "},

            {"Gluttony: (The man places down the bowl of soup in his hands) Ah, welcome Detective. I would cook up a meal for you were the situation... not this. Where was I? Well I was in the kitchen, of course, making my home-recipe soup when I hear a shriek from the lady of the house and run in to find my boss, the noble lying on the floor dead! I don't know anything more than that.", "Gluttony:Detective can I be honest with you? I don't know! I don't remember what happened, I barely remember my own name! I was pitch black drunk, and now I'm beginning to fear that I was the killer! I would never do something like this but I don't know what happened! I don't know anything anymore. I only know that I was told to give the nobles meal to lust and let her serve it."},

            {"Wrath: Hmph. There you are. You took too long- you’d be a horrible business owner. You want MY statement? Fine. I was to meet up with the Noble concerning a business deal- you see we were to reach an agreement about a part of his business that was infringing on mine- and I brought everything I needed with me. When I got here, I was told by that lazy butler to wait while he got him- and guess WHAT! That OAF died on me. I couldn’t have had anything to do with this.", "Wrath: Just WHAT are you implying, you beast? Are you implying that I would DARE sabotage the Noble’s business? What- because I’m JEALOUS? That I would kill him because he treated me wrongly?! No! I would NOT kill him for that, no no no. I would make him SUFFER. Drag his business into the MUD. (He pauses. And then blanks). Forget- Forget I said anything. This has nothing to do with you."},

            {"Envy: Well Well Well. If it isn't the fancy detective, listen pal, I don't have anything to do with this. All I do is tend the fields! I wasn't even here when the murder took place- I was out tending to the cabbages.", "Envy: Ok fine you want the truth? I’ll give you the truth! I wasn't out tending the cabbages- I was scheming. Us peasants are treated like garbage and all we want is a little respect and to be able to own the things that we spend our lives making. Yes, I was planning to murder the noble and my only regret was that I wasn't the one to carry it out. That I didn't get the satisfaction of seeing him die. I wish I was the killer but I'm not and THAT is the truth, I hope you're happy."},

            {"Greed: Aha! The detective, right on time here to solve the crime. I hope you know what you’re doing. I don’t intend to pay you if you don’t do this right. The noble was my father after all- what reason would I have to kill him! But I suppose you want to know what I was doing. Fine. I was in my room, writing a letter to a… friend of mine, and playing my games when I hear a scream. It sounded like my mothers, so I run downstairs and what do I find? My father- DEAD! You better solve this crime.\r\n", " Fine! Listen, I didn't kill him I swear! I would never do something like that! I've been taking out money in his name for the past few years to try and pay off some of my old debt-  but unlike my mother, I DO love my father "},
        };

        /*string[] pushReact = //holds placeholders for the reaction to being pushed for more information
        {
            "Lust: Reaction", "Sloth: Reaction", "Greed: Reaction", "Wrath: Reaction", "Envy: Reaction", "Gluttony: Reaction", "Pride: Reaction"
        };*/

        //we most likely want three items per each suspect, and One react Else. 4 rows per char.
        string[,] itemReact = //holds placeholders for the reaction to being shown an item
        {
            {"Lust: Oh? This FALLEN BOOK? I was indeed reading it a few days ago, darling. I’m a little interested in these types of things, you know? I wanted to be an author before I married my lovely spouse","Lust: Oh these LOVE LETTERS? Those aren't anything, darling. I was simply fantasing about my long-lost lover.", "Lust: (She inspects the SMALL BOTTLE) This...? Ah, dear this is just perfume! I can see how you think it might be 'poison' but its not. Why would little ol' me have something as scary as that?","Lust: My dear, I simply haven't the faintest idea of what that could possibly be."},

            {"Sloth: (he yawns) Dirty clothes...? I could've.. (mmm) sworn I washed those ealier... let me... think on that. (he sits down in a chair, closing his eyes)","Sloth: Mhhm.. Those… TOWELS? Yes, indeed.. (he yawns). I should’ve… washed it earlier… the Lady said… not to, though. (He closes his eyes and appears to go back to sleep)","Sloth: (Snore) Aah...? That FANCY NOTE? (snore) hmum…(yawn) wha..huh…oh.. Yes, I remember those, I believe they were instructions given to me by Lady Lust a bit odd but I didn't think anything of it when she gave it to me..","Sloth: (He gives a big yawn) Huh..? What... mmn.. thats not... mine. (He yawns again)"},

            {"Pride: Those DUEL CLOTHES? Of course I know whose those are! They’re mine! Why are they torn and bloody, you ask me? Well, I had come here from a duel before hand. I couldn’t appear like a murder suspect, so I took it off and changed!","Pride: Yeah, that's my sword! The finest blade in all the land, crafted by only the BEST blacksmiths. Why, I'm certain its the finest in all of the kingdoms combined. Why do you ask? Ah- Blood, on my favoured blade? Well I wouldn't know anything about that! Someone else must have been using my beloved blade, not that I would blame them-- who WOULDN'T want to use this beauty?", "Pride: (you hand the DUEL NOTE to Pride) Oho! Indeed yes, I did send this to him! He never got back to me though, what a total letdown! I would’ve loved to fight him", "Pride: Are you trying to make a fool of me? That *thing* does not belong to me!"},

            {"Gluttony: (he chews hastily) mmhmg? Yes- yes, those ALCOHOL BOTTLES- They did come from my kitchen! I don’t remember who took them out though.","Gluttony: Twugh rwadio? (they remember their manners, and swallow the food) That RADIO? Oh! Y-Yes, that should’ve been in the kitchen, but I was cooking last night and it wasn’t there", "Gluttony: ”What? That BOWL of Cereal? Whats that gotta do with anything? I was craving something to eat, and so I got some food! You get it, right, Detective?" , "Gluttony: (They chew on a bite of food for a moment, before speaking) MNmg? Thats not my stuff."},

            {"Wrath: My COMPUTER? Why do you have that?! Those documents are none of your business, you absolute CLOWN. It has nothing to do with YOU.\r\n","Wrath: (You hand the LETTERS to Wrath) Hah? What do these have to do with me? Can’t you CLEARLY tell that that damned Noble wrote those? Perhaps in an attempt to intimidate ME.", "Wrath: Those BOOKS? What, I can’t enjoy a good crime novel now? I don’t see the purpose in bothering me with those things. Pathetic. Jumping to conclusions just because I just so happened to have this Book in my possession beforehand.", "Wrath: What WORTHLESS trash! This one hasn't seen such a disgusting thing in his life! How dare you assume this is mine!"},

            {"Envy: (They take the SERVANTS NOTE) Hey! What's with that look, hah? Its not as though others haven’t done this in the past. Us Servants have an honor code, back off, Mx Detective.","Envy: Ah? The SERVANT CALENDER? So *what*? Just cause I wasn’t supposed to be working the day he was murdered doesn’t mean anything! Its not like I had anything to do. Its not like I went to the town or anything, whats got you so pressed?", "Envy: Gimme that! That's my DIARY! Personal thoughts and all! Not like some 2-bit detective like you would understand, looking into it and all.", "Envy: Hmph. Thats not mine. I don’t have enough money to even get something like that. Now get outta here."},

            {"Greed: My BANK STATEMENT? Dear Detective, sometimes a man has to get money from his father! Especially, if, say, they were running out of money! Not that I would be running out of money, of course!","Greed: Oh! Is this my CHECK BOOK? Why, thank you for finding it for me! --What, you say it’s not mine? Well, I’ve been using it for quite awhile, so I’d consider it mine, Detective.", "Greed: (You hand the DEBT LETTER to Greed) Where did you find this?! No, no, this is nothing for you to worry about Dear Detective! In fact- why don’t we pretend *you* never saw this?", "Greed: Ooh! I wish that were mine, but I’m afraid, dear detective, that it’s not. But~ You could give it to me. No? Fine then."}
        };





        public Npcs(int startRoom)
        {
            //theoretically places the NPCS within the main room. 
            _roomIndex = startRoom;
            _currentRoom = Environment.Scene[_roomIndex];
            for (int i = 0; i < counters.Length; i++)
            {
                counters[i] = 0;
            }

        }

        //public static Room RoomIndex { get { return _roomIndex; } set { _roomIndex = value; } }
        public static Room CurrentRoom { get { return _currentRoom; } set { _currentRoom = value; } }

        //method for push reactions
        //loops through the length of the array, then checks if it contains the suspects name, before printing it out and then breaking out of the loop.
        //chose not to use for this submission, but theoretically in an actual game would be part of the alibi finding.
       /* public string pushReaction(string suspect) {
            string rv = "";

            for (int i = 0; i < pushReact.Length - 1; i++) {
                //has to ignore the case due to passing it in. If anyone needs to use .Contains anywhere else, don't forget to add:
                //StringComparison.OrdinalIgnoreCase since it doesn't care abt casing, just wants to see if the words match.
                if (pushReact[i].Contains(suspect, StringComparison.OrdinalIgnoreCase)) {
                    rv = pushReact[i];
                    break;
                }
            }
            return rv;
        }*/

        public void suspectList() {
            Console.WriteLine("The suspects introduce themselves to you, some nervous, some angry, and some oddly enough, flirtatious. You now know their names, making it easier to SHOW them items and TALK to them.");
            for (int i = 0; i < suspects.Length; i++) {
                Console.WriteLine(suspects[i]);
            }
        }

        //method to react to items
        public string reactToItems(string suspect, string item) {
            string reaction = "";
            //int row = getIndexOfSuspect(suspect);

            //if suspect is = to statement in array
            //find the item reaction of item

            for (int row = 0; row < itemReact.GetLength(0); row++) {
                //??? compare suspect to Suspect name?
                for (int col = 0; col < 4; col++) {

                    //connect/store the reacted to item in 
                    if (itemReact[row, col].Contains(suspect, StringComparison.OrdinalIgnoreCase) && itemReact[row, col].Contains(item, StringComparison.OrdinalIgnoreCase))
                    {
                        reaction = itemReact[row, col];
                        //after reaction, store it in JOURNAL. 
                        //need to be able to access it.
                        journal.AddEntry(reaction);
                        //using [row] as the where to place the counter, increments item counter by 1
                        //making sure you can't go past 3 with worlds stupidest foolproof plan
                        if (counters[row] == 3)
                        {
                            counters[row] = 3;
                        }
                        else {

                            counters[row]++;
                        }

                        break;
                    }
                    if (itemReact[row, col].Contains(suspect, StringComparison.OrdinalIgnoreCase) && !itemReact[row, col].Contains(item, StringComparison.OrdinalIgnoreCase)) {
                        reaction = itemReact[row, 3];
                    }
                }

            }

            return reaction;
        }

        private int getIndexOfSuspect(string arr) {
            int rv = -1;

            for (int i = 0; i < suspects.Length; i++)
            {
                if (arr.ToUpper() == suspects[i].ToUpper()) {
                    rv = i;
                }
            }

            return rv;
        }

        public string TalkToSuspect(string suspect)
        {
            string reaction = "";
            int row = getIndexOfSuspect(suspect);


            //chose to do 2 alibis for the amount of time we have left. the [og] alibi and the [full] alibi.
            //espec with the "Item" reaction to help solve mystery.
            if (counters[row] == 3)
            {
                reaction = susAlibi[row, 1];
                journal.AddEntry(reaction);
            }
            else
            {
                reaction = susAlibi[row, 0];
                journal.AddEntry(reaction);
            }
            return reaction;

        }


        //method for winning & loosing
        //checks what the player said for the suspect. if it wasn't lust it goes down a guess until you have none left.
        //then exits the game no matter if you win or lose
        public int winLose(string suspect)
        {
            int rv = 0;
            if (suspect == "lust")
            {
                Console.WriteLine("Lust stares at you their false seductive expression now overcome with rage as they pull out a stiletto dagger. \r\n“YOU INSOLENT FU-!” \r\nSuddenly Envy lunges toward Lust, tackling them to the ground and the two begin to fight with the knife clattering across the floor straight to the feet of Greed who looks up from his DS in shock. Two officers charge through the front door of the house and immediately begin shouting commands to the shocked crowd of suspects. They separate Lust and Envy, placing Lust in handcuffs. As you silently exit the scene and go back to your carriage you state to yourself, amused:\r\n “Another day another case solved” \r\nwith the feeling that this is only the beginning.\r\n");
                rv = 1;
            }
            else
            {
                guesses--;
                if (guesses == 0)
                {
                    Console.WriteLine("You lost. You failed both of your guesses, and the culprit walks free. It was admirable, but ultimately you lost.");
                    rv = 2;

                }
                else
                {
                    Console.WriteLine("That was the wrong suspect. You have 1 guess remaining");
                    rv = 0;
                }

            }

            return rv;
        }

        //create method to let player know how many items / 3 that they've found for each suspect.
        //only increments the / 3 count AFTER you have talked with the correct suspect and they've reacted to it.
        //this is a *feature* and it was *on purpose*. It is *not a bug*.
        public string itemsLeft(string suspect) {
            string rv = "";
            int itemsLeftOf = getIndexOfSuspect(suspect);

            rv = "Of " + suspects[itemsLeftOf] + " item's, you have " + counters[itemsLeftOf] + " / 3 found";

            return rv;
        }
        // : )

    }
}