using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisualProgrammingOnlineTestingSystemAss.QuestionClasses;

namespace VisualProgrammingOnlineTestingSystemAss.UserControls
{
    public partial class UserControlFinished : UserControl
    {
        MainForm parent;
        UserControlQuestionContainer container;

        public UserControlFinished(MainForm parent, UserControlQuestionContainer container)
        {
            InitializeComponent();
            this.parent = parent;
            this.container = container;

            int marks = 0;
            int totalMarks = 0;
            double percentage = 0;

            for (int i = 0; i < container.Test.Questions.Count; i++)
            {
                marks += container.Test.Questions[i].MarksGained;
                totalMarks += container.Test.Questions[i].MarksAvailable;
            }

            percentage = Math.Round( ((double)marks / (double)totalMarks) * 100,2);

            labelMarks.Text = "You Earned "+marks+" out of "+totalMarks+" marks.\nThis is equivalent to "+percentage+"%";
            if (percentage > 60)
            {
                labelMarks.Text += "\nWell Done";
            }
            else
            {
                labelMarks.Text += "\nKeep Trying";
            }
        }

        private void buttonRetryFailedQuestions_Click(object sender, EventArgs e)
        {
            int failedCount = 0;
            for (int i = 0; i < container.Test.Questions.Count; i++)
            {
                if (container.Test.Questions[i].MarksGained < container.Test.Questions[i].MarksAvailable)
                {
                    failedCount++;
                }
            }

            if (failedCount > 0)
            {
                Test failedQuestions = new Test();

                for (int i = 0; i < container.Test.Questions.Count; i++)
                {
                    if (container.Test.Questions[i].MarksGained < container.Test.Questions[i].MarksAvailable)
                    {
                        //we setup a new question because there is no clone method, to  prevent affecting the real results
                        Question failedQuestion = new Question();
                        failedQuestion.QuestionText = container.Test.Questions[i].QuestionText;
                        failedQuestion.QuestionChoices = container.Test.Questions[i].QuestionChoices;
                        failedQuestion.MarksAvailable = container.Test.Questions[i].MarksAvailable;
                        failedQuestion.MultipleChoice = container.Test.Questions[i].MultipleChoice;
                        failedQuestion.NoChoicesRequired = container.Test.Questions[i].NoChoicesRequired;
                        failedQuestions.Questions.Add(failedQuestion);
                    }
                }

                Form failedForm = new Form();
                UserControlQuestionContainer ucqc = new UserControlQuestionContainer(parent, failedQuestions);
                failedForm.Controls.Add(ucqc);
                ucqc.Dock = DockStyle.Fill;
                failedForm.MinimumSize = parent.MinimumSize;
                failedForm.Text = "Retry Questions";
                
                
                failedForm.FormClosing += delegate (object o, FormClosingEventArgs ea)
                {

                    parent.Enabled = true;
                };

                MessageBox.Show("This will allow you to attempt the questions you did not receive full marks in.\nThis does not affect your final marks.","Alert");
                parent.Enabled = false;
                failedForm.ShowDialog();

            }
            
        }
    }
}
