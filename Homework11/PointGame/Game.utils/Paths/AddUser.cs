namespace Game.utils.Paths
{
    public class AddUser
    {
        public string? UserName { get; set; }
        public string? Color { get; set; }

        public AddUser(string userName, string color)
        {
            UserName = userName;
            Color = color;
        }
        
        public AddUser() {}
    }
}
