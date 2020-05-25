using System;
using System.Collections.Generic;
using System.IO;

public class Arguments
{
  private enum ArgAction
  {
    getKey,
    addSeconds,
    addMinutes,
    addHours,
    addDays,
    addWeeks,
    addMonths,
    addYears,
    pattern,
    folder,
    quiet,
    verbose,
  }

  private static Dictionary<string, ArgAction> argMap = new Dictionary<string, ArgAction>() {
    { "--add-seconds", ArgAction.addSeconds },
    { "-s",            ArgAction.addSeconds },
    { "--add-minutes", ArgAction.addMinutes },
    { "-m"           , ArgAction.addMinutes },
    { "--add-hours",   ArgAction.addHours   },
    { "-h",            ArgAction.addHours   },
    { "--add-days",    ArgAction.addDays    },
    { "-d",            ArgAction.addDays    },
    { "--add-weeks",   ArgAction.addWeeks   },
    { "-w",            ArgAction.addWeeks   },
    { "--add-months",  ArgAction.addMonths  },
    { "-mo",           ArgAction.addMonths  },
    { "--add-years",   ArgAction.addYears   },
    { "-y",            ArgAction.addYears   },
    { "--pattern",     ArgAction.pattern    },
    { "-p",            ArgAction.pattern    },
    { "--folder",      ArgAction.folder     },
    { "-f",            ArgAction.folder     },
    { "--quiet",       ArgAction.quiet      },
    { "-q",            ArgAction.quiet      },
    { "--verbose",     ArgAction.verbose    },
    { "-v",            ArgAction.verbose    },
  };

  private static void PrintArgs()
  {
    Console.WriteLine("Modifies the create and modified timestamps on files");
    Console.WriteLine("Usage: date-shift [--option value] -f path/to/files");
    Console.WriteLine("Arguments:");
    Console.WriteLine("  -s,  --add-seconds: Number of seconds to add");
    Console.WriteLine("  -m,  --add-minutes: Number of minutes to add");
    Console.WriteLine("  -h,  --add-hours:   Number of hours to add");
    Console.WriteLine("  -d,  --add-days:    Number of days to add");
    Console.WriteLine("  -w,  --add-weeks:   Number of weeks to add");
    Console.WriteLine("  -mo, --add-months:  Number of months to add");
    Console.WriteLine("  -y,  --add-years:   Number of years to add");
    Console.WriteLine("  -p,  --pattern:     Basic wildcarding LIKE pattern for file matching (i.e. *.jpg)");
    Console.WriteLine("  -f,  --folder:      Folder to scan for files within");
    Console.WriteLine("  -q,  --quiet:       Do not print progress output");
    Console.WriteLine("  -v,  --verbose:     Print additional progress output");
    Console.WriteLine("");
    Console.WriteLine("Note: Values can include a negative sign (i.e. \"--add-days 1 --add-hours -12\" is the same as \"--add-hours 12\")");
    Console.WriteLine("");
  }

  public int AddSeconds { get; private set; }
  public int AddMinutes { get; private set; }
  public int AddHours { get; private set; }
  public int AddDays { get; private set; }
  public int AddWeeks { get; private set; }
  public int AddMonths { get; private set; }
  public int AddYears { get; private set; }
  public string Pattern { get; private set; }
  public string Folder { get; private set; }
  public bool Quiet { get; private set; }
  public bool Verbose { get; private set; }


  /// <summary>
  /// Private constructor.
  /// Use <see cref="parse(string[])"/> to create a new instance.
  /// </summary>
  private Arguments() { }


  /// <summary>
  /// Parse command line arguments into an Arguments object
  /// </summary>
  /// <param name="args">The args passed to the program</param>
  /// <returns>a new Arguments instance if valid, otherwise null</returns>
  public static Arguments parse(string[] args)
  {
    Arguments a = new Arguments();
    if (!a.processArgs(args))
    {
      PrintArgs();
      return null;
    }

    return a;
  }


