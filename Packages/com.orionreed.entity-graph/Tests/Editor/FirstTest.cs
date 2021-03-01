using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FirstTest
{
  // A Test behaves as an ordinary method
  [Test]
  public void FirstTestSimplePasses()
  {
    Assert.AreEqual(true, false);
  }
}
