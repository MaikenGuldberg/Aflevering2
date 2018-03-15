using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using MicrowaveOvenClasses;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

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

        [TestCase(0,3000)]
        [TestCase(101,3000)]

        public void StartCooking_ToHighOrToLowPower_ThrowsException(int power, int time)
        {
            Assert.That(() => _uut.StartCooking(power, time), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]

        public void StartCooking_MicrowaveOvenIsAllardyOn_ThrowsException()
        {
            _powerTube.TurnOn(80);

            Assert.That(() => _uut.StartCooking(80, 3000),Throws.TypeOf<ApplicationException>());
        }//der burde være en exception handling

        [TestCase(3001,3)]
        [TestCase(3000,2)]

        public void StartCooking_DifferentTimes_LoggerIsCalledEachTick(int time,int tick)
        {
            _uut.StartCooking(80,time);
            Thread.Sleep(time+3);
            _output.Received(tick+1);
        }

        [Test]

        public void OnTimerTick_TickedOnce_OutputIsEqualToTime()
        {
            _uut.StartCooking(50,2500);
            Thread.Sleep(2503);
            _output.Received().OutputLine(Arg.Is<string>(str => str.ToLower().Contains("00:01")));
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("00:00")));
            Thread.Sleep(500);
            _output.Received().OutputLine("PowerTube turned off");
        }
        [Test]
        public void OnTimerExpired_Check_PowerTubeIsOff()
        {
           _uut.StartCooking(50,10000);
            Thread.Sleep(10001);
            _output.Received().OutputLine("PowerTube turned off");
        }

        [Test]
        public void OnTimerTick_PrintTimeToUI()
        {
            _uut.StartCooking(50,5000);
            Thread.Sleep(5005);
            _userInterface.Received().CookingIsDone();
        }



    }
}
