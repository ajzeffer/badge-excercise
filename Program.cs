using System;
using System.Collections.Generic;
using System.Linq;

/*

We are working on a security system for a badged-access room in our company's building.

Given an ordered list of employees who used their badge to enter or exit the room, write a function that returns two collections:

1. All employees who didn't use their badge while exiting the room - they recorded an enter without a matching exit. (All employees are required to leave the room before the log ends.)

2. All employees who didn't use their badge while entering the room - they recorded an exit without a matching enter. (The room is empty when the log begins.)

Each collection should contain no duplicates, regardless of how many times a given employee matches the criteria for belonging to it.

badge_records_1 = [
  ["Martha",   "exit"],
  ["Paul",     "enter"],
  ["Martha",   "enter"],
  ["Martha",   "exit"],
  ["Jennifer", "enter"],
  ["Paul",     "enter"],
  ["Curtis",   "exit"],
  ["Curtis",   "enter"],
  ["Paul",     "exit"],
  ["Martha",   "enter"],
  ["Martha",   "exit"],
  ["Jennifer", "exit"],
  ["Paul",     "enter"],
  ["Paul",     "enter"],
  ["Martha",   "exit"],
]

Expected output: ["Curtis", "Paul"], ["Martha", "Curtis"]

Additional test cases:

badge_records_2 = [
  ["Paul", "enter"],
  ["Paul", "enter"],
  ["Paul", "exit"],
]

Expected output: ["Paul"], []

badge_records_3 = [
  ["Paul", "enter"],
  ["Paul", "exit"],
  ["Paul", "exit"],
]
        var badgeRecords = new string[][] {
          new string[] {"Paul",     "enter"},
          new string[] {"Paul",   "exit"},
          new string[] {"Paul",   "exit"},
        };
Expected output: [], ["Paul"]

badge_records_4 = [
  ["Paul", "exit"],
  ["Paul", "enter"],
  ["Martha", "enter"],
  ["Martha", "exit"],
]

        var badgeRecords = new string[][] {
          new string[] {"Paul",     "exit"},
          new string[] {"Paul",     "enter"},
          new string[] {"Martha",   "enter"},
          new string[] {"Martha",   "exit"},
        };

Expected output: ["Paul"], ["Paul"]

badge_records_5 = [
  ["Paul", "enter"],
  ["Paul", "exit"],
]

Expected output: [], []

badge_records_6 = [
  ["Paul", "enter"],
  ["Paul", "enter"],
  ["Paul", "exit"],
  ["Paul", "exit"],
]

        var badgeRecords = new string[][] {
          new string[] {"Paul",     "enter"},
          new string[] {"Paul",     "enter"},
          new string[] {"Paul",   "exit"},
          new string[] {"Paul",   "exit"},
        };
Expected output: ["Paul"], ["Paul"]


n: length of the badge records array


*/

public class BadgeOutput
{
    public List<string> BadEnters { get; set; }
    public List<string> BadExits { get; set; }
}

public class UserStatus{
    public bool HasBadEntries{get;set;}
    public bool HasBadExists{get;set;}
}

class Solution
{
    private static string _entryText = "enter";
    private static string _exitText = "exit";
    static void Main(String[] args)
    {


        var badgeRecords = new string[][] {
          new string[] {"Paul",     "enter"},
          new string[] {"Paul",   "exit"},
          new string[] {"Paul",   "exit"},
        };

        var output = GetBadgeOutput(GetGroupedBadgeTraffic(GetUniqueNames(badgeRecords), badgeRecords));

        Console.WriteLine($"Bad Enters: {String.Join(",", output.BadEnters)}");
        Console.WriteLine($"Bad Exits: {String.Join(",", output.BadExits)}");
        Console.WriteLine("Finished");

    }

    public static List<string> GetUniqueNames(string[][] users)
    {
        var namedUsers = new List<string>();
        for (var i = 0; i < users.Length; i++)
        {
            var user = users[i];
            if (!namedUsers.Any(x => x == user[0]))
            {
                namedUsers.Add(user[0]);
            }
        };
        return namedUsers;
    }

    public static BadgeOutput GetBadgeOutput(Dictionary<string, List<string>> usersTraffic) {
        var output = new BadgeOutput{
            BadEnters = new List<string>(),
            BadExits = new List<string>()
        };

        foreach(var user in usersTraffic){
            var userStatus = GetUserStatus(user.Value);
            if(userStatus.HasBadEntries){
                output.BadEnters.Add(user.Key);
            }
            if(userStatus.HasBadExists){
                output.BadExits.Add(user.Key);
            }
        }
        return output;
    }
    public static Dictionary<string, List<string>> GetGroupedBadgeTraffic(List<string> names, string[][] users)
    {
        var namedUsers = new Dictionary<string, List<string>>();
        names.ForEach(name =>
        {
            var entriesAndExists = new List<string>();
            for (var iPath = 0; iPath < users.Length; iPath++)
            {
                var item = users[iPath];
                var itemName = item[0];
                if (name == itemName)
                {
                    entriesAndExists.Add(item[1]);
                }
            }
            namedUsers.Add(name, entriesAndExists);
        });

        return namedUsers;
    }

    public static UserStatus GetUserStatus(List<string> usersTraffic){
       var currentRequiredDirection = _entryText;
       var iBadEnters = 0;
       var iBadExists = 0;

        usersTraffic.ForEach(direction => {
            if(direction == currentRequiredDirection){
                // switch to new required direction
                currentRequiredDirection = currentRequiredDirection == _entryText ? _exitText : _entryText;
               return;
            }
            if(currentRequiredDirection == _entryText){
                iBadEnters++;
            }
            if(currentRequiredDirection == _exitText){
                iBadExists++;
            }

        });

        // make sure user always exists
        var final = usersTraffic[usersTraffic.Count -1];
        if(final != _exitText){
            iBadExists++;
        }

        return  new UserStatus{
           HasBadEntries = iBadEnters > 0,
           HasBadExists = iBadExists > 0
       };
    }
    public static List<string> GetBadExists(){
        return null;
    }
}
