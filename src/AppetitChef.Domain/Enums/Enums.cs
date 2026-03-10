namespace AppetitChef.Domain.Enums;

public enum StatusMesa
{
    Livre = 0,
    Ocupada = 1,
    Reservada = 2,
    Manutencao = 3
}

public enum StatusPedido
{
    Aberto = 0,
    EmPreparo = 1,
    Pronto = 2,
    Entregue = 3,
    Fechado = 4,
    Cancelado = 5
}

public enum StatusItemPedido
{
    Pendente = 0,
    EmPreparo = 1,
    Pronto = 2,
    Entregue = 3,
    Cancelado = 4
}

public enum TipoPedido
{
    Mesa = 0,
    Balcao = 1,
    Delivery = 2
}

public enum TipoCategoria
{
    Comida = 0,
    Bebida = 1,
    Sobremesa = 2,
    Outro = 3
}

public enum StatusReserva
{
    Pendente = 0,
    Confirmada = 1,
    Cancelada = 2,
    Concluida = 3
}

public enum TipoMovimentoEstoque
{
    Entrada = 0,
    Saida = 1,
    Ajuste = 2,
    Perda = 3
}

public enum StatusPagamento
{
    Pendente = 0,
    Aprovado = 1,
    Recusado = 2,
    Estornado = 3
}

public enum StatusDelivery
{
    Aguardando = 0,
    Despachado = 1,
    Entregue = 2,
    Cancelado = 3
}
