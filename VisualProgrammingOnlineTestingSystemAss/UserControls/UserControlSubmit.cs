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
    public partial class UserControlSubmit : UserControl
    {
        MainForm parent;
        UserControlQuestionContainer container;

        public UserControlSubmit(MainForm parent, UserControlQuestionContainer container)
        {
            InitializeComponent();
            this.parent = parent;
            this.container = container;

            labelSubmission.Text = "Once Submitted you cannot continue the examination.\nCheck your answers before submission.\nBe aware of the time remaining before mandatory submission.";
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            //Flag Detection
            int flaggedCount = 0;
            for (int i = 0; i < container.Test.Questions.Count; i++)
            {
                if (container.Test.Questions[i].Flagged)
                {
                    flaggedCount++;
                }
            }

            if (flaggedCount > 0)
            {
                DialogResult dialogResult = MessageBox.Show("You have flagged questions, do you still want to submit?", "Alert", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    container.FinishExam();
                }
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("You have time remaining, do you still want to submit?", "Alert", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    container.FinishExam();
                }
            }
        }

        private void buttonFlagReturn_Click(object sender, EventArgs e)
        {
            int flaggedIndex = container.Test.Questions.Count;
            for (int i = 0; i < container.Test.Questions.Count; i++)
            {
                if (container.Test.Questions[i].Flagged)
                {
                    flaggedIndex = i;
                }
            }
            if (flaggedIndex != container.Test.Questions.Count)
            {
                container.Test.Questions[flaggedIndex].ToggleFlag();
                container.SetTreeviewIndex(flaggedIndex);
            }
        }
    }
}
