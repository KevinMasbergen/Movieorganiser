using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieOrganiser2
{
    public partial class SelectMovie : Form
    {
        public SelectMovie(Movie[] movies)
        {
            InitializeComponent();

            listBox1.Items.AddRange(movies);
        }

        public Movie GetSelectedItem()
        {
            return (Movie)listBox1.SelectedItem;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
