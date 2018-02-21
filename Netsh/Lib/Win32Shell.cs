using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Netsh.Lib
{
  public class Win32Shell
  {
    public Win32Shell()
    {
      // these are necessary, otherwise we throw an exception because List defaults to null
      this.StandardOutput = new List<string>();
      this.StandardError = new List<string>();
    }

    public List<string> StandardOutput { get; private set; }
    public List<string> StandardError { get; private set; }
    public int ExitCode { get; private set; }

    public void CheckForStandardError(Func<List<string>, string> errorAction)
    {
      if(this.StandardError.Any())
        errorAction(this.StandardError);
    }

    public void CheckForBadExitCode(Func<int, string, string, int> errorAction)
    {
      if(this.ExitCode != 0)
        errorAction(this.ExitCode, string.Join(" ", this.StandardOutput), string.Join(" ", this.StandardError));
    }

    /// <summary>
    /// Method for executing processes on Windows
    /// </summary>
    /// <param name="executable"></param>
    /// <param name="arguments"></param>
    /// <param name="standardOut"></param>
    /// <param name="standardError"></param>
    public ShellResult Exec(string executable, string arguments)
    {
      using(var process = new Process())
      {
        var start = new ProcessStartInfo(executable, arguments);

        start.WindowStyle = ProcessWindowStyle.Hidden;
        start.CreateNoWindow = true;
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;
        start.RedirectStandardError = true;

        process.OutputDataReceived += new DataReceivedEventHandler(readOutput);
        process.ErrorDataReceived += new DataReceivedEventHandler(readError);

        process.StartInfo = start;
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();

        this.ExitCode = process.ExitCode;

        return new ShellResult(this.ExitCode, this.StandardOutput, this.StandardError);
      }
    }

    public class ShellResult
    {
      public ShellResult(int returnCode, List<string> stdout, List<string> stderr)
      {
        this.ReturnCode = returnCode;
        this.StandardOutput = stdout;
        this.StandardError = stderr;
      }

      public int ReturnCode { get; private set; }
      public List<string> StandardOutput { get; private set; }
      public List<string> StandardError { get; private set; }
    }

    // adapted small portion from https://stackoverflow.com/questions/14094771/problems-getting-desired-output-from-process-start
    private void readOutput(object sender, DataReceivedEventArgs e)
    {
      if(e.Data != null)
      {
        this.StandardOutput.Add(e.Data);
      }
    }

    private void readError(object sender, DataReceivedEventArgs e)
    {
      if(e.Data != null)
      {
        this.StandardError.Add(e.Data);
      }
    }
  }
}
