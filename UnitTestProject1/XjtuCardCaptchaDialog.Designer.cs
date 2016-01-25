namespace UnitTestProject1
{
    partial class XjtuCardCaptchaDialog
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
            this.CaptchaPictureBox = new System.Windows.Forms.PictureBox();
            this.CaptchaCodeBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.OKButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.CaptchaPictureBox)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CaptchaPictureBox
            // 
            this.CaptchaPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CaptchaPictureBox.Location = new System.Drawing.Point(3, 3);
            this.CaptchaPictureBox.Name = "CaptchaPictureBox";
            this.CaptchaPictureBox.Size = new System.Drawing.Size(373, 141);
            this.CaptchaPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.CaptchaPictureBox.TabIndex = 0;
            this.CaptchaPictureBox.TabStop = false;
            // 
            // CaptchaCodeBox
            // 
            this.CaptchaCodeBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CaptchaCodeBox.Location = new System.Drawing.Point(3, 150);
            this.CaptchaCodeBox.Name = "CaptchaCodeBox";
            this.CaptchaCodeBox.Size = new System.Drawing.Size(373, 21);
            this.CaptchaCodeBox.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.CaptchaPictureBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.CaptchaCodeBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.OKButton, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(379, 203);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // OKButton
            // 
            this.OKButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Location = new System.Drawing.Point(152, 177);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "确定";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // XjtuCardCaptchaDialog
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 203);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "XjtuCardCaptchaDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XjtuCardCaptchaDialog";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.CaptchaPictureBox)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox CaptchaPictureBox;
        private System.Windows.Forms.TextBox CaptchaCodeBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button OKButton;
    }
}