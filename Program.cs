namespace ParkingSimulation2023
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int amountOfParkingSlots = 5;
            VehicleGenerator vehicleGenerator = new VehicleGenerator();
            Parking parking = new Parking(amountOfParkingSlots);
            int vehicleCounter = 0;

            while (true)
            {
                Console.Clear();
                parking.DrawParking();

                if (vehicleCounter % 3 == 0 && vehicleCounter > 0)
                {
                    Console.WriteLine($"Do you want to check out a vehicle (C) or continue parking (P)?");
                    char response = char.ToUpper(Console.ReadKey().KeyChar);
                    if (response == 'C')
                    {
                        parking.CheckingOut();
                        Console.Clear();
                        parking.DrawParking();
                    }
                }

                Vehicle newVehicle = vehicleGenerator.GenerateVehicle();
                parking.Park(newVehicle);

                vehicleCounter++;
            }
        }
    }
}