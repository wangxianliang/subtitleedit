﻿using Nikse.SubtitleEdit.Core;
using Nikse.SubtitleEdit.Core.ContainerFormats.Matroska;
using Nikse.SubtitleEdit.Core.ContainerFormats.Mp4.Boxes;
using Nikse.SubtitleEdit.Logic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace Nikse.SubtitleEdit.Forms
{
    public sealed partial class MatroskaSubtitleChooser : Form
    {
        public MatroskaSubtitleChooser(string fileType)
        {
            UiUtil.PreInitialize(this);
            InitializeComponent();
            UiUtil.FixFonts(this);

            Text = fileType.Equals("mp4", StringComparison.Ordinal) ? Configuration.Settings.Language.MatroskaSubtitleChooser.TitleMp4 : Configuration.Settings.Language.MatroskaSubtitleChooser.Title;
            labelChoose.Text = Configuration.Settings.Language.MatroskaSubtitleChooser.PleaseChoose;
            buttonOK.Text = Configuration.Settings.Language.General.Ok;
            buttonCancel.Text = Configuration.Settings.Language.General.Cancel;
            UiUtil.FixLargeFonts(this, buttonOK);
        }

        public int SelectedIndex { get; private set; }

        internal void Initialize(List<MatroskaTrackInfo> subtitleInfoList)
        {
            listBox1.Visible = false;
            listView1.Visible = true;
            var format = Configuration.Settings.Language.MatroskaSubtitleChooser.TrackXLanguageYTypeZ;
            foreach (var info in subtitleInfoList)
            {
                var track = string.Format((!string.IsNullOrWhiteSpace(info.Name) ? "{0} - {1}" : "{0}"), info.TrackNumber, info.Name);
                listBox1.Items.Add(string.Format(format, track, info.Language, info.CodecId));
                AddListViewItem(info);
            }
            listBox1.SelectedIndex = 0;
            listView1.Items[0].Selected = true;
            listView1.FocusedItem = listView1.Items[0];
        }

        private void AddListViewItem(MatroskaTrackInfo info)
        {
            var item = new ListViewItem(info.TrackNumber.ToString());
            item.SubItems.Add(info.Name);
            item.SubItems.Add(info.Language);
            item.SubItems.Add(info.CodecId);
            item.SubItems.Add(info.IsDefault.ToString(CultureInfo.InvariantCulture));
            item.SubItems.Add(info.IsForced.ToString(CultureInfo.InvariantCulture));
            listView1.Items.Add(item);
        }

        internal void Initialize(List<Trak> mp4SubtitleTracks)
        {
            listBox1.Visible = true;
            listView1.Visible = false;
            int i = 0;
            foreach (var track in mp4SubtitleTracks)
            {
                i++;
                string handler = (track.Mdia.HandlerType + " " + track.Mdia.HandlerName).Trim();
                if (handler.Length > 1)
                {
                    handler = " - " + handler;
                }

                string s = $"{i}: {track.Mdia.Mdhd.Iso639ThreeLetterCode} - {track.Mdia.Mdhd.LanguageString}{handler}";
                listBox1.Items.Add(s);
            }
            listBox1.SelectedIndex = 0;
        }

        private void FormMatroskaSubtitleChooser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                SelectedIndex = listView1.SelectedItems[0].Index;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedIndex = listBox1.SelectedIndex;
        }
    }
}
