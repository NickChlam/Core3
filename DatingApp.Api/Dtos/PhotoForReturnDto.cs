using System;

namespace DatingApp.Api.Dtos
{
    public class PhotoForReturnDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
    }
}