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
    public partial class Form_Settings : Form
    {
        public string SelectedLanguage { get; private set; }

        public Form_Settings()
        {
            InitializeComponent();

            comboBox_language.Items.Add("Українська");
            comboBox_language.Items.Add("English");

            comboBox_language.SelectedIndex = 0;
            comboBox_language.SelectedIndexChanged += comboBox_language_SelectedIndexChanged; // додано подію SelectedIndexChanged
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            SelectedLanguage = comboBox_language.SelectedItem.ToString();

            DialogResult = DialogResult.OK;

        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public void SetLanguage(string language)
        {
            if (language == "Українська")
            {
                button_Save.Text = "Зберегти";
                button_Cancel.Text = "Скасувати";
                label1.Text = "Мова";
            }
            else if (language == "English")
            {
                button_Save.Text = "Save";
                button_Cancel.Text = "Cancel";
                label1.Text = "Language";
            }
        }
        private void comboBox_language_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedLanguage = comboBox_language.SelectedItem.ToString();
            SetLanguage(SelectedLanguage); // виклик методу SetLanguage з обраною мовою
        }
    }
}
