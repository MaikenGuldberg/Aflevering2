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
    [TestFixture]
    public class IT3
    {
        private Door _uut;

        private IUserInterface _userInterface;
        private ILight _light;
        private ICookController _cookController;
        private IDisplay _display;
        private ITimer _timer;
        private IPowerTube _powerTube;
        private IOutput _output;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;



        [SetUp]
        public void SetUp()
        {
            _uut = new Door();
            _timer = new Timer();
            _output = Substitute.For<IOutput>();
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _light = new Light(_output);
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);

            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _uut, _display, _light,
                _cookController);




        }

        [Test]
        public void OpenDoor_LightIsTurned()
        {
            _uut.Open();
            _output.Received().OutputLine("Light is turned on");
        }

        [Test]
        public void CloseDoor_LightIsTurnedOff()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(1000);
            _startCancelButton.Press();
            Thread.Sleep(500);
            _uut.Close();
            _output.Received().OutputLine("Light is turned off");
        }

        [Test]
        public void OpenDoorWhileCooking_PowerTubeIsTurnedOff()
        {
            _userInterface.OnPowerPressed(this,EventArgs.Empty);
            _userInterface.OnTimePressed(this,EventArgs.Empty);
            _userInterface.OnStartCancelPressed(this,EventArgs.Empty);
            Thread.Sleep(500);
            _uut.Open();
            Thread.Sleep(1000);
            _output.Received().OutputLine("PowerTube turned off");
        }

        
    }
}
