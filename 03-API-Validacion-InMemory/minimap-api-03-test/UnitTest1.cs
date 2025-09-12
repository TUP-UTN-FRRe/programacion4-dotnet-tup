using minimal_api_01.Entidades;
using minimal_api_01.Enums;

namespace minimap_api_03_test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var pedido1 = new Pedido();
            Assert.Equal(minimal_api_01.Enums.PedidoEstadoEnum.PENDIENTE, pedido1.Estado);
        }

        [Fact]
        public void Test2()
        {
            var pedido1 = new Pedido();
            Assert.Equal(minimal_api_01.Enums.PedidoEstadoEnum.PENDIENTE, pedido1.Estado);

            pedido1.Estado = (PedidoEstadoEnum)666;

        }
    }
}
