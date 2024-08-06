using System.Collections.Generic;
using System.Threading.Tasks;

namespace TemplateTest.Core.Contract
{
    public interface IClientFactory
    {
        Task<SampleResponse> PostDataAsync<SampleResponse, SampleRequest>(string endPoint, SampleRequest dto);
        Task<SampleResponse> PostDataAsync<SampleResponse>(string endPoint, ICollection<KeyValuePair<string, string>> headers);
        Task<SampleResponse> PostDataAsync<SampleResponse, SampleRequest>(string endPoint, SampleRequest dto, ICollection<KeyValuePair<string, string>> headers);
        Task<SampleResponse> GetDataAsync<SampleResponse>(string endPoint, ICollection<KeyValuePair<string, string>> headers);
        Task<SampleResponse> GetDataAsync<SampleResponse, SampleRequest>(string endPoint, SampleRequest request);
        Task<SampleResponse> GetDataAsync<SampleResponse>(string endPoint);
    }
}
