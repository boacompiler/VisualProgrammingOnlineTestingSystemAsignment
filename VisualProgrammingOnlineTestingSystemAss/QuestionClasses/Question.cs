using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualProgrammingOnlineTestingSystemAss.QuestionClasses
{
    public class Question
    {
        private string questionText;
        private List<QuestionChoice> questionChoices;
        private bool multipleChoice;
        private bool flagged;
        private int flagCount;
        private int flagLimit;
        private int attemptCount;
        private int attemptLimit;
        private int marksAvailable;
        private int marksGained;
        private int noChoicesRequired;
        private List<QuestionChoice> studentChoices; //the choices the student made

        #region GettersAndSetters

        public string QuestionText
        {
            get
            {
                return questionText;
            }

            set
            {
                questionText = value;
            }
        }

        public bool Flagged
        {
            get
            {
                return flagged;
            }
            set
            {
                flagged = value;
            }
        }

        public int FlagCount
        {
            get
            {
                return flagCount;
            }

            set
            {
                flagCount = value;
            }
        }

        public int AttemptCount
        {
            get
            {
                return attemptCount;
            }

            set
            {
                attemptCount = value;
            }
        }

        public int AttemptLimit
        {
            get
            {
                return attemptLimit;
            }

            set
            {
                attemptLimit = value;
            }
        }

        public List<QuestionChoice> QuestionChoices
        {
            get
            {
                return questionChoices;
            }

            set
            {
                questionChoices = value;
            }
        }

        public int FlagLimit
        {
            get
            {
                return flagLimit;
            }

            set
            {
                flagLimit = value;
            }
        }

        public int MarksAvailable
        {
            get
            {
                return marksAvailable;
            }

            set
            {
                marksAvailable = value;
            }
        }

        public int MarksGained
        {
            get
            {
                return marksGained;
            }

            set
            {
                marksGained = value;
            }
        }

        public int NoChoicesRequired
        {
            get
            {
                return noChoicesRequired;
            }

            set
            {
                noChoicesRequired = value;
            }
        }

        public List<QuestionChoice> StudentChoices
        {
            get
            {
                return studentChoices;
            }

            set
            {
                studentChoices = value;
            }
        }

        public bool MultipleChoice
        {
            get
            {
                return multipleChoice;
            }

            set
            {
                multipleChoice = value;
            }
        }





        #endregion

        public Question()
        {
            //defaults
            questionChoices = new List<QuestionChoice>();
            multipleChoice = true;
            flagged = false;
            flagCount = 0;
            FlagLimit = 999999;
            attemptCount = 0;
            attemptLimit = 999999;
            marksAvailable = 1;
            marksGained = 0;
            noChoicesRequired = 1;
            studentChoices = new List<QuestionChoice>();
        }
        /// <summary>
        /// Adds multiple answer choices to the question, if the question is not mutiple choice, "choiceText" should be identical to the expected inputs ingnoring case
        /// </summary>
        /// <param name="choiceText">Text displayed for this choice</param>
        /// <param name="correct">whether the answer is correct or not. True == correct</param>
        public void AddChoice(string choiceText, bool correct)
        {
            QuestionChoice choice = new QuestionChoice();
            choice.ChoiceText = choiceText;
            choice.Correct = correct;
            questionChoices.Add(choice);
        }
        /// <summary>
        /// Toggle whether the question is flagged or not, als handles flag limit
        /// </summary>
        public void ToggleFlag()
        {
            if (!flagged)
            {
                if (flagCount >= flagLimit)
                {
                    throw new System.ArgumentException("Flag Limit Exceeded");
                }
                flagCount++;
            }
            flagged = !flagged;
        }
        /// <summary>
        /// validates the choices submitted and updates marksGained accordingly
        /// </summary>
        /// <param name="studentAnswer">a list of selected questionchoices</param>
        public void submitAnswer(List<QuestionChoice> studentAnswer)
        {
            if (attemptCount == attemptLimit)
            {
                throw new System.ArgumentException("Attempt Limit Exceeded");
            }
            studentChoices = studentAnswer;
            int correctAnswerCount = 0;
            for (int i = 0; i < studentChoices.Count; i++)
            {
                if(studentChoices[i].Correct)
                {
                    correctAnswerCount++;
                }
            }
            double marks = ((double)correctAnswerCount / (double)noChoicesRequired) * (double)marksAvailable;
            marksGained = (int)Math.Round(marks);

            //TODO currently this gets big fast, this is due to a poor compromise on the spec
            //attemptCount++;            
        }
        /// <summary>
        /// validates a string against questionchoices available.
        /// should be used for questions which are not multiple choice
        /// </summary>
        /// <param name="studentAnswer">the string submitted as an answer</param>
        public void submitAnswer(string studentAnswer)
        {
            List<QuestionChoice> studentAnswerChoices = new List<QuestionChoice>();
            for (int i = 0; i < questionChoices.Count; i++)
            {
                if (string.Equals(studentAnswer,questionChoices[i].ChoiceText,StringComparison.OrdinalIgnoreCase))
                {
                    studentAnswerChoices.Add(questionChoices[i]);
                }
            }
            if (studentAnswerChoices.Count < 1)
            {
                QuestionChoice studentChoice = new QuestionChoice();
                studentChoice.ChoiceText = studentAnswer;
                studentAnswerChoices.Add(studentChoice);
            }

            submitAnswer(studentAnswerChoices);
        }
    }
}
