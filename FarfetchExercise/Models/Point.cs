using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FarfetchExercise.Models
{
    public class Point
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }

        public override bool Equals(object obj)
        {
            var point = obj as Point;
            return point != null &&
                   Id == point.Id &&
                   Name == point.Name;
        }

        public override int GetHashCode()
        {
            var hashCode = -1919740922;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}
