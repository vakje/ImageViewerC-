using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;
using WindowsFormsC_app.Properties;

namespace WindowsFormsC_app
{
    public partial class Form1 : Form
    {
        // making some default photo loader when there is no images
        private static string emptyiconpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tech", "empty.jpg");
        // timer for slide show function
        Timer forslideshowtimer = new Timer();
        string runDirectory = AppDomain.CurrentDomain.BaseDirectory;
        
        bool button4wasclicked;

        bool checkboxloop = false;
        





        private string[] IMAGES;
        private int CURRENTIMAGES;

        // function to make  exact location for my buttons or items into form 
        private Point GetPointInPercentages(float x, float y, Size size)
        {
            return new Point((int)(this.ClientSize.Width * x) - (size.Width / 2), (int)(this.ClientSize.Height * y) - (size.Height / 2));
        }
        // default and nondefalt constructors for run them  with different files 
        public Form1()
        {

            InitializeComponent();
            configurator();
            LOADER_OF_IMAGES_O(runDirectory);
            LOAD_DIRS();
           
            
           
            DISPLAY();
            
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            this.FormClosing += Form1_FormClosing;
            this.Load += Form1_Load;
           


            pictureBox2.GetPreferredSize(new Size(GetPointInPercentages(0.2f, 0.2f, pictureBox2.Size)));
            pictureBox3.GetPreferredSize(new Size(GetPointInPercentages(0.2f, 0.2f, pictureBox3.Size)));
            textBox1.GetPreferredSize(new Size(GetPointInPercentages(0.1f,0.1f, textBox1.Size)));
           
            
        }
        public Form1(String dir)
        {
            InitializeComponent();
            configurator();
            LOADER_OF_IMAGES_O(dir);
            LOAD_DIRS();
           
           
            DISPLAY();
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            this.FormClosing += Form1_FormClosing;
            this.Load += Form1_Load;
           
            pictureBox2.GetPreferredSize(new Size(GetPointInPercentages(0.2f, 0.2f, pictureBox2.Size)));
            pictureBox3.GetPreferredSize(new Size(GetPointInPercentages(0.2f, 0.2f, pictureBox3.Size)));
            textBox1.GetPreferredSize(new Size(GetPointInPercentages(0.1f, 0.1f, textBox1.Size)));
           
        }

