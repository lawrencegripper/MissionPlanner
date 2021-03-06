﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Windows.Forms;

namespace MissionPlanner.Utilities.AltitudeAngel
{
    public partial class AASettings : Form
    {
        public AASettings()
        {
            InitializeComponent();

            ThemeManager.ApplyThemeTo(this);

            // load settings
            chk_grounddata.Checked = Utilities.AltitudeAngel.AltitudeAngel.service.GroundDataDisplay;
            chk_airdata.Checked = Utilities.AltitudeAngel.AltitudeAngel.service.AirDataDisplay;

            foreach (var item in AltitudeAngelWings.ApiClient.Client.Extensions.FiltersSeen)
            {
                if (AltitudeAngel.service.FilteredOut.Contains(item))
                    chklb_layers.Items.Add(item, false);
                else
                    chklb_layers.Items.Add(item, true);
            }
        }

        private void but_enable_Click(object sender, EventArgs e)
        {
            if (Utilities.AltitudeAngel.AltitudeAngel.service.IsSignedIn)
            {
                CustomMessageBox.Show("You are already signed in", "AltitudeAngel");
                return;
            }

            Utilities.AltitudeAngel.AltitudeAngel.service.SignInAsync();
        }

        private void but_disable_Click(object sender, EventArgs e)
        {
            Utilities.AltitudeAngel.AltitudeAngel.service.DisconnectAsync();
        }

        private void chk_airdata_CheckedChanged(object sender, EventArgs e)
        {
            Utilities.AltitudeAngel.AltitudeAngel.service.AirDataDisplay = chk_airdata.Checked;
        }

        private void chk_grounddata_CheckedChanged(object sender, EventArgs e)
        {
            Utilities.AltitudeAngel.AltitudeAngel.service.GroundDataDisplay = chk_grounddata.Checked;
        }

        private async void chklb_layers_SelectedIndexChanged(object sender, EventArgs e)
        {
            AltitudeAngel.service.FilteredOut.Clear();

            foreach (string item in chklb_layers.Items)
            {
                if (!chklb_layers.CheckedItems.Contains(item))
                {
                    AltitudeAngel.service.FilteredOut.Add(item);
                }
            }

            // reset state on change
            AltitudeAngel.service.ProcessAllFromCache(AltitudeAngel.MP.FlightDataMap);
        }
    }
}
