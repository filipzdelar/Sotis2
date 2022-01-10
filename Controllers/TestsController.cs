using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sotis2.Data;
using Sotis2.Models;
using Sotis2.Models.DTO;
using VDS.RDF;
using VDS.RDF.Ontology;
using VDS.RDF.Storage;
using VDS.RDF.Writing;

namespace Sotis2.Controllers
{
    public class TestsController : Controller
    {
        private readonly DBContext _context;

        public TestsController(DBContext context)
        {
            _context = context;
        }

        // GET: Tests
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tests.ToListAsync());
        }

        // GET: Tests/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }



            var test = await _context.Tests
                .FirstOrDefaultAsync(m => m.ID == id);



            List<Question> questions = _context.Questions.Where(x => x.Test.ID == test.ID).ToList();
            TestQuestionAnswerDTO dTO = new TestQuestionAnswerDTO();
            dTO.Id = test.ID;
            dTO.TestDuration = test.TestDuration;
            dTO.qWA = new List<QuestionWithAnswaresDTO>();
            for (int i = 0; i < questions.Count(); i++)
            {
                List<Answare> answares = _context.Answares.Where(x => x.QuestionID == questions[i].ID).ToList();
                dTO.qWA.Add(new QuestionWithAnswaresDTO(answares, questions[i].QuestionText));
            }

            if (test == null)
            {
                return NotFound();
            }

            return View(dTO);
        }

        // GET: Tests/Create
        public IActionResult Create()
        {
            TestDTO testDTO = new TestDTO();
            testDTO.Questions = new List<Question>();

            Question question = new Question();
            question.QuestionText = "EnterText here... ";
            testDTO.Questions.Add(question);
            return View(testDTO);
        }

        // POST: Tests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TestDuration")] Test test)
        {
            if (ModelState.IsValid)
            {
                _context.Add(test);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(test);
        }

        // GET: Tests/Create
        /*
        public IActionResult CreateFullTest()
        {
            TestQuestionAnswerDTO testDTO = new TestQuestionAnswerDTO();
            testDTO.QuestionsDTO = new List<QuestionDTO>();

            QuestionDTO question = CreateTmpQuestion();
            testDTO.QuestionsDTO.Add(question);

            question = CreateTmpQuestion();
            testDTO.QuestionsDTO.Add(question);

            question = CreateTmpQuestion();
            testDTO.QuestionsDTO.Add(question);

            return View(testDTO);
        }
        */
        private static QuestionWithAnswaresDTO CreateTmpQuestion()
        {
            QuestionWithAnswaresDTO question = new QuestionWithAnswaresDTO();
            question.QuestionText = "Enter Question here... ";
            question.Answares = new List<Answare>();

            Answare answare = new Answare();
            answare.AnswareText = "Answare text 1";
            question.Answares.Add(answare);

            answare = new Answare();
            answare.AnswareText = "Answare text 2";
            question.Answares.Add(answare);

            answare = new Answare();
            answare.AnswareText = "Answare text 3";
            question.Answares.Add(answare);

            answare = new Answare();
            answare.AnswareText = "Answare text 4";
            question.Answares.Add(answare);

            answare = new Answare();
            answare.AnswareText = "Answare text 5";
            question.Answares.Add(answare);
            return question;
        }

        // POST: Tests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFullTest([Bind("ID,TestDuration")] Test test)
        {
            if (ModelState.IsValid)
            {
                _context.Add(test);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(test);
        }*/


        // GET: Tests/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Tests.FindAsync(id);
            if (test == null)
            {
                return NotFound();
            }
            return View(test);
        }

        // POST: Tests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,TestDuration")] Test test)
        {
            if (id != test.ID)
            {
                return NotFound();
            }



            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(test);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestExists(test.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(test);
        }

        public IActionResult BlankAnsware()
        {
            return PartialView("_AnswareEditor", new Answare());
        }

        
        public ActionResult AddAnsware(int? k, int? i)
        {
            ViewBag.k = i;
            ViewBag.i = i;
            return PartialView();
        }

        // GET: Tests/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Tests
                .FirstOrDefaultAsync(m => m.ID == id);
            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        // POST: Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var test = await _context.Tests.FindAsync(id);
            _context.Tests.Remove(test);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestExists(long id)
        {
            return _context.Tests.Any(e => e.ID == id);
        }


        public IActionResult AddQuestion([Bind("ID,TestDuration")] TestDTO testDTO)
        {

            return View(testDTO);
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>


        // GET: Questions/Create
        public async Task<IActionResult> CreateFullTestAsync()
        {

            TestQuestionAnswerDTO testDTO = new TestQuestionAnswerDTO();
            testDTO.qWA = new List<QuestionWithAnswaresDTO>();

            QuestionWithAnswaresDTO question = CreateTmpQuestion();
            question.ID = 1;
            testDTO.qWA.Add(question);

            question = CreateTmpQuestion();
            question.ID = 2;
            testDTO.qWA.Add(question);

            question = CreateTmpQuestion();
            question.ID = 3;
            testDTO.qWA.Add(question);

            testDTO.Courses = new List<SelectListItem>();
            List<Course> courses = await _context.Courses.ToListAsync();
            foreach (Course course in courses)
            {
                testDTO.Courses.Add(new SelectListItem(course.Name, course.ID.ToString()));
            }

            return View(testDTO);
        }

        /// <param name="question"></param>
        /// <returns></returns>
        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFullTest(TestQuestionAnswerDTO question)
        {
            //[Bind("ID,QuestionText,AnswareText")]  , [Bind("AnswareText")] string AnswareText
            if (ModelState.IsValid)
            {
                Course course = await _context.Courses.FindAsync(Convert.ToInt64(question.CourseID));
                Test t = new Test(question.TestDuration, course, question.Name);
                
                _context.Tests.Add(t);

                await _context.SaveChangesAsync();
                for (int i = 0; i < question.qWA.Count(); i++)
                {
                    Question q = new Question(t, question.qWA[i].QuestionText);
                    _context.Questions.Add(q);
                    await _context.SaveChangesAsync();

                    for (int j = 0; j < question.qWA[i].Answares.Count(); j++)
                    {
                        _context.Answares.Add(new Answare(question.qWA[i].Answares[j].AnswareText, question.qWA[i].Answares[j].IsItTrue, q.ID));
                    }
                    await _context.SaveChangesAsync();
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(question);
        }

        public ActionResult QuestionEditor(int? i)
        {
            ViewBag.i = i;
            QuestionWithAnswaresDTO questionWithAnswaresDTO = new QuestionWithAnswaresDTO();
            questionWithAnswaresDTO.QuestionText = "Enter question here ";
            questionWithAnswaresDTO.ID = ((int)i) + 1;
            questionWithAnswaresDTO.Answares = new List<Answare>();
            questionWithAnswaresDTO.Answares.Append(new Answare("Odgovori", true));
            return PartialView("QuestionEditor", questionWithAnswaresDTO);
        }

        public  ActionResult ExportRDF()
        {
            string path = "C:\\Users\\Panonit\\Desktop\\owlS\\ConvertTests\\exp_2.owl";

            

            string fullPath = "http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/"; // &untitled-ontology-7;

            List<Test> tests = _context.Tests.ToList();
            foreach (Test test in tests)
            {
                using (StreamWriter sw = System.IO.File.AppendText(path))
                {
                    sw.WriteLine("<owl:NamedIndividual rdf:about = \""+fullPath + (test.Name ==null ? test.Name : test.Name.Replace(' ', '_')) + "\">");
                    sw.WriteLine("    <rdf:type rdf:resource = \"" + fullPath + "Test\"/>");
                    sw.WriteLine("    <startTimeOfTest rdf:datatype = \"&xsd;dateTime\">" + test.StartOfTest + "</startTimeOfTest>");
                    sw.WriteLine("</owl:NamedIndividual >");
                }
            }
            /*
            List<Subject> subjects = _context.Subjects.ToList();
            foreach (Subject subject in subjects)
            {
                using (StreamWriter sw = System.IO.File.AppendText(path))
                {
                    sw.WriteLine("<owl:NamedIndividual rdf:about = \"" + fullPath  + (subject.NameOfSubject == null ? subject.NameOfSubject : subject.NameOfSubject.Replace(' ', '_')) + "\">");
                    sw.WriteLine("    <rdf:type rdf:resource = \"" + fullPath + "Test\"/>");
                    sw.WriteLine("</owl:NamedIndividual >");
                }

            }*/

            /*
            List<Course> courses = _context.Courses.ToList();
            foreach (Course course in courses)
            {
                using (StreamWriter sw = System.IO.File.AppendText(path))
                {
                    sw.WriteLine("<owl:NamedIndividual rdf:about = \"" + fullPath + (course.Name == null ? course.Name : course.Name.Replace(' ', '_')) + "\">");
                    sw.WriteLine("    <rdf:type rdf:resource = \"" + fullPath + "Test\"/>");
                    sw.WriteLine("</owl:NamedIndividual >");
                }

            }*/

            List<Question> questions = _context.Questions.ToList();
            foreach (Question question in questions)
            {
                using (StreamWriter sw = System.IO.File.AppendText(path))
                {
                    sw.WriteLine("<owl:NamedIndividual rdf:about = \"" + fullPath + (question.QuestionText == null ? question.QuestionText : question.QuestionText.Replace(' ', '_')) + "\">");
                    sw.WriteLine("    <rdf:type rdf:resource = \"" + fullPath + "Question\"/>");
                    sw.WriteLine("</owl:NamedIndividual >");
                }

            }

            List<Domain> domains = _context.Domains.ToList();
            foreach (Domain domain in domains)
            {
                using (StreamWriter sw = System.IO.File.AppendText(path))
                {
                    sw.WriteLine("<owl:NamedIndividual rdf:about = \"" + fullPath +  (domain.Type == null ? domain.Type : domain.Type.Replace(' ', '_')) + "\">");
                    sw.WriteLine("    <rdf:type rdf:resource = \"" + fullPath + "Domain\"/>");
                    sw.WriteLine("</owl:NamedIndividual >");
                }

            }

            foreach (Attempt attempt in attempts)
            {
                using (StreamWriter sw = System.IO.File.AppendText(path))
                {
                    sw.WriteLine("<owl:NamedIndividual rdf:about = \"" + fullPath + (attempt.Name == null ? attempt.Name : attempt.Name.Replace(' ', '_')) + "\">");
                    sw.WriteLine("    <rdf:type rdf:resource = \"" + fullPath + "Attempt\"/>");
                    sw.WriteLine("    <precentOfCorrect rdf:datatype = \"&xsd;float\" >" + attempt.Accuracy.ToString() + "</precentOfCorrect>");
                    sw.WriteLine("</owl:NamedIndividual >");

                    sw.WriteLine("<rdf:Description>");
                    sw.WriteLine("<rdf:type rdf:resource = \"&owl;NegativePropertyAssertion\"/>");
                    sw.WriteLine("  <owl:targetIndividual rdf:resource = \"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/E2/144/2017\" />");
                    sw.WriteLine("  <owl:sourceIndividual rdf:resource = \"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/Pokusaj\" />");
                    sw.WriteLine("  <owl:assertionProperty rdf:resource = \"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/attemptByStudent\" />");
                    sw.WriteLine("</rdf:Description >");
                }

            }

            List<Answare> answares = _context.Answares.ToList();
            foreach (Answare answare in answares)
            {
                using (StreamWriter sw = System.IO.File.AppendText(path))
                {
                    sw.WriteLine("<owl:NamedIndividual rdf:about = \"" + fullPath + (answare.AnswareText == null ? answare.AnswareText : answare.AnswareText.Replace(' ', '_')) + "\">");
                    sw.WriteLine("    <rdf:type rdf:resource = \"" + fullPath + "Answare\"/>");
                    sw.WriteLine("</owl:NamedIndividual >");
                }

            }

            List<StudentsAnsware> studentsAnswares = _context.StudentsAnswares.ToList();
            foreach (StudentsAnsware studentsAnsware in studentsAnswares)
            {
                using (StreamWriter sw = System.IO.File.AppendText(path))
                {
                    sw.WriteLine("<owl:NamedIndividual rdf:about = \"" + fullPath + (studentsAnsware.AnswareText == null ? studentsAnsware.AnswareText : studentsAnsware.AnswareText.Replace(' ', '_')) + "\">");
                    sw.WriteLine("    <rdf:type rdf:resource = \"" + fullPath + "AnswareStudent\"/>");
                    sw.WriteLine("</owl:NamedIndividual >");
                }

            }

            List<Attempt> attempts = _context.Attempts.ToList();
            foreach (Attempt attempt in attempts)
            {
                using (StreamWriter sw = System.IO.File.AppendText(path))
                {
                    sw.WriteLine("<owl:NamedIndividual rdf:about = \"" + fullPath + (attempt.Name == null ? attempt.Name : attempt.Name.Replace(' ', '_')) + "\">");
                    sw.WriteLine("    <rdf:type rdf:resource = \"" + fullPath + "Attempt\"/>");
                    sw.WriteLine("    <precentOfCorrect rdf:datatype = \"&xsd;float\" >" + attempt.Accuracy.ToString() + "</precentOfCorrect>");
                    sw.WriteLine("</owl:NamedIndividual >");

                    sw.WriteLine("<rdf:Description>");
                    sw.WriteLine("<rdf:type rdf:resource = \"&owl;NegativePropertyAssertion\"/>");
                    sw.WriteLine("  <owl:targetIndividual rdf:resource = \"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/E2/144/2017\" />");
                    sw.WriteLine("  <owl:sourceIndividual rdf:resource = \"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/Pokusaj\" />");
                    sw.WriteLine("  <owl:assertionProperty rdf:resource = \"http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/attemptByStudent\" />");
                    sw.WriteLine("</rdf:Description >");
                }

            }


            IGraph g = new Graph();

            //g.LoadFromFile(path);


            //IGraph g = new Graph();


            /*
            IUriNode dotNetRDF = g.CreateUriNode(UriFactory.Create("http://www.dotnetrdf.org"));
            IUriNode says = g.CreateUriNode(UriFactory.Create("http://example.org/says"));
            ILiteralNode helloWorld = g.CreateLiteralNode("Hello World");
            ILiteralNode bonjourMonde = g.CreateLiteralNode("Bonjour tout le Monde", "fr");

            g.Assert(new Triple(dotNetRDF, says, helloWorld));
            g.Assert(new Triple(dotNetRDF, says, bonjourMonde));*/

            /*
            var o = new OntologyGraph();
            var lion = o.CreateOntologyClass(UriFactory.Create("http://my.taxonomies.com/myModel/Lion"));
            lion.AddType(UriFactory.Create(OntologyHelper.OwlClass));
            lion.AddLabel("Lion", "en");
            var animals = o.CreateOntologyClass(UriFactory.Create("http://my.taxonomies.com/myModel/Animals"));
            lion.AddSuperClass(animals);

            o.CreateIndividual(new Uri("http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#kolokvijum_1"),
                               new Uri("http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7#Test"));

            RdfXmlWriter rdfxmlwriter = new RdfXmlWriter();
            rdfxmlwriter.Save(o, "C:\\Users\\Panonit\\Desktop\\owlS\\ConvertTests\\Tests.owl");*/



            return Ok();
        }

        public ActionResult ImportOwl()
        {
            //try
            //{
                //VirtuosoManager manager = new VirtuosoManager();
            //"MYDB", "dba", "dba"

            IGraph g = new Graph();

                //virtuoso.LoadGraph(g, new Uri("http://example.org/"));
            //}
            //catch
            //{
            //    Console.WriteLine();
            //}

            return Ok();
        }

        public async Task<IActionResult> Virtuoso()
        {
            VirtuosoManager virtuoso = new VirtuosoManager("localhost", 1234, VirtuosoManager.DefaultDB, "dba", "dba");
            return Ok();
        }

        public async Task<IActionResult> Iita(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questions = await _context.Questions.Where(x => x.Test.ID == id).ToListAsync();

            List<List<int>> matrix = new List<List<int>>(); 

            for (int i = 0; i < questions.Count(); i++)
            {
                var answers = await _context.Answares.Where(x => x.QuestionID == questions[i].ID).ToListAsync();
                var answerIDs = answers.Select(x => x.ID);

                var allAtteptsIDs = await _context.TmpAnswares.Where(l => answerIDs.Any(id => id == l.AnswareID)).Select(e => e.AttemptID).Distinct().ToListAsync();
                //var allAtteptsIDs = await _context.TmpAnswares.Where(y => y.WasChecked).OrderBy(z => z.AttemptID).Select(e => e.AttemptID).Distinct().ToListAsync();
               // var allAttepts = await _context.Attempts.Where(l => allAtteptsIDs.Any(id => id == l.ID)).ToListAsync();//.OrderBy(l => allAtteptsIDs.IndexOf(l.ID));

                if (matrix.Count == 0)
                {
                    for (int j = 0; j < allAtteptsIDs.Count; j++)
                    {
                        matrix.Add(new List<int>());
                    }
                }

                var tmpAnswers = await _context.TmpAnswares.Where(y => y.WasChecked).OrderBy(z => z.AttemptID).ToListAsync();

                bool IsQuestinoRight = true;

                for (int j = 0; j < allAtteptsIDs.Count; j++)
                {
                    // Check if attempt is for this test by checking questions from attepts

                    //for (int w = 0; w < questions.Count; w++)
                    //{
                    //    if(questions[w].ID == allAttepts[j].) 
                    //}

                    //continue;


                    IsQuestinoRight = true;
                    for (int q = 0; q < tmpAnswers.Count; q++)
                    {
                        if(allAtteptsIDs[j] == tmpAnswers[q].AttemptID)
                        {
                            for (int a = 0; a < answers.Count(); a++)
                            {
                                if ( answers[a].ID == tmpAnswers[q].AnswareID)
                                {
                                    if( answers[a].IsItTrue != tmpAnswers[q].WasChecked)
                                    {
                                        IsQuestinoRight = false;
                                        goto nextOne;
                                    }
                                }
                            }
                        }
                    }

                nextOne:

                    matrix[j].Add(Convert.ToInt32(IsQuestinoRight));
                }
            }

            //var attempts = await _context.Attempts.Where(x => x.)
            if (questions == null)
            {
                return NotFound();
            }

            string json = JsonSerializer.Serialize(matrix);

            return Redirect("http://127.0.0.23:5001/iita?json=" + json);

            //return View(questions);
        }

    }
}
