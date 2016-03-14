using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisualProgrammingOnlineTestingSystemAss.UserControls
{
    public partial class UserControlQuestionContainer : UserControl
    {
        MainForm parent;
        Test test;

        List<UserControlQuestion> multiChoiceControls;
        UserControlSubmit submissionControl;
        UserControlFinished finishedControl;

        Image blankImage;
        Image tick;
        Image cross;
        Image flag;

        public Test Test
        {
            get
            {
                return test;
            }

            set
            {
                test = value;
            }
        }

        public UserControlQuestionContainer(MainForm parent, Test test)
        {
            InitializeComponent();

            this.parent = parent;
            this.test = test;

            multiChoiceControls = new List<UserControlQuestion>();

            ImageList imglist = new ImageList();

            blankImage = Image.FromFile("Blank.png");
            tick = Image.FromFile("Tick.png");
            cross = Image.FromFile("Cross.png");
            flag = Image.FromFile("Flag.png");

            imglist.Images.Add(blankImage);
            imglist.Images.Add(tick);
            imglist.Images.Add(cross);
            imglist.Images.Add(flag);

            treeViewQuestions.ImageList = imglist;

            SetupTree();
            SetupQuestions();
            submissionControl = new UserControlSubmit(parent, this);
            finishedControl = new UserControlFinished(parent, this);

            DisplayQuestionControl(multiChoiceControls[0]);

    }

        void SetupTree()
        {
            int selected = treeViewQuestions.Nodes.Count > 0 ? treeViewQuestions.SelectedNode.Index : 0;
            treeViewQuestions.Nodes.Clear();
            for (int i = 0; i < test.Questions.Count; i++)
            {
                int iconIndex = 0;
                if (test.Questions[i].Flagged)
                {
                    iconIndex = 3;
                }
                if (test.Questions[i].MarksGained > 0 && test.Completed)
                {
                    iconIndex = 1;
                }
                if (test.Questions[i].MarksGained == 0 && test.Completed)
                {
                    iconIndex = 2;
                }
                

                string nodeText = "Question " + (i + 1);
                if (test.Completed)
                {
                    nodeText += " " + test.Questions[i].MarksGained + "/" + test.Questions[i].MarksAvailable;
                }

                TreeNode node = new TreeNode(nodeText, iconIndex, iconIndex);
                treeViewQuestions.Nodes.Add(node);
            }
            if (test.Completed)
            {
                TreeNode resultsNode = new TreeNode("Results", 0, 0);
                treeViewQuestions.Nodes.Add(resultsNode);
            }
            else
            {
                TreeNode submissionNode = new TreeNode("Submit", 0, 0);
                treeViewQuestions.Nodes.Add(submissionNode);
            }
            

            treeViewQuestions.SelectedNode = treeViewQuestions.Nodes[selected];
        }

        void SetupQuestions()
        {
            for (int i = 0; i < test.Questions.Count; i++)
            {
                multiChoiceControls.Add(new UserControlQuestion(test.Questions[i]));
            }
        }

        void DisplayQuestionControl(UserControl uControl)
        {
            if (!panelQuestion.Controls.Contains(uControl))
            {
                panelQuestion.Controls.Add(uControl);
            }
            foreach (Control cont in panelQuestion.Controls)
            {
                if (cont is UserControl)
                {
                    cont.Enabled = false;
                }
            }

            uControl.Dock = DockStyle.Fill;
            uControl.Enabled = (!test.Completed || uControl == finishedControl);
            uControl.Visible = true;
            uControl.BringToFront();
        }

        public void MarkAll()
        {
            if (!test.Completed)
            {
                try
                {
                    for (int i = 0; i < multiChoiceControls.Count; i++)
                    {
                        multiChoiceControls[i].SubmitAnswer();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning");
                }
            }
            
        }
        public void FinishExam()
        {
            MarkAll();
            test.Completed = true;
            parent.ExamTimer.Stop();
            finishedControl = new UserControlFinished(parent, this);
            SetupTree();
        }

        private void buttonFlag_Click(object sender, EventArgs e)
        {
            if (treeViewQuestions.SelectedNode.Index < test.Questions.Count)
            {
                try
                {
                    test.Questions[treeViewQuestions.SelectedNode.Index].ToggleFlag();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Warning");
                }
            }
            SetupTree();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeViewQuestions.SelectedNode.Index < test.Questions.Count)
            { 
                DisplayQuestionControl(multiChoiceControls[treeViewQuestions.SelectedNode.Index]);
                
            }
            else
            {
                if (test.Completed)
                {
                    DisplayQuestionControl(finishedControl);
                }
                else
                {
                    DisplayQuestionControl(submissionControl);
                }
                
            }
            if (!test.Completed)
            {
                MarkAll();
            }
            
        }

        public void SetTreeviewIndex(int index)
        {
            treeViewQuestions.SelectedNode = treeViewQuestions.Nodes[index];
            SetupTree();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {            
            if (treeViewQuestions.SelectedNode.Index < treeViewQuestions.Nodes.Count -1)
            {
                treeViewQuestions.SelectedNode = treeViewQuestions.SelectedNode.NextNode;
            }
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            if (treeViewQuestions.SelectedNode.Index > 0)
            {
                treeViewQuestions.SelectedNode = treeViewQuestions.SelectedNode.PrevNode;
            }
        }
    }
}
