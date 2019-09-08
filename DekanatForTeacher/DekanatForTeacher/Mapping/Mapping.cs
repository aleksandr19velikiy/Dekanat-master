using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DekanatForTeacher.ViewModels;

namespace DekanatForTeacher.Mapping
{
    public static class Mapping
    {
        public static IMapper Get()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Students, MarksViewModel>();
            });

            return config.CreateMapper();
        }
    }
}
