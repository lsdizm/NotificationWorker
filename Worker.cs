using Newtonsoft;
using System.ComponentModel.DataAnnotations;

namespace NotificationWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly BrowserWorker _browserWorker;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        _browserWorker = new BrowserWorker();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            // await SearchCancelAppointmentSlot();
            var result = await _browserWorker.GetSchedule().ConfigureAwait(false);
            await SendMessage(result).ConfigureAwait(false);
            await Task.Delay(1000 * 60 * 60 * 24, stoppingToken);
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
