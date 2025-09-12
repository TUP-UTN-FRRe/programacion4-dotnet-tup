namespace minimal_api_01.Entidades
{
    public static class PedidoValidator
    {
        public static bool IsValid(this Pedido pedido, out string mensajeError)
        {
            mensajeError = string.Empty;

            if (pedido is null)
            {
                mensajeError = "El pedido no puede ser nulo.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(pedido.Cliente))
            {
                mensajeError = "Campo cliente invalido. Debe enviar el campo cliente";
                return false;
            }

            return true;
        }
    }
}
