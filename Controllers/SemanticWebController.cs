using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sotis2.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Controllers
{
    public class SemanticWebController : Controller
    {
        // GET: SemanticWeb
        public ActionResult Index()
        {
            return View();
        }

        // GET: SemanticWeb/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SemanticWeb/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SemanticWeb/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SemanticWeb/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SemanticWeb/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SemanticWeb/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SemanticWeb/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Question1()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Question2()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Question3()
        {
            SemanticWebDTO semanticWebDTO = new SemanticWebDTO();
            semanticWebDTO.order = true;
            return View(semanticWebDTO);
        }

        [HttpGet]
        public ActionResult Question4()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Question5()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Question6()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetQuestion(SemanticWebDTO semanticWebDTO)
        {

            string query = "";
            switch (semanticWebDTO.id.ToString())
            {
                // #1] Prebrojati broj izbornih predmeta po studijskom programu
                /*
                    prefix owl: <http://www.owl-ontologies.com/travel.owl#>
                    prefix travel: <http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/>
                    prefix w3: <http://www.w3.org/2002/07/owl#>
                    prefix aiiso: <http://purl.org/vocab/aiiso/schema#>

                    select (STRAFTER(str(?programme ), "-7/")) as ?ProgrammeName COUNT(?programme) as ?Counter
                    where 
                    {  
                          ?course  ?v aiiso:Course 
                          FILTER(SUBSTR((STRAFTER(str(?course  ), "-7/")), 0 , 5) = "Izbor") .
                          ?courseNode w3:targetIndividual ?course .
                          ?courseNode  w3:sourceIndividual ?programme 
                           FILTER (?programme  != travel:Faculty_of_Technical_Sciences)
                    }
                    group by ?programme 

                 */
                case "1":

                    query = "prefix+owl%3A+%3Chttp%3A%2F%2Fwww.owl-ontologies.com%2Ftravel.owl%23%3E%0D%0Aprefix+travel%3A+%3Chttp%3A%2F%2Fwww.semanticweb.org%2Fpanonit%2Fontologies%2F2021%2F11%2Funtitled-ontology-7%2F%3E%0D%0Aprefix+w3%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2002%2F07%2Fowl%23%3E%0D%0Aprefix+aiiso%3A+%3Chttp%3A%2F%2Fpurl.org%2Fvocab%2Faiiso%2Fschema%23%3E%0D%0A%0D%0Aselect+%28STRAFTER%28str%28%3Fprogramme+%29%2C+%22-7%2F%22%29%29+as+%3FProgrammeName+COUNT%28%3Fprogramme%29+as+%3FCounter%0D%0Awhere+%0D%0A%7B++%0D%0A++++++%3Fcourse++%3Fv+aiiso%3ACourse+%0D%0A++++++FILTER%28SUBSTR%28%28STRAFTER%28str%28%3Fcourse++%29%2C+%22-7%2F%22%29%29%2C+0+%2C+5%29+%3D+%22Izbor%22%29+.%0D%0A++++++%3FcourseNode+w3%3AtargetIndividual+%3Fcourse+.%0D%0A++++++%3FcourseNode++w3%3AsourceIndividual+%3Fprogramme+%0D%0A+++++++FILTER+%28%3Fprogramme++%21%3D+travel%3AFaculty_of_Technical_Sciences%29%0D%0A%7D%0D%0Agroup+by+%3Fprogramme+";
                    break;

                case "2":
                    // #2] Question 2: Filtrirati sve predmete koji imaju više od zadatog broja ESP bodova
                    /*
                        prefix owl: <http://www.owl-ontologies.com/travel.owl#>

                        select (STRAFTER(str(?c), "-7/")) ?NumberOfESPPoints 
                        where 
                        {  
                              ?c
                              owl:ESP_PointsPerCourse  
                              ?NumberOfESPPoints  FILTER (?NumberOfESPPoints > 4  &&  ?NumberOfESPPoints < 7 ) 
                        }
                     */
                    int lowerBoundery = (int) semanticWebDTO.lowerBoundery;
                    int upperBoundery = (int) semanticWebDTO.upperBoundery;

                    query = "prefix+owl%3A+%3Chttp%3A%2F%2Fwww.owl-ontologies.com%2Ftravel.owl%23%3E%0D%0A%0D%0Aselect+%28STRAFTER%28str%28%3Fc%29%2C+%22-7%2F%22%29%29+%3FNumberOfESPPoints+%0D%0Awhere+%0D%0A%7B++%0D%0A++++++%3Fc%0D%0A++++++owl%3AESP_PointsPerCourse++%0D%0A++++++%3FNumberOfESPPoints++FILTER+%28%3FNumberOfESPPoints+%3E+"+lowerBoundery+"++%26%26++%3FNumberOfESPPoints+%3C+"+upperBoundery+"+%29+%0D%0A%7D";
                    break;

                case "3":
                    // #3] Question 3: Za sve studente koji su ostvarili preko 51% procenat zadatom testu, ispisati studente i njihov procenat uradjenog testa. Rezultat sortirati po procentima.
                    /*
                        prefix sw: <http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/>
                        prefix owl: <http://www.w3.org/2002/07/owl#>
                        prefix xmlns: <http://www.owl-ontologies.com/travel.owl#>

                        select ?b  (GROUP_CONCAT(CONCAT(?g1, ?g2) ; SEPARATOR = ", ") AS ?StudentsNameAndSurname)
                        where 
                        {  
                                ?s   owl:targetIndividual sw:Test_Front_End .
                                ?s owl:sourceIndividual ?f .
                                ?f xmlns:accuracy ?b FILTER(?b > 0.5)  .
                                ?w ?h ?f .
                                ?w ?e xmlns:studentHasAttempt .
                                ?w <http://www.w3.org/2002/07/owl#sourceIndividual> ?t .

                            {?t <http://xmlns.com/foaf/0.1/surname> ?g1}
                                 UNION
                            {?t <http://purl.org/vocab/aiiso/schema#name> ?g2}
                        }
                        GROUP BY ?t ?b
                        ORDER BY ?b
                     */
                    if ((bool) semanticWebDTO.order)
                    {
                        query = "%0D%0Aprefix+sw%3A+%3Chttp%3A%2F%2Fwww.semanticweb.org%2Fpanonit%2Fontologies%2F2021%2F11%2Funtitled-ontology-7%2F%3E%0D%0Aprefix+owl%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2002%2F07%2Fowl%23%3E%0D%0Aprefix+xmlns%3A+%3Chttp%3A%2F%2Fwww.owl-ontologies.com%2Ftravel.owl%23%3E%0D%0A%0D%0Aselect+%3Fb++%28GROUP_CONCAT%28CONCAT%28%3Fg1%2C+%3Fg2%29+%3B+SEPARATOR+%3D+%22%2C+%22%29+AS+%3FStudentsNameAndSurname%29%0D%0Awhere+%0D%0A%7B++%0D%0A++++++++%3Fs+++owl%3AtargetIndividual+sw%3ATest_Front_End+.%0D%0A++++++++%3Fs+owl%3AsourceIndividual+%3Ff+.%0D%0A++++++++%3Ff+xmlns%3Aaccuracy+%3Fb+FILTER%28%3Fb+%3E+0.5%29++.%0D%0A++++++++%3Fw+%3Fh+%3Ff+.%0D%0A++++++++%3Fw+%3Fe+xmlns%3AstudentHasAttempt+.%0D%0A++++++++%3Fw+%3Chttp%3A%2F%2Fwww.w3.org%2F2002%2F07%2Fowl%23sourceIndividual%3E+%3Ft+.%0D%0A%0D%0A++++%7B%3Ft+%3Chttp%3A%2F%2Fxmlns.com%2Ffoaf%2F0.1%2Fsurname%3E+%3Fg1%7D%0D%0A+++++++++UNION%0D%0A++++%7B%3Ft+%3Chttp%3A%2F%2Fpurl.org%2Fvocab%2Faiiso%2Fschema%23name%3E+%3Fg2%7D%0D%0A%7D%0D%0AGROUP+BY+%3Ft+%3Fb%0D%0AORDER+BY+%3Fb";
                    }
                    else
                    {
                        query = "prefix+sw%3A+%3Chttp%3A%2F%2Fwww.semanticweb.org%2Fpanonit%2Fontologies%2F2021%2F11%2Funtitled-ontology-7%2F%3E%0D%0Aprefix+owl%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2002%2F07%2Fowl%23%3E%0D%0Aprefix+xmlns%3A+%3Chttp%3A%2F%2Fwww.owl-ontologies.com%2Ftravel.owl%23%3E%0D%0A%0D%0Aselect+%3Fb++%28GROUP_CONCAT%28CONCAT%28%3Fg1%2C+%3Fg2%29+%3B+SEPARATOR+%3D+%22%2C+%22%29+AS+%3FStudentsNameAndSurname%29%0D%0Awhere+%0D%0A%7B++%0D%0A++++++++%3Fs+++owl%3AtargetIndividual+sw%3ATest_Front_End+.%0D%0A++++++++%3Fs+owl%3AsourceIndividual+%3Ff+.%0D%0A++++++++%3Ff+xmlns%3Aaccuracy+%3Fb+FILTER%28%3Fb+%3E+0.5%29++.%0D%0A++++++++%3Fw+%3Fh+%3Ff+.%0D%0A++++++++%3Fw+%3Fe+xmlns%3AstudentHasAttempt+.%0D%0A++++++++%3Fw+%3Chttp%3A%2F%2Fwww.w3.org%2F2002%2F07%2Fowl%23sourceIndividual%3E+%3Ft+.%0D%0A%0D%0A++++%7B%3Ft+%3Chttp%3A%2F%2Fxmlns.com%2Ffoaf%2F0.1%2Fsurname%3E+%3Fg1%7D%0D%0A+++++++++UNION%0D%0A++++%7B%3Ft+%3Chttp%3A%2F%2Fpurl.org%2Fvocab%2Faiiso%2Fschema%23name%3E+%3Fg2%7D%0D%0A%7D%0D%0AGROUP+BY+%3Ft+%3Fb%0D%0AORDER+BY+DESC%28%3Fb%29";
                    }
                    break;

                case "4":
                    // Question 4: Izračunati srednju vrednost procenta, koji su studenti dobili na testu
                    /* 
                        prefix sw: <http://www.semanticweb.org/panonit/ontologies/2021/11/untitled-ontology-7/>
                        prefix owl: <http://www.w3.org/2002/07/owl#>
                        prefix xmlns: <http://www.owl-ontologies.com/travel.owl#>

                        select ROUND(AVG(?b)*100)/100 as ?Average
                        where 
                        {  
                                ?s owl:targetIndividual sw:Test_Front_End .
                                ?s owl:sourceIndividual ?f .
                                ?f xmlns:accuracy ?b
                        }
                     */
                    query = "prefix+sw%3A+%3Chttp%3A%2F%2Fwww.semanticweb.org%2Fpanonit%2Fontologies%2F2021%2F11%2Funtitled-ontology-7%2F%3E%0D%0Aprefix+owl%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2002%2F07%2Fowl%23%3E%0D%0Aprefix+xmlns%3A+%3Chttp%3A%2F%2Fwww.owl-ontologies.com%2Ftravel.owl%23%3E%0D%0A%0D%0Aselect+ROUND%28AVG%28%3Fb%29%2A100%29%2F100+as+%3FAverage%0D%0Awhere+%0D%0A%7B++%0D%0A++++++++%3Fs+++owl%3AtargetIndividual+sw%3ATest_Front_End+.%0D%0A++++++++%3Fs+owl%3AsourceIndividual+%3Ff+.%0D%0A++++++++%3Ff+xmlns%3Aaccuracy+%3Fb%0D%0A%7D";
                    break;

                case "5":
                    // Question 5: Prebrojati broji predmeta po studijskom programu
                    /*
                        prefix owl: <http://www.w3.org/2002/07/owl#>
                        prefix sw: <http://www.owl-ontologies.com/travel.owl#>

                        select (STRAFTER(str(?programme), "-7/") as ?programmeName) (COUNT(?programme) as ?howMany) 
                        where 
                        {  
                              ?o owl:assertionProperty sw:programmeHas .
                              ?o owl:sourceIndividual ?programme
                         }
                        GROUP BY ?programme
                     */
                    query = "prefix+owl%3A+%3Chttp%3A%2F%2Fwww.w3.org%2F2002%2F07%2Fowl%23%3E%0D%0Aprefix+sw%3A+%3Chttp%3A%2F%2Fwww.owl-ontologies.com%2Ftravel.owl%23%3E%0D%0A%0D%0Aselect+%28STRAFTER%28str%28%3Fprogramme%29%2C+%22-7%2F%22%29+as+%3FprogrammeName%29+%28COUNT%28%3Fprogramme%29+as+%3FhowMany%29+%0D%0Awhere+%0D%0A%7B++%0D%0A++++++%3Fo+owl%3AassertionProperty+sw%3AprogrammeHas+.%0D%0A++++++%3Fo+owl%3AsourceIndividual+%3Fprogramme%0D%0A+%7D%0D%0AGROUP+BY+%3Fprogramme";
                    break;

                case "6":
                    // Question 6: Ispisati 15 predmeta sa najvećom razlikom fonda časova između predavanja i vežbi, u opadajucem poredku
                    //             
                    /*
                        prefix owl: <http://www.owl-ontologies.com/travel.owl#>

                        select (STRAFTER(str(?programme ), "-7/")) as ?programmeName 
                                   ?fondOfLectures  as ?Lectures 
                                   xsd:integer(SUBSTR(?fondOfExercisess, 0, 1)) as ?Exercisess 
                                   abs((?fondOfLectures  - xsd:integer(SUBSTR(?fondOfExercisess, 0, 1)))) as ?Difference
                        where 
                        {  
                              ?programme owl:Found_Lectures ?fondOfLectures .
                              ?programme  owl:Found_Exercisess ?fondOfExercisess
                        }
                        ORDER BY DESC (abs((?fondOfLectures  - xsd:integer(SUBSTR(?fondOfExercisess, 0, 1)))))
                        LIMIT 20
                    
                     */
                    query = "prefix+owl%3A+%3Chttp%3A%2F%2Fwww.owl-ontologies.com%2Ftravel.owl%23%3E%0D%0A%0D%0Aselect+%28STRAFTER%28str%28%3Fprogramme+%29%2C+%22-7%2F%22%29%29+as+%3FprogrammeName+%0D%0A+++++++++++%3FfondOfLectures++as+%3FLectures+%0D%0A+++++++++++xsd%3Ainteger%28SUBSTR%28%3FfondOfExercisess%2C+0%2C+1%29%29+as+%3FExercisess+%0D%0A+++++++++++abs%28%28%3FfondOfLectures++-+xsd%3Ainteger%28SUBSTR%28%3FfondOfExercisess%2C+0%2C+1%29%29%29%29+as+%3FDifference%0D%0Awhere+%0D%0A%7B++%0D%0A++++++%3Fprogramme+owl%3AFound_Lectures+%3FfondOfLectures+.%0D%0A++++++%3Fprogramme++owl%3AFound_Exercisess+%3FfondOfExercisess%0D%0A%7D%0D%0AORDER+BY+DESC+%28abs%28%28%3FfondOfLectures++-+xsd%3Ainteger%28SUBSTR%28%3FfondOfExercisess%2C+0%2C+1%29%29%29%29%29%0D%0ALIMIT+20%0D%0A";
                    break;

            }


            return Redirect("http://localhost:8890/sparql?default-graph-uri=http%3A%2F%2Flocalhost%3A8890%2FTestIRIFinal4&query=" + query + "&format=text%2Fx-html%2Btr&debug=on");
        }
    }
}
