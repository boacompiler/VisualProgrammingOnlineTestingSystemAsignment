using System;
using System.IO;
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
    public partial class UserControlStartPage : UserControl
    {
        MainForm parent;

        public UserControlStartPage(MainForm parent)
        {
            InitializeComponent();
            this.parent = parent;

            labelName.Text = parent.T1.Name;
            labelDescription.Text = parent.T1.Description;
            labelRules.Text = parent.T1.Rules;
            //this delegate allows for wordwrap in a label
            this.Resize += delegate (object o, EventArgs ea)
            {
                labelName.MaximumSize = groupBoxName.Size;
                labelDescription.MaximumSize = groupBoxDescription.Size;
                labelRules.MaximumSize = groupBoxRules.Size;
            };
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            //real validation would be needed in production, but this is ok for a test, we would need access to the school systems student database for proper validation
            if (textBoxStudentID.Text != "")
            {
                parent.MyXML.Path = textBoxStudentID.Text + ".xml";
                if (File.Exists(parent.MyXML.Path))
                {
                    parent.T1 = parent.MyXML.Deserialise(parent.T1);
                    parent.QuestionPage = new UserControlQuestionContainer(parent, parent.T1);

                    if (parent.T1.Completed)
                    {
                        
                        if (parent.T1.AllowRetakes)
                        {
                            DialogResult dr = MessageBox.Show("You have completed this exam, Do you wish to retake it?","Retake",MessageBoxButtons.YesNo);
                            if (dr == DialogResult.Yes)
                            {
                                parent.MyXML.Path = "Master.xml";
                                parent.T1 = new Test();
                                parent.T1 = parent.MyXML.Deserialise(parent.T1);
                                parent.MyXML.Path = textBoxStudentID.Text + ".xml";//TODO backup old scores
                                parent.QuestionPage = new UserControlQuestionContainer(parent, parent.T1);
                            }
                            
                        }
                        else
                        {
                            MessageBox.Show("You have completed this exam, viewing results", "Viewing");
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("You have an attempt in progress, resuming", "Resuming");
                    }
                }

                parent.ControlManager.DisplayControl(parent.QuestionPage);
                if (!parent.T1.Completed)
                {
                    parent.ExamTimer.Start();
                }
            }
            else
            {
                MessageBox.Show("Student ID must be entered","Warning");
            }

            
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
