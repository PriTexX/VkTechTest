namespace VkTechTest.Contracts;

public sealed class GetAllUsersRequest
{
    /// <summary>
    /// Размер страницы
    /// </summary>
    public required int PageSize { get; init; }
    
    /// <summary>
    /// Количество записей, которые нужно пропустить
    /// </summary>
    public required int OffSet { get; init; }
}