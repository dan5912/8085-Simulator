﻿namespace _8050_Simulator
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
            this.runButton = new System.Windows.Forms.Button();
            this.regBTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.regDTextbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.regHTextbox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.regCTextbox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.regETextbox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.regLTextbox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.flagsTextbox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.regATextbox = new System.Windows.Forms.TextBox();
            this.StackListBox = new System.Windows.Forms.ListBox();
            this.label9 = new System.Windows.Forms.Label();
            this.StepButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.opcodesTextbox = new System.Windows.Forms.ListBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.outP0 = new System.Windows.Forms.CheckBox();
            this.outP1 = new System.Windows.Forms.CheckBox();
            this.outP2 = new System.Windows.Forms.CheckBox();
            this.outP3 = new System.Windows.Forms.CheckBox();
            this.outP7 = new System.Windows.Forms.CheckBox();
            this.outP6 = new System.Windows.Forms.CheckBox();
            this.outP5 = new System.Windows.Forms.CheckBox();
            this.outP4 = new System.Windows.Forms.CheckBox();
            this.haltedCheckbox = new System.Windows.Forms.CheckBox();
            this.speedScrollbar = new System.Windows.Forms.HScrollBar();
            this.label12 = new System.Windows.Forms.Label();
            this.pcTextbox = new System.Windows.Forms.TextBox();
            this.PC = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // runButton
            // 
            this.runButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runButton.Location = new System.Drawing.Point(571, 15);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(86, 35);
            this.runButton.TabIndex = 0;
            this.runButton.Text = "Run";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // regBTextbox
            // 
            this.regBTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.regBTextbox.Location = new System.Drawing.Point(95, 12);
            this.regBTextbox.Name = "regBTextbox";
            this.regBTextbox.Size = new System.Drawing.Size(100, 26);
            this.regBTextbox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(35, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Reg B";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(34, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Reg D";
            // 
            // regDTextbox
            // 
            this.regDTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.regDTextbox.Location = new System.Drawing.Point(95, 44);
            this.regDTextbox.Name = "regDTextbox";
            this.regDTextbox.Size = new System.Drawing.Size(100, 26);
            this.regDTextbox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(34, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Reg H";
            // 
            // regHTextbox
            // 
            this.regHTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.regHTextbox.Location = new System.Drawing.Point(95, 76);
            this.regHTextbox.Name = "regHTextbox";
            this.regHTextbox.Size = new System.Drawing.Size(100, 26);
            this.regHTextbox.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(208, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Reg C";
            // 
            // regCTextbox
            // 
            this.regCTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.regCTextbox.Location = new System.Drawing.Point(268, 12);
            this.regCTextbox.Name = "regCTextbox";
            this.regCTextbox.Size = new System.Drawing.Size(100, 26);
            this.regCTextbox.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(208, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "Reg E";
            // 
            // regETextbox
            // 
            this.regETextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.regETextbox.Location = new System.Drawing.Point(268, 44);
            this.regETextbox.Name = "regETextbox";
            this.regETextbox.Size = new System.Drawing.Size(100, 26);
            this.regETextbox.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(208, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 20);
            this.label6.TabIndex = 12;
            this.label6.Text = "Reg L";
            // 
            // regLTextbox
            // 
            this.regLTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.regLTextbox.Location = new System.Drawing.Point(268, 76);
            this.regLTextbox.Name = "regLTextbox";
            this.regLTextbox.Size = new System.Drawing.Size(100, 26);
            this.regLTextbox.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(35, 111);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 20);
            this.label7.TabIndex = 14;
            this.label7.Text = "Flags";
            // 
            // flagsTextbox
            // 
            this.flagsTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flagsTextbox.Location = new System.Drawing.Point(95, 108);
            this.flagsTextbox.Name = "flagsTextbox";
            this.flagsTextbox.Size = new System.Drawing.Size(100, 26);
            this.flagsTextbox.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(208, 111);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 20);
            this.label8.TabIndex = 16;
            this.label8.Text = "Reg A";
            // 
            // regATextbox
            // 
            this.regATextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.regATextbox.Location = new System.Drawing.Point(268, 108);
            this.regATextbox.Name = "regATextbox";
            this.regATextbox.Size = new System.Drawing.Size(100, 26);
            this.regATextbox.TabIndex = 15;
            // 
            // StackListBox
            // 
            this.StackListBox.Cursor = System.Windows.Forms.Cursors.No;
            this.StackListBox.Enabled = false;
            this.StackListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StackListBox.FormattingEnabled = true;
            this.StackListBox.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.StackListBox.ItemHeight = 20;
            this.StackListBox.Location = new System.Drawing.Point(687, 38);
            this.StackListBox.Name = "StackListBox";
            this.StackListBox.ScrollAlwaysVisible = true;
            this.StackListBox.Size = new System.Drawing.Size(133, 164);
            this.StackListBox.TabIndex = 17;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(683, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 20);
            this.label9.TabIndex = 18;
            this.label9.Text = "Stack";
            // 
            // StepButton
            // 
            this.StepButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StepButton.Location = new System.Drawing.Point(479, 15);
            this.StepButton.Name = "StepButton";
            this.StepButton.Size = new System.Drawing.Size(86, 35);
            this.StepButton.TabIndex = 19;
            this.StepButton.Text = "Step";
            this.StepButton.UseVisualStyleBackColor = true;
            this.StepButton.Click += new System.EventHandler(this.StepButton_Click);
            // 
            // resetButton
            // 
            this.resetButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetButton.Location = new System.Drawing.Point(387, 15);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(86, 35);
            this.resetButton.TabIndex = 20;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // opcodesTextbox
            // 
            this.opcodesTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.opcodesTextbox.FormattingEnabled = true;
            this.opcodesTextbox.ItemHeight = 20;
            this.opcodesTextbox.Location = new System.Drawing.Point(387, 107);
            this.opcodesTextbox.Name = "opcodesTextbox";
            this.opcodesTextbox.Size = new System.Drawing.Size(270, 84);
            this.opcodesTextbox.TabIndex = 21;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(509, 221);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(139, 20);
            this.label10.TabIndex = 30;
            this.label10.Text = "Output (Port 0x11)";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(655, 242);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(163, 13);
            this.label11.TabIndex = 31;
            this.label11.Text = "7     6      5     4     3     2     1     0";
            // 
            // outP0
            // 
            this.outP0.AutoCheck = false;
            this.outP0.AutoSize = true;
            this.outP0.Location = new System.Drawing.Point(805, 225);
            this.outP0.Name = "outP0";
            this.outP0.Size = new System.Drawing.Size(15, 14);
            this.outP0.TabIndex = 32;
            this.outP0.UseVisualStyleBackColor = true;
            // 
            // outP1
            // 
            this.outP1.AutoCheck = false;
            this.outP1.AutoSize = true;
            this.outP1.Location = new System.Drawing.Point(784, 225);
            this.outP1.Name = "outP1";
            this.outP1.Size = new System.Drawing.Size(15, 14);
            this.outP1.TabIndex = 33;
            this.outP1.UseVisualStyleBackColor = true;
            // 
            // outP2
            // 
            this.outP2.AutoCheck = false;
            this.outP2.AutoSize = true;
            this.outP2.Location = new System.Drawing.Point(763, 225);
            this.outP2.Name = "outP2";
            this.outP2.Size = new System.Drawing.Size(15, 14);
            this.outP2.TabIndex = 34;
            this.outP2.UseVisualStyleBackColor = true;
            // 
            // outP3
            // 
            this.outP3.AutoCheck = false;
            this.outP3.AutoSize = true;
            this.outP3.Location = new System.Drawing.Point(742, 225);
            this.outP3.Name = "outP3";
            this.outP3.Size = new System.Drawing.Size(15, 14);
            this.outP3.TabIndex = 35;
            this.outP3.UseVisualStyleBackColor = true;
            // 
            // outP7
            // 
            this.outP7.AutoCheck = false;
            this.outP7.AutoSize = true;
            this.outP7.Location = new System.Drawing.Point(658, 225);
            this.outP7.Name = "outP7";
            this.outP7.Size = new System.Drawing.Size(15, 14);
            this.outP7.TabIndex = 39;
            this.outP7.UseVisualStyleBackColor = true;
            // 
            // outP6
            // 
            this.outP6.AutoCheck = false;
            this.outP6.AutoSize = true;
            this.outP6.Location = new System.Drawing.Point(679, 225);
            this.outP6.Name = "outP6";
            this.outP6.Size = new System.Drawing.Size(15, 14);
            this.outP6.TabIndex = 38;
            this.outP6.UseVisualStyleBackColor = true;
            // 
            // outP5
            // 
            this.outP5.AutoCheck = false;
            this.outP5.AutoSize = true;
            this.outP5.Location = new System.Drawing.Point(700, 225);
            this.outP5.Name = "outP5";
            this.outP5.Size = new System.Drawing.Size(15, 14);
            this.outP5.TabIndex = 37;
            this.outP5.UseVisualStyleBackColor = true;
            // 
            // outP4
            // 
            this.outP4.AutoCheck = false;
            this.outP4.AutoSize = true;
            this.outP4.Location = new System.Drawing.Point(721, 225);
            this.outP4.Name = "outP4";
            this.outP4.Size = new System.Drawing.Size(15, 14);
            this.outP4.TabIndex = 36;
            this.outP4.UseVisualStyleBackColor = true;
            // 
            // haltedCheckbox
            // 
            this.haltedCheckbox.AutoCheck = false;
            this.haltedCheckbox.AutoSize = true;
            this.haltedCheckbox.Location = new System.Drawing.Point(387, 85);
            this.haltedCheckbox.Name = "haltedCheckbox";
            this.haltedCheckbox.Size = new System.Drawing.Size(82, 17);
            this.haltedCheckbox.TabIndex = 40;
            this.haltedCheckbox.Text = "CPU Halted";
            this.haltedCheckbox.UseVisualStyleBackColor = true;
            // 
            // speedScrollbar
            // 
            this.speedScrollbar.Location = new System.Drawing.Point(436, 56);
            this.speedScrollbar.Name = "speedScrollbar";
            this.speedScrollbar.Size = new System.Drawing.Size(221, 17);
            this.speedScrollbar.TabIndex = 41;
            this.speedScrollbar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.speedScrollbar_Scroll);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(384, 57);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(49, 16);
            this.label12.TabIndex = 42;
            this.label12.Text = "Speed";
            // 
            // pcTextbox
            // 
            this.pcTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pcTextbox.Location = new System.Drawing.Point(603, 79);
            this.pcTextbox.Name = "pcTextbox";
            this.pcTextbox.Size = new System.Drawing.Size(54, 24);
            this.pcTextbox.TabIndex = 43;
            // 
            // PC
            // 
            this.PC.AutoSize = true;
            this.PC.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PC.Location = new System.Drawing.Point(476, 82);
            this.PC.Name = "PC";
            this.PC.Size = new System.Drawing.Size(123, 18);
            this.PC.TabIndex = 44;
            this.PC.Text = "Program Counter";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(832, 359);
            this.Controls.Add(this.PC);
            this.Controls.Add(this.pcTextbox);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.speedScrollbar);
            this.Controls.Add(this.haltedCheckbox);
            this.Controls.Add(this.outP7);
            this.Controls.Add(this.outP6);
            this.Controls.Add(this.outP5);
            this.Controls.Add(this.outP4);
            this.Controls.Add(this.outP3);
            this.Controls.Add(this.outP2);
            this.Controls.Add(this.outP1);
            this.Controls.Add(this.outP0);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.opcodesTextbox);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.StepButton);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.StackListBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.regATextbox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.flagsTextbox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.regLTextbox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.regETextbox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.regCTextbox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.regHTextbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.regDTextbox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.regBTextbox);
            this.Controls.Add(this.runButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.TextBox regBTextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox regDTextbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox regHTextbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox regCTextbox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox regETextbox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox regLTextbox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox flagsTextbox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox regATextbox;
        private System.Windows.Forms.ListBox StackListBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button StepButton;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.ListBox opcodesTextbox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox outP0;
        private System.Windows.Forms.CheckBox outP1;
        private System.Windows.Forms.CheckBox outP2;
        private System.Windows.Forms.CheckBox outP3;
        private System.Windows.Forms.CheckBox outP7;
        private System.Windows.Forms.CheckBox outP6;
        private System.Windows.Forms.CheckBox outP5;
        private System.Windows.Forms.CheckBox outP4;
        private System.Windows.Forms.CheckBox haltedCheckbox;
        private System.Windows.Forms.HScrollBar speedScrollbar;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox pcTextbox;
        private System.Windows.Forms.Label PC;
    }
}

