using MosadAPIServer.ModelsHelpers;
namespace MosadAPIServer.Models
{
    public interface ILocationable
    {
        public int? LocationX { get; set; }
        public int? LocationY { get; set; }

        public Location GetLocation();
        public void SetLocation(Location newLocation);
    }
}
