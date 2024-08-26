using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MosadAPIServer.Data;
using MosadAPIServer.DTO;
using MosadAPIServer.Models;
using MosadAPIServer.Services;

namespace MosadAPIServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TargetsController : CruController<Target,TargetDTO>
    {


        public TargetsController(MosadAPIServerContext context, TargetService targetService)
            : base(context, targetService) { }
       

       
    }
}
