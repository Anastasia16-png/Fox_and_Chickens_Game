using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fox_and_Chickens_Game
{
    public partial class Form1 : Form
    {
        private Form2_game form2; // змінна для збереження посилання на другу форму
        private Form1 form1;
        private string selectedLanguage = "Українська";

        public Form1()
        {
            InitializeComponent();
            form1 = this;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            SetLanguage(selectedLanguage);
        }
        //почати гру та перейти на форму гри
        private void button_Start_Click(object sender, EventArgs e)
        {
            form2 = new Form2_game(); // нова форма для гри

            form2.FormClosed += Form2_game_FormClosed; // обробник події закриття форми 2

            form2.Show(); // показати форму

            this.Hide(); // приховати дану

        }
        private void Form2_game_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show(); // відображається форма1 при закритті форми2
        }
        private void button_Exit_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
        private void button_Settings_Click(object sender, EventArgs e)
        {
            Form_Settings formS = new Form_Settings();
            DialogResult result = formS.ShowDialog();
       
            if (result == DialogResult.OK)
            {
                string selectedLanguage = formS.SelectedLanguage;

                formS.SetLanguage(selectedLanguage);
                form1.SetLanguage(selectedLanguage);
            } 
        }
        //мова
        public void SetLanguage(string language)
        {
            if (language == "Українська")
            {
                button_Start.Text = "Почати гру";
                button_Exit.Text = "Вихід";
                button_Settings.Text = "Налаштування";
                RulesButton.Text = "Правила гри";
            }
            else if (language == "English")
            {
                button_Start.Text = "Start Game";
                button_Exit.Text = "Exit";
                button_Settings.Text = "Settings";
                RulesButton.Text = "Rules of the game";
            }
        }
        // форма з правилами
        private void RulesButton_Click(object sender, EventArgs e)
        {
            Rules rules = new Rules();
            rules.FormClosed += Rules_FormClosed; 

            rules.Show(); 

            this.Hide(); 
        }
        private void Rules_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show(); 
        }
    }
}
