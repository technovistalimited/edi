﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.3.0.0
//      SpecFlow Generator Version:2.3.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace AIMS_BD_IATI.Test.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.3.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class PolicyMarkersFromIATIFeature : Xunit.IClassFixture<PolicyMarkersFromIATIFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "PolicyMarker.feature"
#line hidden
        
        public PolicyMarkersFromIATIFeature(PolicyMarkersFromIATIFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Policy markers from IATI", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute(DisplayName="User can see policy markers in the IATI import interface")]
        [Xunit.TraitAttribute("FeatureTitle", "Policy markers from IATI")]
        [Xunit.TraitAttribute("Description", "User can see policy markers in the IATI import interface")]
        public virtual void UserCanSeePolicyMarkersInTheIATIImportInterface()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User can see policy markers in the IATI import interface", ((string[])(null)));
#line 3
  this.ScenarioSetup(scenarioInfo);
#line 4
    testRunner.Given("User uses the IATI import module to import a project", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 5
    testRunner.And("User proceeds to the `5. Set import preferences` step", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 6
    testRunner.Then("the page includes the list of policy markers that have a significance code that i" +
                    "s not `0`.", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(DisplayName="User can import a policy marker")]
        [Xunit.TraitAttribute("FeatureTitle", "Policy markers from IATI")]
        [Xunit.TraitAttribute("Description", "User can import a policy marker")]
        public virtual void UserCanImportAPolicyMarker()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("User can import a policy marker", ((string[])(null)));
#line 8
  this.ScenarioSetup(scenarioInfo);
#line 9
    testRunner.Given("User imports a project from IATI data", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 10
    testRunner.And("the project contains at least one policy marker with a significance code that is " +
                    "not `0`", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 11
    testRunner.Then("on the `Sector Contribution` tab the `Policy Marker` collapsible contains the sam" +
                    "e policy markers", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.3.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                PolicyMarkersFromIATIFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                PolicyMarkersFromIATIFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
