using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectOrientedDesign.Objects;

namespace ObjectOrientedDesign
{
    public class ListsDatabase
    {
        public List<Airport> airports = new List<Airport>();
        public List<Crew> crews = new List<Crew>();
        public List<PassengerPlane> passengerPlanes  = new List<PassengerPlane>();
        public List<CargoPlane> cargoPlanes = new List<CargoPlane>();
        public List<Cargo> cargos = new List<Cargo>();
        public List<Passenger> passengers = new List<Passenger>();
        public List<Flight> flights = new List<Flight>();
        public List<Entity> entities = new List<Entity>();
        private static ListsDatabase instance;
        private ListsDatabase() 
        {
            airports = new List<Airport>();
            crews = new List<Crew>();
            passengerPlanes = new List<PassengerPlane>();
            cargoPlanes = new List<CargoPlane>();
            cargos = new List<Cargo>();
            passengers = new List<Passenger>();
            flights = new List<Flight>();
            entities = new List<Entity>();
            instance = this;
        }

        public List<Airport> GetAirports()
        {
            return airports;
        }
        public static ListsDatabase GetInstance()
        {
            if(instance == null)
            {
                instance = new ListsDatabase();
            }
            return instance;
        }

    }
}
