using Microsoft.EntityFrameworkCore.Metadata;
using MosadAPIServer.Exceptions;
using MosadAPIServer.ModelsHelpers;

namespace MosadAPIServer.Services
{
    static public class DirectionsService
    {
        private static readonly int AssignmentRange = 200;
        private static readonly Location MaxBound = new(1000,1000);
        private static readonly Location MinBound = new (0, 0);

       

        private static Location GetDirectionFromString(string dir) => dir switch
        {
            "s" => new Location() { X = 0, Y = 1 },
            "e" => new Location() { X = 1, Y = 0 },
            "n" => new Location() { X = 0, Y = -1 },
            "w" => new Location() { X = -1, Y = 0 },
            "ne" => new Location() { X = 1, Y = -1 },
            "nw" => new Location() { X = -1, Y = -1 },
            "se" => new Location() { X = 1, Y = 1 },
            "sw" => new Location() { X = -1, Y = 1 },
            _ => throw new InvalidDirectionException()
        };

        public static Location? Move(Location src , string dir)
        {
            
           
            var newLocation = src + GetDirectionFromString(dir);
            return newLocation >= MinBound && newLocation <= MaxBound? newLocation :  throw new OutOfRangeMoveException() ;
            
        }

        public static Location MoveTowards(Location src, Location target)
        {
           // add 1 or -1 to dir to come closer
           return new Location(
               src.X < target.X ? 1 
               : src.X > target.X ? -1 
               : 0, 
               src.Y < target.Y ? 1 
               : src.Y > target.Y ? -1 
               : 0);     
        }

        public static bool IsInRange(Location location1, Location location2)
        {
            return Location.IsInRange(location1,location2, AssignmentRange);

            
        }

        internal static double GetAirDistance(Location? location1, Location? location2)
        {
            return Math.Sqrt(Math.Pow((double)(location2.X - location1.X), 2) 
                         + Math.Pow((double)(location2.Y - location1.Y), 2));
        }

        internal static bool AreOutOfAssignRange(Location? location1, Location? location2)
        {
            return !IsInRange(location1, location2);
        }
    }
}
