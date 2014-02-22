using System.Collections.Generic;

namespace QA
{
    public class Question
    {
        public Question()
        {
            Answers = new List<string>();
        }

        public string Text { get; set; }
        public List<string> Answers { get; set; }
        public int CorrectAnswerIndex { get; set; }
    }
}
