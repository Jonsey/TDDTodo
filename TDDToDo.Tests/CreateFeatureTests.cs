using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TDDToDo.Models;

namespace TDDToDo.Tests
{
    
    // Feature API is not intuitive
    // Completed item set to inprogress should not be completed
    // Completed item should not be in progress
    // Flag scenario as completed
    // Flag scenario as in progress 
    // Add an scenario to the feature
    // Give feature a category
    // List of features 

    [TestFixture]
    public class CreateFeatureTests
    {
        const string Title = "Make excellent bit of software";
        const string Detail = "In order the do something As a user I want to be able to do something";

        Feature feature;
        Scenario scenario;

        void InitialiseFeatureList()
        {
            feature = new Feature(Title, Detail);
        }

        void AddScenario()
        {
            scenario = new Scenario("Title");
            feature.AddScenario(scenario);
        }

        [Test]
        public void ShouldBeAbleToCreateAFeature()
        {
            InitialiseFeatureList();
            Assert.IsNotNull(feature);
        }

        [Test]
        public void ShouldBeAbleToAddANewScenario()
        {
            InitialiseFeatureList();
            AddScenario();

            Assert.AreEqual(scenario, feature.Scenarios[0]);
        }

        [Test]
        public void FeatureShouldHaveATitle()
        {
            InitialiseFeatureList();

            Assert.AreEqual(Title, feature.Title);
        }

        [Test]
        public void FeatureShouldHaveDetail()
        {
            InitialiseFeatureList();

            Assert.AreEqual(Detail, feature.Detail);
        }

        [Test]
        public void ShouldMarkAsInProgress()
        {
            InitialiseFeatureList();
            AddScenario();

            feature.Scenarios[0].SetInProgress();

            Assert.IsTrue(feature.Scenarios[0].InProgress);
        }

        [Test]
        public void ShouldMarkAsCompleted()
        {
            InitialiseFeatureList();
            AddScenario();

            feature.Scenarios[0].SetCompleted();

            Assert.IsTrue(feature.Scenarios[0].Completed);
        }

        [Test]
        public void InProgressItemShouldNotBeInprogressAfterCompletion()
        {
            InitialiseFeatureList();
            AddScenario();

            feature.Scenarios[0].SetInProgress();
            feature.Scenarios[0].SetCompleted();

            Assert.IsFalse(feature.Scenarios[0].InProgress);
        }

        [Test]
        public void CompletedScenarioSetToInprogressShouldNotBeCompleted()
        {
            InitialiseFeatureList();
            AddScenario();

            feature.Scenarios[0].SetCompleted();
            feature.Scenarios[0].SetInProgress();

            Assert.IsFalse(feature.Scenarios[0].Completed);
        }
    }
}
