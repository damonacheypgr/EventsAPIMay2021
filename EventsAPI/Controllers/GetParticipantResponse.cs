namespace EventsAPI.Controllers
{
    internal class GetParticipantResponse
    {
        private int id;
        private string name;
        private string eMail;
        private string phone;

        public GetParticipantResponse(int id, string name, string eMail, string phone)
        {
            this.id = id;
            this.name = name;
            this.eMail = eMail;
            this.phone = phone;
        }
    }
}