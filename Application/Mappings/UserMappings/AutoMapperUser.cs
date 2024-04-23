using Application.Cqrs.Commands.UserCommands;
using Application.Cqrs.Queries.UserQueries;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings.UserMappings
{
    public class AutoMapperUser:Profile
    {
        public AutoMapperUser() {
            CreateMap<CreateUserCommand, User>();
            CreateMap<UserLoginQueries, User>();

        }
    }
}
