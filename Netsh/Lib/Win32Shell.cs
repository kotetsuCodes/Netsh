using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Netsh.Lib
{
  public class Win32Shell
  {
    public Win32Shell()
    {
      this.StandardOutput = new List<string>();
      this.StandardError = new List<string>();
    }

    public List<string> StandardOutput { get; private set; }
    public List<string> StandardError { get; private set; }

    /// <summary>
    /// Method for executing processes on Windows
    /// </summary>
    /// <param name="executable"></param>
    /// <param name="arguments"></param>
    /// <param name="standardOut"></param>
    /// <param name="standardError"></param>
    public List<string> Exec(string executable, string arguments)
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

        return this.StandardOutput;
      }
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
