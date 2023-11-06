using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSimulation2023
{
    public abstract class Vehicle
    {
        public string RegistrationNumber { get; set; }
        public string Colour { get; set; }
        public double Size { get; set; }
        public DateTime ParkStartTime { get; set; }
        public double ParkingCost { get; internal set; }
        public Vehicle(string registrationNumber, string colour, double size)
        {
            RegistrationNumber = registrationNumber;
            Colour = colour;
            Size = size;
            ParkStartTime = DateTime.Now;
        }
        public virtual string Description()
        {
            return null;
        }
    }
    public class Car : Vehicle
    {
        public bool IsElectric { get; set; }
        public Car(string registrationNumber, string colour, bool isElectric) : base(registrationNumber, colour, 1.0)
        {
            IsElectric = isElectric;
            ParkStartTime = DateTime.Now;
        }
        public override string Description()
        {
            return $"Car\t{RegistrationNumber}\t{Colour}\t{(IsElectric == true ? "Electric" : "Gas/Diesle")}";
        }
    }
    public class Motorcycle : Vehicle
    {
        public string Brand { get; set; }
        public Motorcycle(string registrationNumber, string colour, string brand) : base(registrationNumber, colour, 0.5)
        {
            Brand = brand;
            ParkStartTime = DateTime.Now;
        }
        public override string Description()
        {
            return $"Mc\t{RegistrationNumber}\t{Colour}\t{Brand}";
        }
    }
    public class Bus : Vehicle
    {
        public int PassengerCapacity { get; set; }
        public Bus(string registrationNumber, string colour, int passengerCapacity) : base(registrationNumber, colour, 2.0)
        {
            PassengerCapacity = passengerCapacity;
            ParkStartTime = DateTime.Now;
        }

        public override string Description()
        {
            return $"Bus\t{RegistrationNumber}\t{Colour}\tPassenger-capacity: {PassengerCapacity}";
        }
    }
}