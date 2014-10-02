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
            this.Input1.Location = new System.Drawing.Point(108, 44);
            this.Input1.Name = "Input1";
            this.Input1.Size = new System.Drawing.Size(192, 22);
            this.Input1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Input 1";
            // 
            // ReverseStringButton
            // 
            this.ReverseStringButton.Location = new System.Drawing.Point(784, 48);
            this.ReverseStringButton.Name = "ReverseStringButton";
            this.ReverseStringButton.Size = new System.Drawing.Size(275, 31);
            this.ReverseStringButton.TabIndex = 2;
            this.ReverseStringButton.Text = "ReverseString";
            this.ReverseStringButton.UseVisualStyleBackColor = true;
            this.ReverseStringButton.Click += new System.EventHandler(this.ReverseStringButtonClick);
            // 
            // ReverseStringWithoutCollectionsButton
            // 
            this.ReverseStringWithoutCollectionsButton.Location = new System.Drawing.Point(784, 85);
            this.ReverseStringWithoutCollectionsButton.Name = "ReverseStringWithoutCollectionsButton";
            this.ReverseStringWithoutCollectionsButton.Size = new System.Drawing.Size(275, 31);
            this.ReverseStringWithoutCollectionsButton.TabIndex = 3;
            this.ReverseStringWithoutCollectionsButton.Text = "ReverseStringWithoutCollections";
            this.ReverseStringWithoutCollectionsButton.UseVisualStyleBackColor = true;
            this.ReverseStringWithoutCollectionsButton.Click += new System.EventHandler(this.ReverseStringWithoutCollectionsButtonClick);
            // 
            // OutputTextBox
            // 
            this.OutputTextBox.Location = new System.Drawing.Point(31, 146);
            this.OutputTextBox.Name = "OutputTextBox";
            this.OutputTextBox.Size = new System.Drawing.Size(716, 262);
            this.OutputTextBox.TabIndex = 4;
            this.OutputTextBox.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Input 2";
            // 
            // Input2
            // 
            this.Input2.Location = new System.Drawing.Point(108, 72);
            this.Input2.Name = "Input2";
            this.Input2.Size = new System.Drawing.Size(192, 22);
            this.Input2.TabIndex = 5;
            // 
            // MultiplyButton
            // 
            this.MultiplyButton.Location = new System.Drawing.Point(784, 122);
            this.MultiplyButton.Name = "MultiplyButton";
            this.MultiplyButton.Size = new System.Drawing.Size(275, 31);
            this.MultiplyButton.TabIndex = 7;
            this.MultiplyButton.Text = "Multiply";
            this.MultiplyButton.UseVisualStyleBackColor = true;
            this.MultiplyButton.Click += new System.EventHandler(this.MultiplyButtonClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1071, 420);
            this.Controls.Add(this.MultiplyButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Input2);
            this.Controls.Add(this.OutputTextBox);
            this.Controls.Add(this.ReverseStringWithoutCollectionsButton);
            this.Controls.Add(this.ReverseStringButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Input1);
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

