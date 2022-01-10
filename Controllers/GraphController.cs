﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sotis2.Data;
using Sotis2.Models;
using Sotis2.Models.Graph;
using Sotis2.Models.Graph.DTO;
using Sotis2.Models.Relations;
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

        private readonly DBContext _context;

        public GraphController(DBContext context, IGraphService service)
        {
            _context = context;
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

        [HttpGet("schematest")]
        public ActionResult<GraphDTO> GetNodesSchemaTest()
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
            //Test test = _context.Tests.Find("1");

            List<Domain> domains = _context.Domains.ToList();

            var source = service.GetGraphSchema("Nodes");

            for (int i = 0; i < source.Nodes.Count(); i++)
            {
                //source.Nodes[i].Name = "Q" + i.ToString();
                source.Nodes[i].Name = domains[i].Type;
                if (i == domains.Count()-1)
                    break;
            }
            for (int i = source.Nodes.Count() - 1; i >= domains.Count(); i--)
            {
                source.Nodes.RemoveAt(i);
            }


            for (int i = source.Edges.Count() - 1; i >= 0; i--)
            {
                if(i != 0)
                {
                    source.Edges.RemoveAt(i);
                }
                else
                {
                    source.Edges[i].From = source.Nodes[i].Id;
                    source.Edges[i].To = source.Nodes[i + 1].Id;
                    source.Edges[i].Name = "";

                }
            }
            
                //source.Edges = new List<Edge>();
                /*
                for (int i = 0; i < source.Edges.Count(); i++)
                {
                    if (i < source.Edges.Count() - 5)
                    {
                        source.Edges[i].From = source.Nodes[i].Id;
                        source.Edges[i].To = source.Nodes[i + 1].Id;
                        source.Edges[i].Name = "";
                    }
                    source.Edges[i].Name = "";

                }*/

                //source.Nodes = new List<Node>();
                //source.Edges = new List<Edge>();
                /*
                for (int i = 0; i < domains.Count(); i++)
                {
                    source.Nodes.Add(new Node());
                    source.Nodes[i].Name = domains[i].Type;
                }

                for (int i = 0; i < 1; i++)
                {
                    source.Edges.Add(new Edge());
                    source.Edges[i].From = source.Nodes[i].Id;
                    source.Edges[i].To = source.Nodes[i + 1].Id;
                    source.Edges[i].Name = "";
                }*/

                var target = Converters.Convert(source);
            // VIew
            return Ok(target);
            //return RedirectToPage("", target);// Ok(target); //View
        }

        [HttpGet("test")]
        public ActionResult<GraphDTO> Test()
        {
            long Testid = 4;
            //Test test = _context.Tests.Find("1");

            List<Domain> domains = _context.Domains.ToList();
            List<Question> questions = _context.Questions.Where(x => x.Test.ID == Testid).ToList();

            var source = service.GetGraphSchema("Nodes");

            for (int i = 0; i < source.Nodes.Count(); i++)
            {
                //source.Nodes[i].Name = "Q" + i.ToString();
                if (i <= domains.Count() - 1)
                {

                    source.Nodes[i].Name = domains[i].Type;
                    source.Nodes[i].Id = (int)(domains[i].ID + 1000000);
                }
                else if( i <= domains.Count() + questions.Count() - 1)
                {
                    source.Nodes[i].Name = questions[i - domains.Count()].QuestionText;
                    source.Nodes[i].Id = (int) questions[i - domains.Count()].ID;
                }
            }

            

            for (int i = source.Nodes.Count() - 1; i >= domains.Count() + questions.Count(); i--)
            {
                source.Nodes.RemoveAt(i);
            }


            List<EdgeDD> edgeDDs = _context.EdgeDDs.ToList();
            List<EdgeQD> edgeQDs = _context.EdgeQDs.ToList();

            for (int i = source.Edges.Count() - 1; i >= 0; i--)
            {
                if (i < edgeDDs.Count)
                {
                    source.Edges[i].From = (int)edgeDDs[i].DomainFromID;// source.Nodes[i].Id;
                    source.Edges[i].To = (int)edgeDDs[i].DomainToID;
                    source.Edges[i].Name = "";
                }
                else if(i < edgeQDs.Count + edgeDDs.Count)
                {
                    source.Edges[i].From = (int)edgeQDs[i - edgeDDs.Count].QuestionFromID;// source.Nodes[i].Id;
                    source.Edges[i].To = (int)edgeQDs[i - edgeDDs.Count].DomainToID;
                    source.Edges[i].Name = "";

                }
                else
                {
                    source.Edges.RemoveAt(i);
                }
            }

            //source.Edges = new List<Edge>();
            /*
            for (int i = 0; i < source.Edges.Count(); i++)
            {
                if (i < source.Edges.Count() - 5)
                {
                    source.Edges[i].From = source.Nodes[i].Id;
                    source.Edges[i].To = source.Nodes[i + 1].Id;
                    source.Edges[i].Name = "";
                }
                source.Edges[i].Name = "";

            }*/

            //source.Nodes = new List<Node>();
            //source.Edges = new List<Edge>();
            /*
            for (int i = 0; i < domains.Count(); i++)
            {
                source.Nodes.Add(new Node());
                source.Nodes[i].Name = domains[i].Type;
            }

            for (int i = 0; i < 1; i++)
            {
                source.Edges.Add(new Edge());
                source.Edges[i].From = source.Nodes[i].Id;
                source.Edges[i].To = source.Nodes[i + 1].Id;
                source.Edges[i].Name = "";
            }*/

            var target = Converters.Convert(source);
            // VIew
            return Ok(target);
            //return RedirectToPage("", target);// Ok(target); //View
        }

        public ActionResult ChangeDomain(int? id)
        {
            List<Domain> domains = _context.Domains.ToList();
            List<Question> questions = _context.Questions.Where(x => x.Test.ID == id).ToList();

            var source = service.GetGraphSchema("Nodes");

            for(int i = 0; i < source.Nodes.Count(); i++)
            {
                //source.Nodes[i].Name = "Q" + i.ToString();
                if (i <= domains.Count() - 1)
                {

                    source.Nodes[i].Name = domains[i].Type;
                }
                else if (i <= domains.Count() + questions.Count() - 1)
                {
                    source.Nodes[i].Name = questions[i - domains.Count()].QuestionText;
                }
            }



            for (int i = source.Nodes.Count() - 1; i >= domains.Count() + questions.Count(); i--)
            {
                source.Nodes.RemoveAt(i);
            }

            for (int i = source.Edges.Count() - 1; i >= 0; i--)
            {
                if (i != 0)
                {
                    source.Edges.RemoveAt(i);
                }
                else
                {
                    source.Edges[i].From = source.Nodes[i].Id;
                    source.Edges[i].To = source.Nodes[i + 1].Id;
                    source.Edges[i].Name = "";

                }
            }

            var target = Converters.Convert(source);

            return View(target);
        }

        [HttpGet("saveaload")]
        public IActionResult SaveAndLoad()
        {
            return View();
        }
        /*
        [HttpPost("save/{json}")]
        public IActionResult Save(string json)
        {
            
            return Ok();
        }*/

        [HttpGet("save/{json}")]
        public async Task<IActionResult> SaveAsync(string json)
        {
            List<GraphExportDTO> graph = JsonConvert.DeserializeObject<List<GraphExportDTO>>(json);

            for (int i = 0; i < graph.Count; i++)
            {

                if (LowerMilion(graph[i].id) > 1000000)
                {
                    for (int j = 0; j < graph[i].connections.Count(); j++)
                    {
                        //Domain domainTo = await _context.Domains.FindAsync(graph[i].connections[j]);
                        EdgeDD edgeDD = _context.EdgeDDs.Where(x => x.DomainFromID == LowerMilion(graph[i].id) && x.DomainToID == graph[i].connections[j]).FirstOrDefault();

                        if (edgeDD == null)
                        {
                            _context.EdgeDDs.Add(new EdgeDD(LowerMilion(graph[i].id), (long)graph[i].connections[j]));
                        }
                    }
                }
                else
                {

                    for (int j = 0; j < graph[i].connections.Count(); j++)
                    {
                        //Domain domainTo = await _context.Domains.FindAsync(graph[i].connections[j]);
                        EdgeQD edgeQD = _context.EdgeQDs.Where(x => x.QuestionFromID == LowerMilion(graph[i].id) && x.DomainToID == (long)graph[i].connections[j]).FirstOrDefault();

                        if (edgeQD == null)
                        {
                            _context.EdgeQDs.Add(new EdgeQD(LowerMilion(graph[i].id), (long)graph[i].connections[j]));
                        }
                    }
                }


                
            }

            await _context.SaveChangesAsync();
            

            return Ok();
        }

        private static long LowerMilion(string id)
        {
            return Convert.ToInt64(id) - 1000000;
        }
    }
}