  /// <summary>
  /// Process the command line argumens, setting properties as appropriate on the this object.
  /// Errors will be emitted to standard error as messages.
  /// </summary>
  /// <param name="args">The parameters</param>
  /// <returns>True if successful, otherwise false.</returns>
  private bool processArgs(string[] args)
  {
    var action = ArgAction.getKey;
    int i;
    foreach (var arg in args)
    {
      switch (action)
      {
        case ArgAction.getKey:
          if (arg.StartsWith("-"))
          {
            if (!argMap.ContainsKey(arg.ToLower()))
            {
              Console.Error.WriteLine("Invalid argument: " + arg);
              return false;
            }
            action = argMap[arg.ToLower()];
          } else
          {
            Console.Error.WriteLine("Invalid argument: " + arg);
            return false;
          }
          break;

        case ArgAction.addSeconds:
          if(AddSeconds != 0)
          {
            Console.Error.WriteLine("Add Seconds set more than once!");
            return false;
          }
          if (!int.TryParse(arg, out i))
          {
            Console.Error.WriteLine("Invalid seconds value: " + arg);
            return false;
          }
          AddSeconds = i;
          action = ArgAction.getKey;
          break;

        case ArgAction.addMinutes:
          if (AddMinutes != 0)
          {
            Console.Error.WriteLine("Add Minutes set more than once!");
            return false;
          }
          if (!int.TryParse(arg, out i))
          {
            Console.Error.WriteLine("Invalid minutes value: " + arg);
            return false;
          }
          AddMinutes = i;
          action = ArgAction.getKey;
          break;

        case ArgAction.addHours:
          if (AddHours != 0)
          {
            Console.Error.WriteLine("Add Hours set more than once!");
            return false;
          }
          if (!int.TryParse(arg, out i))
          {
            Console.Error.WriteLine("Invalid hours value: " + arg);
            return false;
          }
          AddHours = i;
          action = ArgAction.getKey;
          break;

        case ArgAction.addDays:
          if (AddDays != 0)
          {
            Console.Error.WriteLine("Add Days set more than once!");
            return false;
          }
          if (!int.TryParse(arg, out i))
          {
            Console.Error.WriteLine("Invalid days value: " + arg);
            return false;
          }
          AddDays = i;
          action = ArgAction.getKey;
          break;

        case ArgAction.addWeeks:
          if (AddWeeks != 0)
          {
            Console.Error.WriteLine("Add Weeks set more than once!");
            return false;
          }
          if (!int.TryParse(arg, out i))
          {
            Console.Error.WriteLine("Invalid weeks value: " + arg);
            return false;
          }
          AddWeeks = i;
          action = ArgAction.getKey;
          break;

        case ArgAction.addMonths:
          if (AddMonths != 0)
          {
            Console.Error.WriteLine("Add Months set more than once!");
            return false;
          }
          if (!int.TryParse(arg, out i))
          {
            Console.Error.WriteLine("Invalid months value: " + arg);
            return false;
          }
          AddMonths = i;
          action = ArgAction.getKey;
          break;

        case ArgAction.addYears:
          if (AddYears != 0)
          {
            Console.Error.WriteLine("Add Years set more than once!");
            return false;
          }
          if (!int.TryParse(arg, out i))
          {
            Console.Error.WriteLine("Invalid years value: " + arg);
            return false;
          }
          AddYears = i;
          action = ArgAction.getKey;
          break;

        case ArgAction.pattern:
          Pattern = arg;
          action = ArgAction.getKey;
          break;

        case ArgAction.folder:
          if(!Directory.Exists(arg))
          {
            Console.Error.WriteLine("Invalid directory!");
            return false;
          }
          Folder = arg;
          action = ArgAction.getKey;
          break;

        default:
          Console.Error.WriteLine("Invalid argument: " + arg);
          return false;
      } // end-switch

      // Handle single arg actions
      if(action == ArgAction.quiet)
      {
        Quiet = true;
        action = ArgAction.getKey;
      }
      else if(action == ArgAction.verbose)
      {
        Verbose = true;
        action = ArgAction.getKey;
      }

    } // end-for

    if(action != ArgAction.getKey)
    {
      Console.Error.WriteLine("Missing value for " + action.ToString());
      return false;
    }

    // Parms must have an effect on time
    var dt = DateTime.Now;
    if(applyShift(dt).Equals(dt))
    {
      Console.Error.WriteLine("No change in time given!");
      return false;
    }

    // Must have set a folder path
    if(Folder == null || Folder.Length == 0)
    {
      Console.Error.WriteLine("Folder path not specified!");
      return false;
    }

    return true;
  } // processArgs


  /// <summary>
  /// Get a <see cref="TimeSpan"/> that represents the shift to be made to 
  /// </summary>
  /// <returns></returns>
  public DateTime applyShift(DateTime dt)
  {
    DateTime result = dt;
    if (AddYears != 0) result = result.AddYears(AddYears);
    if (AddMonths != 0) result = result.AddMonths(AddMonths);
    if (AddWeeks != 0) result = result.AddDays(7 * AddWeeks);
    if (AddDays != 0) result = result.AddDays(AddDays);
    if (AddHours != 0) result = result.AddHours(AddHours);
    if (AddMinutes != 0) result = result.AddMinutes(AddMinutes);
    if (AddSeconds != 0) result = result.AddSeconds(AddSeconds);
    return result;
  }

}
