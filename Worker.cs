using Newtonsoft;

namespace NotificationWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await SearchCancelAppointmentSlot();
            await Task.Delay(1000 * 60 * 60, stoppingToken);
        }
    }

    private async Task SearchCancelAppointmentSlot()
    {
        var date = "20230327";
        var url = string.Format(@"https://camp.xticket.kr/Web/Book/GetBookProduct010001.json?product_group_code=0003&start_date={0}&end_date={0}&book_days=1&two_stay_days=0&shopCode=212820734901", 
                                date);
        var httpClient = new HttpClient();

        try
        {
            var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            var result = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            using var streamReader = new StreamReader(contentStream);
            using var jsonReader = new JsonTextReader(streamReader);
            
            await SendMessage(jsonReader.ToString());
        }
        catch (Exception ex)
        {
            await SendMessage(ex.ToString());
        }

    }

    private async Task<string> SendMessage(string message)
    {
        var token = "5974273292:AAH_dQslxH-pj78N-PffAwkYoVmbnPtj3bM";
        var chatId = "415767607";
        var url = string.Format(@"https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}", 
                                token, chatId, message);
        var httpClient = new HttpClient();

        try
        {
            var response = await httpClient.GetAsync(url).ConfigureAwait(false);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }
}
