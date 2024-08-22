using Microsoft.EntityFrameworkCore.Metadata;
using MosadAPIServer.ModelsHelpers;

namespace MosadAPIServer.Services
{
    static public class DirectionsService
    {
        private static readonly Location MaxBound = new Location(200,200);
        private static readonly Location MinBound = new Location(0, 0);

        static readonly public Dictionary<string, Location> _directions = new Dictionary<string, Location>()
        {
            {"s" , new Location() { X =  0, Y =  0 }},
            {"e" , new Location() { X =  1, Y =  0 }},
            {"n" , new Location() { X =  0, Y = -1 }},
            {"w" , new Location() { X = -1, Y =  0 }},
            {"ne", new Location() { X =  1, Y = -1 }},
            {"nw", new Location() { X = -1, Y = -1 }},
            {"se", new Location() { X =  1, Y =  1 }},
            {"sw", new Location() { X = -1, Y =  1 }}

        };

        private static Location GetDirectionFromString(string dir) => dir switch
        {
            "s" => new Location() { X = 0, Y = 0 },
            "e" => new Location() { X = 1, Y = 0 },
            "n" => new Location() { X = 0, Y = -1 },
            "w" => new Location() { X = -1, Y = 0 },
            "ne" => new Location() { X = 1, Y = -1 },
            "nw" => new Location() { X = -1, Y = -1 },
            "se" => new Location() { X = 1, Y = 1 },
            "sw" => new Location() { X = -1, Y = 1 },
            _ => throw new Exception("")
        };

        public static Location? Move(Location src , string dir)
        {
            var newLocation = src + GetDirectionFromString(dir);
           return newLocation > MinBound && newLocation < MaxBound? newLocation : null ;
        }

        public static Location MoveTowards(Location src, Location target)
        {
            throw new NotImplementedException();
        }

    }
}
