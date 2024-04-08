using System.ComponentModel.DataAnnotations.Schema;

namespace Joomla.Models;

public class Article
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid AuthorId { get; set; }
    [ForeignKey(nameof(AuthorId))]
    public JUser Author { get; set; }
}
