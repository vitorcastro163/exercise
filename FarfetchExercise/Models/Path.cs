using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarfetchExercise.Models
{
    public class Path
    {
        public Path()
        {
            Points = new List<string>();
            Time = 0;
            Cost = 0;
        }
        public Path(List<Point> points, int time, int cost, string endNode) : this()
        {
            foreach(Point p in points)
            {
                this.Points.Add(p.Name);
            }
            this.Points.Add(endNode);
            this.Time += time;
            this.Cost += cost;
        }
        public List<string> Points { get; set; }
        public int Time { get; set; }
        public int Cost { get; set; }

        public override bool Equals(object obj)
        {
            var path = obj as Path;
            return path != null &&
                   EqualityComparer<List<string>>.Default.Equals(Points, path.Points) &&
                   Time == path.Time &&
                   Cost == path.Cost;
        }

        public override int GetHashCode()
        {
            var hashCode = 559234626;
            hashCode = hashCode * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(Points);
            hashCode = hashCode * -1521134295 + Time.GetHashCode();
            hashCode = hashCode * -1521134295 + Cost.GetHashCode();
            return hashCode;
        }
    }
}
