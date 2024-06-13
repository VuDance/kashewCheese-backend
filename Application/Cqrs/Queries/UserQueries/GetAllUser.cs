using Application.Interfaces;
using Domain.Authentication;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Cqrs.Queries.UserQueries
{
    public class GetAllUser:IRequest<ApiResponse<List<User>>>
    {
        public class GetAllUserHandler : IRequestHandler<GetAllUser, ApiResponse<List<User>>>
        {
            private readonly IApplicationDbContext _context;
            public GetAllUserHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<ApiResponse<List<User>>> Handle(GetAllUser request, CancellationToken cancellationToken)
            {
                ApiResponse<List<User>> apiResponse = new ApiResponse<List<User>>();

                List<User> users = new List<User>();
                users = _context.Users.Include(o=>o.UserRoles).ThenInclude(u=>u.Role).ThenInclude(l=>l.RolePermissions).ThenInclude(f=>f.Permission).ToList();
                apiResponse.Data = users;
                return apiResponse;
            }
        }
    }
}
