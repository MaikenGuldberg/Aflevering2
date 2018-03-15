using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using MicrowaveOvenClasses;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;

namespace Microwave.Test.Intergration
{
    [TestFixture]
    public class IT1
    {
        private CookController _uut;
        private ITimer _timer;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private IOutput _output;
        private IUserInterface _userInterface;

        [SetUp]
        public void SetUp()
        {
            _userInterface = Substitute.For<IUserInterface>();
            _output = Substitute.For<IOutput>();
            _timer = new Timer();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _uut = new CookController(_timer, _display, _powerTube) {UI = _userInterface};
        }

        [Test]

        public void StartCooking_Power80Time3000_IsOK()
        {
            _uut.StartCooking(80,3000);

            _output.Received().OutputLine(Arg.Is<string>(str => str.ToLower().Contains("works") && str.ToLower().Contains("80")));

        }

        [TestCase()]

        public void StartCooking_ToHighOrToLowPower_ThrowsException()
        {
            
        }




    }
}
