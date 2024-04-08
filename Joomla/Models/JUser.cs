using Microsoft.AspNetCore.Identity;

namespace Joomla.Models;

public class JUser: IdentityUser<Guid>
{
    public string FullName { get; set; }
    public DateTime BirthDate { get; set; }

    public ICollection<Article> Articles { get; set; }
}
