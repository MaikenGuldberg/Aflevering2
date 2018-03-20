using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NUnit.Framework;
using NSubstitute;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Intergration
{
    class IT2
    {
        private UserInterface _uut;
        private ILight _light;
        private ICookController _cookController;
        private IDisplay _display;
        private ITimer _timer;
        private IPowerTube _powerTube;
        private IOutput _outputFake;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDoor _door;

        [SetUp]

        public void SetUp()
        {
            _outputFake = Substitute.For<IOutput>();
            _light = new Light(_outputFake);
            _display = new Display(_outputFake);
            _timer = new Timer();
            _powerTube = new PowerTube(_outputFake);
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();
            _cookController = new CookController(_timer, _display, _powerTube) {UI = _uut};
            _uut = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
        }

        [Test]

        public void OnDoorOpened_OpdenDoor_LightComesOn()
        {
            _door.Open();
            _outputFake.Received().OutputLine("Light is turned on");
        }

        [Test]

        public void OnDoorClosed_LightTurnsOff()
        {
            _door.Open();
            _door.Close();
            _outputFake.Received().OutputLine("Light is turned off");
        }

        [Test]
        public void OnPowerPressed_ShowPowerLevel50()
        {
            _powerButton.Press();
            _outputFake.Received().OutputLine("Display shows: 50 W");
        }

        [Test]
        public void OnPowerPressed_PowerButtonPressedTwoTimes_ShowPowerLevel100()
        {
            _powerButton.Press();
            _powerButton.Press();
            _outputFake.Received().OutputLine("Display shows: 100 W");
        }

        [Test]
        public void OnPowerPressed_PowerButtonPressedThreeTimes_ShowPowerLevel150()
        {
            _powerButton.Press();
            _powerButton.Press();
            _powerButton.Press();
            _outputFake.Received().OutputLine("Display shows: 150 W");
        }
        //Tjekker at den højeste værdi man kan sætte power til er 700 W. Hvis man kommer over denne bliver den sat til default 50 W igen.
        [Test]
        public void OnPowerPressed_PowerButtonPressedFifteenTimes_ShowPowerLevel50()
        {
            _powerButton.Press();
            _powerButton.Press();
            _powerButton.Press();
            _powerButton.Press();
            _powerButton.Press();
            _powerButton.Press();
            _powerButton.Press();
            _powerButton.Press();
            _powerButton.Press();
            _powerButton.Press();
            _powerButton.Press();
            _powerButton.Press();
            _powerButton.Press();
            _powerButton.Press();
            _powerButton.Press();
            _outputFake.Received(2).OutputLine("Display shows: 50 W");
        }

        [Test]
        public void OnTimePressed_TimeButtonPressedOneTime_ShowOneMinutAndZeroSekunds()
        {
            _powerButton.Press();
            _timeButton.Press();
            _outputFake.Received().OutputLine("Display shows: 01:00");
        }

        [Test]
        public void OnTimerPressed_TimerButtonPressedTwoTimes_ShowTwoMinutsAndZeroSekunds()
        {
            _powerButton.Press();
            _timeButton.Press();
            _timeButton.Press();
            _outputFake.Received().OutputLine("Display shows: 02:00");
        }

        //Note man har ikke lavet noget der handler hvis man indstiller uret til over en time. (sker måske ikke i virkeligheden)

        [Test]

        public void OnStartCancelPressed_LightTurnsOn()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _outputFake.Received().OutputLine("Light is turned on");
        }

        [Test]
        public void OnStartCancelPressed_PowerIsOn()
        {
            _powerButton.Press();
            _powerButton.Press();
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(100);
            _outputFake.Received().OutputLine("PowerTube works with 21 %");
        } // Der er en fejl her idet man i metoden turn on i powerTube klassen ikke omregner de indstillede watt til procent af samlede antal watt den kan indstilles på.

        [Test]
        public void OnStartCancelPressed_TimeTickIsShown()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(1001);
            _outputFake.Received().OutputLine("Display shows: 00:59");
            Thread.Sleep(1001);
            _outputFake.Received().OutputLine("Display shows: 00:58");
        }
        // fundet en fejl i Timer klassen. den tid man indtaster er i sekunder men timeren regner i millisekunder

        [Test]
        public void OnStartCancelPressed_PowerTubeTurnsOffWhenCookingIsDone()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(60100);
            _outputFake.Received().OutputLine("PowerTube turned off");
        }

        [Test]
        public void CookingIsDone_DisplayIsCleared()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(60300);
            _outputFake.Received().OutputLine("Display cleared");
        }

        [Test]
        public void CookingIsDone_LightTurnsOff()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(62000);
            _outputFake.Received().OutputLine("Light is turned off");
        } //Der er en eller anden fejl her..


        //Tjek med mere end default
    }
}
