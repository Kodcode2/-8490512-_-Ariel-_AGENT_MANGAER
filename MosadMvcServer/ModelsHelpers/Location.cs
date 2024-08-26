using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosadMvcServer.ModelsHelpers
{
    public class Location
    {
        public Location() { }
        public Location(int? x, int? y) { X = x; Y = y; }
        public int? X { get; set; }
        public int? Y { get; set; }

        public static Location operator +(Location left, Location right) => new Location() { X = left.X + right.X, Y = left.Y + right.Y };
        public static Location operator -(Location left, Location right) => new Location() { X = left.X - right.X, Y = left.Y - right.Y };
        public static bool operator >(Location left, Location right) =>  left.X > right.X && left.Y > right.Y;
        public static bool operator <(Location left, Location right) => left.X < right.X && left.Y < right.Y;
        public static bool operator >=(Location left, Location right) => left.X >= right.X && left.Y >= right.Y;
        public static bool operator <=(Location left, Location right) => left.X <= right.X && left.Y <= right.Y;
        public static bool operator <(Location left, decimal right) => left.X < right && left.Y < right;
        public static bool operator >(Location left, decimal right) => left.X > right && left.Y > right;

       public static bool operator ==(Location left , Location right) => left.X == right.X && left.Y == right.Y;
       public static bool operator !=(Location left , Location right) => !(left == right);

        public static bool IsInRange(Location location1, Location location2, int assignmentRange)
        {
            return Math.Abs((int)location1.X - (int)location2.X) < assignmentRange;
        }
    }
}
