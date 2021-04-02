using System.ComponentModel.DataAnnotations;

namespace carlibraryapi.Model
{
    public class CarModel
    {
        [Required]
        public int Id {get; set;}

        [Required]
        public string Name {get; set;}

        [Required]
        public CarBrand CarBrand {get; set;}
    }

    public class CarModelOut
    {
        [Required]
        public int Id {get; set;}

        [Required]
        public string Name {get; set;}

        [Required]
        public CarBrandOut CarBrand {get; set;}

        public static CarModelOut fromCarModel(CarModel carModel) { return new CarModelOut {Id = carModel.Id, Name = carModel.Name, CarBrand = new CarBrandOut{Id=carModel.CarBrand.Id,Name=carModel.CarBrand.Name}}; }
    }

    public class CarModelIn
    {

        [Required]
        public string Name {get; set;}

        [Required]
        public int CarBrandId {get; set;}

        public static CarModelIn fromCarModel(CarModel carModel) { return new CarModelIn {Name = carModel.Name, CarBrandId = carModel.CarBrand.Id}; }

    }
}