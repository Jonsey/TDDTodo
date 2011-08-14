using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TDDToDo.Models;

namespace TDDToDo.Tests
{
    [TestFixture]
    public class AddScenarioDetailsTest
    {
        [Test]
        public void ShouldBeAbleToAddAListOfStepsToAScenario()
        {
            var scenario = new Scenario("Add Steps to a scenario");
            scenario.AddStep("Given I have selected a feature");
            scenario.AddStep("And I have selelected a scenario");
            scenario.AddStep("When I press 'Add steps'");
            scenario.AddStep("And I enter 'something'");
            scenario.AddStep("And I press 'Create step");
            scenario.AddStep("Then a step should be added to the scenario");

            Assert.IsTrue(scenario.Steps.Count == 6);
        }
    }
}
