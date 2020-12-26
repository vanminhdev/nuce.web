using nuce.web.api.ViewModel.Survey;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Core.Interfaces
{
    public interface ISendEmailService
    {
        public Task SendEmailByNuceAsync(SendEmailByNuceModel model);
    }
}
