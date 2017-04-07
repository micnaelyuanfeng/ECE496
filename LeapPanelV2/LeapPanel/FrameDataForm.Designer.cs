namespace LeapPanel
{
    partial class Form1
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
            System.Windows.Forms.Label label5;
            this.ServerIPTextbox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.InstructionPort = new System.Windows.Forms.TextBox();
            this.palmRoll = new System.Windows.Forms.RichTextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.x_displacement = new System.Windows.Forms.RichTextBox();
            this.z_displacement = new System.Windows.Forms.RichTextBox();
            this.y_displacement = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.right_key = new System.Windows.Forms.TextBox();
            this.down_key = new System.Windows.Forms.TextBox();
            this.left_key = new System.Windows.Forms.TextBox();
            this.up_key = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.VideoPort = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.grapVal = new System.Windows.Forms.RichTextBox();
            this.stop_key = new System.Windows.Forms.TextBox();
            this.label37 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.whichHand = new System.Windows.Forms.RichTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.baseServoText = new System.Windows.Forms.RichTextBox();
            this.baseServoTransText = new System.Windows.Forms.RichTextBox();
            this.shoulderServoTransText = new System.Windows.Forms.RichTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.shoulderServoText = new System.Windows.Forms.RichTextBox();
            this.elbowServoTransText = new System.Windows.Forms.RichTextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.elbowServoText = new System.Windows.Forms.RichTextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.M1Instruction = new System.Windows.Forms.RichTextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.M2Instruction = new System.Windows.Forms.RichTextBox();
            this.testBox = new System.Windows.Forms.RichTextBox();
            label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.CausesValidation = false;
            label5.Enabled = false;
            label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label5.Location = new System.Drawing.Point(291, 14);
            label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(146, 22);
            label5.TabIndex = 28;
            label5.Text = "Instruction Port";
            // 
            // ServerIPTextbox
            // 
            this.ServerIPTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.ServerIPTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerIPTextbox.Location = new System.Drawing.Point(40, 54);
            this.ServerIPTextbox.Margin = new System.Windows.Forms.Padding(2);
            this.ServerIPTextbox.Multiline = true;
            this.ServerIPTextbox.Name = "ServerIPTextbox";
            this.ServerIPTextbox.Size = new System.Drawing.Size(202, 39);
            this.ServerIPTextbox.TabIndex = 26;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(36, 14);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(172, 22);
            this.label4.TabIndex = 27;
            this.label4.Text = "Server IP Address";
            // 
            // InstructionPort
            // 
            this.InstructionPort.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.InstructionPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InstructionPort.ForeColor = System.Drawing.SystemColors.Desktop;
            this.InstructionPort.Location = new System.Drawing.Point(295, 54);
            this.InstructionPort.Margin = new System.Windows.Forms.Padding(2);
            this.InstructionPort.Multiline = true;
            this.InstructionPort.Name = "InstructionPort";
            this.InstructionPort.Size = new System.Drawing.Size(202, 38);
            this.InstructionPort.TabIndex = 29;
            // 
            // palmRoll
            // 
            this.palmRoll.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.palmRoll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.palmRoll.Location = new System.Drawing.Point(40, 367);
            this.palmRoll.Margin = new System.Windows.Forms.Padding(2);
            this.palmRoll.Name = "palmRoll";
            this.palmRoll.ReadOnly = true;
            this.palmRoll.Size = new System.Drawing.Size(253, 38);
            this.palmRoll.TabIndex = 63;
            this.palmRoll.Text = "";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Enabled = false;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(36, 331);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(151, 22);
            this.label12.TabIndex = 64;
            this.label12.Text = "Palm Roll (Deg)";
            // 
            // x_displacement
            // 
            this.x_displacement.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.x_displacement.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.x_displacement.Location = new System.Drawing.Point(40, 273);
            this.x_displacement.Margin = new System.Windows.Forms.Padding(2);
            this.x_displacement.Name = "x_displacement";
            this.x_displacement.ReadOnly = true;
            this.x_displacement.Size = new System.Drawing.Size(253, 38);
            this.x_displacement.TabIndex = 111;
            this.x_displacement.Text = "";
            // 
            // z_displacement
            // 
            this.z_displacement.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.z_displacement.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.z_displacement.Location = new System.Drawing.Point(666, 269);
            this.z_displacement.Margin = new System.Windows.Forms.Padding(2);
            this.z_displacement.Name = "z_displacement";
            this.z_displacement.ReadOnly = true;
            this.z_displacement.Size = new System.Drawing.Size(253, 38);
            this.z_displacement.TabIndex = 112;
            this.z_displacement.Text = "";
            // 
            // y_displacement
            // 
            this.y_displacement.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.y_displacement.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.y_displacement.Location = new System.Drawing.Point(354, 272);
            this.y_displacement.Margin = new System.Windows.Forms.Padding(2);
            this.y_displacement.Name = "y_displacement";
            this.y_displacement.ReadOnly = true;
            this.y_displacement.Size = new System.Drawing.Size(253, 38);
            this.y_displacement.TabIndex = 113;
            this.y_displacement.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(663, 233);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 22);
            this.label1.TabIndex = 114;
            this.label1.Text = "Z Axis Coordinate";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(351, 233);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 22);
            this.label2.TabIndex = 115;
            this.label2.Text = "Y Axis Coordinate";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(42, 233);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(171, 22);
            this.label3.TabIndex = 116;
            this.label3.Text = "X Axis Coordinate";
            this.label3.UseMnemonic = false;
            // 
            // right_key
            // 
            this.right_key.BackColor = System.Drawing.Color.Chartreuse;
            this.right_key.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.right_key.ForeColor = System.Drawing.SystemColors.Highlight;
            this.right_key.Location = new System.Drawing.Point(510, 489);
            this.right_key.Margin = new System.Windows.Forms.Padding(2);
            this.right_key.Multiline = true;
            this.right_key.Name = "right_key";
            this.right_key.Size = new System.Drawing.Size(84, 41);
            this.right_key.TabIndex = 117;
            this.right_key.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // down_key
            // 
            this.down_key.BackColor = System.Drawing.Color.Chartreuse;
            this.down_key.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.down_key.ForeColor = System.Drawing.SystemColors.Highlight;
            this.down_key.Location = new System.Drawing.Point(181, 489);
            this.down_key.Margin = new System.Windows.Forms.Padding(2);
            this.down_key.Multiline = true;
            this.down_key.Name = "down_key";
            this.down_key.Size = new System.Drawing.Size(84, 41);
            this.down_key.TabIndex = 118;
            this.down_key.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // left_key
            // 
            this.left_key.BackColor = System.Drawing.Color.Chartreuse;
            this.left_key.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.left_key.ForeColor = System.Drawing.SystemColors.Highlight;
            this.left_key.Location = new System.Drawing.Point(353, 489);
            this.left_key.Margin = new System.Windows.Forms.Padding(2);
            this.left_key.Multiline = true;
            this.left_key.Name = "left_key";
            this.left_key.Size = new System.Drawing.Size(84, 41);
            this.left_key.TabIndex = 119;
            this.left_key.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // up_key
            // 
            this.up_key.BackColor = System.Drawing.Color.Chartreuse;
            this.up_key.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.up_key.ForeColor = System.Drawing.SystemColors.Highlight;
            this.up_key.Location = new System.Drawing.Point(40, 489);
            this.up_key.Margin = new System.Windows.Forms.Padding(2);
            this.up_key.Multiline = true;
            this.up_key.Name = "up_key";
            this.up_key.Size = new System.Drawing.Size(84, 41);
            this.up_key.TabIndex = 120;
            this.up_key.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Enabled = false;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(177, 449);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 22);
            this.label6.TabIndex = 121;
            this.label6.Text = "Backward";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(506, 449);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 22);
            this.label7.TabIndex = 122;
            this.label7.Text = "Right Turn";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(351, 449);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 22);
            this.label8.TabIndex = 123;
            this.label8.Text = "Left Trun";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(35, 449);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(83, 22);
            this.label13.TabIndex = 124;
            this.label13.Text = "Forward";
            // 
            // VideoPort
            // 
            this.VideoPort.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.VideoPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VideoPort.ForeColor = System.Drawing.SystemColors.Desktop;
            this.VideoPort.Location = new System.Drawing.Point(550, 55);
            this.VideoPort.Margin = new System.Windows.Forms.Padding(2);
            this.VideoPort.Multiline = true;
            this.VideoPort.Name = "VideoPort";
            this.VideoPort.Size = new System.Drawing.Size(202, 38);
            this.VideoPort.TabIndex = 129;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Enabled = false;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(546, 16);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(104, 22);
            this.label17.TabIndex = 128;
            this.label17.Text = "Video Port";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(656, 331);
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(136, 22);
            this.label19.TabIndex = 133;
            this.label19.Text = "Grap Strength";
            // 
            // grapVal
            // 
            this.grapVal.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.grapVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grapVal.Location = new System.Drawing.Point(660, 367);
            this.grapVal.Margin = new System.Windows.Forms.Padding(2);
            this.grapVal.Name = "grapVal";
            this.grapVal.ReadOnly = true;
            this.grapVal.Size = new System.Drawing.Size(253, 38);
            this.grapVal.TabIndex = 132;
            this.grapVal.Text = "";
            // 
            // stop_key
            // 
            this.stop_key.BackColor = System.Drawing.Color.Chartreuse;
            this.stop_key.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stop_key.ForeColor = System.Drawing.SystemColors.Highlight;
            this.stop_key.Location = new System.Drawing.Point(660, 489);
            this.stop_key.Margin = new System.Windows.Forms.Padding(2);
            this.stop_key.Multiline = true;
            this.stop_key.Name = "stop_key";
            this.stop_key.Size = new System.Drawing.Size(84, 41);
            this.stop_key.TabIndex = 134;
            this.stop_key.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label37.Location = new System.Drawing.Point(656, 449);
            this.label37.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(115, 22);
            this.label37.TabIndex = 135;
            this.label37.Text = "Hand Break";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.Location = new System.Drawing.Point(44, 123);
            this.label38.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(105, 22);
            this.label38.TabIndex = 137;
            this.label38.Text = "Hand L/R?";
            // 
            // whichHand
            // 
            this.whichHand.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.whichHand.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.whichHand.Location = new System.Drawing.Point(46, 161);
            this.whichHand.Margin = new System.Windows.Forms.Padding(2);
            this.whichHand.Name = "whichHand";
            this.whichHand.ReadOnly = true;
            this.whichHand.Size = new System.Drawing.Size(253, 38);
            this.whichHand.TabIndex = 136;
            this.whichHand.Text = "";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(42, 601);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(113, 22);
            this.label9.TabIndex = 139;
            this.label9.Text = "Base Servo";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label9.UseMnemonic = false;
            // 
            // baseServoText
            // 
            this.baseServoText.BackColor = System.Drawing.Color.CadetBlue;
            this.baseServoText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseServoText.Location = new System.Drawing.Point(40, 641);
            this.baseServoText.Margin = new System.Windows.Forms.Padding(2);
            this.baseServoText.Name = "baseServoText";
            this.baseServoText.ReadOnly = true;
            this.baseServoText.Size = new System.Drawing.Size(253, 38);
            this.baseServoText.TabIndex = 138;
            this.baseServoText.Text = "";
            // 
            // baseServoTransText
            // 
            this.baseServoTransText.BackColor = System.Drawing.Color.Lavender;
            this.baseServoTransText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseServoTransText.Location = new System.Drawing.Point(39, 732);
            this.baseServoTransText.Margin = new System.Windows.Forms.Padding(2);
            this.baseServoTransText.Name = "baseServoTransText";
            this.baseServoTransText.ReadOnly = true;
            this.baseServoTransText.Size = new System.Drawing.Size(253, 38);
            this.baseServoTransText.TabIndex = 140;
            this.baseServoTransText.Text = "";
            // 
            // shoulderServoTransText
            // 
            this.shoulderServoTransText.BackColor = System.Drawing.Color.Lavender;
            this.shoulderServoTransText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shoulderServoTransText.Location = new System.Drawing.Point(352, 732);
            this.shoulderServoTransText.Margin = new System.Windows.Forms.Padding(2);
            this.shoulderServoTransText.Name = "shoulderServoTransText";
            this.shoulderServoTransText.ReadOnly = true;
            this.shoulderServoTransText.Size = new System.Drawing.Size(253, 38);
            this.shoulderServoTransText.TabIndex = 143;
            this.shoulderServoTransText.Text = "";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(355, 601);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(148, 22);
            this.label10.TabIndex = 142;
            this.label10.Text = "Shoulder Servo";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label10.UseMnemonic = false;
            // 
            // shoulderServoText
            // 
            this.shoulderServoText.BackColor = System.Drawing.Color.CadetBlue;
            this.shoulderServoText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shoulderServoText.Location = new System.Drawing.Point(353, 641);
            this.shoulderServoText.Margin = new System.Windows.Forms.Padding(2);
            this.shoulderServoText.Name = "shoulderServoText";
            this.shoulderServoText.ReadOnly = true;
            this.shoulderServoText.Size = new System.Drawing.Size(253, 38);
            this.shoulderServoText.TabIndex = 141;
            this.shoulderServoText.Text = "";
            // 
            // elbowServoTransText
            // 
            this.elbowServoTransText.BackColor = System.Drawing.Color.Lavender;
            this.elbowServoTransText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.elbowServoTransText.Location = new System.Drawing.Point(665, 732);
            this.elbowServoTransText.Margin = new System.Windows.Forms.Padding(2);
            this.elbowServoTransText.Name = "elbowServoTransText";
            this.elbowServoTransText.ReadOnly = true;
            this.elbowServoTransText.Size = new System.Drawing.Size(253, 38);
            this.elbowServoTransText.TabIndex = 146;
            this.elbowServoTransText.Text = "";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(668, 601);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(122, 22);
            this.label11.TabIndex = 145;
            this.label11.Text = "Elbow Servo";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label11.UseMnemonic = false;
            // 
            // elbowServoText
            // 
            this.elbowServoText.BackColor = System.Drawing.Color.CadetBlue;
            this.elbowServoText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.elbowServoText.Location = new System.Drawing.Point(666, 641);
            this.elbowServoText.Margin = new System.Windows.Forms.Padding(2);
            this.elbowServoText.Name = "elbowServoText";
            this.elbowServoText.ReadOnly = true;
            this.elbowServoText.Size = new System.Drawing.Size(253, 38);
            this.elbowServoText.TabIndex = 144;
            this.elbowServoText.Text = "";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(41, 808);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(36, 22);
            this.label14.TabIndex = 148;
            this.label14.Text = "M1";
            this.label14.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label14.UseMnemonic = false;
            // 
            // M1Instruction
            // 
            this.M1Instruction.BackColor = System.Drawing.Color.CadetBlue;
            this.M1Instruction.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.M1Instruction.Location = new System.Drawing.Point(39, 848);
            this.M1Instruction.Margin = new System.Windows.Forms.Padding(2);
            this.M1Instruction.Name = "M1Instruction";
            this.M1Instruction.ReadOnly = true;
            this.M1Instruction.Size = new System.Drawing.Size(253, 38);
            this.M1Instruction.TabIndex = 147;
            this.M1Instruction.Text = "";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(354, 808);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(36, 22);
            this.label15.TabIndex = 150;
            this.label15.Text = "M2";
            this.label15.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label15.UseMnemonic = false;
            // 
            // M2Instruction
            // 
            this.M2Instruction.BackColor = System.Drawing.Color.CadetBlue;
            this.M2Instruction.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.M2Instruction.Location = new System.Drawing.Point(352, 848);
            this.M2Instruction.Margin = new System.Windows.Forms.Padding(2);
            this.M2Instruction.Name = "M2Instruction";
            this.M2Instruction.ReadOnly = true;
            this.M2Instruction.Size = new System.Drawing.Size(253, 38);
            this.M2Instruction.TabIndex = 149;
            this.M2Instruction.Text = "";
            // 
            // testBox
            // 
            this.testBox.BackColor = System.Drawing.Color.CadetBlue;
            this.testBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testBox.Location = new System.Drawing.Point(39, 941);
            this.testBox.Margin = new System.Windows.Forms.Padding(2);
            this.testBox.Name = "testBox";
            this.testBox.ReadOnly = true;
            this.testBox.Size = new System.Drawing.Size(572, 38);
            this.testBox.TabIndex = 151;
            this.testBox.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 1133);
            this.Controls.Add(this.testBox);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.M2Instruction);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.M1Instruction);
            this.Controls.Add(this.elbowServoTransText);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.elbowServoText);
            this.Controls.Add(this.shoulderServoTransText);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.shoulderServoText);
            this.Controls.Add(this.baseServoTransText);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.baseServoText);
            this.Controls.Add(this.label38);
            this.Controls.Add(this.whichHand);
            this.Controls.Add(this.label37);
            this.Controls.Add(this.stop_key);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.grapVal);
            this.Controls.Add(this.VideoPort);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.up_key);
            this.Controls.Add(this.left_key);
            this.Controls.Add(this.down_key);
            this.Controls.Add(this.right_key);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.y_displacement);
            this.Controls.Add(this.z_displacement);
            this.Controls.Add(this.x_displacement);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.palmRoll);
            this.Controls.Add(this.InstructionPort);
            this.Controls.Add(label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ServerIPTextbox);
            this.ForeColor = System.Drawing.Color.Blue;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox ServerIPTextbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox InstructionPort;
        private System.Windows.Forms.RichTextBox palmRoll;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RichTextBox x_displacement;
        private System.Windows.Forms.RichTextBox z_displacement;
        private System.Windows.Forms.RichTextBox y_displacement;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox right_key;
        private System.Windows.Forms.TextBox down_key;
        private System.Windows.Forms.TextBox left_key;
        private System.Windows.Forms.TextBox up_key;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox VideoPort;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.RichTextBox grapVal;
        private System.Windows.Forms.TextBox stop_key;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.RichTextBox whichHand;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RichTextBox baseServoText;
        private System.Windows.Forms.RichTextBox baseServoTransText;
        private System.Windows.Forms.RichTextBox shoulderServoTransText;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RichTextBox shoulderServoText;
        private System.Windows.Forms.RichTextBox elbowServoTransText;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.RichTextBox elbowServoText;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.RichTextBox M1Instruction;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.RichTextBox M2Instruction;
        private System.Windows.Forms.RichTextBox testBox;
    }
}

