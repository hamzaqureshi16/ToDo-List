using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace To_Do_List
{
    public partial class NewTaskForm : Form
    {
        private SqlConnection connection;
        private TreeView treeview;
        private TreeNode curselect;
        private bool edit;
        private Form1.Populate populate;

        
        public NewTaskForm(SqlConnection connection,TreeNode node, Form1.Populate pop,bool toedit)
        {
            InitializeComponent();
            this.connection = connection;
            this.curselect = node;
            this.Tasktxtbox.Text = node.Text;
            this.populate = pop;
            edit = toedit;
            this.Text = "Edit Task";
            AddTaskbtn.Text = "Edit";

        }
        public NewTaskForm(SqlConnection connection, TreeView treeView1, TreeNode currSelect, Form1.Populate populate)
        {
            InitializeComponent();
            this.connection = connection;
            this.treeview = treeView1;
            this.curselect = currSelect;
            this.populate = populate;
        }

        private bool IsEmpty() => string.IsNullOrEmpty(Tasktxtbox.Text) ? true : false;
        
        private void AddTaskbtn_Click(object sender, EventArgs e)
        {
            Console.WriteLine("currselect is" +curselect);
            if (!IsEmpty())
            {
                if (!edit)
                {
                    string query;

                    if (curselect != null)
                    {
                        Console.WriteLine(curselect.Text);
                        curselect.Nodes.Add(Tasktxtbox.Text);
                        curselect.Expand();
                        query = "AddSubTask";

                    }
                    else
                    {
                        treeview.Nodes.Add(Tasktxtbox.Text);
                        query = $"INSERT INTO ToDoList (Task) VALUES ('{Tasktxtbox.Text.Replace("'", "''")}')";

                    }


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (query.Equals("AddSubTask"))
                        {
                            Console.WriteLine(Tasktxtbox.Text);

                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@task", curselect.Text);
                            command.Parameters.AddWithValue("@subTask", Tasktxtbox.Text);
                            command.Parameters.AddWithValue("@completed", 0);

                            command.ExecuteNonQuery();
                        }
                        else
                        {

                            command.ExecuteNonQuery();
                        }
                    }
                    Tasktxtbox.Text = "";
                }
                else
                {
                   
                    if (curselect != null)
                    {
                       if(curselect.Parent == null)
                        {
                            string query = "UPDATE ToDoList SET Task = @Task WHERE Id = @Id";
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@Task", Tasktxtbox.Text);
                                command.Parameters.AddWithValue("@Id", curselect.Tag);
                                command.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string query = "UPDATE SubTasks SET SubTask = @NewSubTask WHERE TaskId = @TaskId AND SubTask = @OldSubTask";
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@NewSubTask", Tasktxtbox.Text);
                                command.Parameters.AddWithValue("@TaskId", curselect.Tag);
                                command.Parameters.AddWithValue("@OldSubTask", curselect.Text);
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    
                }
            }
            


            this.populate();
                this.Dispose();


        }
    }
}
