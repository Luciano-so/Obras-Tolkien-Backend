namespace TerraMedia.Domain.Contracts.Data;

public interface IUnitOfWork
{
    Task<bool> Commit();
}
