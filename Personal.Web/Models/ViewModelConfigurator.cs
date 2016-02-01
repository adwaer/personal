using Personal.Mapping;

namespace Personal.Web.Models
{
    public class ViewModelConfigurator
    {
        private readonly IConfigurator _configurator;

        public ViewModelConfigurator(IConfigurator configurator)
        {
            _configurator = configurator;
        }

        public void Start()
        {
            //_configurator.Configure<ProgramRequest, JurProgramRequestViewModel>()
            //    .ForMember(dest => dest.Students, src => src.MapFrom(x => x.StudentRels.Select(s => s.Student)));
            //_configurator.ConfigureToEntity<JurProgramRequestViewModel, ProgramRequest>()
            //    .ForMember(dest => dest.OwnerId, src => src.MapFrom(x => x.Owner.Id))
            //    .ForMember(dest => dest.RecordDate, src => src.MapFrom(x => x.ScheduledProgram.StartDate))
            //    .ForMember(dest => dest.Comment, src => src.MapFrom(x => x.ScheduledProgram.Comment))
            //    .ForMember(dest => dest.StudentRels,
            //        src => src.MapFrom(x => x.Students.Select(s => new ProgramRequestToUserRel { StudentId = s.Id })))
            //    .AfterMap((model, entity) =>
            //    {
            //        if (entity.Id == 0)
            //        {
            //            entity.MakeDate = DateTime.UtcNow;
            //            entity.MakeUserId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            //        }
            //        entity.OwnerId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            //    });
        }
    }
}
