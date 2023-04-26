using BindingSourceNETFramework.Lib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BindingProject.UI
{
    public partial class Form1 : Form
    {
        private void Form1_Resize(object sender, EventArgs e)
        {
            splitContainer1.Size = new Size(splitContainer1.Size.Width * Size.Width / _basicWindowSize.Width,
                                            splitContainer1.Size.Height * Size.Height / _basicWindowSize.Height);
            _basicWindowSize = new Size(Size.Width, Size.Height);
        }

        private void CreateNewFile_Click(object sender, EventArgs e)
        {
            saveFileDialog1 = new SaveFileDialog
            {
                Filter = "json files (*.json)|*.json|bin (*.bin)|*.bin|all (*.*)|*.*",
                FilterIndex = 1,
                AddExtension = true,
                RestoreDirectory = true
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fileStream;
                fileStream = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
                _showingPath = new FileInfo(saveFileDialog1.FileName);
                fileStream.Close();
            }
        }

        private void OpenExistingFile_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog
            {
                Filter = "json files (*.json)|*.json|bin (*.bin)|*.bin|all (*.*)|*.*",
                FilterIndex = 1,
                AddExtension = true,
                RestoreDirectory = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _showingPath = new FileInfo(openFileDialog1.FileName);
                if (_showingPath.Extension == ".json")
                {
                    Lib.DataSerialization.LoadDataJson(ref bjjWrestlers, _showingPath);
                    bs.DataSource = bjjWrestlers;
                    bs.ResetBindings(false);
                }
                else
                {
                    Lib.DataSerialization.LoadDataBin(ref bjjWrestlers, _showingPath);
                    bs.DataSource = bjjWrestlers;
                    bs.ResetBindings(false);
                }
                
            }
            
            
        }

        private void SaveCurrentTableToFile_CLick(object sender, EventArgs e)
        {
            List<BjjWrestler> temp = (bs.DataSource as List<BjjWrestler>);
            if (folderBrowserDialog1.ShowDialog()==DialogResult.OK)
            {
                _showingPath = new FileInfo(folderBrowserDialog1.SelectedPath);
                if (_binOrJson)
                {
                    Lib.DataSerialization.SaveDataJson(temp, _showingPath);
                }
                else
                {
                    Lib.DataSerialization.SaveDataBin(temp, _showingPath);
                }
            }
        }


        private void ToolStripButton4_Click(object sender, EventArgs e)
        {
            _binOrJson = false;
            toolStripButton4.Enabled = false;
            toolStripButton5.Enabled = true;
        }

        private void ToolStripButton5_Click(object sender, EventArgs e)
        {
            _binOrJson = true;
            toolStripButton5.Enabled = false;
            toolStripButton4.Enabled = true;
        }
    }
}
