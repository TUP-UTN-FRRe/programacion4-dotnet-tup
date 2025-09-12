using Microsoft.AspNetCore.Mvc.RazorPages;
using minimal_api_01.Enums;
using minimal_api_01.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace minimal_api_01.Entidades
{
   

    public class Pedido: IValidatableObject, 
                         IValidable
    {
        public int PedidoId { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Producto { get; set; } = string.Empty;
        public PedidoEstadoEnum Estado { get; set; } = PedidoEstadoEnum.PENDIENTE;

        public string MensajeError { get; set; } = string.Empty;

        public void Cancelar() { 
            this.Estado = PedidoEstadoEnum.CANCELADO;
        }

        public bool IsValid()
        {
            MensajeError = string.Empty;

            if (string.IsNullOrWhiteSpace(this.Cliente))
            {
                MensajeError = "Campo cliente invalido. Debe enviar el campo cliente";
                return false;
            }

            return true;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (string.IsNullOrWhiteSpace(this.Cliente))
            {
                yield return new ValidationResult(
                    "Campo cliente invalido. Debe enviar el campo cliente",
                    new[] { nameof(Cliente), nameof(Cliente) });
            }
        }
    }
}
