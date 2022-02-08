namespace Sotis2.Controllers
{
    public class AttemptsControllerBase
    {




        public async Task<IActionResult> StartAttempt(long? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Tests.FindAsync(id);
            List<Question> questionsInChaos = await _context.Questions.Where(x => x.Test.ID == test.ID).ToListAsync();
            AttemptDTO attemptDTO = await PreprocessQuestionsAsync(questionsInChaos);
            if (test == null)
            {
                return NotFound();
            }
            return View(attemptDTO);
        }

        private List<List<long>> FindAllKnowlageStates(List<Domain> domains)
        {
            List<EdgeDD> edgeDD = _context.EdgeDDs.ToList();
            CpSolver solver = new CpSolver();

            CpModel model = new CpModel();
            List<IntVar> availabilityMatrix = new List<IntVar>();
            for (int d = 0; d < domains.Count; d++)
            {
                availabilityMatrix.Add(model.NewBoolVar("questino" + d));
            }

            throw new NotImplementedException();
        }

        private async Task<List<Question>> OrderAsync(List<Question> questionsInChaos)
        {
            List<Domain> domains = await _context.Domains.ToListAsync();
            List<EdgeDD> edgeDDs = await _context.EdgeDDs.ToListAsync();
            List<EdgeQD> edgeQDs = await _context.EdgeQDs.ToListAsync();

            //int[,] matrix = new int[questionsInChaos.Count + domains.Count, questionsInChaos.Count + domains.Count];
            int[,] matrix = new int[domains.Count, domains.Count];

            for (int e = 0; e < edgeDDs.Count; e++)
            {
                for (int d = 0; d < domains.Count; d++)
                {
                    if (edgeDDs[e].DomainFromID == domains[d].ID)
                    {
                        for (int d2 = 0; d2 < domains.Count; d2++)
                        {
                            if (edgeDDs[e].DomainToID == domains[d2].ID)
                            {
                                matrix[d, d2] = 1;
                            }
                        }
                    }
                }
            }

            List<int> ord = new List<int>();
            bool hasNonNegativeValues = true;
            bool hasNotZeroes = true;

            while (hasNonNegativeValues)
            {
                hasNonNegativeValues = false;
                for (int d = 0; d < domains.Count; d++)
                {
                    hasNotZeroes = true;
                    for (int d2 = 0; d2 < domains.Count; d2++)
                    {
                        if (matrix[d2, d] == 1)
                        {
                            hasNonNegativeValues = true;
                            hasNotZeroes = false;
                            break;
                        }

                        if (matrix[d2, d] == 0)
                        {
                            hasNonNegativeValues = true;
                        }
                    }
                }
            }

            return questionsInChaos;
        }

        private List<Question> OrderWithGraph(List<Question> questionsInChaos)
        {
            List<EdgeDD> edgeDD = _context.EdgeDDs.ToList();
            List<Sotis2.Models.Domain> domains = _context.Domains.ToList();
            //List<int> ids = edgeDD.Select(x => x.ID).ToList();

            List<long> canBeDomain = _context.Domains.ToList().Select(x => x.ID).ToList();
            List<long> canBeQuestion = questionsInChaos.Select(x => x.ID).ToList();

            CpModel model = new CpModel();
            List<EdgeQD> edgeQD = _context.EdgeQDs.ToList();
            for (int r = edgeQD.Count - 1; r >= 0; r--)
            {
                if (!canBeDomain.Contains(edgeQD[r].DomainToID) || !canBeQuestion.Contains(edgeQD[r].QuestionFromID))
                {
                    edgeQD.RemoveAt(r);
                }
            }

            List<IntVar> availabilityMatrix = new List<IntVar>();
            for (int d = 0; d < domains.Count; d++)
            {
                availabilityMatrix.Add(model.NewIntVar(0, domains.Count - 1, "questino" + d));
            }

            // First constraint
            model.AddAllDifferent(availabilityMatrix);


            // Second Constraint
            for (int edd = 0; edd < edgeDD.Count; edd++)
            {
                for (int d1 = 0; d1 < domains.Count; d1++)
                {
                    for (int d2 = 0; d2 < domains.Count; d2++)
                    {
                        if (domains[d1].ID == edgeDD[edd].DomainFromID && domains[d2].ID == edgeDD[edd].DomainToID)
                        {
                            model.Add(availabilityMatrix[d1] < availabilityMatrix[d2]);
                        }
                    }
                }
            }

            CpSolver solver = new CpSolver();

            CpSolverStatus status = solver.Solve(model);
            if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
            {
                int[] ord = new int[domains.Count];
                for (int d = 0; d < domains.Count; d++)
                {
                    ord[solver.Value(availabilityMatrix[d])] = d;
                    //Console.WriteLine(d + " = " + solver.Value(availabilityMatrix[d]));
                }

                int startWith = 0;
                for (int o = 0; o < ord.Length; o++)
                {
                    for (int eqd = 0; eqd < edgeQD.Count(); eqd++)
                    {
                        if (edgeQD[eqd].DomainToID == domains[ord[o]].ID)
                        {
                            for (int qic = 0; qic < questionsInChaos.Count; qic++)
                            {
                                if (edgeQD[eqd].QuestionFromID == questionsInChaos[qic].ID)
                                {
                                    Question question = questionsInChaos[startWith];
                                    questionsInChaos[startWith] = questionsInChaos[qic];
                                    questionsInChaos[qic] = question;
                                    startWith++;
                                    break;
                                }
                            }

                        }
                    }
                }

                return questionsInChaos;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private async Task<AttemptDTO> PreprocessQuestionsAsync(List<Question> questionsInChaos)
        {
            Attempt attempt = new Attempt();
            attempt.StartTime = DateTime.Now;
            _context.Add(attempt);
            await _context.SaveChangesAsync();

            AttemptDTO attemptDTO = new AttemptDTO();
            attemptDTO.TmpQuestionDTOs = new List<TmpQuestionDTO>();

            List<Question> questionsInOrder = OrderWithGraph(questionsInChaos); //OrderAsync

            List<Domain> domains = _context.Domains.ToList();
            List<List<long>> K = FindAllKnowlageStates(domains);


            List<Answare> answares = new List<Answare>();

            foreach (Question question in questionsInOrder)
            {
                TmpQuestionDTO tmpQuestionDTO = new TmpQuestionDTO();
                tmpQuestionDTO.QuestionText = question.QuestionText;
                tmpQuestionDTO.TmpAnswaresDTO = new List<TmpAnsware>();

                answares = await _context.Answares.Where(x => x.QuestionID == question.ID).ToListAsync();

                foreach (Answare answare in answares)
                {
                    TmpAnsware tmpAnsware = new TmpAnsware();
                    tmpAnsware.AnswareText = answare.AnswareText;
                    tmpAnsware.WasChecked = false;
                    tmpAnsware.AnswareID = answare.ID;
                    tmpAnsware.AttemptID = attempt.ID;

                    _context.Add(tmpAnsware);
                    await _context.SaveChangesAsync();

                    tmpQuestionDTO.TmpAnswaresDTO.Add(tmpAnsware);
                }

                attemptDTO.TmpQuestionDTOs.Add(tmpQuestionDTO);
            }

            attemptDTO.TmpSerialQuestion = 0;
            attemptDTO.TotalNumberOfQuestions = attemptDTO.TmpQuestionDTOs.Count();

            return attemptDTO;
        }
    }
}