using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinMine
{
    public partial class Options : Form
    {
        public int _rows = Properties.Settings.Default.rows;
        public int _cols = Properties.Settings.Default.cols;
        public int _mines = Properties.Settings.Default.mines;
        private bool easyChecked, MediumChecked, HardChecked, customChecked;
        public Options()
        {
            InitializeComponent();
            rowTextBox.Text = _rows.ToString();
            colTextBox.Text = _cols.ToString();
            mineTextBox.Text = _mines.ToString();
            easyChecked = easyRadioButton.Checked;
            MediumChecked = mediumRadioButton.Checked;
            HardChecked = hardRadioButton.Checked;
            customChecked = customRadioButton.Checked;
        }
        private void OK_Click(object sender, EventArgs e)
        {
            if (easyRadioButton.Checked)
            {
                easyChecked = true;
                MediumChecked = false;
                HardChecked = false;
                customChecked = false;
                _rows = 9;
                _cols = 9;
                _mines = 10;
                rowTextBox.Text = _rows.ToString();
                colTextBox.Text = _cols.ToString();
                mineTextBox.Text = _mines.ToString();
            }
            else if (mediumRadioButton.Checked)
            {
                easyChecked = false;
                MediumChecked = true;
                HardChecked = false;
                customChecked = false;
                _rows = 16;
                _cols = 16;
                _mines = 40;
                rowTextBox.Text = _rows.ToString();
                colTextBox.Text = _cols.ToString();
                mineTextBox.Text = _mines.ToString();
            }
            else if (hardRadioButton.Checked)
            {
                easyChecked = false;
                MediumChecked = false;
                HardChecked = true;
                customChecked = false;
                _rows = 16;
                _cols = 30;
                _mines = 99;
                rowTextBox.Text = _rows.ToString();
                colTextBox.Text = _cols.ToString();
                mineTextBox.Text = _mines.ToString();
            }
            else if (customRadioButton.Checked)
            {
                try
                {
                    easyChecked = false;
                    MediumChecked = false;
                    HardChecked = false;
                    customChecked = true;
                    if (Convert.ToInt32(rowTextBox.Text) < 31 && Convert.ToInt32(colTextBox.Text) < 51)
                    {
                        _rows = Convert.ToInt32(rowTextBox.Text);
                        _cols = Convert.ToInt32(colTextBox.Text);
                        _mines = Convert.ToInt32(mineTextBox.Text);
                    }
                }
                catch
                {
                }
            }
            Properties.Settings.Default.rows = _rows;
            Properties.Settings.Default.cols = _cols;
            Properties.Settings.Default.mines = _mines;
            Properties.Settings.Default.Save();
            this.Close();
        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            easyRadioButton.Checked = easyChecked;
            mediumRadioButton.Checked = MediumChecked;
            hardRadioButton.Checked = HardChecked;
            customRadioButton.Checked = customChecked;
            rowTextBox.Text = _rows.ToString();
            colTextBox.Text = _cols.ToString();
            mineTextBox.Text = _mines.ToString();
        }
        private void rowTextBox_Enter(object sender, EventArgs e)
        {
            customRadioButton.Checked = true;
        }
        private void colTextBox_Enter(object sender, EventArgs e)
        {
            customRadioButton.Checked = true;
        }
        private void mineTextBox_Enter(object sender, EventArgs e)
        {
            customRadioButton.Checked = true;
        }
        private void easyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (easyRadioButton.Checked)
            {
                rowTextBox.Text = "9";
                colTextBox.Text = "9";
                mineTextBox.Text = "10";
            }
        }
        private void mediumRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (mediumRadioButton.Checked)
            {
                rowTextBox.Text = "16";
                colTextBox.Text = "16";
                mineTextBox.Text = "40";
            }
        }
        private void hardRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (hardRadioButton.Checked)
            {
                rowTextBox.Text = "16";
                colTextBox.Text = "30";
                mineTextBox.Text = "99";
            }

        }
        private void customRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (customRadioButton.Checked)
            {
                rowTextBox.Text = "";
                colTextBox.Text = "";
                mineTextBox.Text = "";
            }
        }
    }
}