namespace WinFormUI
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
            this.Input1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ReverseStringButton = new System.Windows.Forms.Button();
            this.ReverseStringWithoutCollectionsButton = new System.Windows.Forms.Button();
            this.OutputTextBox = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Input2 = new System.Windows.Forms.TextBox();
            this.MultiplyButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Input1
            // 
            this.Input1.Location = new System.Drawing.Point(81, 36);
            this.Input1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Input1.Name = "Input1";
            this.Input1.Size = new System.Drawing.Size(145, 20);
            this.Input1.TabIndex = 0;
            this.Input1.TextChanged += new System.EventHandler(this.Input1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 39);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Input 1";
            // 
            // ReverseStringButton
            // 
            this.ReverseStringButton.Location = new System.Drawing.Point(588, 39);
            this.ReverseStringButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ReverseStringButton.Name = "ReverseStringButton";
            this.ReverseStringButton.Size = new System.Drawing.Size(206, 25);
            this.ReverseStringButton.TabIndex = 2;
            this.ReverseStringButton.Text = "ReverseString";
            this.ReverseStringButton.UseVisualStyleBackColor = true;
            this.ReverseStringButton.Click += new System.EventHandler(this.ReverseStringButtonClick);
            // 
            // ReverseStringWithoutCollectionsButton
            // 
            this.ReverseStringWithoutCollectionsButton.Location = new System.Drawing.Point(588, 69);
            this.ReverseStringWithoutCollectionsButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ReverseStringWithoutCollectionsButton.Name = "ReverseStringWithoutCollectionsButton";
            this.ReverseStringWithoutCollectionsButton.Size = new System.Drawing.Size(206, 25);
            this.ReverseStringWithoutCollectionsButton.TabIndex = 3;
            this.ReverseStringWithoutCollectionsButton.Text = "ReverseStringWithoutCollections";
            this.ReverseStringWithoutCollectionsButton.UseVisualStyleBackColor = true;
            this.ReverseStringWithoutCollectionsButton.Click += new System.EventHandler(this.ReverseStringWithoutCollectionsButtonClick);
            // 
            // OutputTextBox
            // 
            this.OutputTextBox.Location = new System.Drawing.Point(23, 119);
            this.OutputTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OutputTextBox.Name = "OutputTextBox";
            this.OutputTextBox.Size = new System.Drawing.Size(538, 214);
            this.OutputTextBox.TabIndex = 4;
            this.OutputTextBox.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 62);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Input 2";
            // 
            // Input2
            // 
            this.Input2.Location = new System.Drawing.Point(81, 58);
            this.Input2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Input2.Name = "Input2";
            this.Input2.Size = new System.Drawing.Size(145, 20);
            this.Input2.TabIndex = 5;
            // 
            // MultiplyButton
            // 
            this.MultiplyButton.Location = new System.Drawing.Point(588, 99);
            this.MultiplyButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MultiplyButton.Name = "MultiplyButton";
            this.MultiplyButton.Size = new System.Drawing.Size(206, 25);
            this.MultiplyButton.TabIndex = 7;
            this.MultiplyButton.Text = "Multiply";
            this.MultiplyButton.UseVisualStyleBackColor = true;
            this.MultiplyButton.Click += new System.EventHandler(this.MultiplyButtonClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 341);
            this.Controls.Add(this.MultiplyButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Input2);
            this.Controls.Add(this.OutputTextBox);
            this.Controls.Add(this.ReverseStringWithoutCollectionsButton);
            this.Controls.Add(this.ReverseStringButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Input1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Coding Interviews";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Input1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ReverseStringButton;
        private System.Windows.Forms.Button ReverseStringWithoutCollectionsButton;
        private System.Windows.Forms.RichTextBox OutputTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Input2;
        private System.Windows.Forms.Button MultiplyButton;
    }
}

