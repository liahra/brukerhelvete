namespace Noested.Models.ViewModels
{

    public class UserListViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
    }
    public class UserViewModel
    {
        public string? UserId { get; set; }
        public string? Email { get; set; }
    }
}
