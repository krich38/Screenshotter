namespace ScreenShifter
{
    partial class FormPreview
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPreview));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonUndo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripComboBoxShapeType = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripDropDownButtonEdge = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItemEdgeEnabled = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemEdgeFillColour = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSetAlpha = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBoxEdgeAlpha = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripMenuItemEdgeWidth = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBoxEdgeWidth = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripDropDownButtonFill = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItemFillEnabled = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFillColour = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFillAlpha = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBoxFillAlpha = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonUpload = new System.Windows.Forms.ToolStripButton();
            this.panelImage = new System.Windows.Forms.Panel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.toolStrip.SuspendLayout();
            this.panelImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonUndo,
            this.toolStripSeparator1,
            this.toolStripComboBoxShapeType,
            this.toolStripDropDownButtonEdge,
            this.toolStripDropDownButtonFill,
            this.toolStripSeparator2,
            this.toolStripButtonUpload});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(599, 25);
            this.toolStrip.TabIndex = 6;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripButtonUndo
            // 
            this.toolStripButtonUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonUndo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonUndo.Image")));
            this.toolStripButtonUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUndo.Name = "toolStripButtonUndo";
            this.toolStripButtonUndo.Size = new System.Drawing.Size(40, 22);
            this.toolStripButtonUndo.Text = "Undo";
            this.toolStripButtonUndo.Click += new System.EventHandler(this.toolStripButtonUndo_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripComboBoxShapeType
            // 
            this.toolStripComboBoxShapeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxShapeType.Name = "toolStripComboBoxShapeType";
            this.toolStripComboBoxShapeType.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripDropDownButtonEdge
            // 
            this.toolStripDropDownButtonEdge.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemEdgeEnabled,
            this.toolStripMenuItemEdgeFillColour,
            this.toolStripMenuItemSetAlpha,
            this.toolStripMenuItemEdgeWidth});
            this.toolStripDropDownButtonEdge.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonEdge.Image")));
            this.toolStripDropDownButtonEdge.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonEdge.Name = "toolStripDropDownButtonEdge";
            this.toolStripDropDownButtonEdge.Size = new System.Drawing.Size(62, 22);
            this.toolStripDropDownButtonEdge.Text = "Edge";
            // 
            // toolStripMenuItemEdgeEnabled
            // 
            this.toolStripMenuItemEdgeEnabled.Checked = true;
            this.toolStripMenuItemEdgeEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItemEdgeEnabled.Name = "toolStripMenuItemEdgeEnabled";
            this.toolStripMenuItemEdgeEnabled.Size = new System.Drawing.Size(129, 22);
            this.toolStripMenuItemEdgeEnabled.Text = "Enabled";
            this.toolStripMenuItemEdgeEnabled.Click += new System.EventHandler(this.toolStripMenuItemEdgeFill_Click);
            // 
            // toolStripMenuItemEdgeFillColour
            // 
            this.toolStripMenuItemEdgeFillColour.Name = "toolStripMenuItemEdgeFillColour";
            this.toolStripMenuItemEdgeFillColour.Size = new System.Drawing.Size(129, 22);
            this.toolStripMenuItemEdgeFillColour.Text = "Set Colour";
            this.toolStripMenuItemEdgeFillColour.Click += new System.EventHandler(this.toolStripMenuItemEdgeFillColour_Click);
            // 
            // toolStripMenuItemSetAlpha
            // 
            this.toolStripMenuItemSetAlpha.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBoxEdgeAlpha});
            this.toolStripMenuItemSetAlpha.Name = "toolStripMenuItemSetAlpha";
            this.toolStripMenuItemSetAlpha.Size = new System.Drawing.Size(129, 22);
            this.toolStripMenuItemSetAlpha.Text = "Alpha";
            // 
            // toolStripTextBoxEdgeAlpha
            // 
            this.toolStripTextBoxEdgeAlpha.Name = "toolStripTextBoxEdgeAlpha";
            this.toolStripTextBoxEdgeAlpha.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBoxEdgeAlpha.Text = "255";
            // 
            // toolStripMenuItemEdgeWidth
            // 
            this.toolStripMenuItemEdgeWidth.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBoxEdgeWidth});
            this.toolStripMenuItemEdgeWidth.Name = "toolStripMenuItemEdgeWidth";
            this.toolStripMenuItemEdgeWidth.Size = new System.Drawing.Size(129, 22);
            this.toolStripMenuItemEdgeWidth.Text = "Width";
            // 
            // toolStripTextBoxEdgeWidth
            // 
            this.toolStripTextBoxEdgeWidth.Name = "toolStripTextBoxEdgeWidth";
            this.toolStripTextBoxEdgeWidth.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBoxEdgeWidth.Text = "3";
            // 
            // toolStripDropDownButtonFill
            // 
            this.toolStripDropDownButtonFill.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFillEnabled,
            this.toolStripMenuItemFillColour,
            this.toolStripMenuItemFillAlpha});
            this.toolStripDropDownButtonFill.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonFill.Image")));
            this.toolStripDropDownButtonFill.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonFill.Name = "toolStripDropDownButtonFill";
            this.toolStripDropDownButtonFill.Size = new System.Drawing.Size(51, 22);
            this.toolStripDropDownButtonFill.Text = "Fill";
            // 
            // toolStripMenuItemFillEnabled
            // 
            this.toolStripMenuItemFillEnabled.Name = "toolStripMenuItemFillEnabled";
            this.toolStripMenuItemFillEnabled.Size = new System.Drawing.Size(129, 22);
            this.toolStripMenuItemFillEnabled.Text = "Enabled";
            this.toolStripMenuItemFillEnabled.Click += new System.EventHandler(this.toolStripMenuItemFillEnabled_Click);
            // 
            // toolStripMenuItemFillColour
            // 
            this.toolStripMenuItemFillColour.Name = "toolStripMenuItemFillColour";
            this.toolStripMenuItemFillColour.Size = new System.Drawing.Size(129, 22);
            this.toolStripMenuItemFillColour.Text = "Set Colour";
            this.toolStripMenuItemFillColour.Click += new System.EventHandler(this.toolStripMenuItemFillColour_Click);
            // 
            // toolStripMenuItemFillAlpha
            // 
            this.toolStripMenuItemFillAlpha.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBoxFillAlpha});
            this.toolStripMenuItemFillAlpha.Name = "toolStripMenuItemFillAlpha";
            this.toolStripMenuItemFillAlpha.Size = new System.Drawing.Size(129, 22);
            this.toolStripMenuItemFillAlpha.Text = "Alpha";
            // 
            // toolStripTextBoxFillAlpha
            // 
            this.toolStripTextBoxFillAlpha.Name = "toolStripTextBoxFillAlpha";
            this.toolStripTextBoxFillAlpha.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBoxFillAlpha.Text = "50";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonUpload
            // 
            this.toolStripButtonUpload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonUpload.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonUpload.Image")));
            this.toolStripButtonUpload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUpload.Name = "toolStripButtonUpload";
            this.toolStripButtonUpload.Size = new System.Drawing.Size(49, 22);
            this.toolStripButtonUpload.Text = "Upload";
            this.toolStripButtonUpload.Click += new System.EventHandler(this.toolStripButtonUpload_Click);
            // 
            // panelImage
            // 
            this.panelImage.AutoScroll = true;
            this.panelImage.Controls.Add(this.pictureBox);
            this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImage.Location = new System.Drawing.Point(0, 25);
            this.panelImage.Name = "panelImage";
            this.panelImage.Size = new System.Drawing.Size(599, 492);
            this.panelImage.TabIndex = 7;
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(339, 313);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 2;
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
            // 
            // FormPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 517);
            this.Controls.Add(this.panelImage);
            this.Controls.Add(this.toolStrip);
            this.Name = "FormPreview";
            this.Text = "FormPreview";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormPreview_FormClosed);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.panelImage.ResumeLayout(false);
            this.panelImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.Panel panelImage;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ToolStripButton toolStripButtonUndo;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxShapeType;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonEdge;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEdgeEnabled;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEdgeFillColour;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEdgeWidth;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSetAlpha;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxEdgeAlpha;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxEdgeWidth;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonFill;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFillEnabled;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFillColour;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFillAlpha;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxFillAlpha;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonUpload;




    }
}