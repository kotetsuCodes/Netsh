using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetshTests
{
  [TestClass]
  public class FirewallTests
  {
    private string getGeneratedTestRuleName()
    {
      return $"{Guid.NewGuid().ToString()}_NetshTesting";
    }

    [TestMethod]
    public void GetFirewallRules()
    {
      var rules = Netsh.Firewall.Rules.GetFirewallRules();

      Assert.IsTrue(rules.Any());
    }

    [TestMethod]
    public void CreateFirewallRuleShouldSucceed()
    {
      Netsh.Firewall.Rules.CreateFirewallRule(getGeneratedTestRuleName(), "in", "allow", "TCP", "7777");
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CreateFirewallRuleShouldFail()
    {
      Netsh.Firewall.Rules.CreateFirewallRule(getGeneratedTestRuleName(), "this should fail", "this should fail", "this should fail", "this should fail");
    }

    [TestMethod]
    public void CreateAndDeleteFirewallRuleShouldSuccess()
    {
      string firewallRuleName = getGeneratedTestRuleName();

      Netsh.Firewall.Rules.CreateFirewallRule(firewallRuleName, "in", "allow", "TCP", "7777");

      Netsh.Firewall.Rules.DeleteFirewallRule(firewallRuleName, "TCP", "7777");
    }
  }
}
