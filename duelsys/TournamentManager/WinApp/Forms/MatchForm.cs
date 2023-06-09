﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BLL.Objects.Sports;
using BLL.Objects;

namespace WinApp.Forms
{
    public partial class MatchForm : Form
    {
        private readonly ISport sport;
        public Match CurrentMatch { get; private set; }

        public MatchForm(ISport sport, Match match)
        {
            InitializeComponent();
            this.sport = sport;
            CurrentMatch = match;
            numHomeScore.Maximum = sport.MaxScore;
            numAwayScore.Maximum = sport.MaxScore;
            SetupForm();
        }

        private void SetupForm()
        {
            lblHomeName.Text = CurrentMatch.HomeName;
            numHomeScore.Value = CurrentMatch.HomeScore;
            lblAwayName.Text = CurrentMatch.AwayName;
            numAwayScore.Value = CurrentMatch.AwayScore;
            lblScoring.Text = sport.Scoring;
        }

        private void btnSaveMatchResults_Click(object sender, EventArgs e)
        {
            try
            {
                if (sport.ScoreIsValid(numHomeScore.Value, numAwayScore.Value))
                {
                    CurrentMatch.HomeScore = Convert.ToInt32(numHomeScore.Value);
                    CurrentMatch.AwayScore = Convert.ToInt32(numAwayScore.Value);
                    CurrentMatch.IsFinished = true;
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show($"The entered score is not in accordance with the rules of {sport.Name}.");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter numeric-only characters in the score fields.");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "An unknown error occured:");
            }
        }
    }
}
