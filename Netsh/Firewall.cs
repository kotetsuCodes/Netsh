﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netsh.Lib;

namespace Netsh
{
  /// <summary>
  /// Class for managing Windows Firewall Rules using netsh
  /// </summary>
  public static class Firewall
  {
    /// <summary>
    /// Organizational class housing static methods for managing Windows Firewall Rules
    /// </summary>
    public static class Rules
    {
      /// <summary>
      /// Returns a List of Rule objects generated from netsh firewall command
      /// </summary>
      /// <returns></returns>
      public static List<Rule> GetFirewallRules()
      {
        var shell = new Win32Shell();
        var standardOutput = shell.Exec(Win32Paths.NetshExe, "advfirewall firewall show rule name=all");

        return getRulesFromStandardOutput(standardOutput);
      }
    }

    /// <summary>
    /// Generates Rule objects from netsh firewall command output
    /// </summary>
    /// <param name="netshOutput"></param>
    /// <returns>List of Rule objects</returns>
    private static List<Rule> getRulesFromStandardOutput(List<string> netshOutput)
    {
      List<Rule> rules = new List<Rule>();
      var currentRule = new Rule();

      foreach(var line in netshOutput)
      {
        switch(line)
        {
          case var testLine when testLine.StartsWith("Rule Name:"):
            currentRule = new Rule();
            currentRule.Name = formatRuleProperty(testLine);
            break;

          case var testLine when testLine.StartsWith("Enabled:"):
            currentRule.Enabled =
formatRuleProperty(testLine); break;

          case var testLine when testLine.StartsWith("Direction:"):
            currentRule.Direction =
formatRuleProperty(testLine); break;

          case var testLine when testLine.StartsWith("Profiles:"):
            currentRule.Profiles =
formatRuleProperty(testLine); break;

          case var testLine when testLine.StartsWith("Grouping:"):
            currentRule.Grouping =
formatRuleProperty(testLine); break;

          case var testLine when testLine.StartsWith("LocalIP:"):
            currentRule.LocalIP =
formatRuleProperty(testLine); break;

          case var testLine when testLine.StartsWith("RemoteIP:"):
            currentRule.RemoteIP =
formatRuleProperty(testLine); break;

          case var testLine when testLine.StartsWith("Protocol:"):
            currentRule.Protocol =
formatRuleProperty(testLine); break;

          case var testLine when testLine.StartsWith("LocalPort:"):
            currentRule.LocalPort =
formatRuleProperty(testLine); break;

          case var testLine when testLine.StartsWith("RemotePort:"):
            currentRule.RemotePort =
formatRuleProperty(testLine); break;

          case var testLine when testLine.StartsWith("Edge traversal:"):
            currentRule.EdgeTraversal =
formatRuleProperty(testLine); break;

          case var testLine when testLine.StartsWith("Action:"):
            currentRule.Action = formatRuleProperty(testLine);
            rules.Add(currentRule);
            break;
        }
      }

      return rules;
    }

    private static string formatRuleProperty(string testLine)
    {
      return testLine.Substring(testLine.IndexOf(':') + 1).Trim();
    }

    public class Rule
    {
      public string Name { get; set; }
      public string Direction { get; set; }
      public string Profiles { get; set; }
      public string Grouping { get; set; }
      public string LocalIP { get; set; }
      public string RemoteIP { get; set; }
      public string Protocol { get; set; }
      public string LocalPort { get; set; }
      public string RemotePort { get; set; }
      public string EdgeTraversal { get; set; }
      public string Action { get; set; }
      public string Enabled { get; set; }
    }
  }
}
