using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using VisualProgrammingOnlineTestingSystemAss.QuestionClasses;
using VisualProgrammingOnlineTestingSystemAss.UserControls;

namespace VisualProgrammingOnlineTestingSystemAss
{
    public partial class MainForm : Form
    {
        private Test t1;
        private XMLSerialiser<Test> myXML;
        private UserControlManager controlManager;

        private UserControlStartPage startPage;
        private UserControlQuestionContainer questionPage;

        private string timeLimit;

        #region GettersAndSetters

        public XMLSerialiser<Test> MyXML
        {
            get
            {
                return myXML;
            }

            set
            {
                myXML = value;
            }
        }

        public UserControlManager ControlManager
        {
            get
            {
                return controlManager;
            }

            set
            {
                controlManager = value;
            }
        }

        public Test T1
        {
            get
            {
                return t1;
            }

            set
            {
                t1 = value;
            }
        }

        public Timer ExamTimer
        {
            get
            {
                return timerExam;
            }
        }

        public UserControlQuestionContainer QuestionPage
        {
            get
            {
                return questionPage;
            }

            set
            {
                questionPage = value;
            }
        }

        #endregion

        public MainForm()
        {
            InitializeComponent();
            DoubleBuffered = true;

            t1 = new Test();
            myXML = new XMLSerialiser<Test>(t1);
            controlManager = new UserControlManager(this);

            //t1.Name = "Quantum mechanics";
            //t1.Description = "A test of your quant knowledge, this exam will be grueling, difficult and almost impossible. we expect 75% of students to fail. you will be examined on knowledge, and flare for the subject.";
            //t1.Rules = "no cheating \n\u2022 NO CHEATING \n\u2022 NO CHEATING \n\u2022 NO CHEATING \n\u2022 NO CHEATING";

            //Question q1 = new Question();
            //q1.QuestionText = "First Question";
            //q1.AddChoice("Without a doubt the worst jedi i have ever seen", false);
            //q1.AddChoice("Mammal", false);
            //q1.AddChoice("answer 3", false);
            //q1.AddChoice("answer 4", true);

            //t1.Questions.Add(q1);

            //Question q2 = new Question();
            //q2.NoChoicesRequired = 2;
            //q2.MarksAvailable = 10;
            //q2.QuestionText = "Second Question";
            //q2.AddChoice("answer 1", false);
            //q2.AddChoice("answer 2", false);
            //q2.AddChoice("answer 3", true);
            //q2.AddChoice("answer 4", true);

            //t1.Questions.Add(q2);

            //Question q3 = new Question();
            //q3.MultipleChoice = false;
            //q3.QuestionText = "Third Question";
            //q3.AddChoice("answer 1", false);
            //q3.AddChoice("answer 2", false);
            //q3.AddChoice("answer 3", false);
            //q3.AddChoice("answer 4", true);

            //t1.Questions.Add(q3);
            myXML.Path = "Master.xml";
            //myXML.Serialise(t1);

            if (File.Exists(myXML.Path))
            {
                t1 = myXML.Deserialise(t1);
            }
            else
            {
                MessageBox.Show("There is no available test data","Warning");
                Environment.Exit(0);
            }

            
            myXML.Path = "TEMP";

            startPage = new UserControlStartPage(this);
            questionPage = new UserControlQuestionContainer(this,t1);
            controlManager.DisplayControl(startPage);

            timeLimit = TimeSpan.FromSeconds(t1.TimeLimit).ToString();
            string time = TimeSpan.FromSeconds(t1.CurrentTime).ToString();
            toolStripLabelTime.Text = time+"/"+timeLimit;
        }

        private void timerExam_Tick(object sender, EventArgs e)
        {
            t1.CurrentTime += 1;
            string time = TimeSpan.FromSeconds(t1.CurrentTime).ToString();
            toolStripLabelTime.Text = time + "/" + timeLimit;

            if (t1.CurrentTime >= t1.TimeLimit)
            {
                timerExam.Stop();
                t1.CurrentTime = t1.TimeLimit;
                questionPage.FinishExam();
                toolStripLabelTime.Text = timeLimit + "/" + timeLimit;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            myXML.Serialise(t1);
        }
    }
}
