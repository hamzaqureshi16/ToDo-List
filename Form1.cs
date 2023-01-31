using System.Data.SqlClient;
using System.Threading.Tasks;

namespace To_Do_List
{
    public partial class Form1 : Form
    {
        private string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ToDoList;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private SqlConnection connection;
        private TreeNode currSelect = null;

        public delegate void Populate();

        private Populate populate;
        public Form1()
        {
            InitializeComponent();
            
        }

        private void PopulateTreeView()
        {
            treeView1.Nodes.Clear();
                // Load tasks
                string taskQuery = "SELECT Id, Task FROM ToDoList";
                using (SqlCommand taskCommand = new SqlCommand(taskQuery, connection))
                {
                    using (SqlDataReader taskReader = taskCommand.ExecuteReader())
                    {
                        while (taskReader.Read())
                        {
                            int taskId = taskReader.GetInt32(0);
                            string task = taskReader.GetString(1);

                            TreeNode taskNode = new TreeNode(task);
                            taskNode.Tag = taskId;
                            treeView1.Nodes.Add(taskNode);
                        }
                    }
                }

            PopulateAllSubTasks();
        }
        private void PopulateAllSubTasks()
        {
           
             

                foreach (TreeNode taskNode in treeView1.Nodes)
                {
                
                    int taskId = (int)taskNode.Tag;

                    string subTaskQuery = $"SELECT SubTask, Completed FROM SubTasks WHERE TaskId = {taskId}";
                    using (SqlCommand subTaskCommand = new SqlCommand(subTaskQuery, connection))
                    {
                        using (SqlDataReader subTaskReader = subTaskCommand.ExecuteReader())
                        {
                            while (subTaskReader.Read())
                            {
                                string subTask = subTaskReader.GetString(0);
                                bool completed = subTaskReader.GetBoolean(1);

                                TreeNode subTaskNode = new TreeNode(subTask);
                                subTaskNode.Tag = taskId;
                                subTaskNode.ForeColor = completed ? Color.Green : Color.Red;

                                taskNode.Nodes.Add(subTaskNode);
                            }
                        }
                    }
                taskNode.Expand();
            }
            
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            populate += this.PopulateTreeView;
            
            connection = new SqlConnection(ConnectionString);
            connection.Open();
            PopulateTreeView();
            

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            connection.Close();
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackColor = Color.FromArgb(255, 255, 255);
        }
         
        private void button1_Click(object sender, EventArgs e)
        {
            
            NewTaskForm ntf = new NewTaskForm(connection, treeView1,currSelect,populate);
            ntf.Show();

            treeView1.SelectedNode = null;
            currSelect = null;
        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e) { 
        
            if(e.Node.Parent != null)
            {
                    treeView1.SelectedNode = null;
                currSelect = e.Node;
                    e.Cancel = true;
                
            }
            else
            {
                currSelect = e.Node;
            }

            Console.WriteLine(currSelect);
        }

        private void treeView1_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {

            if(e.Node.Parent != null)
            {
                //it is a subtask;
                
                string delQuery = "DELETE FROM SubTasks WHERE TaskId = @TaskId AND SubTask = @SubTask AND Completed = 1";
                string completed = "UPDATE SubTasks SET Completed = @Completed WHERE TaskId = @TaskId AND SubTask = @SubTask";
                
                   using (SqlCommand comm = new SqlCommand(completed, connection))
                 {
                     comm.Parameters.AddWithValue("@Completed", e.Node.Checked ? 0 : 1);
                     comm.Parameters.AddWithValue("@TaskId", e.Node.Tag);
                     comm.Parameters.AddWithValue("@SubTask", e.Node.Text);
                     comm.ExecuteNonQuery();


                    treeView1.CheckBoxes = false;
                    //.Sleep(2000);
                    treeView1.CheckBoxes = true;
                    
                    comm.CommandText = delQuery;

                     comm.ExecuteNonQuery();
                 }

            }
            else
            {
                //it is a task
                string delQuery = "DELETE FROM SubTasks WHERE TaskId = @TaskId;";

                using (SqlCommand delcom = new SqlCommand(delQuery, connection))
                {
                    delcom.Parameters.AddWithValue("@TaskId", e.Node.Tag);
                    delcom.ExecuteNonQuery();
                }
                
                
                    delQuery = "DELETE FROM ToDoList WHERE Id = @Id AND Task = @Task";
                    string completed = "UPDATE ToDoList SET Completed = @Completed WHERE Id = @Id AND Task = @Task";
                    using (SqlCommand comm = new SqlCommand(completed, connection))
                    {
                        comm.Parameters.AddWithValue("@Completed", e.Node.Checked ? 0 : 1);
                        comm.Parameters.AddWithValue("@Id", e.Node.Tag);
                        comm.Parameters.AddWithValue("@Task", e.Node.Text);
                        try
                        {
                            comm.ExecuteNonQuery();
                        }
                        catch (Exception x)
                        {

                        }
                        treeView1.CheckBoxes = false;
                        //Thread.Sleep(2000);
                        treeView1.CheckBoxes = true;

                        comm.CommandText = delQuery;

                        comm.ExecuteNonQuery();
                    }
                
            }
            populate();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine(currSelect);
            if(currSelect != null)
            {
                NewTaskForm ntf = new NewTaskForm(connection, currSelect, populate, true);
                ntf.Show();
            }
            treeView1.SelectedNode = null;
            currSelect = null;
        }
    }
}