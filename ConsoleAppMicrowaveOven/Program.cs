using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using System.Threading;

namespace Microwave.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            // Setup all the objects, 
            var _output = new Output();
            var _light = new Light(_output);
            var _display = new Display(_output);
            var _timer = new MicrowaveOvenClasses.Boundary.Timer();
            var _powerTube = new PowerTube(_output);
            var _powerButton = new Button();
            var _timerButton = new Button();
            var _startCancelButton = new Button();
            var _door = new Door();
            var _cookControl = new CookController(_timer, _display, _powerTube);
            var _userInterface = new UserInterface(_powerButton,_timerButton,_startCancelButton,_door,_display,_light,_cookControl);
            _cookControl.UI = _userInterface;

            // Simulate user activities (main senario)
            _door.Open();
            _door.Close();

            _powerButton.Press();
            _powerButton.Press();
            _powerButton.Press();

            _timerButton.Press();

            _startCancelButton.Press();

            //// Simulate user activities (extension 1: The user presses the Start-Cancel button during setup)
            //_door.Open();
            //_door.Close();

            //_powerButton.Press();
            //_powerButton.Press();
            //_startCancelButton.Press();

            //// Simulate user activities (extension 2: The user opens the Door during setup)
            //_door.Open();
            //_door.Close();

            //_powerButton.Press();
            //_powerButton.Press();
            //_door.Open();

            //// Simulate user activities (extension 3: The user presses the Start-Cancel button during cooking)
            //_door.Open();
            //_door.Close();

            //_powerButton.Press();
            //_powerButton.Press();
            //_powerButton.Press();

            //_timerButton.Press();

            //_startCancelButton.Press();
            //_startCancelButton.Press();

            //// Simulate user activities (extension 4: The user opens the Door during cooking)
            //_door.Open();
            //_door.Close();

            //_powerButton.Press();
            //_powerButton.Press();
            //_powerButton.Press();

            //_timerButton.Press();

            //_startCancelButton.Press();
            //_door.Open();

            // Wait while the classes, including the timer, do their job
            System.Console.WriteLine("Tast enter når applikationen skal afsluttes");
            System.Console.ReadLine();
        }
    }
}
