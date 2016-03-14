using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualProgrammingOnlineTestingSystemAss.QuestionClasses;
using System.Xml.Serialization;

namespace VisualProgrammingOnlineTestingSystemAss
{
    public class Test
    {
        
        private string name;
        private string description;
        private string rules;
        private int timeLimit; //in seconds
        private int currentTime;
        private List<Question> questions;
        private int currentQuestion; //index of the current question //TODO still needed?
        private bool completed;
        private bool allowRetakes;

        #region GettersAndSetters

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }

        public string Rules
        {
            get
            {
                return rules;
            }

            set
            {
                rules = value;
            }
        }

        public int TimeLimit
        {
            get
            {
                return timeLimit;
            }

            set
            {
                timeLimit = value;
            }
        }
        //[XmlIgnore]
        public int CurrentTime
        {
            get
            {
                return currentTime;
            }

            set
            {
                currentTime = value;
            }
        }

        public List<Question> Questions
        {
            get
            {
                return questions;
            }

            set
            {
                questions = value;
            }
        }
        //[XmlIgnore]
        public int CurrentQuestion
        {
            get
            {
                return currentQuestion;
            }

            set
            {
                currentQuestion = value;
            }
        }

        public bool Completed
        {
            get
            {
                return completed;
            }

            set
            {
                completed = value;
            }
        }

        public bool AllowRetakes
        {
            get
            {
                return allowRetakes;
            }

            set
            {
                allowRetakes = value;
            }
        }

        #endregion

        public Test()
        {
            //defaults
            timeLimit = 999999;
            currentTime = 0;
            questions = new List<Question>();
            currentQuestion = 0;
            completed = false;
            allowRetakes = true;
        }

    }
}
