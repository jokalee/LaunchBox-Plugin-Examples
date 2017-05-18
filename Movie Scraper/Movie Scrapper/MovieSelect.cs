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

namespace Movie_Scrapper
{
    public partial class MovieSelect : Form
    {
        public MovieSelect(string strip)
        {
            InitializeComponent();

            var matches = Regex.Matches(strip, @"{(.*?)}", RegexOptions.Singleline);
            foreach (var match in matches)
            {
                dynamic result = JObject.Parse(match.ToString());
                ComboboxItem item = new ComboboxItem();
                item.Text = result.original_title + " (" + result.release_date + ")";
                item.Value = result.id;

                comboBox1.Items.Add(item);

                comboBox1.SelectedIndex = 0;

            }
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string moviedb = (comboBox1.SelectedItem as ComboboxItem).Value.ToString();
            string movieinfo = FanartTv.Helper.Json.GetJson("https://api.themoviedb.org/3/movie/" + moviedb + "?api_key=6caeea089cc15cefb0ecb71d257b8c86&language=en-US");

            String search = "imdb_id" + '"' + ':' + '"';

            int pFrom = movieinfo.IndexOf(search) + search.Length;
            int pTo = movieinfo.LastIndexOf('"' + "," + '"' + "original_language");



            String result = movieinfo.Substring(pFrom, pTo - pFrom);
            Class1.imdbid = result;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
