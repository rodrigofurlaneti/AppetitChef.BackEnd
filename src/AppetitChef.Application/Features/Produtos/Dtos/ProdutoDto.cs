namespace AppetitChef.Application.Features.Produtos.Dtos
{
    public record ProdutoDto(int Id, string Codigo, string Nome, string? Descricao,
        decimal PrecoVenda, decimal CustoEstimado, int TempoPreparo,
        bool Disponivel, bool VendidoNoBar, bool VendidoNoSalao, bool VendidoDelivery,
        string Categoria, string? ImagemUrl);
}
