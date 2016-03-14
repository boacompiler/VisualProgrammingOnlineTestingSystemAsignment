namespace VisualProgrammingOnlineTestingSystemAss.QuestionClasses
{
    public class QuestionChoice
    {
        private string choiceText;
        private bool correct;

        #region GettersAndSetters

        public string ChoiceText
        {
            get
            {
                return choiceText;
            }

            set
            {
                choiceText = value;
            }
        }

        public bool Correct
        {
            get
            {
                return correct;
            }

            set
            {
                correct = value;
            }
        }

        #endregion

        public QuestionChoice()
        {
            //defaults
            bool correct = false;
        }

    }
}
