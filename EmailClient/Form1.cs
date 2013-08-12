using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace EmailClient
{
    public partial class Form1 : Form
    {
        private string[] smtpUrl = { "smtp.gmail.com", "smtp.live.com" };
        private int[] portNo = { 587, 587 };
        int smtpObject;
        public Form1()
        {
            InitializeComponent();
        }

        private void send_Click(object sender, EventArgs e)
        {
            smtpObject = comboBox1.SelectedIndex;
            if (!validate())
                return;
            try
            {
                send.Enabled = false;
                progressBar.Visible = true;
                progressBar.Value = 0;
                UseWaitCursor = true;

                MailMessage message = new MailMessage();
                message.From = new MailAddress(email.Text);
                message.Subject = subject.Text;
                message.Body = textMessage.Text;
                progressBar.Value += 10;
                foreach (string s in recipients.Text.Split(';'))
                {
                    message.To.Add(s);
                }
                progressBar.Value += 10;
                SmtpClient client = new SmtpClient();
                progressBar.Value += 10;
                client.Credentials = new NetworkCredential(email.Text, password.Text);
                progressBar.Value += 10;
                client.Host = smtpUrl[smtpObject];
                progressBar.Value += 10;
                client.Port = portNo[smtpObject];
                client.EnableSsl = true;
                progressBar.Value += 10;
                client.Send(message);
                progressBar.Value = 100;
                MessageBox.Show("Your Email was sent successully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                progressBar.Visible = false;
            }
            catch (Exception e1)
            {
                MessageBox.Show("Incorrect Credentials (or)\r\nMake sure your internet connection is working", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                send.Enabled = true;
                UseWaitCursor = false;
                progressBar.Value = 0;
                progressBar.Visible = false;
            }
        }

        private bool validate()
        {
            if (!email.Text.Contains("@gmail.com") && !email.Text.Contains("@live."))
            {
                MessageBox.Show("Enter a valid " + comboBox1.SelectedItem + " address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (recipients.Text.Trim() == "" || password.Text.Trim() == "")
            {
                MessageBox.Show("Enter atleast 1 Recipient", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (subject.Text.Trim() == "")
            {
                if (MessageBox.Show("The Subject is empty. Continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return false;
            }
            if (textMessage.Text.Trim() == "")
            {
                if (MessageBox.Show("The Body is empty. Continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return false;
            }
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Select an Email Provider", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (MessageBox.Show("Are You Sure", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return false;
            return true;
        }
    }
}