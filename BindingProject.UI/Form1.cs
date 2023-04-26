using BindingSourceNETFramework.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Resources;
using System.Runtime.Remoting.Channels;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Windows.Forms.DataVisualization.Charting;

namespace BindingProject.UI
{

    public partial class Form1 : Form
    {
        private Size _basicWindowSize = new Size(1280, 720);
        private FileInfo _showingPath;
        private List<BjjWrestler> bjjWrestlers = new List<BjjWrestler>();
        private BindingSource bs = new BindingSource();
        public string searchEntries { get; set; }
        /// <summary>
        /// basic is 0 - binary serialization
        /// </summary>
        private bool _binOrJson = false;
        public Form1()
        {
            InitializeComponent();

            dataGridView1.Location = new Point(12, 30);
            dataGridView1.AutoGenerateColumns = false;
            bjjWrestlers = new List<BjjWrestler>{new BjjWrestler(1, "Adam", "Wardzinski", 241, 55, BjjCountries.Poland,
                                                                       BjjBeltsEnum.Black, BjjTeams.CheckMat, "..\\..\\Pics\\Adam_Wardzinski.jpg"),
                                                                       new BjjWrestler(2, "Felipe", "Pena", 154, 23, BjjCountries.Brasil,
                                                                       BjjBeltsEnum.Black, BjjTeams.FP, "..\\..\\Pics\\Felipe_Pena.jpg"),
                                                                       new BjjWrestler(3, "Roger", "Gracie", 76, 7, BjjCountries.Brasil,
                                                                       BjjBeltsEnum.Black, BjjTeams.Gracie_Barra, "..\\..\\Pics\\Roger_Gracie.jpg")};

            bs.DataSource = bjjWrestlers;
            dataGridView1.DataSource = bs;
            bindingNavigator1.BindingSource = bs;
            chart1.DataSource = bs;
            propertyGrid1.DataBindings.Add("SelectedObject", bs, "");

            toolStripButton1.Text = "Create New";
            toolStripButton2.Text = "Open Existing";
            toolStripButton3.Text = "Save Current";
            toolStripButton4.Text = "Serialization to bin";
            toolStripButton5.Text = "Serialization to json";

            Size = new Size(1280, 720);
            _basicWindowSize = new Size(1280, 720);
            CenterToScreen();
            splitContainer1.Size = new Size(_basicWindowSize.Width, _basicWindowSize.Height - bindingNavigator1.Size.Height);
            GenerateDataGridView(dataGridView1);
            toolStripButton4.Enabled = false;


            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.DataBindings.Add("ImageLocation", bs, "ImagePath", true);

            chart1.DataSource = from w in bjjWrestlers
                                group w by w.TeamToString into g
                                select new { Team = g.Key, Avg = g.Average(w => w.Wins) };
            chart1.Series[0].XValueMember = "Team";
            chart1.Series[0].YValueMembers = "Avg";
            chart1.Palette = ChartColorPalette.Pastel;
            chart1.Legends.Clear();
            chart1.Titles.Add("Количество побед борцов разных клубов в среднем");
            bs.CurrentChanged += (o, e) => chart1.DataBind();
            toolStripTextBox1.Control.DataBindings.Add("Text", this, "searchEntries");
            toolStripTextBox1.BackColor = Color.LightBlue;
            toolStripTextBox1.ForeColor = Color.Black;
            toolStripTextBox1.Font = new Font("Ermilov", 10);
        }
        private void GenerateDataGridView(DataGridView dataGridView)
        {
            var column1 = new DataGridViewTextBoxColumn()
            {
                Name = "id",
                HeaderText = "Id",
                DataPropertyName = "Id"
            };
            dataGridView.Columns.Add(column1);
            var column2 = new DataGridViewTextBoxColumn()
            {
                Name = "first_name",
                HeaderText = "First Name",
                DataPropertyName = "FirstName"
            };
            dataGridView.Columns.Add(column2);
            var column3 = new DataGridViewTextBoxColumn()
            {
                Name = "last_name",
                HeaderText = "Last Name",
                DataPropertyName = "LastName"
            };
            dataGridView.Columns.Add(column3);
            var column4 = new DataGridViewTextBoxColumn()
            {
                Name = "wins",
                HeaderText = "Wins",
                DataPropertyName = "wins"
            };
            dataGridView.Columns.Add(column4);
            var column5 = new DataGridViewTextBoxColumn()
            {
                Name = "losses",
                HeaderText = "losses",
                DataPropertyName = "losses"
            };
            dataGridView.Columns.Add(column5);
            var column6 = new DataGridViewComboBoxColumn()
            {
                Name = "country",
                HeaderText = "country",
                DataPropertyName = "country",
                DataSource = Enum.GetValues(typeof(BjjCountries))
            };
            dataGridView.Columns.Add(column6);
            var column7 = new DataGridViewComboBoxColumn()
            {
                Name = "belt",
                HeaderText = "Belt",
                DataPropertyName = "belt",
                DataSource = Enum.GetValues(typeof(BjjBeltsEnum))
            };
            dataGridView.Columns.Add(column7);
            var column8 = new DataGridViewComboBoxColumn()
            {
                Name = "team",
                HeaderText = "Team",
                DataPropertyName = "team",
                DataSource = Enum.GetValues(typeof(BjjTeams))
            };
            dataGridView.Columns.Add(column8);

        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog
            {
                Filter = "png files (*.png)|*.png|jpg (*.jpg)|*.jpg|all (*.*)|*.*",
                FilterIndex = 2,
                AddExtension = true,
                RestoreDirectory = true
            };


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(openFileDialog1.FileName);

                int currInd = bs.Position;
                string fileName = fi.Name;
                string codeBase = Assembly.GetExecutingAssembly().Location;
                int cnt = 0;
                int tempIndex = -1;
                for (int i = codeBase.Length - 1; i >= 0; i--)
                {
                    if (codeBase[i] == '\\')
                    {
                        cnt++;
                    }
                    if (cnt == 3)
                    {
                        tempIndex = i;
                        break;
                    }
                }

                codeBase = codeBase.Substring(0, tempIndex);
                try
                {
                    fi.CopyTo(String.Format(codeBase + "\\Pics\\" + fileName));
                }
                catch (Exception)
                { }
                bjjWrestlers[currInd].ImagePath = String.Format("..\\..\\Pics\\" + fileName);
                bs.ResetBindings(false);

            }
        }

