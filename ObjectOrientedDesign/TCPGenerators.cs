using ObjectOrientedDesign.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ObjectOrientedDesign
{
    public interface ITCPGenerator
    {
        public IEntity Generate(byte[] bytes);
    }

    public class TCPAirportGenerator() : ITCPGenerator
    {
        public IEntity Generate(byte[] bytes)
        {
            int lastp = 0;
            ulong ID = BitConverter.ToUInt64(bytes[0..8]);
            ushort namelength = BitConverter.ToUInt16(bytes[8..10]);
            lastp = 10;
            string name = System.Text.Encoding.ASCII.GetString(bytes[lastp..(lastp + namelength)]);
            lastp = lastp + namelength;
            string code = System.Text.Encoding.ASCII.GetString(bytes[lastp..(lastp + 3)]);
            lastp = lastp + 3;
            float longitude = BitConverter.ToSingle(bytes[lastp..(lastp + 4)]);
            lastp = lastp + 4;
            float latitude = BitConverter.ToSingle(bytes[lastp..(lastp + 4)]);
            lastp = lastp + 4;
            float AMSL = BitConverter.ToSingle(bytes[lastp..(lastp + 4)]);
            lastp = lastp + 4;
            string country = System.Text.Encoding.ASCII.GetString(bytes[lastp..(lastp + 3)]);
            return new Airport(ID, name, code, longitude, latitude, AMSL, country);

        }
    }
    public class TCPCrewGenerator : ITCPGenerator
    {
        public IEntity Generate(byte[] bytes)
        {
            ulong ID = BitConverter.ToUInt64(bytes[0..8]);
            ushort namelength = BitConverter.ToUInt16(bytes[8..10]);
            int lastp = 10;
            string name = System.Text.Encoding.ASCII.GetString(bytes[lastp..(lastp + namelength)]);
            lastp = lastp + namelength;
            ushort Age = BitConverter.ToUInt16(bytes[lastp..(lastp + 2)]);
            lastp = lastp + 2;
            string Phone = System.Text.Encoding.ASCII.GetString(bytes[lastp..(lastp + 12)]);
            lastp = lastp + 12;
            ushort emaillength = BitConverter.ToUInt16(bytes[lastp..(lastp + 2)]);
            lastp = lastp + 2;
            string Email = System.Text.Encoding.ASCII.GetString(bytes[lastp..(lastp + emaillength)]);
            lastp = lastp + emaillength;
            ushort Practice = BitConverter.ToUInt16(bytes[lastp..(lastp + 2)]);
            lastp = lastp + 2;
            string Role = System.Text.Encoding.ASCII.GetString(bytes[lastp..(lastp + 1)]); //duh
            return new Crew(ID, name, Age, Phone, Email, Practice, Role);
        }

    }
    public class TCPCargoGenerator : ITCPGenerator
    {
        public IEntity Generate(byte[] bytes)
        {
            ulong ID = BitConverter.ToUInt64(bytes[0..8]);
            float weight = BitConverter.ToSingle(bytes[8..12]);
            string code = System.Text.Encoding.ASCII.GetString(bytes[12..18]);
            ushort description_length = BitConverter.ToUInt16(bytes[18..20]);
            string desc = System.Text.Encoding.ASCII.GetString(bytes[20..(20 + description_length)]);
            return new Cargo(ID, weight, code, desc);
        }

    }

    public class TCPPassengerGenerator : ITCPGenerator
    {
        public IEntity Generate(byte[] bytes)
        {
            ulong ID = BitConverter.ToUInt64(bytes[0..8]);
            ushort namelength = BitConverter.ToUInt16(bytes[8..10]);
            int lastp = 10;
            string name = System.Text.Encoding.ASCII.GetString(bytes[lastp..(lastp + namelength)]);
            lastp = lastp + namelength;
            ushort Age = BitConverter.ToUInt16(bytes[lastp..(lastp + 2)]);
            lastp = lastp + 2;
            string Phone = System.Text.Encoding.ASCII.GetString(bytes[lastp..(lastp + 12)]);
            lastp = lastp + 12;
            ushort emaillength = BitConverter.ToUInt16(bytes[lastp..(lastp + 2)]);
            lastp = lastp + 2;
            string Email = System.Text.Encoding.ASCII.GetString(bytes[lastp..(lastp + emaillength)]);
            lastp = lastp + emaillength;
            string Class = System.Text.Encoding.ASCII.GetString(bytes[lastp..(lastp + 1)]); //duh
            lastp++;
            ulong Miles = BitConverter.ToUInt64(bytes[lastp..(lastp + 8)]);
            return new Passenger(ID, name, Age, Phone, Email, Class, Miles);
        }

    }

    public class TCPCargoPlaneGenerator : ITCPGenerator
    {
        public IEntity Generate(byte[] bytes)
        {
            ulong ID = BitConverter.ToUInt64(bytes[0..8]);
            string serial = System.Text.Encoding.ASCII.GetString(bytes[8..18]);
            serial = serial.Replace("\0", String.Empty);
            string country = System.Text.Encoding.ASCII.GetString(bytes[18..21]);
            ushort model_length = BitConverter.ToUInt16(bytes[21..23]);
            string model = System.Text.Encoding.ASCII.GetString(bytes[23..(23 + model_length)]);
            int lastp = model_length + 23;
            float max_load = BitConverter.ToSingle(bytes[lastp..(lastp + 4)]);
            return new CargoPlane(ID, serial, country, model, max_load);
        }

    }
    public class TCPPassengerPlaneGenerator : ITCPGenerator
    {
        public IEntity Generate(byte[] bytes)
        {
            ulong ID = BitConverter.ToUInt64(bytes[0..8]);
            string serial = System.Text.Encoding.ASCII.GetString(bytes[8..18]);
            serial = serial.Replace("\0", String.Empty);
            string country = System.Text.Encoding.ASCII.GetString(bytes[18..21]);
            ushort model_length = BitConverter.ToUInt16(bytes[21..23]);
            string model = System.Text.Encoding.ASCII.GetString(bytes[23..(23 + model_length)]);
            int lastp = model_length + 23;
            ushort firstclass = BitConverter.ToUInt16(bytes[lastp..(lastp + 2)]);
            lastp = lastp + 2;
            ushort businessclass = BitConverter.ToUInt16(bytes[lastp..(lastp + 2)]);
            lastp = lastp + 2;
            ushort economyclass = BitConverter.ToUInt16(bytes[lastp..(lastp + 2)]);
            return new PassengerPlane(ID, serial, country, model, firstclass, businessclass, economyclass);
        }

    }

    public class TCPFlightGenerator : ITCPGenerator
    {
        public IEntity Generate(byte[] bytes)
        {
            ulong ID = BitConverter.ToUInt64(bytes[0..8]);
            ulong Origin = BitConverter.ToUInt64(bytes[8..16]);
            ulong Target = BitConverter.ToUInt64(bytes[16..24]);
            long Takeoff = BitConverter.ToInt64(bytes[24..32]);
            string to = Takeoff.ToString();
            long Landing = BitConverter.ToInt64(bytes[32..40]);
            string la = Landing.ToString();
            ulong PlaneID = BitConverter.ToUInt64(bytes[40..48]);
            ushort crewcount = BitConverter.ToUInt16(bytes[48..50]);
            ulong[] crew = new ulong[crewcount];
            int lastp = 50;
            for (int i = 0; i < crewcount; i++)
            {
                crew[i] = BitConverter.ToUInt64(bytes[lastp..(lastp + 8)]);
                lastp += 8;
            }
            ushort passengercount = BitConverter.ToUInt16(bytes[lastp..(lastp + 2)]);
            ulong[] passenger = new ulong[passengercount];
            for (int i = 0; i < passengercount; i++)
            {
                passenger[i] = BitConverter.ToUInt64(bytes[lastp..(lastp + 8)]);
                lastp += 8;
            }

            return new Flight(ID, Origin, Target, to, la, null, null, null, PlaneID, crew, passenger);
        }


    }
}
