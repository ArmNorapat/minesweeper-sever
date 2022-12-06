namespace TrueAxion.FFAMinesweepers.ResponseMessage
{
    class SimpleResponse: IResponse
    {
        private readonly string receivedData;

        public SimpleResponse(string receivedData)
        {
            this.receivedData = receivedData;
        }

        public string CreateResponse()
        {
            return receivedData;
        }
    }
}
