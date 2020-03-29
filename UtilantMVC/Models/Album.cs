using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UtilantMVC.Models
{
    public class Album
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public Address address { get; set; }
        public string firstThumb { get; set; }
    }
}
