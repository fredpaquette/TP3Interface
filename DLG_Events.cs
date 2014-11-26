﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PasswordKeeper
{
    public partial class DLG_Events : Form
    {
        public Event Event { get; set; }
        private bool blockUpdate;
        public bool delete = false;
        public DLG_Events()
        {
            InitializeComponent();
        }

        private void DLG_Events_Load(object sender, EventArgs e)
        {
            delete = false;
            EventToDLG();

        }

        public static DateTime Klone(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
        }

        private void EventToDLG()
        {
            if (Event != null)
            {
                TBX_Title.Text = Event.Title;
                TBX_Description.Text = Event.Description;
                blockUpdate = true;
                DTP_Date.Value = Klone(Event.Starting);
                DTP_Starting.Value = Klone(Event.Starting);
                DTP_Ending.Value = Klone(Event.Ending);
                blockUpdate = false;
                CB_Type.SelectedIndex = Event.Event_Type;

            }
            else
            {
                Event = new Event();
                CB_Type.SelectedIndex = 0;
            }
                
        }

        private bool ValidateData_Events() //F.L.
        {
            if (string.IsNullOrEmpty(TBX_Title.Text)||
                DateTime.Parse(DTP_Date.Value.Date.ToString()) < DateTime.Parse(DateTime.Now.Date.ToString()) ||
                (DateTime.Parse(DTP_Date.Value.Date.ToString()) == DateTime.Parse(DateTime.Now.Date.ToString())  &&
                    DateTime.Parse(DTP_Starting.Value.TimeOfDay.ToString())<DateTime.Parse(DateTime.Now.TimeOfDay.ToString()))
                || DateTime.Parse(DTP_Starting.Value.TimeOfDay.ToString())>=DateTime.Parse(DTP_Ending.Value.TimeOfDay.ToString()))
                return false;
            
            return true;
        }

        private void TBX_Title_TextChanged(object sender, EventArgs e)
        {
            Event.Title = TBX_Title.Text;
            FB_Ok.Enabled = ValidateData_Events();
        }


        private void TBX_Description_TextChanged(object sender, EventArgs e)
        {
            Event.Description = TBX_Description.Text;
        }

        private void DTP_Date_ValueChanged(object sender, EventArgs e)
        {
            if (!blockUpdate)
            {
                Event.Starting = new DateTime(DTP_Date.Value.Year,
                                                DTP_Date.Value.Month,
                                                DTP_Date.Value.Day,
                                                DTP_Starting.Value.Hour,
                                                DTP_Starting.Value.Minute,
                                                0);

                Event.Ending = new DateTime(DTP_Date.Value.Year,
                                            DTP_Date.Value.Month,
                                            DTP_Date.Value.Day,
                                            DTP_Ending.Value.Hour,
                                            DTP_Ending.Value.Minute,
                                            0);
            }
            FB_Ok.Enabled = ValidateData_Events();
        }

        private void DTP_Starting_ValueChanged(object sender, EventArgs e)
        {
            if (!blockUpdate)
            {
                Event.Starting = new DateTime(DTP_Date.Value.Year,
                                                 DTP_Date.Value.Month,
                                                 DTP_Date.Value.Day,
                                                 DTP_Starting.Value.Hour,
                                                 DTP_Starting.Value.Minute,
                                                 0);
            }
            FB_Ok.Enabled = ValidateData_Events();
        }

        private void DTP_Ending_ValueChanged(object sender, EventArgs e)
        {
            if (!blockUpdate)
            {

                Event.Ending = new DateTime(DTP_Date.Value.Year,
                                            DTP_Date.Value.Month,
                                            DTP_Date.Value.Day,
                                            DTP_Ending.Value.Hour,
                                            DTP_Ending.Value.Minute,
                                            0);
            }
            FB_Ok.Enabled = ValidateData_Events();
        }


        private void FB_Ok_Click(object sender, EventArgs e)
        {

            Event.Event_Type = CB_Type.SelectedIndex;
            Properties.Settings.Default.Save();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }



        private void FB_Abort_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }

        private void CB_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            BT_Couleur.BackColor=Color.FromArgb(Int32.Parse( Properties.Settings.Default.Event_Type_Colors[CB_Type.SelectedIndex].Split(',').ElementAt(0)), Int32.Parse(Properties.Settings.Default.Event_Type_Colors[CB_Type.SelectedIndex].Split(',').ElementAt(1)), Int32.Parse(Properties.Settings.Default.Event_Type_Colors[CB_Type.SelectedIndex].Split(',').ElementAt(2)));
        }

        private void BT_Couleur_Click(object sender, EventArgs e)
        {
            DLG_HLSColorPicker ColorPicker = new DLG_HLSColorPicker();
            ColorPicker.color = BT_Couleur.BackColor;
            if(ColorPicker.ShowDialog()==DialogResult.OK)
            {
                Properties.Settings.Default.Event_Type_Colors[CB_Type.SelectedIndex]=ColorPicker.color.R.ToString()+","+ColorPicker.color.G.ToString()+","+ColorPicker.color.B.ToString();
                BT_Couleur.BackColor = Color.FromArgb(Int32.Parse(Properties.Settings.Default.Event_Type_Colors[CB_Type.SelectedIndex].Split(',').ElementAt(0)), Int32.Parse(Properties.Settings.Default.Event_Type_Colors[CB_Type.SelectedIndex].Split(',').ElementAt(1)), Int32.Parse(Properties.Settings.Default.Event_Type_Colors[CB_Type.SelectedIndex].Split(',').ElementAt(2)));
            }

        }
    }
}
