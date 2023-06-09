﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BLL.Registries;
using BLL.Objects.Users;
using DAL.Repositories;
using DAL;
using System.Security.Authentication;

namespace WinApp.Forms
{
    public partial class LoginForm : Form
    {
        private LoginHandler loginHandler;
        private MainForm? mainForm;

        public LoginForm()
        {
            InitializeComponent();
            loginHandler = new LoginHandler(new LoginRepository(new DbContext()));
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Credentials creds = new Credentials()
            {
                Email = inputEmail.Text,
                Password = inputPassword.Text
            };
            try
            {
                Account account = loginHandler.AuthenticateForm(creds);
                mainForm ??= new MainForm(account);
                mainForm.FormClosing += MainForm_FormClosing;
                mainForm.VisibleChanged += MainForm_VisibleChanged;
                mainForm.Show();
                this.Hide();
            }
            catch(AuthenticationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "An unknown error has occured");
            }
        }

        private void MainForm_VisibleChanged(object? sender, EventArgs e)
        {
            if (!mainForm!.Visible)
            {
                mainForm.FormClosing -= MainForm_FormClosing;
                mainForm.VisibleChanged -= MainForm_VisibleChanged;
                mainForm = null;
                this.Show();
                inputEmail.Clear();
                inputPassword.Clear();
            }
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing) this.Close();
        }
    }
}
