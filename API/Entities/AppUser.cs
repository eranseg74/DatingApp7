using API.Extensions;

namespace API.Entities
{
    public class AppUser
    {
        //The Id name is a convension. If another name is used (such as TheId or anything else) we would have to declare this field
        // as a primary key by adding the [Key] annotation above the field.
        // Since we are using the Id convension, entity framework identifies it as a primary key
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<Photo> Photos { get; set; } = new List<Photo>(); // Possible to write only 'new()'
        public int GetAge()
        {
            return DateOfBirth.CalculateAge();
        }
        public List<UserLike> LikedByUsers { get; set; }
        public List<UserLike> LikedUsers { get; set; }
        public List<Message> MessagesSent { get; set; }
        public List<Message> MessagesReceived { get; set; }
    }
}