using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;

namespace Microwave.Test.Intergration
{
    [TestFixture]
    public class IT4
    {
        //Arrange
        private Button uut_TimeButton;
        private Button uut_PowerButton;
        private Button uut_startCancelButton;

        private IUserInterface _userInterface;
        private ILight _light;
        private ICookController _cookController;
        private IDisplay _display;
        private ITimer _timer;
        private IPowerTube _powerTube;
        private IOutput _output;
        private IDoor _door;

        private int power;
        private int min;
        private int sec;
    
        

        [SetUp]
        public void SetUp()
        {
            uut_TimeButton = new Button();
            uut_PowerButton = new Button();
            uut_startCancelButton = new Button();

            _output = Substitute.For<IOutput>();
            _timer = new Timer();
            _light = new Light(_output);
            _cookController = new CookController(_timer, _display, _powerTube);
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _door = new Door();
            _userInterface = new UserInterface(uut_PowerButton, uut_TimeButton, uut_startCancelButton, _door, _display, _light, _cookController);
        }
       

        
        public void PowerButtonPressed_DisplayShowingText()
        {

            //Act
            uut_PowerButton.Press();
            //Assert
            _output.Received().OutputLine($"Display shows: {power} W");
        }

        public void TimeButtonPressed_DisplayShowingTime()
        {
            //Act
            uut_TimeButton.Press();
            //Assert
            _output.Received().OutputLine($"Display shows: {min:D2}:{sec:D2}");
        }

        public void StartCancelbuttonPressed_ClearDisplayWithExplainingText()
        {
            //Act
            //extesnsion nr.3
            uut_startCancelButton.Press();
            //Assert
            _output.Received().OutputLine("Display cleared");
        }
            
        }

    }

