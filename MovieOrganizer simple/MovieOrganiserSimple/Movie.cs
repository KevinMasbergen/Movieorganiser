using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Objects.Search;

namespace MovieOrganiser2
{
    public class Movie
    {
        /// <summary>
        /// Full pathname to the movie folder (e.g. C:\Movies\A Good Year (1999)  )
        /// </summary>
        public string OriginalFolder { get; set; }

        /// <summary>
        /// Human readable, configurable name. Can be changed without breaking reference to external sources. e.g. "A Good Year (romantic)".
        /// </summary>
        public string DisplayName { get; set; }

        public string MovieTitle { get; set; }
        public string MovieId { get; set; }

        public SearchMovie MovieObject { get; set; }
        
        // this is for the listbox, so it can display all objects as a list 
        public override string ToString()
        {
            return DisplayName;
        }
    }
}
