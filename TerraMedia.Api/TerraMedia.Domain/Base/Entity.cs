using System.ComponentModel.DataAnnotations;

namespace TerraMedia.Domain.Base;

public abstract class Entity
{
    [Key]
    public Guid Id { get; set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
    }
    public abstract void Validate();
}
