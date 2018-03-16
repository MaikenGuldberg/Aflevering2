using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NUnit.Framework;
using NSubstitute;

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
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _uut, _display, _light,
                _cookController);
            _light = new Light(_output);
            _cookController = new CookController(_timer, _display, _powerTube);
            _display = new Display(_output);
            _timer = new Timer();
            _powerTube = new PowerTube(_output);
            _output = Substitute.For<IOutput>();
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
        }

        [Test]
        public void OpenDoor()
        {
            _uut.Open();
            Assert.That(_output);
        }
    }
}
