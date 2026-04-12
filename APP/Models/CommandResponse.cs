namespace APP.Models
{
    public class CommandResponse
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public int Id { get; set; }

        public CommandResponse(bool isSuccessful, string message = "", int id = 0)
        {
            IsSuccessful = isSuccessful;
            Message = message;
            Id = id;
        }
    }
}
