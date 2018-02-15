using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetshTests
{
  [TestClass]
  public class Firewall
  {
    [TestMethod]
    public void GetFirewallRules()
    {
      var rules = Netsh.Firewall.Rules.GetFirewallRules();

      Assert.IsTrue(rules.Any());
    }
  }
}