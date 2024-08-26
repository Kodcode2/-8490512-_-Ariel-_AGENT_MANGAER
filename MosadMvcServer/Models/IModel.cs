
using System.ComponentModel.DataAnnotations;

namespace MosadMvcServer.Models
{
    public interface IModel
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
       
    }
}
