using Microsoft.AspNetCore.Mvc;
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
        private long? _testID;

        private readonly IGraphService service;

        private readonly DBContext _context;

        public GraphController(DBContext context, IGraphService service)
        {
            _context = context;
            this.service = service;
            _testID = null;
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
                if (i == domains.Count() - 1)
                    break;
            }
            for (int i = source.Nodes.Count() - 1; i >= domains.Count(); i--)
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
        public ActionResult<GraphDTO> Test(int id)
        {
            long Testid = (long)id;//(long) _testID;
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
                else if (i <= domains.Count() + questions.Count() - 1)
                {
                    source.Nodes[i].Name = questions[i - domains.Count()].QuestionText;
                    source.Nodes[i].Id = (int)questions[i - domains.Count()].ID;
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
                    source.Edges[i].From = (int)edgeDDs[i].DomainFromID + 1000000;// source.Nodes[i].Id;
                    source.Edges[i].To = (int)edgeDDs[i].DomainToID + 1000000;
                    source.Edges[i].Name = "";
                }
                else if (i < edgeQDs.Count + edgeDDs.Count)
                {
                    source.Edges[i].From = (int)edgeQDs[i - edgeDDs.Count].QuestionFromID;// source.Nodes[i].Id;
                    source.Edges[i].To = (int)edgeQDs[i - edgeDDs.Count].DomainToID + 1000000;
                    source.Edges[i].Name = "";

                }
                else
                {
                    source.Edges.RemoveAt(i);
                }
            }

            var target = Converters.Convert(source);
            // VIew
            return Ok(target);
            //return RedirectToPage("", target);// Ok(target); //View
        }

        public ActionResult ChangeDomain(int? id)
        {
            _testID = id;
            List<Domain> domains = _context.Domains.ToList();
            List<Question> questions = _context.Questions.Where(x => x.Test.ID == id).ToList();

            var source = service.GetGraphSchema("Nodes");

            for (int i = 0; i < source.Nodes.Count(); i++)
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
                    source.Edges[i].Name = "fff";

                }
            }

            GraphDTO target = Converters.Convert(source);
            //GraphDTO target2 = new GraphDTO();
            target.testID = id;

            return View(target); //View
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

        private static List<List<int>> InitGraphEdges()
        {
            List<List<int>> edgesFromDS = new List<List<int>>();
            // [[0, 2], [1, 3], [2, 4], [3, 5], [4, 1], [4, 5]]
            edgesFromDS.Add(new List<int>() { 0, 1 });
            edgesFromDS.Add(new List<int>() { 1, 2 });

            // 06/02/2022
            //edgesFromDS.Add(new List<int>() { 1, 3 });
            //edgesFromDS.Add(new List<int>() { 3, 5 });
            //edgesFromDS.Add(new List<int>() { 4, 1 });
            //edgesFromDS.Add(new List<int>() { 4, 5 });
            return edgesFromDS;
        }


        [HttpGet("save/{json}")]
        public async Task<IActionResult> SaveAsync(string json)
        {
            List<GraphExportDTO> graph = JsonConvert.DeserializeObject<List<GraphExportDTO>>(json);

            for (int i = 0; i < graph.Count; i++)
            {

                if (Int32.Parse(graph[i].id) > 1000000)
                {
                    for (int j = 0; j < graph[i].connections.Count(); j++)
                    {
                        //Domain domainTo = await _context.Domains.FindAsync(graph[i].connections[j]);
                        EdgeDD edgeDD = _context.EdgeDDs.Where(x => x.DomainFromID == LowerMilion(graph[i].id) && x.DomainToID == graph[i].connections[j]).FirstOrDefault();

                        if (edgeDD == null)
                        {
                            _context.EdgeDDs.Add(new EdgeDD(LowerMilion(graph[i].id), graph[i].connections[j] - 1000000));
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
                            _context.EdgeQDs.Add(new EdgeQD(LowerMilion(graph[i].id), graph[i].connections[j] - 1000000));
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

        [HttpGet("iita")]
        public ActionResult<GraphDTO> Iita(int? id)
        {
            // VIew
            List<Domain> domains = _context.Domains.ToList();
            List<Question> questions = _context.Questions.Where(x => x.Test.ID == id).ToList();

            var source = service.GetGraphSchema("Nodes");
            source.Nodes.Add(new Node());

            for (int i = 0; i < source.Nodes.Count(); i++)
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

            List<List<int>> edgesFromDS = InitGraphEdges();

            for (int i = source.Edges.Count() - 1; i >= 0; i--)
            {
                if (i < edgesFromDS.Count())
                {
                    source.Edges[i].From = source.Nodes[edgesFromDS[i][0] + domains.Count()].Id;
                    source.Edges[i].To = source.Nodes[edgesFromDS[i][1] + domains.Count()].Id;
                    source.Edges[i].Name = "";
                }
                else
                {
                    source.Edges.RemoveAt(i);
                }
            }

            GraphDTO target = Converters.Convert(source);
            //GraphDTO target2 = new GraphDTO();

            for (int j = 0; j < target.Nodes.Count(); j++)
            {
                if (j < 3)
                {
                    target.Nodes[j].Color = "red";
                }
                else
                {
                    target.Nodes[j].Color = "blue";

                }
            }

            return Ok(target); //View
            //return RedirectToPage("", target);// Ok(target); //View
        }


        [HttpGet("IitaChangeDomain/{json}")]
        public ActionResult IitaChangeDomain(string json)
        {
            Console.WriteLine(json);
            IitaResonseDTO graph = JsonConvert.DeserializeObject<IitaResonseDTO>(json);
            //{'diff': array([0.        , 0.088     , 0.49614035]), 'implications': [(0, 2), (1, 3), (2, 4), (3, 5), (4, 1), (4, 5)], 'error.rate': -0.0, 'selection.set.index': 0, 'v': 1}
            // VIew
            List<Domain> domains = _context.Domains.ToList();
            List<Question> questions = _context.Questions.Where(x => x.Test.ID == graph.testID).ToList();


            var source = service.GetGraphSchema("Nodes");
            source.Nodes.Add(new Node());

            for (int i = 0; i < source.Nodes.Count(); i++)
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

            for (int i = graph.implications.Count() - 1; i >= 0; i--)
            {

                source.Edges[i].From = source.Nodes[graph.implications[i][0] + domains.Count()].Id;
                source.Edges[i].To = source.Nodes[graph.implications[i][1] + domains.Count()].Id;
                source.Edges[i].Name = "";


            }

            GraphDTO target = Converters.Convert(source);

            for (int j = 0; j < target.Nodes.Count(); j++)
            {
                if (j < 3)
                {
                    target.Nodes[j].Color = "red";
                }
                else
                {
                    target.Nodes[j].Color = "blue";

                }
            }

            //GraphDTO target2 = new GraphDTO();
            target.testID = graph.testID;

            return View(target); //View
        }

        [HttpGet("clear")]
        public ActionResult Clear() 
        {
            _context.EdgeDDs.RemoveRange(_context.EdgeDDs.ToList());
            _context.SaveChangesAsync();
            return Ok();
        }
    }
}
