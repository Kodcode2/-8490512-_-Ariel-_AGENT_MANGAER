using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace MosadAPIServer.Models
{
    public interface IModel
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
       
    }
}
