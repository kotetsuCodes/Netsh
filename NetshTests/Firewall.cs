using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetshTests
{
  [TestClass]
  public class Firewall
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
      Netsh.Firewall.Rules.CreateFirewallRule(new Netsh.Firewall.Rule
      {
        Name = $"{Guid.NewGuid().ToString()}_NetshTesting",
        Direction = "in",
        Action = "allow",
        Protocol = "TCP",
        LocalPort = "7777"
      });
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CreateFirewallRuleShouldFail()
    {
      Netsh.Firewall.Rules.CreateFirewallRule(new Netsh.Firewall.Rule
      {
        Name = getGeneratedTestRuleName(),
        Direction = "this should fail",
        Action = "this should fail",
        Protocol = "this should fail",
        LocalPort = "this should fail"
      });
    }
  }
}
