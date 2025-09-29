using NUnit.Framework;
using ProjectMars_AdvanceTask_NUnit.Hooks;
using ProjectMars_AdvanceTask_NUnit.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMars_AdvanceTask_NUnit.Tests
{

   [TestFixture]
    public class ProfileTests : TestBase
    {
        private ProfileDetailSteps _profileSteps;

        [SetUp]
        public void Setup()
        {
            _profileSteps = new ProfileDetailSteps(Driver);
        }
       
        [Test, Category("Profile")]
        public void GivenValidAvailability_WhenUpdate_ThenSuccess()
        {

            _profileSteps.NavigateToProfile();

            _profileSteps.GivenIHaveAvailability("Profile","availability");
            _profileSteps.SetAvailability();

            Assert.That(_profileSteps.GetActualMessage(), Is.EqualTo(_profileSteps.GetExpectedMessage()),"Actual Message is not as expected.");
            Assert.That(_profileSteps.GetActualAvailability(), Is.EqualTo(_profileSteps.GetAvailabilityData()), "Availability is not updated");
        }
       
        [Test, Category("Profile")]
        public void GivenValidHours_WhenUpdate_ThenSuccess()
        {

            _profileSteps.NavigateToProfile();

            _profileSteps.GivenIHaveHours("Profile", "hours");
            _profileSteps.SetHours();

            Assert.That(_profileSteps.GetActualMessage(), Is.EqualTo(_profileSteps.GetExpectedMessage()), "Actual Message is not as expected.");
            Assert.That(_profileSteps.GetActualHours(), Is.EqualTo(_profileSteps.GetHoursData()), "Hours is not updated");
        }

        [Test, Category("Profile")]
        public void GivenValidEarnTarget_WhenUpdate_ThenSuccess()
        {

            _profileSteps.NavigateToProfile();

            _profileSteps.GivenIHaveEarnTarget("Profile", "earnTarget");
            _profileSteps.SetEarnTarget();

            Assert.That(_profileSteps.GetActualMessage(), Is.EqualTo(_profileSteps.GetExpectedMessage()), "Actual Message is not as expected.");
            Assert.That(_profileSteps.GetActualEarnTarget(), Is.EqualTo(_profileSteps.GetEarnTargetData()), "Earn Target is not updated");
        }


    }

}
