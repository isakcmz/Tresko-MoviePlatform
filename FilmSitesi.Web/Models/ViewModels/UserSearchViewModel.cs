namespace FilmSitesi.Web.Models.ViewModels;

public class UserSearchViewModel
{
    public string Query { get; set; } = string.Empty;

    public List<UserListItemViewModel> Results { get; set; } = new();
}