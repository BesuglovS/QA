using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace QA
{
    static class Program
    {
        static void Main()
        {
            var currentDirectory = Path.GetDirectoryName(
                System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            var questions = ReadQuestionsFromFIle(currentDirectory + "\\Questions.txt");

            var testResult = RunTest(questions);

            PrintResultStatistics(testResult);

            Console.ReadKey();
        }

        private static void PrintResultStatistics(TestResult testResult)
        {
            Console.WriteLine("Всего вопросов: " + testResult.QuestionsCount);

            Console.WriteLine("Правильных ответов: {0} ({1:0.##}%)",
                testResult.RightAnswers,
                (testResult.RightAnswers * 100) / testResult.QuestionsCount);

            Console.WriteLine("Неправильных ответов: {0} ({1:0.##}%)",
                testResult.WrongAnswers,
                (testResult.WrongAnswers * 100) / testResult.QuestionsCount);
        }

        private static TestResult RunTest(List<Question> questions)
        {
            var result = new TestResult {QuestionsCount = questions.Count};

            foreach (var question in questions)
            {
                Console.WriteLine(question.Text);
                for (int i = 0; i < question.Answers.Count; i++)
                {
                    Console.WriteLine((i + 1).ToString(CultureInfo.InvariantCulture) + ") " + question.Answers[i]);
                }

                var stringAnswer = Console.ReadLine() ?? "";
                var answerIndex = int.Parse(stringAnswer) - 1;

                if (answerIndex == question.CorrectAnswerIndex)
                {
                    result.RightAnswers++;

                    Console.WriteLine("Вы восхитительны!");
                }
                else
                {
                    result.WrongAnswers++;

                    Console.WriteLine("А правильный ответ на самом деле: " + question.Answers[question.CorrectAnswerIndex]);
                }

                Console.WriteLine();
            }

            return result;
        }

        private static List<Question> ReadQuestionsFromFIle(string filename)
        {
            var result = new List<Question>();

            var sr = new StreamReader(filename);

            int status = 1; // 1 - Ждём вопроса; 2 - Читаем ответы
            int answerIndex = 0;
            var newQuestion = new Question();

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line == "")
                {
                    if (status != 1)
                    {
                        result.Add(newQuestion);
                        newQuestion = new Question();
                    }

                    status = 1;
                    continue;
                }

                switch (status)
                {
                    case 1:
                        newQuestion.Text = line;
                        status = 2;
                        answerIndex = 0;
                        break;
                    case 2:
                        if (line.StartsWith("*"))
                        {
                            newQuestion.CorrectAnswerIndex = answerIndex;
                            line = line.Substring(1, line.Length - 1);
                        }

                        newQuestion.Answers.Add(line);

                        answerIndex++;
                        break;
                }
            }

            if (newQuestion.Text != "")
            {
                result.Add(newQuestion);
            }

            sr.Close();

            return result;
        }
    }
}
