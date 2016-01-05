namespace HV9104_GUI
{
    partial class RunView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.customButton1 = new HV9104_GUI.CustomButton();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(555, 260);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // customButton1
            // 
            this.customButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.customButton1.ForeColor = System.Drawing.Color.White;
            this.customButton1.HoverImage = null;
            this.customButton1.Location = new System.Drawing.Point(450, 112);
            this.customButton1.Name = "customButton1";
            this.customButton1.PressedImage = null;
            this.customButton1.Size = new System.Drawing.Size(400, 100);
            this.customButton1.TabIndex = 1;
            this.customButton1.Text = "customButton1";
            // 
            // RunView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customButton1);
            this.Controls.Add(this.button1);
            this.Name = "RunView";
            this.Size = new System.Drawing.Size(1014, 599);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private CustomButton customButton1;
    }
}
