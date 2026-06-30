namespace OrderService.Domain.Common;

/// <summary>
/// Базовые свойства сущности
/// </summary>
public abstract class BaseItem
{
    /// <summary>
    /// Дата создания записи
    /// </summary>
    public DateTimeOffset CreateDateTime { get; set; }

    /// <summary>
    /// Дата изменения записи
    /// </summary>
    public DateTimeOffset UpdateDateTime { get; set; }

    /// <summary>
    /// Признак удаления записи
    /// </summary>
    public bool IsDeleted { get; set; }
}