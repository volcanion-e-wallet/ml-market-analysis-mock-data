namespace MarketAnalysis.Application.Mappings;

using AutoMapper;
using MarketAnalysis.Application.DTOs;
using MarketAnalysis.Domain.Entities;

/// <summary>
/// AutoMapper profile for entity to DTO mappings
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Stock mappings
        CreateMap<Stock, StockDto>()
            .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Symbol.Value))
            .ForMember(dest => dest.CurrentPrice, opt => opt.MapFrom(src => src.CurrentPrice.Value))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.CurrentPrice.Currency));

        // MarketTick mappings
        CreateMap<MarketTick, MarketTickDto>()
            .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Symbol.Value))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Value))
            .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => src.Volume.Value))
            .ForMember(dest => dest.High, opt => opt.MapFrom(src => src.High.Value))
            .ForMember(dest => dest.Low, opt => opt.MapFrom(src => src.Low.Value))
            .ForMember(dest => dest.Open, opt => opt.MapFrom(src => src.Open.Value))
            .ForMember(dest => dest.PreviousClose, opt => opt.MapFrom(src => src.PreviousClose.Value));

        // TradingHistory mappings
        CreateMap<TradingHistory, TradingHistoryDto>()
            .ForMember(dest => dest.Symbol, opt => opt.MapFrom(src => src.Symbol.Value))
            .ForMember(dest => dest.Open, opt => opt.MapFrom(src => src.Open.Value))
            .ForMember(dest => dest.High, opt => opt.MapFrom(src => src.High.Value))
            .ForMember(dest => dest.Low, opt => opt.MapFrom(src => src.Low.Value))
            .ForMember(dest => dest.Close, opt => opt.MapFrom(src => src.Close.Value))
            .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => src.Volume.Value))
            .ForMember(dest => dest.AdjustedClose, opt => opt.MapFrom(src => src.AdjustedClose.Value));
    }
}
