using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingSimulation2023
{
    internal class Parking
    {
        private static int startIndex = 1;
        private int SlotsAmount { get; set; }
        private Dictionary<string, List<Vehicle>> Slots { get; set; }

        public Parking(int slotsAmount)
        {
            SlotsAmount = slotsAmount;
            Slots = new Dictionary<string, List<Vehicle>>();

            for (int i = 1; i <= slotsAmount; i++)
            {
                string slotDescription = $"Slot {i}";
                Slots.Add(slotDescription, new List<Vehicle>());
            }
        }

        public void DrawParking()
        {
            foreach (KeyValuePair<string, List<Vehicle>> slot in Slots)
            {
                string slotDescription = slot.Key;
                List<Vehicle> vehicles = slot.Value;
                if (vehicles.Count > 0)
                {
                    foreach (Vehicle vehicle in vehicles)
                    {
                        Console.WriteLine($"{slotDescription}: {vehicle.Description()}");
                    }
                }
                else
                {
                    Console.WriteLine($"{slotDescription}: Empty");
                }
            }
        }

        public void Park(Vehicle vehicle)
        {
            if (!HasAvailableSpaceForVehicle(vehicle))
            {
                Console.WriteLine($"No space available for the {vehicle.GetType().Name} with registration number {vehicle.RegistrationNumber}");
                if (startIndex == 0)
                {
                    CheckingOut();
                    startIndex = 1;
                }
                else
                {
                    startIndex--;
                }
                Console.ReadKey();
                return;
            }

            DateTime parkStartTime = DateTime.Now;

            foreach (KeyValuePair<string, List<Vehicle>> slot in Slots)
            {
                string slotDescription = slot.Key;
                List<Vehicle> vehicles = slot.Value;

                if (vehicle.Size == 2.0 && CanParkInSlotAndNextSlot(slotDescription, vehicle))
                {
                    vehicles.Add(vehicle);
                    vehicle.ParkStartTime = parkStartTime;

                    if (int.TryParse(slotDescription.Replace("Slot ", ""), out int slotNumber))
                    {
                        var nextSlot = Slots[$"Slot {slotNumber + 1}"];
                        nextSlot.Add(vehicle);
                    }

                    break;
                }
                else if (vehicle.Size == 0.5 && CanParkInSlot(vehicle, vehicles))
                {
                    if (vehicles.Count == 0 || (vehicles.Count == 1 && vehicles[0].Size == 0.5))
                    {
                        vehicles.Add(vehicle);
                        vehicle.ParkStartTime = parkStartTime;
                        break;
                    }
                }
                else if (vehicle.Size == 1.0 && CanParkInSlot(vehicle, vehicles) && vehicles.Count == 0)
                {
                    vehicles.Add(vehicle);
                    vehicle.ParkStartTime = parkStartTime;
                    break;
                }
            }
        }

        public void CheckingOut()
        {
            bool success = false;

            while (!success)
            {
                Console.WriteLine("Registration-number: ");
                string registrationNumber = Console.ReadLine().ToUpper();

                double totalParkingCost = 0;

                foreach (KeyValuePair<string, List<Vehicle>> slot in Slots)
                {
                    string slotDescription = slot.Key;
                    List<Vehicle> vehicles = slot.Value;

                    for (int i = 0; i < vehicles.Count; i++)
                    {
                        Vehicle vehicle = vehicles[i];

                        if (vehicle.RegistrationNumber == registrationNumber)
                        {
                            DateTime checkoutTime = DateTime.Now;
                            TimeSpan parkingDuration = checkoutTime - vehicle.ParkStartTime;
                            double parkingCost = CalculateParkingCost(vehicle, parkingDuration);

                            totalParkingCost += parkingCost;

                            Console.WriteLine($"The {vehicle.GetType().Name} {registrationNumber} has checked out.");
                            Console.WriteLine($"Parking duration: {Math.Round(parkingDuration.TotalMinutes, 1)} minutes");
                            Console.WriteLine($"Parking cost: {parkingCost} :-");
                            Console.ReadKey();

                            if (vehicle.Size == 2.0)
                            {
                                foreach (KeyValuePair<string, List<Vehicle>> otherSlot in Slots)
                                {
                                    List<Vehicle> otherVehicles = otherSlot.Value;

                                    for (int j = otherVehicles.Count - 1; j >= 0; j--)
                                    {
                                        Vehicle otherVehicle = otherVehicles[j];
                                        if (otherVehicle.GetType() == vehicle.GetType() && otherVehicle.RegistrationNumber == registrationNumber)
                                        {
                                            otherVehicles.RemoveAt(j);
                                        }
                                    }
                                }
                            }

                            vehicles.Remove(vehicle);

                            success = true;
                            i--;
                            break;
                        }
                    }
                }

                if (totalParkingCost > 0)
                {
                    foreach (KeyValuePair<string, List<Vehicle>> slot in Slots)
                    {
                        List<Vehicle> vehicles = slot.Value;

                        foreach (Vehicle vehicle in vehicles)
                        {
                            if (vehicle.RegistrationNumber == registrationNumber)
                            {
                                vehicle.ParkingCost = totalParkingCost;
                            }
                        }
                    }
                }
            }
        }


        private bool HasAvailableSpaceForVehicle(Vehicle vehicle)
        {
            foreach (KeyValuePair<string, List<Vehicle>> slot in Slots)
            {
                string slotDescription = slot.Key;
                List<Vehicle> vehicles = slot.Value;
                if (vehicle.Size >= 2.0)
                {
                    if (CanParkInSlotAndNextSlot(slotDescription, vehicle))
                    {
                        return true;
                    }
                }
                else
                {
                    if (CanParkInSlot(vehicle, vehicles))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CanParkInSlot(Vehicle vehicle, List<Vehicle> vehicles)
        {
            switch (vehicle.Size)
            {
                case 2.0:
                    return vehicles.Count == 0;

                case 1.0:
                    return vehicles.Count == 0;

                case 0.5:
                    return vehicles.Count == 0 || (vehicles.Count == 1 && vehicles[0].Size == 0.5);

                default:
                    return false;
            }
        }

        private bool CanParkInSlotAndNextSlot(string currentSlot, Vehicle vehicle)
        {
            if (!Slots.ContainsKey(currentSlot) || !CanParkInSlot(vehicle, Slots[currentSlot]))
            {
                return false;
            }

            if (!int.TryParse(currentSlot.Replace("Slot ", ""), out int currentSlotNumber))
            {
                return false;
            }

            if (currentSlotNumber >= SlotsAmount - 1)
            {
                return false;
            }

            string nextSlot = $"Slot {currentSlotNumber + 1}";
            string nextNextSlot = $"Slot {currentSlotNumber + 2}";

            if (!Slots.ContainsKey(nextSlot) || !Slots.ContainsKey(nextNextSlot) ||
                !CanParkInSlot(vehicle, Slots[nextSlot]) || !CanParkInSlot(vehicle, Slots[nextNextSlot]))
            {
                return false;
            }

            return true;
        }

        private double CalculateParkingCost(Vehicle vehicle, TimeSpan parkingDuration)
        {
            double pricePerMinute = 1.5;
            if (vehicle.Size == 2.0)
            {
                pricePerMinute *= 2;
            }
            double totalMinutes = parkingDuration.TotalMinutes;
            double cost = Math.Round(totalMinutes * pricePerMinute);
            return cost;
        }
    }
}
