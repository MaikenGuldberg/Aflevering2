using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Intergration
{
    class IT3_Extensions
    {
        private UserInterface _uut;
        private ILight _light;
        private ICookController _cookController;
        private IDisplay _display;
        private ITimer _timer;
        private IPowerTube _powerTube;
        private IOutput _output;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDoor _door;

        [SetUp]
        public void SetUp()
        {
            _door = new Door();
            _output = Substitute.For<IOutput>();
            _timer = new Timer();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _light = new Light(_output); 

            _cookController = new CookController(_timer, _display, _powerTube) { UI = _uut };
            _uut = new UserInterface(_powerButton,_timeButton,_startCancelButton,_door,_display,_light,_cookController);
           

        }

        [Test]
        public void OpenDoorWhileCooking_PowerTubeTurnsOff()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(500);
            _door.Open();
            Thread.Sleep(1000);
            _output.Received().OutputLine("PowerTube turned off");
        }

        [Test]
        public void PressCancelWhileTheMachineIsRunning_PowerTubeTurnsOff()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(1000);
            _startCancelButton.Press();
            _output.Received().OutputLine("PowerTube turned off");
        }

        [Test]
        public void PressCancelWhileTheMachineIsRunning_LightsTurnsOff()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            Thread.Sleep(1000);
            _startCancelButton.Press();
            Thread.Sleep(500);
            _output.Received().OutputLine("Light is turned off");
        }


        
    }
}
