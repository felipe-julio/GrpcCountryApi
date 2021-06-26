using AutoMapper;
using GrpcCountryApi.Protos.v1;
using GrpcCountryApi.Services.Interfaces;

namespace GrpcCountryApi.Web.Services
{
    public class CountryGrpcService : CountryService.CountryServiceBase
    {
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;

        public CountryGrpcService(ICountryService countryService, IMapper mapper)
        {
            _countryService = countryService;
            _mapper = mapper;
        }

        //public override async Task GetAllStreamed(
        //    EmptyRequest request, 
        //    IServerStreamWriter<CountryReply> responseStream,
        //    ServerCallContext context)
        //{
        //    var headers = context.GetHttpContext().Request.Headers;
        //    var lst = await _countryService.GetAsync();

        //    foreach(var country in lst)
        //    {
        //        await responseStream.WriteAsync(_mapper.Map<CountryReply>(country));
        //    }

        //    await Task.CompletedTask;
        //}
    }
}
