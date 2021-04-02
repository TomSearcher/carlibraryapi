using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace carlibraryapi.Model
{
    public class CarBrand
    {
        [Required]
        public int Id {get; set;}

        [Required]
        public string Name {get; set;}

        public virtual ICollection<CarModel> CarModels {get;}
    }

    public class CarBrandIn
    {
        [Required]
        public string Name {get; set;}

        public static CarBrandIn fromCarBrand(CarBrand carBrand) { return new CarBrandIn {Name = carBrand.Name}; }

    }

    public class CarBrandOut
    {
        [Required]
        public int Id {get; set;}

        [Required]
        public string Name {get; set;}

        public static CarBrandOut fromCarBrand(CarBrand carBrand) { return new CarBrandOut {Id = carBrand.Id, Name = carBrand.Name}; }

    }
}