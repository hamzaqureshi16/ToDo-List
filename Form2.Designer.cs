namespace To_Do_List
{
    partial class NewTaskForm
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
            this.Tasktxtbox = new System.Windows.Forms.TextBox();
            this.AddTaskbtn = new System.Windows.Forms.Button();
            this.TaskLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Tasktxtbox
            // 
            this.Tasktxtbox.Location = new System.Drawing.Point(12, 58);
            this.Tasktxtbox.Name = "Tasktxtbox";
            this.Tasktxtbox.Size = new System.Drawing.Size(762, 35);
            this.Tasktxtbox.TabIndex = 0;
            // 
            // AddTaskbtn
            // 
            this.AddTaskbtn.Location = new System.Drawing.Point(291, 115);
            this.AddTaskbtn.Name = "AddTaskbtn";
            this.AddTaskbtn.Size = new System.Drawing.Size(230, 40);
            this.AddTaskbtn.TabIndex = 1;
            this.AddTaskbtn.Text = "Add Task";
            this.AddTaskbtn.UseVisualStyleBackColor = true;
            this.AddTaskbtn.Click += new System.EventHandler(this.AddTaskbtn_Click);
            // 
            // TaskLabel
            // 
            this.TaskLabel.AutoSize = true;
            this.TaskLabel.Location = new System.Drawing.Point(12, 9);
            this.TaskLabel.Name = "TaskLabel";
            this.TaskLabel.Size = new System.Drawing.Size(201, 30);
            this.TaskLabel.TabIndex = 2;
            this.TaskLabel.Text = "Enter the task below";
            // 
            // NewTaskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(800, 189);
            this.Controls.Add(this.TaskLabel);
            this.Controls.Add(this.AddTaskbtn);
            this.Controls.Add(this.Tasktxtbox);
            this.Name = "NewTaskForm";
            this.Text = "New Task";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox Tasktxtbox;
        private Button AddTaskbtn;
        private Label TaskLabel;
    }
}