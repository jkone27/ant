namespace AntNet45Tests.Controller
{
    public sealed class ReservationDto
    {
        public ReservationDto(int id, string description)
        {
            Id = id;
            Description = description;
        }

        public int Id { get; }
        public string Description { get; }
    }
}