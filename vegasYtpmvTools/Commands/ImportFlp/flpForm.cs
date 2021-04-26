using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using logger;
using Monad.FLParser;
using ScriptPortal.Vegas;

namespace vegasYtpmvTools
{
    //laziness pro tip: switch comments to enable designer
    //class FlpForm : Form
    class FlpForm : DockableControl
    {
        public EventHandler<ImportEventArgs> OnImport;

        private Panel panel1;
        private Button importBtn;
        private Label label4;
        private Label label3;
        private NumericUpDown frameOffset;
        private Label label2;
        private ComboBox importType;
        private ListView tracks;
        private ColumnHeader track;
        private ColumnHeader items;
        private List<int> indeciesWithItems = new List<int>();

        //public FlpForm(Monad.FLParser.Project selectedFlp)
        public FlpForm(string instanceName, Monad.FLParser.Project selectedFlp) : base(instanceName)
        {
            this.DefaultFloatingSize = new System.Drawing.Size(273, 462);
            this.InitializeComponent();
            
            this.label2.Text = "BPM: " + selectedFlp.Tempo;
            for (var i = 0; i < selectedFlp.Tracks.Length; i++)
            {
                var track = selectedFlp.Tracks[i];
                var trackName = track.Name;

                if (selectedFlp.Tracks[i].Items.Count > 0)
                {
                    indeciesWithItems.Add(i);
                }
                else
                    continue;

                if (trackName == null)
                {
                    trackName = String.Format("Track {0}", (i + 1));
                    if (track.Items.Count > 0)
                    {
                        
                        if(track.Items[0] is ChannelPlaylistItem)
                        {
                            trackName = (track.Items[0] as ChannelPlaylistItem).Channel.Name;
                        }
                        else if (track.Items[0] is PatternPlaylistItem)
                        {
                            Pattern pattern = (track.Items[0] as PatternPlaylistItem).Pattern;
                            if (pattern.Notes.Keys.Count > 0)
                                trackName = pattern.Notes.Keys.First().Name;
                            else
                                trackName = pattern.Name;
                        }
                    }
                }

                var listViewItem = new ListViewItem(new string[] { trackName, track.Items.Count.ToString() });

                tracks.Items.Add(listViewItem);
            }

        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.importBtn = new System.Windows.Forms.Button();
            this.importType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.frameOffset = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.tracks = new System.Windows.Forms.ListView();
            this.track = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.items = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frameOffset)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.importBtn);
            this.panel1.Controls.Add(this.importType);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.frameOffset);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 301);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(252, 122);
            this.panel1.TabIndex = 3;
            // 
            // importBtn
            // 
            this.importBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.importBtn.Location = new System.Drawing.Point(164, 87);
            this.importBtn.Name = "importBtn";
            this.importBtn.Size = new System.Drawing.Size(75, 23);
            this.importBtn.TabIndex = 3;
            this.importBtn.Text = "Import";
            this.importBtn.UseVisualStyleBackColor = true;
            this.importBtn.Click += new System.EventHandler(this.importBtn_Click);
            // 
            // importType
            // 
            this.importType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.importType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.importType.FormattingEnabled = true;
            this.importType.Items.AddRange(new object[] {
            "Solid color",
            "Marker"});
            this.importType.Location = new System.Drawing.Point(118, 60);
            this.importType.Name = "importType";
            this.importType.Size = new System.Drawing.Size(121, 21);
            this.importType.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Import Type:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Offset:";
            // 
            // frameOffset
            // 
            this.frameOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.frameOffset.Location = new System.Drawing.Point(181, 34);
            this.frameOffset.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.frameOffset.Name = "frameOffset";
            this.frameOffset.Size = new System.Drawing.Size(58, 20);
            this.frameOffset.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "BPM: ";
            // 
            // tracks
            // 
            this.tracks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.track,
            this.items});
            this.tracks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tracks.FullRowSelect = true;
            this.tracks.GridLines = true;
            this.tracks.HideSelection = false;
            this.tracks.Location = new System.Drawing.Point(0, 0);
            this.tracks.Name = "tracks";
            this.tracks.Size = new System.Drawing.Size(252, 301);
            this.tracks.TabIndex = 0;
            this.tracks.UseCompatibleStateImageBehavior = false;
            this.tracks.View = System.Windows.Forms.View.Details;
            // 
            // track
            // 
            this.track.Text = "Track";
            this.track.Width = 195;
            // 
            // items
            // 
            this.items.Text = "Items";
            this.items.Width = 45;
            // 
            // FlpForm
            // 
            this.ClientSize = new System.Drawing.Size(252, 423);
            this.Controls.Add(this.tracks);
            this.Controls.Add(this.panel1);
            this.Name = "FlpForm";
            this.Text = "Select Tracks to Import";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frameOffset)).EndInit();
            this.ResumeLayout(false);

        }

        private void importBtn_Click(object sender, EventArgs e)
        {
            int[] selectedTrackIds = new int[tracks.SelectedIndices.Count];

            for(int i=0; i<tracks.SelectedIndices.Count; i++)
            {
                selectedTrackIds[i] = indeciesWithItems[tracks.SelectedIndices[i]];
            }

            this.OnImport?.Invoke(this, new ImportEventArgs()
            {
                FrameOffset = (int)frameOffset.Value,
                SelectedIndices = selectedTrackIds,
                ImportType = importType.SelectedItem.ToString()
            });
        }
    }

    class ImportEventArgs : EventArgs
    {
        public string ImportType;
        public int[] SelectedIndices;
        public int FrameOffset;
    }
}
