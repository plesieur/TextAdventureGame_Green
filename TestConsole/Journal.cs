using System;
using System.Collections.Generic;

public class Journal
{
    
    private List<string> entries;

    public Journal()
    {
        entries = new List<string>();
    }

    public void AddEntry(string text)
    {
        for(int i = 0; i < entries.Count; i++)
        {
            if (entries[i] == text)
            {
                Console.WriteLine("Statement already logged in Journal!");
            }
            else {
                entries.Add(text);
            }
        }
    }

    public void RemoveEntry(int index)
    {
        entries.RemoveAt(index);
    }

    public void PrintEntries()
    {
        Console.WriteLine("Journal Entries:");
        for (int i = 0; i < entries.Count; i++)
        {
            Console.WriteLine(entries[i]);
        }
    }

    //absolutely unsure what this means??? ignoring it and Not adding it to final version.
    public string GetClue()
    {
        string clue = "";
        // Look for specific keywords in the journal entries
        if (entries.Contains("found a clue"))
        {
            clue = "You remember seeing a clue in your journal. Maybe it can help you navigate.";
        }
        else if (entries.Contains("talked to the suspect"))
        {
            clue = " Maybe you should investigate.";
        }
        else
        {
            clue = "You don't have any new clues in your journal.";
        }
        return clue;
    }

    // Create a new journal object
    //Journal journal = new Journal();

    // Add an entry to the journal
   /* journal.AddEntry("boop.");

    // Add another entry to the journal
    journal.AddEntry("Found a map on the table.");

    // Print all journal entries
    journal.PrintEntries();

    // Get a clue from the journal
    string clue = journal.GetClue();

    // Print the clue
    Console.WriteLine(clue);*/


 
}
