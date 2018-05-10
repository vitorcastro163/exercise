using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FarfetchExercise.Models
{
    public class Route
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public int Time { get; set; }
        [Required]
        public int Cost { get; set; }
        [Required]
        public int OriginId { get; set; }
        public Point Origin { get; set; }
        [Required]
        public int DestinationId { get; set; }
        public Point Destination { get; set; }

        public override bool Equals(object obj)
        {
            var route = obj as Route;
            return route != null &&
                   Id == route.Id &&
                   Name == route.Name &&
                   Time == route.Time &&
                   Cost == route.Cost &&
                   OriginId == route.OriginId &&
                   EqualityComparer<Point>.Default.Equals(Origin, route.Origin) &&
                   DestinationId == route.DestinationId &&
                   EqualityComparer<Point>.Default.Equals(Destination, route.Destination);
        }

        public override int GetHashCode()
        {
            var hashCode = -1610014636;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Time.GetHashCode();
            hashCode = hashCode * -1521134295 + Cost.GetHashCode();
            hashCode = hashCode * -1521134295 + OriginId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(Origin);
            hashCode = hashCode * -1521134295 + DestinationId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(Destination);
            return hashCode;
        }
    }
}
