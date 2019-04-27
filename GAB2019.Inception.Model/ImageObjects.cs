using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAB2019.Inception.Model
{
    public class ImageObjects
    {
        public List<ImageObject> objects { get; set; }
        
    }

    public class ImageObject
    {
        public Rectangle rectangle { get; set; }
        [JsonProperty("object")]
        public string Object { get; set; }
        public float confidence { get; set; }
        public ImageObject parent { get; set; }
    }

    public class Rectangle
    {
        public int x { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int h { get; set; }
    }
}
