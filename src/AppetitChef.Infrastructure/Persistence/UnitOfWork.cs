using AppetitChef.Domain.Interfaces;
using AppetitChef.Infrastructure.Persistence.Repositories;

namespace AppetitChef.Infrastructure.Persistence;

public class UnitOfWork(AppetitChefDbContext context) : IUnitOfWork
{
    private IPedidoRepository? _pedidos;
    private IMesaRepository? _mesas;
    private IClienteRepository? _clientes;
    private IProdutoRepository? _produtos;
    private IReservaRepository? _reservas;
    private IInsumoRepository? _insumos;
    private IFuncionarioRepository? _funcionarios;

    public IPedidoRepository Pedidos => _pedidos ??= new PedidoRepository(context);
    public IMesaRepository Mesas => _mesas ??= new MesaRepository(context);
    public IClienteRepository Clientes => _clientes ??= new ClienteRepository(context);
    public IProdutoRepository Produtos => _produtos ??= new ProdutoRepository(context);
    public IReservaRepository Reservas => _reservas ??= new ReservaRepository(context);
    public IInsumoRepository Insumos => _insumos ??= new InsumoRepository(context);
    public IFuncionarioRepository Funcionarios => _funcionarios ??= new FuncionarioRepository(context);

    public async Task<int> CommitAsync(CancellationToken ct = default) =>
        await context.SaveChangesAsync(ct);

    public void Dispose() => context.Dispose();
}
