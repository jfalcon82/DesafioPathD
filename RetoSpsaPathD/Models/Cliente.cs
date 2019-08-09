using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace RetoSpsaPathD.Models
{
    public class Cliente
    {
        private DateTime fechaProbableMuerte;
        private DateTime fechaNacimiento;
        private int edad;
        private int edadProbableMuerte;
        

        public int Id { get; set; }
        [RequiredAttribute(AllowEmptyStrings =false,ErrorMessage ="Ingrese el valor para Nombre")]
        [StringLength(30)]
        public string Nombre { get; set; }
        [RequiredAttribute(AllowEmptyStrings = false, ErrorMessage = "Ingrese el valor para Apellido")]
        [StringLength(30)]
        public string Apellido { get; set; }
        //[RequiredAttribute(AllowEmptyStrings = false, ErrorMessage = "Ingrese una fecha valida para FchaNacimiento")]
        [Range(typeof(DateTime), "01/01/1901", "01/01/2020", ErrorMessage ="Ingrese el valor FechaNacimineto entre 1901 a 2020")]
        
        public DateTime FechaNacimiento {
            get {
                //if (fechaNacimiento == DateTime.Now)
                //{
                //    fechaNacimiento = new DateTime(1901,01,01);
                //}    
                return fechaNacimiento;
            }
            set { fechaNacimiento =  value; }
        }
        [JsonIgnore]
        public int RandomDiasVida { get; set; }             
        public int Edad
        {
            get
            {
                if (FechaNacimiento > DateTime.Today)
                {
                    FechaNacimiento = DateTime.Today;
                }

                edad = DateTime.Today.AddTicks(-FechaNacimiento.Ticks).Year - 1;

                if (edad < 0)
                {
                    edad = 0;
                }

                return edad;
            }            
        }
        public DateTime FechaProbableMuerte
        {
            get
            {                
                fechaProbableMuerte = FechaNacimiento.AddDays(RandomDiasVida);
                return fechaProbableMuerte;
            }
            
        }
        public int EdadProbableMuerte
        {
            get
            {
                edadProbableMuerte = FechaNacimiento.AddDays(RandomDiasVida).AddTicks(-FechaNacimiento.Ticks).Year - 1;
                return edadProbableMuerte;
            }
        }
        
    }
}
