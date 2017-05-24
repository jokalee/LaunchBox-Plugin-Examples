using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FanartTv;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string query = FanartTv.Helper.Json.GetJson("https://api.themoviedb.org/3/search/movie?api_key=6caeea089cc15cefb0ecb71d257b8c86&query=" + "Angry Birds");
            dynamic imdbsearch = JObject.Parse(query);
            string results = imdbsearch.results.ToString();
            string strip = results.TrimStart('[');
            var matches = Regex.Matches(strip, @"{(.*?)}", RegexOptions.Singleline);
            foreach (var match in matches)
            {
                dynamic result = JObject.Parse(match.ToString());

                Console.WriteLine(result.id);
                string query2 = FanartTv.Helper.Json.GetJson("https://api.themoviedb.org/3/movie/153518?api_key=6caeea089cc15cefb0ecb71d257b8c86");
                dynamic imdbsearch2 = JObject.Parse(query2);
                Console.WriteLine(imdbsearch2.imdb_id);
                Console.WriteLine(imdbsearch2.overview);
                Console.WriteLine(imdbsearch2.release_date);
            
              
            }
           
            Console.Read();
        }
    }
}
