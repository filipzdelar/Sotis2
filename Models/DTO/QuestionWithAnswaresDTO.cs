using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Models.DTO
{
    public class QuestionWithAnswaresDTO : Question
    {

        public QuestionWithAnswaresDTO()
        {

        }

        public QuestionWithAnswaresDTO(List<Answare> answares)
        {
            Answares = answares;
        }

        public QuestionWithAnswaresDTO(List<Answare> answares, string questionText) : base(questionText)
        {
            Answares = answares;
        }

        public QuestionWithAnswaresDTO(int ID, string questionText, List<Answare> answares) : base(ID, questionText)
        {
            Answares = answares;
        }

        /*
        public QuestionWithAnswaresDTO(List<Answare> answares, List<Subject> subjects)
        {
            Answares = answares;
            Subjects = subjects;
        }*/

        [BindProperty]
        public List<Answare> Answares { get; set; }
        //public List<Subject> Subjects { get; set; }
        //public List<SelectListItem> Subjects = new List<SelectListItem>();


    }
}
