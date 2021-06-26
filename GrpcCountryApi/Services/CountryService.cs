using AutoMapper;
using Grpc.Core;
using GrpcCountryApi.Domain.Entities;
using GrpcCountryApi.Protos.v1;
using GrpcCountryApi.Services.Interfaces;
using System.Threading.Tasks;

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

        public override async Task GetAllStreamed(
            EmptyRequest request,
            IServerStreamWriter<CountryReply> responseStream,
            ServerCallContext context)
        {
            var headers = context.GetHttpContext().Request.Headers;
            var lst = await _countryService.GetAsync();

            foreach (var country in lst)
            {
                await responseStream.WriteAsync(_mapper.Map<CountryReply>(country));
            }
            await Task.CompletedTask;
        }

        public override async Task<CountriesReply> GetAll(EmptyRequest request, ServerCallContext context)
        {
            var countries = await _countryService.GetAsync();
            return _mapper.Map<CountriesReply>(countries);
        }

        public override async Task<CountryReply> GetById(CountrySearchRequest request, ServerCallContext context)
        {
            var country = await _countryService.GetByIdAsync(request.CountryId);

            return _mapper.Map<CountryReply>(country);
        }

        public override async Task<CountryReply> Create(CountryCreateRequest request, ServerCallContext context)
        {
            var createCountry = _mapper.Map<Country>(request);
            var country = await _countryService.AddAsync(createCountry);
            return _mapper.Map<CountryReply>(country);
        }

        public override async Task<CountryReply> Update(CountryRequest request, ServerCallContext context)
        {
            var updateCountry = _mapper.Map<Country>(request);
            var country = await _countryService.UpdateAsync(updateCountry);
            return _mapper.Map<CountryReply>(country);
        }

        public override async Task<EmptyReply> Delete(CountrySearchRequest request, ServerCallContext context)
        {
            await _countryService.DeleteAsync(request.CountryId);

            return  new EmptyReply();
        }
    }
}
