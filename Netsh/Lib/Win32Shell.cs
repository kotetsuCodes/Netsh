using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Netsh.Lib
{
  public static class Win32Shell
  {
    /// <summary>
    /// Method for executing processes on Windows
    /// </summary>
    /// <param name="executable"></param>
    /// <param name="arguments"></param>
    /// <param name="standardOut"></param>
    /// <param name="standardError"></param>
    public static void Exec(string executable, string arguments, out List<string> standardOut, out List<string> standardError)
    {
      standardOut = new List<string>();
      standardError = new List<string>();

      var processStartInfo = new ProcessStartInfo()
      {
        UseShellExecute = false,
        RedirectStandardError = true,
        RedirectStandardOutput = true,
        Arguments = arguments,
        WindowStyle = ProcessWindowStyle.Hidden,
        FileName = executable
      };

      var process = Process.Start(processStartInfo);

      while(process.StandardOutput.Peek() > -1)
      {
        standardOut.Add(process.StandardOutput.ReadLine());
      }

      process.WaitForExit();
    }
  }
}