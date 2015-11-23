using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TMDbLib.Client;
using TMDbLib.Objects.Search;

namespace MovieOrganiser2
{
    public partial class Mainform : Form
    {
        private string _movieFolder = @"C:\Users\kevin\Videos";
        private string _tmdbKeyFile = @"C:\Users\kevin\Documents\themoviedatabase-apikey.txt";
        

        public Mainform()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // when form is finished loading, scan folder
            ScanFolder();
        }

        // read folder contents and put all directory names in the listbox
        private void ScanFolder()
        {
            // clear listbox
            listBox1.Items.Clear();

            string[] folders = Directory.GetDirectories(_movieFolder, "*", SearchOption.TopDirectoryOnly);

            foreach (string folder in folders)
            {
                Movie m = new Movie();
                m.OriginalFolder = folder;
                m.DisplayName = ExtractMovieName(extractLastPart(folder));

                listBox1.Items.Add(m);
            }
        }

        private string[] _ignoreList = { "S01", "S02", "S03", "S04", "S05", "S06", "S07", "S08", "S09",
            "480p", "720p", "1080p", "H264", "AC97", "x264", "BluRay" };
        public string ExtractMovieName(string text)
        {
            // check for items on the ignore list and remove them
            foreach ( string item in _ignoreList)
            {
                text = text.Replace(item, "");
            }

            StringBuilder sb = new StringBuilder(text.Length);

            bool didWeSeeASpaceYet = false;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                // if a number is encountered then stop
                if (didWeSeeASpaceYet && (c >= '0' && c <= '9')) break;

                // also break on different separators
                if (c == '(' || c == '[' || c == '-' || c == ':') break;

                // if a space, an underscore _ or a point . is found, then add a space
                if (c == ' ' || c == '_' || c == '.')
                {
                    sb.Append(' ');
                    didWeSeeASpaceYet = true;
                }

                // if a character a-z or A-Z is found, add it to the result
                if (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c >= '0' && c <= '9' )
                    sb.Append(text[i]);
            }

            return sb.ToString();
        }

        // give a text and only return the part after the last slash /
        private string extractLastPart(string fullpath)
        {
            return fullpath.Substring(fullpath.LastIndexOf('\\')+1);
        }

        private void identifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            identifySelectedMovie();
        }

        // take name of selected item in listBox1 and send it to TheMovieDb and retrieve possible movies
        private void identifySelectedMovie()
        {
            string selectedItem = listBox1.SelectedItem.ToString();

            // read API key from textfile (see above)
            string key = "";
            try
            {
                key = File.ReadAllText(_tmdbKeyFile);
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong while reading the TheMovieDb API key from \n" + _tmdbKeyFile + "\n" + e.Message, "Error", MessageBoxButtons.OK);
            }

            TMDbClient client = new TMDbClient(key);
            var results = client.SearchMovie(selectedItem).Results;

            // loop over all the found movies, and put the titles in a list
            var movies = new List<Movie>();
            foreach (SearchMovie mov in results)
            {
                Movie m = new Movie();
                m.MovieObject = mov;
                m.DisplayName = mov.Title;

                movies.Add(m);
            }

            // send all movies to the dialog listbox
            SelectMovie dialog;
            dialog = new SelectMovie(movies.ToArray() );
            DialogResult res = dialog.ShowDialog();

            if ( res == DialogResult.OK )
            {
                // ask the dialog which item was selected
                Movie selectedMovieObject = dialog.GetSelectedItem();

                listBox1.Items[listBox1.SelectedIndex] = selectedMovieObject;
            }
        }

        private void writeMovieIdToFile(string id, string folderName)
        {
            File.WriteAllText(folderName + "\\movie.text", id);
        }

        private void identifyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            identifySelectedMovie();
        }

    }
}
