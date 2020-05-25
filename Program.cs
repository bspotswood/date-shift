using System;
using System.IO;

namespace date_shift
{
  class Program
  {

    static Arguments a;

    /// <summary>
    /// Updates the created and updated times on files
    /// </summary>
    /// <param name="args">Program parameters</param>
    /// <returns>0 if successful, 1 if bad arguments, 2 if errors occurred during processing, 3 if some other exception occurs</returns>
    static int Main(string[] args)
    {
      try
      {
        a = Arguments.parse(args);
        if (a == null) return 1;

        {
          var now = DateTime.Now;
          var diff = now.Subtract(a.applyShift(now));
          info("Modifying dates in folder: " + a.Folder);
          info(a.Pattern == null ? "Pattern: All files" : "Pattern: " + a.Pattern);
          info("Date shift: " + diff.ToString());
          info("Files:");
        }

        int updated = 0;
        bool hadErrors = false;
        var files = a.Pattern == null ? Directory.GetFiles(a.Folder) : Directory.GetFiles(a.Folder, a.Pattern);
        foreach (var file in files)
        {
          DateTime ct, ut;

          var name = Path.GetFileName(file);
          info((++updated) + ": " + name);

          try
          {
            ct = File.GetCreationTime(file);
            ut = File.GetLastWriteTime(file);

            verbose("  Created: " + ct.ToString());
            verbose("  Updated: " + ut.ToString());
          }
          catch (Exception ex)
          {
            hadErrors = true;
            err("Error retrieving times for file \"" + name + "\"");
            err(ex.ToString());
            continue;
          }

          try
          {
            ct = a.applyShift(ct);
            File.SetCreationTime(file, ct);
            verbose("  New Created: " + ct);
          }
          catch (Exception ex)
          {
            hadErrors = true;
            err("Error setting create time for file \"" + name + "\"");
            err(ex.ToString());
            continue;
          }

          try
          {
            ut = a.applyShift(ut);
            File.SetLastWriteTime(file, ut);
            verbose("  New Updated: " + ut);
          }
          catch (Exception ex)
          {
            hadErrors = true;
            err("Error setting last write time for file \"" + name + "\"");
            err(ex.ToString());
            continue;
          }
        }

        return hadErrors ? 2 : 0;
      }
      catch(Exception ex)
      {
        err(ex.ToString());
        return 3;
      }
    } // main

    static void err(String msg)
    {
      Console.Error.WriteLine(msg);
    }

    static void info(String msg)
    {
      if (a.Quiet) return;
      Console.WriteLine(msg);
    }

    static void verbose(String msg)
    {
      if (a.Quiet) return;
      if (!a.Verbose) return;
      Console.WriteLine(msg);
    }
  }

}