        /// <summary>
        ///  loading some photos and adding it to combobox for traveling through files and etc
        ///  adding some key symbols for traversing through files 
        /// </summary>
        /// <param name="path"></param>
        private void LOAD_DIRS(string path = null)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    
                    comboBox1.Items.Clear();
                    comboBox1.Text = runDirectory.ToString();
                    comboBox1.Items.Add("..");
                    comboBox1.Items.Add("./");                                                              
                            try
                            {
                                List<string> dirs = new List<string>();
                              
                                GetTheseChildrenRecursively(path, dirs);


                                foreach (string dir in dirs)
                                {
                                    comboBox1.Items.Add(dir);
                                }
                              
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("error with " + ex.Message);

                            }                                           
                }
                else
                {
                    runDirectory = path; //update run directory
                    List<string> allDirectories = new List<string>();
                    GetTheseChildrenRecursively(path, allDirectories);

                    foreach (string dir in allDirectories)
                    {
                        comboBox1.Items.Add(dir);
                    }
                   
                }
            }
            catch (Exception ex) { MessageBox.Show("some error with" + ex.Message); }
        }
        
       /// <summary>
       /// it just read from state txt that some configurations to load up a form like it was previously 
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            
            if (File.Exists("state.txt"))
            {
                using (StreamReader R = new StreamReader("state.txt"))
                {
                    this.Size = new Size(int.Parse(R.ReadLine()), int.Parse(R.ReadLine()));
                    this.Location = new Point(int.Parse(R.ReadLine()), int.Parse(R.ReadLine()));

                }
            }
        }
        /// <summary>
        /// reading from configurator file startup ini there are some values that changing form's sizes 
        /// and ofcourse write to it some values like height width some other resolutions into file
        /// </summary>
        private void configurator()
        {
            
            int maxH = 300;
            int maxW = 300;
            try
            {
                string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tech", "startup.ini");
               
                using (StreamReader sr = new StreamReader(filepath))
                {
                    string newline = sr.ReadLine();

                    while (newline != null)
                    {
                        if (int.TryParse(newline, out int value))
                        {
                            pictureBox1.Width = value;
                            if (value >= 400)
                            {
                                
                                pictureBox1.Width = maxW;
                            }
                        }
                      
                        newline = sr.ReadLine();
                        if (newline != null && int.TryParse(newline, out int value1))
                        {
                            pictureBox1.Height = value1;
                            if (value1 >= 400)
                            {

                                pictureBox1.Height = maxH;
                            }
                        }

                        newline = sr.ReadLine();

                    }
                }


                using (StreamWriter sw = new StreamWriter(filepath))
                {
                   
                    string[] _currentDirectory = Environment.GetCommandLineArgs();
                    if (_currentDirectory != null)
                    {
                        if (_currentDirectory.Length == 2)
                        {
                            string _pathtodir = _currentDirectory[1];
                            sw.WriteLine($"CurrentPath ={_pathtodir}");
                        }
                        else if (_currentDirectory.Length == 1)
                        {
                            sw.WriteLine($"CurrentPath = {runDirectory}");
                        }
                        else
                        {
                            sw.WriteLine("CurrentPath = ");
                        }

                        
                        Size _picturebox1 = pictureBox1.Size;

                        sw.WriteLine($"width,Height:");
                        sw.WriteLine($"{_picturebox1.Width}");
                        sw.WriteLine($"{_picturebox1.Height}");
                        sw.WriteLine("Recursive,Loop checked state");                      
                        sw.WriteLine($"{checkBox1.Checked}");
                       

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("some error with file startup ini " + ex.Message);

            }

        }
        /// <summary>
        /// load up images with these files with this expansions
        /// and writes it to images array 
        /// </summary>
        /// <param name="dir"></param>
        private void LOADER_OF_IMAGES_O(string dir)
        {
            // amowmebs directorias 
            if (!Directory.Exists(dir)) { MessageBox.Show("problems with directory"); }
            // arrayshi shehyavs yvela foto romelicaa qvemot nachvenebi gafartoebebi
            IMAGES = Directory.GetFiles(dir, "*.*")
                                   .Where(file => file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                  file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                                  file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                                  file.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                                                  file.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                                   .ToArray();

            
            if (IMAGES.Length == 0)
            {
                MessageBox.Show("No images found in the current directory.");
                return;
            }
            // index vayeneb dasawyisshi
            CURRENTIMAGES = 0;
        }
        /// <summary>
        /// to display in 3 photo boxes some photos from array IMAGES 
        /// </summary>
        private void DISPLAY()
        {
            if (IMAGES.Length == 0)
            {
                MessageBox.Show("in display method images length is 0");
                return;
            }
            try

            {
                // tu null aris foto anu ar arsebobs mashin tech file idan 
                // maq 1 foto romelic gamochneba aset shemtxvevashi
                if (pictureBox1.Image == null)
                {
                    SetDefaultImage(pictureBox1);
                }
                // tu null ar aris davayenot foto
                pictureBox1.Image = Image.FromFile(IMAGES[CURRENTIMAGES]);
                
                if (CURRENTIMAGES > 0)
                {
                    pictureBox2.Image = Image.FromFile(IMAGES[CURRENTIMAGES - 1]);
                }
                else
                {
                    pictureBox2.Image = null;
                    SetDefaultImage(pictureBox2);
                }
                if (CURRENTIMAGES < IMAGES.Length - 1)
                {
                    pictureBox3.Image = Image.FromFile(IMAGES[CURRENTIMAGES + 1]);
                }
                else
                {
                    pictureBox3.Image = null;
                    SetDefaultImage(pictureBox3);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error with DISPLAY method" + ex.Message);

            }
        }
        // making moves functions to simple traverse through array and view all photos
        private void pictureBox3_Click(object sender, EventArgs e) { makenextmove(); }
        private void pictureBox2_Click(object sender, EventArgs e) { makeprevmove(); }
        private void button1_Click(object sender, EventArgs e) { makeprevmove(); }
        private void button2_Click(object sender, EventArgs e) { makenextmove(); }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkboxloop = true;           
        } 
        /// <summary>
        /// when slide show runing it automaticaly choses interval 1second 
        /// but user can change it how he wants 
        /// </summary>
        private void StartSlideshow()
        {

            forslideshowtimer = new Timer
            {
                Interval = int.TryParse(textBox1.Text, out int interval) ? interval : 1000
            };
            
            forslideshowtimer.Tick += SlideshowTimer_Tick;
            forslideshowtimer.Start();
            
        }
        /// <summary>
        /// to go farther if slideshow runing and then stop when user 
        /// taped to stop button if user didnot check loop checkbox it will stop to last photo in IMAGES array
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SlideshowTimer_Tick(object sender, EventArgs e)
        {
            
            if (IMAGES.Length > 0)
            {
                CURRENTIMAGES = (CURRENTIMAGES + 1) % IMAGES.Length;
                DISPLAY();
            }
          
            if ( !checkBox1.Checked&& CURRENTIMAGES==IMAGES.Length - 1)
            {
                forslideshowtimer.Stop();
                
                DISPLAY();
            }
        }
        /// <summary>
        /// simply handling event when slideshow runing 
        /// it just blocking other buttons and cheboxes 
        /// starting and stoping slideshow 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            button4wasclicked = !button4wasclicked;
            button4.Text = button4wasclicked ? "Stop" : "Play";

            if (button4wasclicked)
            {
                
                StartSlideshow();
                button1.Enabled = false;
                button2.Enabled = false;
                checkBox1.Enabled = false;
                comboBox1.Enabled = false;
               
                
            }
            else
            {
                forslideshowtimer.Stop();
                CURRENTIMAGES = 0;
                button1.Enabled = true;
                button2.Enabled = true;
                checkBox1.Enabled = true;
                comboBox1.Enabled = true;

                DISPLAY();
            }
        }
        /// <summary>
        /// checking if user input the right parameters 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bool isValidInterval = int.TryParse(textBox1.Text, out int interval) && interval > 0;

            if (isValidInterval)
            {
                textBox1.BackColor = SystemColors.Window; 
                button4.Enabled = true;
            }
            else
            {
                textBox1.BackColor = Color.Red;
                button4.Enabled = false;
            }
        }

        /// <summary>
        /// when program closes it writing to file state text some resolutions 
        /// and after that it is saved 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("state.txt"))
                {
                    sw.WriteLine(this.Size.Width);
                    sw.WriteLine(this.Size.Height);
                    sw.WriteLine(this.Location.X);
                    sw.WriteLine(this.Location.Y);

                }

            }
            catch (Exception ex) { MessageBox.Show("error" + ex.Message); }

        }
        /// <summary>
        /// to load default image for no images find 
        /// </summary>
        /// <param name="pictureBox"></param>
        private void SetDefaultImage(PictureBox pictureBox)
        {
            try
            {
                if (pictureBox.Image == null)
                {
                    pictureBox.Image = Image.FromFile(emptyiconpath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading the image: " + ex.Message);
            }
        }
        /// <summary>
        /// for makeing next move 
        /// </summary>
        private void makenextmove()
        {

            try
            {
                
                if (IMAGES.Length == 0)
                {
                    return;
                }

               
                CURRENTIMAGES = (CURRENTIMAGES + 1) % IMAGES.Length;
                DISPLAY();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// making prev move 
        /// </summary>
        private void makeprevmove()
        {
            try
            {
                if (IMAGES.Length == 0)
                {
                    return;
                }

                
                CURRENTIMAGES = (CURRENTIMAGES - 1) % IMAGES.Length;
                DISPLAY();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        /// <summary>
        /// if user choose the .. item  it will go upper  parent folders
        /// if user choose the ./ item it will go to the child folder 
        /// and other option if user simply choose the file instead of these two 
        /// the program will load up  photos from file if there is some 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedItem != null)
                {
                    if (comboBox1.SelectedItem.ToString() == "..")
                    {
                       
                        comboBox1.Items.Clear();
                        string parentDir = Directory.GetParent(runDirectory)?.FullName;
                        if (!IsRootDirectory(parentDir)) { comboBox1.Items.Add(".."); comboBox1.Items.Add("./"); }
                        
                       

                        if (!string.IsNullOrEmpty(parentDir))
                        {
                            LOAD_DIRS(parentDir); // Reload directories from the parent directory
                        }
                    }
                    else if (comboBox1.SelectedItem != null&&comboBox1.SelectedItem.ToString() == "./")
                    {
                        try
                        {   string childdir = comboBox1.SelectedItem.ToString();
                            comboBox1.Items.Clear();
                            comboBox1.Items.Add("..");
                            
                             
                            
                            if (!string.IsNullOrEmpty(childdir))
                            {
                                LOAD_DIRS(childdir); 
                            }
                        }
                        catch (IndexOutOfRangeException ex) { MessageBox.Show("someerror with :" + ex.Message); }
                        catch (NullReferenceException ex) { MessageBox.Show("someerror with :" + ex.Message); }
                        catch (Exception ex) { MessageBox.Show("someerror with :" + ex.Message); }

                    }
                    else
                    {
                       
                        LOADER_OF_IMAGES_O(comboBox1.SelectedItem.ToString());                       
                        DISPLAY();
                    }

                }
            }
            catch (Exception ex) { MessageBox.Show("error with files .. " + ex.Message); }
        }
        /// <summary>
        /// searching recursively subfolder if there is sum 
        /// and writing it to list  
        /// </summary>
        /// <param name="rootpath"></param>
        /// <param name="children"></param>
        private void GetTheseChildrenRecursively(string rootpath, List<string> children)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(rootpath, "*", SearchOption.TopDirectoryOnly);
                foreach (string dir in dirs)
                {
                    children.Add(dir);
                    try
                    {
                        GetTheseChildrenRecursively(dir, children); // Recursively get subdirectories
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Console.WriteLine($"Access denied to directory: {dir}. Exception: {ex.Message}");
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        Console.WriteLine($"Directory not found: {dir}. Exception: {ex.Message}");
                    }
                    catch (PathTooLongException ex)
                    {
                        Console.WriteLine($"Path too long: {dir}. Exception: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Access denied to directory: {rootpath}. Exception: {ex.Message}");
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine($"Directory not found: {rootpath}. Exception: {ex.Message}");
            }
            catch (PathTooLongException ex)
            {
                Console.WriteLine($"Path too long: {rootpath}. Exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }

        
        /// <summary>
        /// just to be clear if we are at root directory 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool IsRootDirectory(string path)
        {
            if(!Directory.Exists(path)) return false;
            if (path.Split('\\').Length == 1)
            {
                return true;
            }

            return false;
        }
       
    }
}
