namespace TerraMedia.Domain.Contracts.Data;

public interface IRepository
{
    IUnitOfWork UnitOfWork { get; }
}
