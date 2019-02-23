
namespace OrdersService
{
    public class AppSettings
    {
        public string RabbitMqConnectionString { get; set; }
        public string WebApiBaseUrl { get; set; }
        public string WebApiHealthUrl { get; set; }
        public string SeqBaseUrl { get; set; }
        public string IdSrvBaseUrl { get; set; }
        public string SelfHostBaseUrl { get; set; }
    }
}
