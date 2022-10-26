using System;
using System.Linq;
using AutoMapper;
using EnterpriseApp.API.Data;
using EnterpriseApp.API.Data.Entity;
using EnterpriseApp.API.Data.ViewModels;
using EnterpriseApp.API.Core.Extensions;
using MongoDB.Bson;

namespace EnterpriseApp.API.Core.Mapping
{
    public class DomainDataMapperProfile : Profile
    {
        public DomainDataMapperProfile()
        {
            CreateMap<ArticleInsertModel, Article>(MemberList.None)
                .ForMember(m => m.Price, opt => opt.MapFrom(p => p.Price))
                .ForMember(m => m.Images, opt => opt.MapFrom(p => p.Images))
                .ForMember(m => m.Tags, opt => opt.MapFrom(p => p.Tags))
                .ForMember(m => m.CreatedOn, opt => opt.MapFrom(p => DateTime.Now))
                .ForMember(m => m.UpdatedOn, opt => opt.MapFrom(p => DateTime.Now))
                .ForMember(m => m.Status, opt => opt.MapFrom(p => WellKnownStatus.Active));

            CreateMap<ArticleUpdateModel, Article>(MemberList.None)
                .ForMember(m => m.Price, opt => opt.MapFrom(p => p.Price))
                .ForMember(m => m.Images, opt => opt.Ignore())
                .ForMember(m => m.Tags, opt => opt.MapFrom(p => p.Tags))
                .ForMember(m => m.UpdatedOn, opt => opt.MapFrom(p => DateTime.Now));

            CreateMap<Article, ArticleModel>(MemberList.None)
               .ForMember(p => p.Tags, opt => opt.MapFrom(p => p.Tags))
               .ForMember(p => p.Images, opt => opt.MapFrom(p => p.Images.Where(x => x.Status == (int)WellKnownStatus.Active)));

            CreateMap<SignUpRequest, User>(MemberList.None)
                .ForMember(m => m.CreatedOn, opt => opt.MapFrom(p => DateTime.Now))
                .ForMember(m => m.UpdatedOn, opt => opt.MapFrom(p => DateTime.Now))
                .ForMember(m => m.Status, opt => opt.MapFrom(p => WellKnownStatus.Active));

            CreateMap<EditUserDetailsRequest, User>(MemberList.None)
                .ForMember(m => m.UpdatedOn, opt => opt.MapFrom(p => DateTime.Now))
                .ForMember(p => p.MyFiles, opt => opt.MapFrom(p => p.MyFiles));

            CreateMap<User, UserModel>(MemberList.None)
                .ForMember(p => p.Permissions, opt => opt.MapFrom(p => p.Permissions))
                .ForMember(p => p.MyFiles, opt => opt.MapFrom(p => p.MyFiles))
                .ForMember(m => m.Location, opt => opt.MapFrom(p => p.Location))
;
            CreateMap<User, LightUserModel>(MemberList.None)
              .ForMember(p => p.Permissions, opt => opt.MapFrom(p => p.Permissions))
              .ForMember(m => m.Location, opt => opt.MapFrom(p => p.Location))
;
            CreateMap<Category, CategoryModel>(MemberList.None)
                .ForMember(p => p.SubCategories, opt => opt.MapFrom(p => p.SubCategories));

            CreateMap<SubCategory, SubCategoryModel>(MemberList.None)
                .ForMember(p => p.Props, opt => opt.MapFrom(p => p.Props))
                .ForMember(p => p.Tags, opt => opt.MapFrom(p => p.Tags));

            CreateMap<CategoryProp, CategoryPropModel>(MemberList.None);

            CreateMap<ArticleTag, ArticleTagModel>(MemberList.None);
            CreateMap<ArticleTagModel, ArticleTag>(MemberList.None)
                .ForMember(m => m.CreatedOn, opt => opt.MapFrom(p => DateTime.Now))
                .ForMember(m => m.UpdatedOn, opt => opt.MapFrom(p => DateTime.Now))
                .ForMember(m => m.Status, opt => opt.MapFrom(p => WellKnownStatus.Active));

            CreateMap<ArticlePrice, ArticlePriceModel>(MemberList.None);
            CreateMap<ArticlePriceModel, ArticlePrice>(MemberList.None);

            CreateMap<FileUpload, FileUploadModel>(MemberList.None);
            CreateMap<FileUploadModel, FileUpload>(MemberList.None)
                .ForMember(m => m.Id, opt => opt.MapFrom(p => ObjectId.GenerateNewId().ToString()))
                .ForMember(m => m.CreatedOn, opt => opt.MapFrom(p => DateTime.Now))
                .ForMember(m => m.UpdatedOn, opt => opt.MapFrom(p => DateTime.Now))
                .ForMember(m => m.Status, opt => opt.MapFrom(p => WellKnownStatus.Active));

            CreateMap<Area, AreaModel>(MemberList.None)
                .ForMember(p => p.Districts, opt => opt.MapFrom(p => p.Districts));

            CreateMap<District, DistrictModel>(MemberList.None)
                .ForMember(p => p.Divisions, opt => opt.MapFrom(p => p.Divisions));

            CreateMap<Division, DivisionModel>(MemberList.None)
               .ForMember(p => p.Villages, opt => opt.MapFrom(p => p.Villages));

            CreateMap<Village, VillageModel>(MemberList.None);

            CreateMap<Stakeholder, StakeholderModel>(MemberList.None);

            CreateMap<InformationCategory, InformationCategoryModel>(MemberList.None);

            CreateMap<DesignationCategory, DesignationCategoryModel>(MemberList.None);

            CreateMap<PublicSentNewsInsertModel, PublicSentNews>(MemberList.None)
               .ForMember(m => m.Images, opt => opt.MapFrom(p => p.Images))
               .ForMember(m => m.CreatedOn, opt => opt.MapFrom(p => DateTime.Now))
               .ForMember(m => m.UpdatedOn, opt => opt.MapFrom(p => DateTime.Now))
               .ForMember(m => m.Status, opt => opt.MapFrom(p => WellKnownStatus.Pending));

            CreateMap<PublicSentNewsUpdateModel, PublicSentNews>(MemberList.None)
                .ForMember(m => m.Images, opt => opt.Ignore())
                .ForMember(m => m.UpdatedOn, opt => opt.MapFrom(p => DateTime.Now));

            CreateMap<PublicSentNews, PublicSentNewsModel>(MemberList.None)
               .ForMember(p => p.Images, opt => opt.MapFrom(p => p.Images.Where(x => x.Status == (int)WellKnownStatus.Active)))
               .ForMember(p => p.UserComments, opt => opt.MapFrom(p => p.UserComments.Where(x => x.Status == (int)WellKnownStatus.Active)))
               .ForMember(m => m.Location, opt => opt.MapFrom(p => p.Location))
               .ForMember(m => m.Reporter, opt => opt.MapFrom(p => p.Reporter));

            CreateMap<PublicSentPartyInformationInsertModel, PublicSentPartyInformation>(MemberList.None)
             .ForMember(m => m.Images, opt => opt.MapFrom(p => p.Images))
             .ForMember(m => m.CreatedOn, opt => opt.MapFrom(p => DateTime.Now))
             .ForMember(m => m.UpdatedOn, opt => opt.MapFrom(p => DateTime.Now))
             .ForMember(m => m.Status, opt => opt.MapFrom(p => WellKnownStatus.Pending));

            CreateMap<PublicSentPartyInformationUpdateModel, PublicSentPartyInformation>(MemberList.None)
                .ForMember(m => m.Images, opt => opt.MapFrom(p => p.Images))
                .ForMember(m => m.UpdatedOn, opt => opt.MapFrom(p => DateTime.Now));

            CreateMap<PublicSentPartyInformation, PublicSentPartyInformationModel>(MemberList.None)
               .ForMember(p => p.Images, opt => opt.MapFrom(p => p.Images.Where(x => x.Status == (int)WellKnownStatus.Active)))
               .ForMember(p => p.UserComments, opt => opt.MapFrom(p => p.UserComments.Where(x => x.Status == (int)WellKnownStatus.Active)))
               .ForMember(m => m.Location, opt => opt.MapFrom(p => p.Location))
               .ForMember(m => m.Reporter, opt => opt.MapFrom(p => p.Reporter));


            CreateMap<PublicSentInformationInsertModel, PublicSentInformation>(MemberList.None)
             .ForMember(m => m.Images, opt => opt.MapFrom(p => p.Images))
             .ForMember(m => m.CreatedOn, opt => opt.MapFrom(p => DateTime.Now))
             .ForMember(m => m.UpdatedOn, opt => opt.MapFrom(p => DateTime.Now))
             .ForMember(m => m.Status, opt => opt.MapFrom(p => WellKnownStatus.Pending));

            CreateMap<PublicSentInformationUpdateModel, PublicSentInformation>(MemberList.None)
                .ForMember(m => m.Images, opt => opt.Ignore())
                .ForMember(m => m.UpdatedOn, opt => opt.MapFrom(p => DateTime.Now));

            CreateMap<PublicSentInformation, PublicSentInformationModel>(MemberList.None)
               .ForMember(p => p.Images, opt => opt.MapFrom(p => p.Images.Where(x => x.Status == (int)WellKnownStatus.Active)))
               .ForMember(p => p.UserComments, opt => opt.MapFrom(p => p.UserComments.Where(x => x.Status == (int)WellKnownStatus.Active)))
               .ForMember(m => m.Location, opt => opt.MapFrom(p => p.Location))
               .ForMember(m => m.Reporter, opt => opt.MapFrom(p => p.Reporter));

            CreateMap<Configuration, ConfigurationModel>(MemberList.None)
            .ForMember(p => p.Images, opt => opt.MapFrom(p => p.Images.Where(x => x.Status == (int)WellKnownStatus.Active)));

            CreateMap<ConfigUpdateRequest, Configuration>(MemberList.None)
              .ForMember(m => m.Images, opt => opt.Ignore())
              .ForMember(m => m.UpdatedOn, opt => opt.MapFrom(p => DateTime.Now));

            CreateMap<ChatInsertModel, Chat>(MemberList.None)
           .ForMember(m => m.Attachments, opt => opt.MapFrom(p => p.Attachments))
           .ForMember(m => m.CreatedOn, opt => opt.MapFrom(p => DateTime.Now))
           .ForMember(m => m.UpdatedOn, opt => opt.MapFrom(p => DateTime.Now))
           .ForMember(m => m.Status, opt => opt.MapFrom(p => WellKnownStatus.Active));

            CreateMap<Chat, ChatModel>(MemberList.None)
               .ForMember(p => p.Attachments, opt => opt.MapFrom(p => p.Attachments.Where(x => x.Status == (int)WellKnownStatus.Active)));

            CreateMap<PublicMessageInsertModel, PublicMessage>(MemberList.None)
            .ForMember(m => m.Images, opt => opt.MapFrom(p => p.Images))
            .ForMember(m => m.CreatedOn, opt => opt.MapFrom(p => DateTime.Now))
            .ForMember(m => m.UpdatedOn, opt => opt.MapFrom(p => DateTime.Now))
            .ForMember(m => m.Status, opt => opt.MapFrom(p => WellKnownStatus.Active));

            CreateMap<PublicMessageUpdateModel, PublicMessage>(MemberList.None)
                .ForMember(m => m.Images, opt => opt.Ignore())
                .ForMember(m => m.UpdatedOn, opt => opt.MapFrom(p => DateTime.Now));

            CreateMap<PublicMessage, PublicMessageModel>(MemberList.None)
               .ForMember(p => p.Images, opt => opt.MapFrom(p => p.Images.Where(x => x.Status == (int)WellKnownStatus.Active)))
               .ForMember(m => m.Location, opt => opt.MapFrom(p => p.Location))
               .ForMember(p => p.UserComments, opt => opt.MapFrom(p => p.UserComments.Where(x => x.Status == (int)WellKnownStatus.Active)))
               .ForMember(m => m.Reporter, opt => opt.MapFrom(p => p.Reporter));

            CreateMap<UpdateDeviceTokenRequest, UserDevice>(MemberList.None)
            .ForMember(m => m.CreatedOn, opt => opt.MapFrom(p => DateTime.Now))
            .ForMember(m => m.UpdatedOn, opt => opt.MapFrom(p => DateTime.Now))
            .ForMember(m => m.Status, opt => opt.MapFrom(p => WellKnownStatus.Active));

            CreateMap<PartyDesignationInsertModel, PartyDesignation>(MemberList.None)
            .ForMember(m => m.Images, opt => opt.MapFrom(p => p.Images))
            .ForMember(m => m.CreatedOn, opt => opt.MapFrom(p => DateTime.Now))
            .ForMember(m => m.UpdatedOn, opt => opt.MapFrom(p => DateTime.Now))
            .ForMember(m => m.Status, opt => opt.MapFrom(p => WellKnownStatus.Active));

            CreateMap<PartyDesignationUpdateModel, PartyDesignation>(MemberList.None)
                .ForMember(m => m.Images, opt => opt.Ignore())
                .ForMember(m => m.UpdatedOn, opt => opt.MapFrom(p => DateTime.Now));

            CreateMap<PartyDesignation, PartyDesignationModel>(MemberList.None)
               .ForMember(p => p.Images, opt => opt.MapFrom(p => p.Images.Where(x => x.Status == (int)WellKnownStatus.Active)));

            CreateMap<Location, LocationModel>(MemberList.None);

            CreateMap<UserCommentInsertModel, UserComment>(MemberList.None)
           .ForMember(m => m.Id, opt => opt.MapFrom(p => ObjectId.GenerateNewId()))
           .ForMember(m => m.CreatedOn, opt => opt.MapFrom(p => DateTime.Now))
           .ForMember(m => m.Status, opt => opt.MapFrom(p => WellKnownStatus.Active));

            CreateMap<UserComment, UserCommentModel>(MemberList.None);

        }
    }
}
