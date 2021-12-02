using Microsoft.AspNetCore.Mvc;
using Sotis2.Models.Graph.DTO;
using Sotis2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Controllers
{
    [Controller]
    [Route("api/graph")]
    public class GraphController : Controller
    {
        private readonly IGraphService service;

        public GraphController(IGraphService service)
        {
            this.service = service;
        }

        [HttpGet("schema")]
        public ActionResult<GraphDTO> GetNodesSchema()
        {
            var source = service.GetGraphSchema("Nodes");

            var target = Converters.Convert(source);
            // VIew
            return View(target);
            //return RedirectToPage("", target);// Ok(target); //View
        }

        

        [HttpGet("display")]
        public ActionResult<GraphDTO> Display()
        {
            var source = service.GetGraphSchema("Nodes");

            for (int i = 0; i < source.Nodes.Count(); i++)
            {
                source.Nodes[i].Name = "Q" + i.ToString();
            }

            for (int i = 0; i < source.Edges.Count(); i++)
            {
                if (i < source.Edges.Count() - 5)
                {
                    source.Edges[i].From = source.Nodes[i].Id;
                    source.Edges[i].To = source.Nodes[i + 1].Id;
                    source.Edges[i].Name = "";
                }

                source.Edges[i].Name = "";

            }

            var target = Converters.Convert(source);
            // VIew
            return Ok(target);
            //return RedirectToPage("", target);// Ok(target); //View
        }
    }
}
