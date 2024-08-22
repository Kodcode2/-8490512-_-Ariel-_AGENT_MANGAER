using Microsoft.CodeAnalysis;
namespace MosadAPIServer.ModelsHelpers
{
    public class Directions
    {
        static readonly public Dictionary<string , Location> _directions = new Dictionary<string, Location>()
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

        public Location GetDirection(string dir)=>  dir switch {
            "s"  => new Location() { X =  0, Y =  0 },
            "e"  => new Location() { X =  1, Y =  0 },
            "n"  => new Location() { X =  0, Y = -1 },
            "w"  => new Location() { X = -1, Y =  0 },
            "ne" => new Location() { X =  1, Y = -1 },
            "nw" => new Location() { X = -1, Y = -1 },
            "se" => new Location() { X =  1, Y =  1 },
            "sw" => new Location() { X = -1, Y =  1 },
            _ => throw new Exception("")
        };
    }
}
