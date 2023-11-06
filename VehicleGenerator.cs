using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSimulation2023
{
    internal class VehicleGenerator
    {
        Helpers helpers = new Helpers();

        public Vehicle GenerateVehicle()
        {
            string typeOfVehicle = GetRandomVehicleType();
            Console.WriteLine($"A new vehicle is coming to the parking-lot. It seems to be a {typeOfVehicle}");
            string registrationNumber = GenerateRegistrationNumber();
            Console.WriteLine($"Registration-number: {registrationNumber}");
            string colour = GetInput("Colour: ");

            switch (typeOfVehicle)
            {
                case "Car":
                    bool isElectric = GetCarType();
                    return new Car(registrationNumber, colour, isElectric);
                case "Motorcycle":
                    string brand = GetMotorcycleBrand();
                    return new Motorcycle(registrationNumber, colour, brand);
                case "Bus":
                    int passengerCapacity = GetBusPassengerCapacity();
                    return new Bus(registrationNumber, colour, passengerCapacity);
                default:
                    return null;
            }
        }
        private string GenerateRegistrationNumber()
        {
            string registrationNumber = string.Empty;

            for (int i = 0; i < 3; i++)
            {
                char randomLetter = (char)('A' + helpers.Randomizer(0, 26));
                registrationNumber += randomLetter;
            }

            for (int i = 0; i < 3; i++)
            {
                char randomNumber = (char)('0' + helpers.Randomizer(0, 10));
                registrationNumber += randomNumber;
            }
            return registrationNumber;
        }

        private bool GetCarType()
        {
            return GetYesOrNo("Is it an electric car?: ");
        }

        private int GetBusPassengerCapacity()
        {
            return GetIntegerInput("How many passengers can the bus take: ");
        }

        private string GetRandomVehicleType()
        {
            string[] vehicleTypes = { "Motorcycle", "Car", "Bus" };
            return vehicleTypes[helpers.Randomizer(0, vehicleTypes.Length)];
        }

        private string GetInput(string question)
        {
            string input = string.Empty;
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.Write(question);
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    TryAgain();
                }
            }
            return FormatString(input);
        }

        private bool GetYesOrNo(string question)
        {
            while (true)
            {
                string input = GetInput(question).ToLower();
                if (input == "yes")
                {
                    return true;
                }
                else if (input == "no")
                {
                    return false;
                }
                else
                {
                    TryAgain();
                }
            }
        }

        private int GetIntegerInput(string question)
        {
            int result;
            while (!int.TryParse(GetInput(question), out result))
            {
                TryAgain();
            }
            return result;
        }

        private string FormatString(string input)
        {
            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

        private string GetMotorcycleBrand()
        {
            return GetInput("Brand: ");
        }

        private void TryAgain()
        {
            Console.WriteLine("Try again.");
        }
    }
}


