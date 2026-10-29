using AutoMapper;
using TeraLinkaCareApi.Application.DTOs;
using TeraLinkaCareApi.Core.Domain.Entities;

namespace TeraLinkaCareApi.Infrastructure.Mappings;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<PtCase, CaseDto>()
            .ForMember(
                dest => dest.CaseTypeShortLabel,
                opt => opt.Ignore() // 忽略這個屬性，因為它來自 join
            );

        CreateMap<PtCase, PreviewCaseDto>()
            .IncludeBase<PtCase, CaseDto>()
            .ForMember(dest => dest.Series, opt => opt.Ignore());

        CreateMap<object, CaseRecordDto>()
            .ForMember(dest => dest.FormDefine, opt => opt.Ignore())
            .ForMember(dest => dest.FormData, opt => opt.Ignore());

        CreateMap<LoginUserDatum, UserAccountDto>()
            .ForMember(
                dest => dest.RoleList,
                opt =>
                    opt.MapFrom(
                        src =>
                            string.IsNullOrEmpty(src.RoleList)
                                ? null
                                : src.RoleList.Split(new[] { ',' }).Select(r => r.Trim()).ToList()
                    )
            )
            .ForMember(
                dest => dest.UserGroupList,
                opt =>
                    opt.MapFrom(
                        src =>
                            string.IsNullOrEmpty(src.UserGroupList)
                                ? null
                                : src.UserGroupList
                                    .Split(new[] { ',' })
                                    .Select(g => g.Trim())
                                    .ToList()
                    )
            );

        CreateMap<UserAccountDto, LoginUserDatum>()
            .ForMember(
                dest => dest.RoleList,
                opt =>
                    opt.MapFrom(
                        src =>
                            src.RoleList == null || !src.RoleList.Any()
                                ? null
                                : string.Join(",", src.RoleList)
                    )
            )
            .ForMember(
                dest => dest.UserGroupList,
                opt =>
                    opt.MapFrom(
                        src =>
                            src.UserGroupList == null || !src.UserGroupList.Any()
                                ? null
                                : string.Join(",", src.UserGroupList)
                    )
            );
    }
}
