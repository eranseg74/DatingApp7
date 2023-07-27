using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Photos")] // Changing the name of the table that will be created in the DB from Photo to Photos
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }

        // Adding the relation properties. This is required in order to make sure that a photo must be connected to a user (not nullable)
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}