        private void ToolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter && searchEntries != "")
            {
                for (int i = 0; i < bjjWrestlers.Count; i++)
                {
                    if (bjjWrestlers[i].FirstName.Length >= searchEntries.Length &&
                        bjjWrestlers[i].FirstName.IndexOf(searchEntries) != -1)
                    {
                        dataGridView1.Rows[i].Visible = true;
                        continue;
                    }
                    else
                    {
                        dataGridView1.CurrentCell = null;
                        dataGridView1.ClearSelection();
                        dataGridView1.Rows[i].Visible = false;
                    }
                    if (bjjWrestlers[i].LastName.Length >= searchEntries.Length &&
                        bjjWrestlers[i].LastName.IndexOf(searchEntries) != -1)
                    {
                        dataGridView1.Rows[i].Visible = true;
                        continue;
                    }
                    else
                    {
                        dataGridView1.CurrentCell = null;
                        dataGridView1.ClearSelection();
                        dataGridView1.Rows[i].Visible = false;
                    }
                    if (bjjWrestlers[i].Country.ToString().Length >= searchEntries.Length &&
                        bjjWrestlers[i].Country.ToString().IndexOf(searchEntries) != -1)
                    {
                        dataGridView1.Rows[i].Visible = true;
                        continue;
                    }
                    else
                    {
                        dataGridView1.CurrentCell = null;
                        dataGridView1.ClearSelection();
                        dataGridView1.Rows[i].Visible = false;
                    }
                    if (bjjWrestlers[i].Team.ToString().Length >= searchEntries.Length &&
                        bjjWrestlers[i].Team.ToString().IndexOf(searchEntries) != -1)
                    {
                        dataGridView1.Rows[i].Visible = true;
                        continue;
                    }
                    else
                    {
                        dataGridView1.CurrentCell = null;
                        dataGridView1.ClearSelection();
                        dataGridView1.Rows[i].Visible = false;
                    }
                    if (bjjWrestlers[i].Belt.ToString().Length >= searchEntries.Length &&
                        bjjWrestlers[i].Belt.ToString().IndexOf(searchEntries) != -1)
                    {
                        dataGridView1.Rows[i].Visible = true;
                        continue;
                    }
                    else
                    {
                        dataGridView1.CurrentCell = null;
                        dataGridView1.ClearSelection();
                        dataGridView1.Rows[i].Visible = false;
                    }
                }

            }
            else
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    dataGridView1.Rows[i].Visible = true;
                }
            }
            dataGridView1.Refresh();
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            searchEntries = toolStripTextBox1.Text;
        }

        private void dataGridView1_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Считываем значение в обязательной ячейке
            var id = dataGridView1["id", e.RowIndex].Value;
            var firstN = dataGridView1["first_name", e.RowIndex].Value;
            var lastN = dataGridView1["last_name", e.RowIndex].Value;
            if (id == null || (int)id == -1)
            {
                e.Cancel = true;
                // Переводим фокус в обязательную ячейку
                dataGridView1.CurrentCell = dataGridView1["id", e.RowIndex];
                // Режим редактирования с выделением
                dataGridView1.BeginEdit(true);
                dataGridView1.CurrentCell.Selected = true;
                dataGridView1["id", e.RowIndex].Style.BackColor = Color.Red;
            }
            else if (firstN == null)
            {
                e.Cancel = true;
                // Переводим фокус в обязательную ячейку
                dataGridView1.CurrentCell = dataGridView1["first_name", e.RowIndex];
                // Режим редактирования с выделением
                dataGridView1.BeginEdit(true);
                dataGridView1.CurrentCell.Selected = true;
                dataGridView1["first_name", e.RowIndex].Style.BackColor = Color.Red;
            }
            else if (lastN == null)
            {
                e.Cancel = true;
                // Переводим фокус в обязательную ячейку
                dataGridView1.CurrentCell = dataGridView1["last_name", e.RowIndex];
                // Режим редактирования с выделением
                dataGridView1.BeginEdit(true);
                dataGridView1.CurrentCell.Selected = true;
                dataGridView1["last_name", e.RowIndex].Style.BackColor = Color.Red;
            }
            else
            {
                e.Cancel = false;
                dataGridView1["id", e.RowIndex].Style.BackColor = Color.White;
                dataGridView1["first_name", e.RowIndex].Style.BackColor = Color.White;
                dataGridView1["last_name", e.RowIndex].Style.BackColor = Color.White;
            }
        }


        private void dataGridView1_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.BackgroundColor = Color.White;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                dataGridView1["id", row.Index].Style.BackColor = Color.White;
                dataGridView1["first_name", row.Index].Style.BackColor = Color.White;
                dataGridView1["last_name", row.Index].Style.BackColor = Color.White;
            }
        }
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                dataGridView1.BackgroundColor = Color.White;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    dataGridView1["id", row.Index].Style.BackColor = Color.White;
                    dataGridView1["first_name", row.Index].Style.BackColor = Color.White;
                    dataGridView1["last_name", row.Index].Style.BackColor = Color.White;
                }

            }
        }

        private void dataGridView1_Validated(object sender, EventArgs e)
        {
            dataGridView1.BackgroundColor = Color.White;
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                dataGridView1["id", row.Index].Style.BackColor = Color.White;
                dataGridView1["first_name", row.Index].Style.BackColor = Color.White;
                dataGridView1["last_name", row.Index].Style.BackColor = Color.White;
            }
        }
    }
}
