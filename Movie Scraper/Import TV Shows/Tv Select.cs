using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Import_TV_Shows
{
    public partial class Tv_Select : Form
    {
        

        public Tv_Select(string strip, string Title)
        {
            InitializeComponent();
            this.Text = Title;
            var matches = Regex.Matches(strip, @"{(.*?)}", RegexOptions.Singleline);
            foreach (var match in matches)
            {
                dynamic result = JObject.Parse(match.ToString());
                ComboboxItem item = new ComboboxItem();
                item.Text = result.original_name + " (" + result.first_air_date + ")";
                item.Value = result.id;

                comboBox1.Items.Add(item);

                comboBox1.SelectedIndex = 0;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string moviedb = (comboBox1.SelectedItem as ComboboxItem).Value.ToString();
            string movieinfo = FanartTv.Helper.Json.GetJson("https://api.themoviedb.org/3/tv/" + moviedb + "?api_key=6caeea089cc15cefb0ecb71d257b8c86&language=en-US");
            dynamic imdbsearch = JObject.Parse(movieinfo);
            if (imdbsearch.poster_path != null)
            {
                pictureBox1.Load("https://image.tmdb.org/t/p/w500" + imdbsearch.poster_path);
            }
            else
            {
                pictureBox1.Load("https://upload.wikimedia.org/wikipedia/commons/thumb/b/ba/No_image_available_400_x_600.svg/500px-No_image_available_400_x_600.svg.png");
            }

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string moviedb = (comboBox1.SelectedItem as ComboboxItem).Value.ToString();
            string movieinfo = FanartTv.Helper.Json.GetJson("https://api.themoviedb.org/3/tv/" + moviedb + "?api_key=6caeea089cc15cefb0ecb71d257b8c86&language=en-US");
            dynamic imdbsearch = JObject.Parse(movieinfo);
            Class1.moviedbid = moviedb;
            Class1.plot = imdbsearch.overview.ToString();
            Class1.movietitle = imdbsearch.original_name.ToString();
            Class1.movieposter = imdbsearch.poster_path;
            Class1.moviebackground = imdbsearch.backdrop_path;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Tv_Select_Load(object sender, EventArgs e)
        {

        }
    }
}
