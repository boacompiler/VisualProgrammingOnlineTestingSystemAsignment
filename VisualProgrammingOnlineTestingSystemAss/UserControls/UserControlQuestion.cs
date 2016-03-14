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
    public partial class UserControlQuestion : UserControl
    {
        Question question;
        int currentlyChecked;

        public Question Question
        {
            get
            {
                return question;
            }

            set
            {
                question = value;
            }
        }

        public UserControlQuestion(Question question)
        {
            InitializeComponent();
            this.question = question;

            labelQuestionText.Text = question.QuestionText +"\n\nMarks Available: "+ question.MarksAvailable ;

            if (question.MultipleChoice)
            {
                labelQuestionText.Text = labelQuestionText.Text + "\n\n(Choose " + question.NoChoicesRequired + ")";
            }

            this.Resize += delegate (object o, EventArgs ea)
            {
                labelQuestionText.MaximumSize = groupBox1.Size;
            };

            SetupButtons();

        }

        void SetupButtons()
        {
            if (question.MultipleChoice)
            {
                if (question.NoChoicesRequired > 1)
                {
                    for (int i = 0; i < question.QuestionChoices.Count; i++)
                    {
                        CheckBox cb = new CheckBox();
                        cb.Text = question.QuestionChoices[i].ChoiceText;
                        cb.AutoSize = true;
                        tableLayoutPanelChoices.Controls.Add(cb);

                        if (question.StudentChoices.Count > 0)
                        {
                            for (int x = 0; x < question.StudentChoices.Count; x++)
                            {
                                if (cb.Text == question.StudentChoices[x].ChoiceText)
                                {
                                    cb.Checked = true;
                                }
                            }
                        }

                        #region CheckboxDelegate
                        //this is needed to prevent submitting all choices checked and getting everything correct
                        cb.CheckedChanged += delegate (object o, EventArgs ea)
                        {
                            currentlyChecked = 0;
                            for (int x = 0; x < tableLayoutPanelChoices.Controls.Count; x++)
                            {
                                CheckBox cb1 = (CheckBox)tableLayoutPanelChoices.Controls[x];
                                if (cb1.Checked)
                                {
                                    currentlyChecked++;
                                }
                            }

                            if (currentlyChecked > question.NoChoicesRequired)
                            {
                                for (int x = 0; x < tableLayoutPanelChoices.Controls.Count; x++)
                                {
                                    CheckBox cb1 = (CheckBox)tableLayoutPanelChoices.Controls[x];
                                    if (cb1.Checked && currentlyChecked > question.NoChoicesRequired && cb1 != cb)
                                    {
                                        cb1.Checked = false;
                                        currentlyChecked--;
                                    }
                                }
                            }
                            SubmitAnswer();
                        };
                        #endregion
                        
                    }
                }
                else
                {
                    for (int i = 0; i < question.QuestionChoices.Count; i++)
                    {
                        RadioButton rb = new RadioButton();
                        rb.Text = question.QuestionChoices[i].ChoiceText;
                        rb.AutoSize = true;
                        tableLayoutPanelChoices.Controls.Add(rb);

                        if (question.StudentChoices.Count > 0)
                        {
                            rb.Checked = (rb.Text == question.StudentChoices[0].ChoiceText);
                        }

                        rb.CheckedChanged += delegate (object o, EventArgs ea)
                        {
                            SubmitAnswer();
                        };
                    }
                }
            }
            else
            {
                TextBox tb = new TextBox();
                tb.Dock = DockStyle.Fill;                
                tableLayoutPanelChoices.Controls.Add(tb);
                tableLayoutPanelChoices.SetColumnSpan(tb,2);

                if (question.StudentChoices.Count > 0)
                {
                    tb.Text = question.StudentChoices[0].ChoiceText;
                }

                tb.TextChanged += delegate (object o, EventArgs ea)
                {
                    SubmitAnswer();
                };
     
            }
        }

        public void SubmitAnswer()
        {
            if (question.MultipleChoice)
            {
                List<QuestionChoice> studentAnswerChoices = new List<QuestionChoice>();
                for (int i = 0; i < tableLayoutPanelChoices.Controls.Count; i++)
                {
                    if (question.NoChoicesRequired > 1)
                    {
                        CheckBox cb = (CheckBox)tableLayoutPanelChoices.Controls[i];
                        if (cb.Checked)
                        {
                            studentAnswerChoices.Add(question.QuestionChoices[i]);
                        }
                    }
                    else
                    {
                        RadioButton rb = (RadioButton)tableLayoutPanelChoices.Controls[i];
                        if (rb.Checked)
                        {
                            studentAnswerChoices.Add(question.QuestionChoices[i]);
                        }
                    }
                    
                }

                question.submitAnswer(studentAnswerChoices);
            }
            else
            {
                question.submitAnswer(tableLayoutPanelChoices.Controls[0].Text);
            }
        }
    }
}
