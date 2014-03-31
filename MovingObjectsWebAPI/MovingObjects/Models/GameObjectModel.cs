using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovingObjects.Models
{
    public class GameObjectModel
    {
        public int ObjectId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double XDirection { get; set; }
        public string YDirection { get; set; }
        public string Color { get; set; }
    }
}