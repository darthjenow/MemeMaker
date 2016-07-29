using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemeGenerator
{
    public partial class Form1 : Form
    {
        Font font = new Font("Impact", 200); // the font for the text
        StringFormat stringFormat = new StringFormat();
        String filePath = Properties.Settings.Default.filePath; // Gets the filepath from the settings
        Bitmap img;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets executed if the form loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // Sets the string format
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
        }

        /// <summary>
        /// Gets executed if the form closes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Writes the path of the last selected file to the settings and saves them
            Properties.Settings.Default.filePath = filePath;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Gets executed if exit in the toolstrip gets clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Gets executed if open in the toolstrip gets clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile();
        }

        /// <summary>
        /// Gets executed if save in the toolstrip gets clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
        }

        /// <summary>
        /// Gets executed if the text in textBoxUpper changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxUpper_TextChanged(object sender, EventArgs e)
        {
            updateImg();
        }

        /// <summary>
        /// Gets executed if the textin textBoxLower changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxLower_TextChanged(object sender, EventArgs e)
        {
            updateImg();
        }

        /// <summary>
        /// If executed, it opens a openFile-Dialog and loads the image
        /// </summary>
        public void openFile()
        {
            if (filePath == "") // if filePath is empty, it sets it to %userprofile%
            {
                filePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }

            openFileDialog1.InitialDirectory = filePath; // sets the starting directory to the last one used

            if (openFileDialog1.ShowDialog() == DialogResult.OK) // Open got clicked
            {
                filePath = openFileDialog1.FileName; // path gets stored

                img = new Bitmap(filePath); // loads the image from the filepath
                
                Font fontTemp = font; // duplicates the font so it can get changed
                font = new Font(fontTemp.Name, (int) (0.0625 * img.Height + 12.5)); // changes the font size

                groupBoxEdit.Enabled = true; // enables the edit boxes

                textBoxUpper.Select(); // select the textBox for the upper text

                updateImg(); // updates the image
            }
        }

        /// <summary>
        /// If executed, it reloads the image with the text
        /// </summary>
        public void updateImg()
        {
            // reset the Bitmap and reload the image
            img = null;
            img = new Bitmap(filePath);

            Graphics graph = Graphics.FromImage(img);
            GraphicsPath p = new GraphicsPath();

            // Adds the text in textBoxUpper
            p.AddString(
                textBoxUpper.Text,
                new FontFamily("Impact"),
                (int)FontStyle.Regular,
                graph.DpiY * font.SizeInPoints / 72,
                new Point((int)(0.5 * img.Width), (int)(0.1 * img.Height)),
                stringFormat);

            // Adds the text in textBoxLower
            p.AddString(
                textBoxLower.Text,
                new FontFamily("Impact"),
                (int)FontStyle.Regular,
                graph.DpiY * font.SizeInPoints / 72,
                new Point((int)(0.5 * img.Width), (int)(0.9 * img.Height)),
                stringFormat);

            Pen pen = new Pen(Brushes.Black, font.SizeInPoints / 10); // pen for the text outline
            pen.Alignment = PenAlignment.Center;
            pen.LineJoin = LineJoin.Round; // Prevents spikes on some letters
            graph.DrawPath(pen, p); // makes the outline
            graph.FillPath(Brushes.White, p); // fills the path

            // sets the pictureBox.Image to to edited one
            pictureBox1.Image = img;
        }

        /// <summary>
        /// If executed, opens a safe dialog to safe the current meme
        /// </summary>
        public void save()
        {
            if (pictureBox1.Image != null) // there is something to safe
            {
                saveFileDialog1.InitialDirectory = filePath; // sets the starting directory to the last one

                if (saveFileDialog1.ShowDialog() == DialogResult.OK) // save got klicked
                {
                    pictureBox1.Image.Save(saveFileDialog1.FileName); // saves to the selected path
                }
            }
            else
            {
                MessageBox.Show("There is nothing to safe!", "Can't safe", MessageBoxButtons.OK, MessageBoxIcon.Error); // shows an error box, that there is nothing to safe
            }
        }

        /// <summary>
        /// Gets executed if version info in the toolstrip gets clicked; shows version info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void versionInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The current version is " + Properties.Settings.Default.version, "Version Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Gets executed if icons-credits gets in the toolstrip gets clicked; shows the icon credits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("All icons came from icons8.com\r\nNavigate to site?", "Icon credits", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            
            if (res == DialogResult.OK)
            {
                System.Diagnostics.Process.Start("http://icons8.com/license/");
            }
        }

        /// <summary>
        /// Gets executed if J3n0W in the toolbar gets clicked; shows link to blog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void developedBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Developed by J3n0W.\r\nGo to the blog?", "Developer info", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (res == DialogResult.OK)
            {
                System.Diagnostics.Process.Start("https://j3n0w.wordpress.com/");
            }
        }
    }
}